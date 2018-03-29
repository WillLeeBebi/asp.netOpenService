using liemei.Common.cache;
using liemei.Common.common;
using liemei.Model;
using liemei.Service.WeChatAPIHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace liemei.Service.Filters
{
    public class WeChartUserLoginFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Models.RedisSession<UserInfo> redisSession = new Models.RedisSession<UserInfo>(filterContext.HttpContext, true, 120);
            if (!redisSession.IsExistKey("UserCode"))
            {
                string Key = filterContext.HttpContext.Request.RawUrl.MD5();
                if (!RedisBase.ContainsKey(Key))
                {
                    RedisBase.Item_Set(Key, filterContext.HttpContext.Request.RawUrl);
                }
                filterContext.Result = new RedirectResult(WeChateSiteHelper.getOauthURL(Key));
            }
        }
    }
}