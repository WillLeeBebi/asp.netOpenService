using liemei.Bll;
using liemei.Common;
using liemei.Common.common;
using liemei.Model;
using liemei.Model.EnumModel;
using liemei.WeChat;
using liemei.WeChat.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace liemei.Service.Controllers
{
    public class WeChatSigController : Controller
    {
        WeChatUserBll userbll = new WeChatUserBll();
        UserInfoBll ubll = new UserInfoBll();
        /// <summary>
        /// 用户消息和开发者需要的事件推送
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="token"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="echostr"></param>
        /// <returns></returns>
        // GET: WeChatSig
        [HttpGet]
        [ActionName("Index")]
        public string GET(string signature,string token,string timestamp,string nonce,string echostr)
        {
            ClassLoger.Info("checkWeChatSignatureController,signature:", signature, "token:" + token, "timestamp:" + timestamp, "nonce:" + nonce, "echostr:" + echostr);
            try
            {
                List<string> paraList = new List<string>() { token, timestamp, nonce };
                var para = paraList.OrderBy(x => x);
                var paraStr = string.Join("", para);
                var sig = Utils.SHA1(paraStr);
                if (sig.Equals(signature))
                {
                    return echostr;
                }
            } catch (Exception ex)
            {
                ClassLoger.Error("WeChatSigController.GET",ex);
            }
            return echostr;
        }
        [HttpPost]
        [ActionName("Index")]
        public string POST(string signature, string token, string timestamp, string nonce, string echostr)
        {
            try
            {
                Stream requestStream = System.Web.HttpContext.Current.Request.InputStream;
                byte[] requestByte = new byte[requestStream.Length];
                requestStream.Read(requestByte, 0, (int)requestStream.Length);
                string requestStr = Encoding.UTF8.GetString(requestByte);

                ClassLoger.Info("checkWeChatSignatureController接收到的信息:", requestStr);
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(requestStr);

                var msg = WeChatXMLMessageHelper.GetWeChatMsg(xd);
                if (msg != null)
                {
                    WxTextMsg msgs = new WxTextMsg();
                    msgs.ToUserName = msg.FromUserName;
                    msgs.FromUserName = msg.ToUserName;
                    msgs.CreateTime = DateTime.Now.Ticks;
                    msgs.MsgType = "text";
                    //订阅服务号
                    if (msg.Event.Equals(MsgTypeEvent.subscribe))
                    {
                        WeChatUserInfo uinfo = WeChatUserInfoAPI.GetWeChatUserInfo(msg.FromUserName);
                        WeChatUser weuser = userbll.GetWeChatUserByUnionID(uinfo.unionid);
                        if (weuser == null)
                        {
                            weuser = new WeChatUser();
                        }
                        weuser.ServiceOpenID = uinfo.openid;
                        weuser.UnionID = uinfo.unionid;
                        userbll.UpdateWeChatUser(weuser);

                        UserInfo user = ubll.GetUserInfoByOpenID(uinfo.unionid);
                        List<string> openIDList = new List<string>(1) { weuser.ServiceOpenID };
                        if (user == null)
                        {
                            user = new UserInfo();
                            user.Openid = uinfo.unionid;
                            user.CreateTime = DateTime.Now;
                            user.Headimgurl = uinfo.headimgurl;
                            user.Nickname = uinfo.nickname;
                            user.Sex = (SexEnum)uinfo.sex;
                            user.Name = uinfo.nickname;
                            user.city = uinfo.city;
                            user.province = uinfo.province;
                            ubll.UpdateUserinfo(user);
                        }
                        else
                        {
                            if (user.IsAdmin)
                            {
                                WeChatTagsAPI.batchtagging(openIDList, SystemSet.AdminUserTagID);
                            }
                        }
                        msgs.Content = "感谢您关注【....】";
                    }
                    return XmlEntityExchange<WxTextMsg>.ConvertEntity2Xml(msgs);
                }
            } catch (Exception ex)
            {
                ClassLoger.Error("WeChatSig.POST",ex);
            }
            return string.Empty;
        }
    }
}