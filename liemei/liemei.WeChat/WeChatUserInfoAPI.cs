using liemei.Common;
using liemei.Common.common;
using liemei.WeChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.WeChat
{
    public class WeChatUserInfoAPI
    {

        /// <summary>
        /// 微信开放平台拉取用户信息(需scope为 snsapi_userinfo)
        /// </summary>
        /// <param name="access_token">网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同</param>
        /// <param name="openid">用户的唯一标识</param>
        /// <returns></returns>
        public static WeChatUserInfo GetWeChatUserInfo(string access_token, string openid)
        {
            WeChatUserInfo uinfo = new WeChatUserInfo();
            string url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", access_token, openid);
            string jsonResult = HttpUtils.Ins.GET(url);
            ClassLoger.Info("WeChatAPIHelper.GetWeChatUserInfo", url, jsonResult);
            if (!jsonResult.IsNull())
            {
                if (jsonResult.Contains("errcode"))
                {
                    ClassLoger.Fail("WeChatAPIHelper.GetWeChatUserInfo", url);
                    ClassLoger.Fail("WeChatAPIHelper.GetWeChatUserInfo", jsonResult);
                }
                else
                {
                    Dictionary<string, object> resultDic = JsonHelper.DeserializeObject(jsonResult);
                    uinfo.city = resultDic["city"].TryToString();
                    uinfo.country = resultDic["country"].TryToString();
                    uinfo.headimgurl = resultDic["headimgurl"].TryToString();
                    uinfo.nickname = resultDic["nickname"].TryToString();
                    uinfo.openid = resultDic["openid"].TryToString();
                    uinfo.privilege = resultDic["privilege"].TryToString();
                    uinfo.province = resultDic["province"].TryToString();
                    uinfo.sex = resultDic["sex"].TryToInt(0);
                    if (resultDic.ContainsKey("unionid"))
                        uinfo.unionid = resultDic["unionid"].TryToString();
                }
            }
            return uinfo;
        }
        /// <summary>
        /// 设置用户备注名称
        /// </summary>
        /// <param name="openID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool SetWeChatUserName(string openID,string name)
        {
            string access_token = WeChatAccessTokenAPI.GetWeChatAccess_token();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token={0}", access_token);
            string json = "{\"openid\":\"" + openID + "\",\"remark\":\"" + name + "\"}";
            string resultJson = HttpUtils.Ins.POST(url,json);
            Dictionary<string, object> result = JsonHelper.DeserializeObject(resultJson);
            if (result["errcode"].TryToInt(0) == 0)
                return true;
            else
            {
                ClassLoger.Fail("WeChatUserInfoAPI.SetWeChatUserName",resultJson);
            }
            return false;
        }
        /// <summary>
        /// 获取微信公共号的用户信息
        /// </summary>
        /// <param name="UnionID"></param>
        /// <returns></returns>
        public static WeChatUserInfo GetWeChatUserInfo(string opendid)
        {
            WeChatUserInfo uinfo = new WeChatUserInfo();
            string access_token = WeChatAccessTokenAPI.GetWeChatAccess_token();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", access_token, opendid);
            string jsonResult = HttpUtils.Ins.GET(url);
            ClassLoger.Info("WeChatAPIHelper.GetWeChatUserInfo", url, jsonResult);
            if (!jsonResult.IsNull())
            {
                if (jsonResult.Contains("errcode"))
                {
                    ClassLoger.Fail("WeChatAPIHelper.GetWeChatUserInfo", url);
                    ClassLoger.Fail("WeChatAPIHelper.GetWeChatUserInfo", jsonResult);
                }
                else
                {
                    Dictionary<string, object> resultDic = JsonHelper.DeserializeObject(jsonResult);
                    uinfo.subscribe = resultDic["subscribe"].TryToInt(0);
                    if (resultDic["subscribe"].TryToInt(0)!=0)
                    {
                        uinfo.city = resultDic["city"].TryToString();
                        uinfo.country = resultDic["country"].TryToString();
                        uinfo.headimgurl = resultDic["headimgurl"].TryToString();
                        uinfo.nickname = resultDic["nickname"].TryToString();
                        uinfo.openid = resultDic["openid"].TryToString();
                        uinfo.province = resultDic["province"].TryToString();
                        uinfo.sex = resultDic["sex"].TryToInt(0);
                        if (resultDic.ContainsKey("unionid"))
                            uinfo.unionid = resultDic["unionid"].TryToString();
                        uinfo.groupid = resultDic["groupid"].TryToInt(0);
                    }
                }
            }
            return uinfo;
        }
    }
}
