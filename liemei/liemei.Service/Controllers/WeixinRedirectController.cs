using liemei.Bll;
using liemei.Common;
using liemei.Common.cache;
using liemei.Common.common;
using liemei.Model;
using liemei.Model.EnumModel;
using liemei.Service.Filters;
using liemei.Service.Models;
using liemei.Service.WeChatAPIHelper;
using liemei.WeChat;
using liemei.WeChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace liemei.Service.Controllers
{
    public class WeixinRedirectController : Controller
    {
        UserInfoBll userbll = new UserInfoBll();
        // GET: WeixinRedirect
        [HttpGet]
        public RedirectResult Index(string type)
        {
            return Redirect(WeChateSiteHelper.getOauthURL(type));
        }
        [CustomAuthenticationFilter(IsUrlDecode =true)]
        public ActionResult CallBackRedirect()
        {
            //网络授权有有效期，最好用redis缓存
            //获取用户网络授权
            string code = Request["code"].TryToString();
            string state = Request["state"].TryToString();
           
            ClassLoger.Info("WeixinRedirectController.CallBackRedirect",state,code);
            string access_token = string.Empty;
            string openid = string.Empty;
            if (RedisBase.ContainsKey(code))
            {
                Access_tokenResult accResult = RedisBase.Item_Get<Access_tokenResult>(code);
                access_token = accResult.access_token;
                openid = accResult.openid;
            }
            else
            {
                Access_tokenResult accResult = WeChatAccessTokenAPI.GetWeChatServiceAccess_token(code);
                RedisBase.Item_Set(code, accResult);
                RedisBase.ExpireEntryAt(code, DateTime.Now.AddSeconds(accResult.expires_in));
                access_token = accResult.access_token;
                openid = accResult.openid;
            }

            //获取用户信息
            WeChatUserInfo userinfo = null;
            string userkey = getWeChatUserKey(access_token, openid);
            if (RedisBase.ContainsKey(userkey))
            {
                userinfo = RedisBase.Item_Get<WeChatUserInfo>(userkey);
            }
            else
            {
                userinfo = WeChatUserInfoAPI.GetWeChatUserInfo(access_token,openid);
                if (userinfo!=null)
                {
                    
                    RedisBase.Item_Set(userkey,userinfo);
                    RedisBase.ExpireEntryAt(userkey,DateTime.Now.AddDays(2));
                }
            }
            UserInfo _user = userbll.GetUserInfoByOpenID(userinfo.unionid);
            if (_user==null)
            {
                _user = new UserInfo();
                _user.Openid = userinfo.unionid;
                _user.CreateTime = DateTime.Now;
                _user.Headimgurl = userinfo.headimgurl;
                _user.Nickname = userinfo.nickname;
                _user.Sex = (SexEnum)userinfo.sex;
                _user.Name = userinfo.nickname;
                _user.city = userinfo.city;
                _user.province = userinfo.province;
                userbll.UpdateUserinfo(_user);
            }
            ClassLoger.Info("CallBackRedirect", userkey);
            RedisSession<UserInfo> redissession = new Models.RedisSession<UserInfo>(HttpContext, true, 120);
            redissession["UserCode"] = _user;
            switch (state)
            {
                //普通用户个人中心
                case "UserCore":
                    return RedirectToAction("Index", "UserCore");
                case "AdminCore":
                    //return RedirectToAction("Index", "AdminCore");
                default:
                    string url = HttpUtils.Ins.UrlDecode(state);
                    if (RedisBase.ContainsKey(state))
                    {
                        url = RedisBase.Item_Get<string>(state);
                    }
                    return Redirect(url);
            }
        }

        /// <summary>
        /// 微信公共号H5页面js签名
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult jssignature()
        {
            string url = Request["url"].TryToString();
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            string timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            //随机数
            var noncestr = url.MD5()+Utils.GetRandom(10000,99999);

            var jsapi_ticket = WeChatAccessTokenAPI.Getjsapi_ticket();
            
            ClassLoger.Info("jsapi_ticket", jsapi_ticket);
            ClassLoger.Info("url", url);
            if (url.IsNull())
            {
                url = Request.Url.ToString();
            }
            string data = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsapi_ticket, noncestr, timestamp, url);
            string signature = Utils.SHA1(data);
            return Json(new { timestamp = timestamp, noncestr = noncestr, appid= SystemSet.Serviceappid, signature= signature },JsonRequestBehavior.AllowGet);
        }
        #region cacheKey
        string getWeChatUserKey(string access_token,string openid)
        {
            return string.Format("getWeChatUserKey_{0}_{1}", access_token, openid).MD5();
        }
        #endregion
    }
}