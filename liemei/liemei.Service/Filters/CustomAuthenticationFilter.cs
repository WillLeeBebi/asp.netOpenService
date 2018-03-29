using liemei.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using liemei.Common.Models;
using System.Net.Http;
using System.Net;
using liemei.Common.common;
using System.Web.Mvc;
using System.Text;

namespace liemei.Service.Filters
{
    public class CustomAuthenticationFilter : ActionFilterAttribute
    {
        public string[] Signpara { get; set; }

        private bool _IsCache=false;
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
        /// <summary>
        /// 缓存超时时间
        /// </summary>
        public int TimeOut { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            JsonResult<string> result = new JsonResult<string>();
            if (CheckClient == ClientEnum.WindowsClient && !filterContext.HttpContext.Request.UserAgent.TryToString().Equals(SystemSet.WindowsClientUserAgent))
            {
                result.code = -21;
                result.msg = "illegal user";
                //filterContext.HttpContext.Response.Status = HttpStatusCode.OK;
                filterContext.HttpContext.Response.Write(JsonHelper.SerializeObject(result));
                return;
            }
            

            if (IsUrlDecode)
            {
                ////filterContext.HttpContext.Request.Url = new Uri(HttpUtils.Ins.UrlDecode(actionContext.Request.RequestUri.TryToString()));
                //HttpRequestBase bases = (HttpRequestBase)filterContext.HttpContext.Request;
                //string url = bases.RawUrl.ToString().ToLower();
                ////获取url中的参数
                //string queryString = HttpUtils.Ins.UrlDecode(bases.QueryString.ToString()).Replace("\\u0026", "&");
                //filterContext.RequestContext.HttpContext
                ////重新填充参数
                //string paramName = "";
                //string paramValue = "";
                //var parameters = queryString.Split('&');
                //foreach (string param in parameters)
                //{
                //    paramName = param.Split('=')[0];
                //    paramValue = HttpUtility.UrlDecode(param.Split('=')[1]);

                //    filterContext.ActionParameters[paramName] = paramValue;
                //}

            }
            //if (IsSigna)
            //{
            //    Dictionary<string, object> signpara = new Dictionary<string, object>();

            //    string querys = actionContext.Request.RequestUri.Query.TrimStart('?');
            //    string signa = string.Empty;
            //    if (querys.Length > 0)
            //    {
            //        string[] param = querys.Split('&');
            //        if (param.Length > 0)
            //        {

            //            foreach (string paStr in param)
            //            {
            //                string[] pa = paStr.Split('=');
            //                if (pa[0].Equals("signa"))
            //                    signa = pa[1];
            //                else
            //                    signpara[pa[0]] = pa[1];
            //            }
            //        }
            //    }
            //    if (!signpara.ContainsKey("times") || string.IsNullOrEmpty(signa))
            //    {
            //        result.code = -2;
            //        result.msg = "signa error";
            //        actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK);
            //        actionContext.Response.Content = new StringContent(JsonHelper.SerializeObject(result));
            //        return;
            //    }
            //    long times = signpara["times"].TryToLong();
            //    if ((Utils.GetUnixTime() - times) > SystemSet.TimeOut)
            //    {
            //        result.code = -3;
            //        result.msg = "time out";
            //        actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK);
            //        actionContext.Response.Content = new StringContent(JsonHelper.SerializeObject(result));
            //        return;
            //    }
            //    string signaLocal = Utils.Signa(signpara);
            //    if (!signa.Equals(signaLocal))
            //    {
            //        result.code = -2;
            //        result.msg = "signa error";
            //        actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK);
            //        actionContext.Response.Content = new StringContent(JsonHelper.SerializeObject(result));
            //        return;
            //    }
            //}
            base.OnActionExecuting(filterContext);
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }


        //public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        //{

        //    base.OnActionExecuted(actionExecutedContext);
        //}

        //public override void OnActionExecuting(HttpActionContext actionContext)
        //{
        //    JsonResult<string> result = new JsonResult<string>();
        //    if (CheckClient == ClientEnum.WindowsClient && !actionContext.Request.Headers.UserAgent.TryToString().Equals(SystemSet.WindowsClientUserAgent))
        //    {
        //        result.code = -21;
        //        result.msg = "illegal user";
        //        actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK);
        //        actionContext.Response.Content = new StringContent(JsonHelper.SerializeObject(result));
        //        return;
        //    }
        //    if (IsUrlDecode)
        //    {
        //        actionContext.Request.RequestUri = new Uri(HttpUtils.Ins.UrlDecode(actionContext.Request.RequestUri.TryToString()));
        //    }
        //    if (IsSigna)
        //    {
        //        Dictionary<string, object> signpara = new Dictionary<string, object>();

        //        string querys = actionContext.Request.RequestUri.Query.TrimStart('?');
        //        string signa = string.Empty;
        //        if (querys.Length>0)
        //        {
        //            string[] param = querys.Split('&');
        //            if (param.Length>0)
        //            {

        //                foreach (string paStr in param)
        //                {
        //                    string[] pa = paStr.Split('=');
        //                    if (pa[0].Equals("signa"))
        //                        signa = pa[1];
        //                    else
        //                        signpara[pa[0]] = pa[1];
        //                }
        //            }
        //        }
        //        if (!signpara.ContainsKey("times") || string.IsNullOrEmpty(signa))
        //        {
        //            result.code = -2;
        //            result.msg = "signa error";
        //            actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK);
        //            actionContext.Response.Content = new StringContent(JsonHelper.SerializeObject(result));
        //            return;
        //        }
        //        long times = signpara["times"].TryToLong();
        //        if ((Utils.GetUnixTime()-times)>SystemSet.TimeOut)
        //        {
        //            result.code = -3;
        //            result.msg = "time out";
        //            actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK);
        //            actionContext.Response.Content = new StringContent(JsonHelper.SerializeObject(result));
        //            return;
        //        }
        //        string signaLocal = Utils.Signa(signpara);
        //        if (!signa.Equals(signaLocal))
        //        {
        //            result.code = -2;
        //            result.msg = "signa error";
        //            actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK);
        //            actionContext.Response.Content = new StringContent(JsonHelper.SerializeObject(result));
        //            return;
        //        }
        //    }
        //    base.OnActionExecuting(actionContext);
        //}
    }
    public enum ClientEnum
    {
        /// <summary>
        /// 不检查客户端
        /// </summary>
        NoCheck,
        WindowsClient,
        WebClient,
        AppClient
    }
}