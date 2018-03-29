using liemei.Common.common;
using liemei.Model;
using liemei.Service.Filters;
using liemei.Service.WeChatAPIHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace liemei.Service.Models
{
    public abstract class BaseController : Controller
    {
        private RedisSession<UserInfo> _RSession;
        /// <summary>
        /// 当前session
        /// </summary>
        public RedisSession<UserInfo> RSession
        {
            get
            {
                if (_RSession == null)
                {

                    _RSession = new RedisSession<UserInfo>(this.HttpContext, true, 120);
                }
                return _RSession;
            }
        }
        /// <summary>
        /// 当前用户
        /// </summary>
        public UserInfo User
        {
            get
            {
                return RSession["UserCode"];
            }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Models.RedisSession<UserInfo> redisSession = new Models.RedisSession<UserInfo>(filterContext.HttpContext, true, 120);
            if (!redisSession.IsExistKey("UserCode"))
            {
                filterContext.Result = Redirect(WeChateSiteHelper.getOauthURL(HttpUtils.Ins.UrlEncode(filterContext.HttpContext.Request.RawUrl)));
            }
        }
    }
}