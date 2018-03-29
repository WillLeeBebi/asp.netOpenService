using liemei.Common;
using liemei.Common.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace liemei.Service.WeChatAPIHelper
{
    public class WeChateSiteHelper
    {
        /// <summary>
        /// 微信扫码登录回调地址
        /// </summary>
        static string wechatCallBackapi = HttpUtils.Ins.UrlEncode("http://xxx/WeChatLoginCallBack/Index");
        /// <summary>
        /// 微信服务号授权回调地址
        /// </summary>
        static string wechatServiceCallBackApi = HttpUtils.Ins.UrlEncode("http://xxx/WeixinRedirect/CallBackRedirect");
        /// <summary>
        /// 生成二维码内容
        /// </summary>
        /// <param name="weChatUserID">自己生成的全局唯一ID</param>
        /// <param name="adminCode">管理员ID</param>
        /// <returns></returns>
        public static string getCRContent(string weChatUserID)
        {
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_login&state={2}#wechat_redirect", SystemSet.Appid, wechatCallBackapi, weChatUserID);
        }
        /// <summary>
        /// 获取微信页面授权跳转链接
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string getOauthURL(string type)
        {
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state={2}#wechat_redirect", SystemSet.Serviceappid, wechatServiceCallBackApi, type);
        }
    }
}