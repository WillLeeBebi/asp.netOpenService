using liemei.Bll;
using liemei.Common;
using liemei.Common.cache;
using liemei.Common.common;
using liemei.Common.Models;
using liemei.Model;
using liemei.Model.EnumModel;
using liemei.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace liemei.Service.Filters
{
    public class ApiActionFilterAttribute: ActionFilterAttribute
    {
        /// <summary>
        /// 签名参数
        /// </summary>
        public string[] Signpara { get; set; }

        public string[] Cachepara { get; set; }

        private bool _IsCache = false;
        public bool IsCache
        {
            get
            {
                return _IsCache;
            }
            set { _IsCache = value; }
        }

        private bool _IsSigna = false;
        public bool IsSigna
        {
            get { return _IsSigna; }
            set { _IsSigna = value; }
        }

        private bool _IsUrlDecode = false;
        /// <summary>
        /// 是否解码
        /// </summary>
        public bool IsUrlDecode
        {
            get { return _IsUrlDecode; }
            set { _IsUrlDecode = value; }
        }

      

        private ClientEnum _CheckClient = ClientEnum.NoCheck;
        public ClientEnum CheckClient
        {
            get { return _CheckClient; }
            set { _CheckClient = value; }
        }
        private bool _IsLogin;
        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogin
        {
            get
            {
                return _IsLogin;
            }
            set
            {
                _IsLogin = value;
            }
        }
        /// <summary>
        /// 缓存超时时间
        /// </summary>
        public int TimeOut { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            #region 接口统计
            string key = "APIRequestReport";
            string requestAPI = actionContext.Request.RequestUri.AbsolutePath;
            RedisBase.SortedSet_Zincrby(key,requestAPI,1);
            string userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(userHostAddress))
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                    userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
            }
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.UserHostAddress;
               
            }
            Task.Run(()=> {
              
            });
            #endregion

            JsonResult<string> result = new JsonResult<string>();
            if (CheckClient == ClientEnum.WindowsClient && !actionContext.Request.Headers.UserAgent.TryToString().Equals(SystemSet.WindowsClientUserAgent))
            {
                result.code = -21;
                result.msg = "illegal client";
                //filterContext.HttpContext.Response.Status = HttpStatusCode.OK;
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, result);
                return;
            }
            if (CheckClient == ClientEnum.WebClient && !actionContext.Request.Headers.UserAgent.TryToString().Equals(SystemSet.WebClientUserAgent))
            {
                result.code = -21;
                result.msg = "illegal client";
                //filterContext.HttpContext.Response.Status = HttpStatusCode.OK;
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, result);
                return;
            }
            if (IsLogin)
            {
                ApiUserManager userManager = new ApiUserManager(actionContext);
                if (!userManager.ExistsLogin())
                {
                    result.code = -22;
                    result.msg = "illegal user";
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, result);
                    return;
                }
                //如果当前用户是普通管理员且需要验证权限
               
            }
            base.OnActionExecuting(actionContext);
        }
    }
}