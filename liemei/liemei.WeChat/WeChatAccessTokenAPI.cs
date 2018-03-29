using liemei.Common;
using liemei.Common.cache;
using liemei.Common.common;
using liemei.WeChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.WeChat
{
    public class WeChatAccessTokenAPI
    {

        /// <summary>
        /// 获取网页用户授权
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Access_tokenResult GetWeChatAccess_token(string code)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", SystemSet.Appid, SystemSet.Appsecret, code);
            Access_tokenResult result = new Access_tokenResult();
            string resultJson = HttpUtils.Ins.GET(url);
            if (!resultJson.IsNull())
            {
                if (resultJson.Contains("errcode"))
                {
                    //WeChatErrorResult errorResult = JsonHelper.DeserializeObject<WeChatErrorResult>(resultJson);
                    ClassLoger.Fail("WeChatAPIHelper.GetWeChatAccess_token(" + code + ")", url);
                    ClassLoger.Fail("WeChatAPIHelper.GetWeChatAccess_token(" + code + ")", resultJson);
                }
                else
                {
                    Dictionary<string, object> resDic = JsonHelper.DeserializeObject(resultJson);
                    result.access_token = resDic["access_token"].TryToString();
                    result.expires_in = resDic["expires_in"].TryToInt(100);
                    result.openid = resDic["openid"].TryToString();
                    result.refresh_token = resDic["refresh_token"].TryToString();
                    result.scope = resDic["scope"].TryToString();
                }
            }
            return result;
        }
        /// <summary>
        /// 刷新access_token（如果需要）
        /// 由于access_token拥有较短的有效期，当access_token超时后，可以使用refresh_token进行刷新，
        /// refresh_token拥有较长的有效期（7天、30天、60天、90天），当refresh_token失效的后，需要用户重新授权。
        /// </summary>
        /// <param name="refresh_token">通过access_token获取到的refresh_token参数</param>
        /// <returns></returns>
        public static Access_tokenResult Refresh_token(string refresh_token)
        {
            Access_tokenResult result = new Access_tokenResult();
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", SystemSet.Appid, refresh_token);
            string resultJson = HttpUtils.Ins.GET(url);
            if (!resultJson.IsNull())
            {
                if (resultJson.Contains("errcode"))
                {
                    //WeChatErrorResult errorResult = JsonHelper.DeserializeObject<WeChatErrorResult>(resultJson);
                    ClassLoger.Fail("WeChatAPIHelper.refresh_token(" + refresh_token + ")", url);
                    ClassLoger.Fail("WeChatAPIHelper.refresh_token(" + refresh_token + ")", resultJson);
                }
                else
                {
                    result = JsonHelper.DeserializeObject<Access_tokenResult>(resultJson);
                }
            }
            return result;
        }
        /// <summary>
        /// 获取微信公共号的Access_token
        /// </summary>
        /// <returns></returns>
        public static string GetWeChatAccess_token()
        {
           
            string access_token = string.Empty;
            string key = getaccess_tokenKey();
            if (RedisBase.ContainsKey(key))
            {
                access_token = RedisBase.Item_Get<string>(key);
            }
            else
            {
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", SystemSet.Serviceappid, SystemSet.Serviceappsecret);
                string resultJson = HttpUtils.Ins.GET(url);
                if (!resultJson.IsNull())
                {
                    if (resultJson.Contains("errcode"))
                    {
                        //WeChatErrorResult errorResult = JsonHelper.DeserializeObject<WeChatErrorResult>(resultJson);
                        ClassLoger.Fail("WeChatAPIHelper.GetWeChatAccess_token", url);
                        ClassLoger.Fail("WeChatAPIHelper.GetWeChatAccess_token", resultJson);
                    }
                    else
                    {
                        Dictionary<string, object> resDic = JsonHelper.DeserializeObject(resultJson);
                        access_token = resDic["access_token"].TryToString();
                        int expires_in = resDic["expires_in"].TryToInt(100);
                        RedisBase.Item_Set<string>(key, access_token);
                        RedisBase.ExpireEntryAt(key, DateTime.Now.AddSeconds(expires_in));
                    }
                }
            }
            return access_token;
        }
        /// <summary>
        /// 微信公共号授权登录
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Access_tokenResult GetWeChatServiceAccess_token(string code)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", SystemSet.Serviceappid, SystemSet.Serviceappsecret, code);
            Access_tokenResult result = new Access_tokenResult();
            string resultJson = HttpUtils.Ins.GET(url);
            ClassLoger.Info("WeChatAccessTokenAPI.GetWeChatServiceAccess_token",url,resultJson);
            if (!resultJson.IsNull())
            {
                if (resultJson.Contains("errcode"))
                {
                    //WeChatErrorResult errorResult = JsonHelper.DeserializeObject<WeChatErrorResult>(resultJson);
                    ClassLoger.Fail("WeChatAPIHelper.GetWeChatAccess_token(" + code + ")", url);
                    ClassLoger.Fail("WeChatAPIHelper.GetWeChatAccess_token(" + code + ")", resultJson);
                }
                else
                {
                    Dictionary<string, object> resDic = JsonHelper.DeserializeObject(resultJson);
                    result.access_token = resDic["access_token"].TryToString();
                    result.expires_in = resDic["expires_in"].TryToInt(100);
                    result.openid = resDic["openid"].TryToString();
                    result.refresh_token = resDic["refresh_token"].TryToString();
                    result.scope = resDic["scope"].TryToString();
                }
            }
            return result;
        }
        /// <summary>
        /// 获取微信公共号jssdk调用接口临时凭证
        /// </summary>
        /// <returns></returns>
        public static string Getjsapi_ticket()
        {
            string key = Getjsapi_ticketKey();
            if (RedisBase.ContainsKey(key))
                return RedisBase.Item_Get<string>(key);
            else
            {
                string ticket = string.Empty;
                string Access_token = GetWeChatAccess_token();
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", Access_token);
                string resultJson = HttpUtils.Ins.GET(url);
                Dictionary<string, object> resDic = JsonHelper.DeserializeObject(resultJson);
                if (resDic["errcode"].TryToInt(0)==0)
                {
                    ticket = resDic["ticket"].TryToString();
                    RedisBase.Item_Set(key, ticket);
                    RedisBase.ExpireEntryAt(key,DateTime.Now.AddSeconds(resDic["expires_in"].TryToInt(7200)));
                }
                return ticket;
            }
        }
        /// <summary>
        /// 长连接转换为短链接
        /// </summary>
        /// <param name="long_url"></param>
        /// <returns></returns>
        public static string GetShortURL(string long_url)
        {
            string access_token = GetWeChatAccess_token();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/shorturl?access_token={0}", access_token);
            string postJson = "{\"action\":\"long2short\",\"long_url\":\""+long_url+"\"}";

            string resultJson = HttpUtils.Ins.POST(url,postJson);
            Dictionary<string, object> result = JsonHelper.DeserializeObject(resultJson);
            if (result["errcode"].TryToInt(0)==0)
            {
                return result["short_url"].TryToString();
            }
            return string.Empty;
        }

        static string getaccess_tokenKey()
        {
            return "WeChatAccessTokenAPI.getaccess_tokenKey".MD5();
        }

        static string Getjsapi_ticketKey()
        {
            return "WeChatAccessTokenAPI.Getjsapi_ticketKey".MD5();
        }
    }
}
