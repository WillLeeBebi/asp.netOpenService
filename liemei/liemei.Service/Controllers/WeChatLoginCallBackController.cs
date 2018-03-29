using liemei.Bll;
using liemei.Common.cache;
using liemei.Common.common;
using liemei.Common.Models;
using liemei.Model;
using liemei.Model.EnumModel;
using liemei.Service.MqttClientHelper;
using liemei.WeChat;
using liemei.WeChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace liemei.Service.Controllers
{
    public class WeChatLoginCallBackController : Controller
    {
        WeChatLoginBll loginbll = new WeChatLoginBll();
        WeChatUserBll userbll = new WeChatUserBll();
        /// <summary>
        /// 用户扫码授权回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        // GET: WeChatLoginCallBack
        public  ActionResult Index( string state,string code)
        {
            ClassLoger.Error("WeChatCallBackController.get.uid:", code, state);
            ThreadPool.QueueUserWorkItem(new WaitCallback(p => {
                try
                {
                    if (!string.IsNullOrEmpty(code))
                    {
                        getUserinfo(code, state);
                    }
                    else
                    {
                        SaveNoUserinfo(state);
                    }
                } catch (Exception ex)
                {
                    ClassLoger.Error("WeChatLoginCallBack",ex);
                }
            }), null);
            //WxPayData res = new WxPayData();
            //res.SetValue("return_code", "SUCCESS");
            //res.SetValue("return_msg", "OK");
            ////ClassLoger.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
            //Response.Write(res.ToXml());
            //Response.End();
            return View();
        }
        /// <summary>
        /// 用户确认授权
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        private void getUserinfo(string code, string state)
        {
            try
            {

                //网络授权有有效期，最好用redis缓存
                //获取用户网络授权

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
                    Access_tokenResult accResult = WeChatAccessTokenAPI.GetWeChatAccess_token(code);
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
                    userinfo = WeChatUserInfoAPI.GetWeChatUserInfo(access_token, openid);
                    if (userinfo != null)
                    {
                        RedisBase.Item_Set(userkey, userinfo);
                        RedisBase.ExpireEntryAt(userkey, DateTime.Now.AddDays(2));
                    }
                }
                WeChatUser wuser = userbll.GetWeChatUserByUnionID(userinfo.unionid);
                if (wuser == null)
                {
                    wuser = new WeChatUser();
                    wuser.PlatformOpenID = userinfo.openid;
                    wuser.UnionID = userinfo.unionid;
                    userbll.AddWeChatUser(wuser);
                }
                else
                {
                    if (string.IsNullOrEmpty(wuser.PlatformOpenID))
                    {
                        wuser.PlatformOpenID = userinfo.openid;
                        wuser.UnionID = userinfo.unionid;
                        userbll.UpdateWeChatUser(wuser);
                    }
                }
                UserInfoBll ubll = new UserInfoBll();
                UserInfo _user = ubll.GetUserInfoByOpenID(userinfo.unionid);
                // 用户绑定微信
                string bindkey = string.Format("bind_{0}", state);
                string msg = MqttAgreement.GetWeChatLoginMA(state, true);
                if (RedisBase.ContainsKey(bindkey))
                {
                    //用户之前已经关注过微信公共号，需要把之前微信公共号账户中的信息更新到这个账户中
                    if (_user!=null)
                    {
                        _user.Openid = "";
                        ubll.UpdateUserinfo(_user);
                    }
                    UserInfo binduser = RedisBase.Item_Get<UserInfo>(bindkey);
                    binduser.Openid = userinfo.unionid;
                    ubll.UpdateUserinfo(binduser);
                }
                else
                {
                    WeChatLogin login = loginbll.GetWeChatLoginByUUID(state);
                    if (login == null)
                    {
                        login = new WeChatLogin();
                        login.UUID = state;
                        login.CreateTime = DateTime.Now;
                        login.LoginData = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    login.Headimgurl = userinfo.headimgurl;
                    login.Nickname = userinfo.nickname;
                    login.Openid = userinfo.unionid;
                    login.Sex = userinfo.sex.TryToString();
                    login.State = 1;
                    login.LoginData = DateTime.Now.ToString("yyyy-MM-dd");
                    login.CreateTime = DateTime.Now;
                    loginbll.UpdateWeChatLogin(login);

                    if (_user == null)
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
                        ubll.UpdateUserinfo(_user);
                    }
                    //向客户端推送消息
                    MqttPublishClient.Ins.PublishOneClient(login.LockCode, msg);
                }
                MqttPublishClient.Ins.PublishAllClient(msg);
            }
            catch (Exception ex)
            {
                ClassLoger.Error("WeChatLoginCallBackController.getUserinfo", ex);
            }
        }
        /// <summary>
        /// 用户拒绝授权给客户端发送通知
        /// </summary>
        /// <param name="state"></param>
        private void SaveNoUserinfo( string state)
        {
            try
            {
                WeChatLogin login = loginbll.GetWeChatLoginByUUID(state);
                login.State = -1;
                login.UUID = state;
                login.LoginData = DateTime.Now.ToString("yyyy-MM-dd");
                login.CreateTime = DateTime.Now;
                loginbll.UpdateWeChatLogin(login);

                //向客户端推送消息
                string msg = MqttAgreement.GetWeChatLoginMA(state, false);
                MqttPublishClient.Ins.PublishOneClient(login.LockCode, msg);
                MqttPublishClient.Ins.PublishAllClient(msg);
            }
            catch (Exception ex)
            {
                ClassLoger.Error("WeChatLoginCallBackController.getUserinfo", ex);
            }
        }

        #region cacheKey
        string getWeChatUserKey(string access_token, string openid)
        {
            return string.Format("getWeChatUserKey_{0}_{1}", access_token, openid).MD5();
        }
        #endregion
    }
}