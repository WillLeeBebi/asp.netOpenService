using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using liemei.Common.common;
using System.Net;
using System.Net.Http;
using liemei.Common.Models;
using System.Web.Mvc;

namespace liemei.Service.Filters
{
    public class DExceptionFilterAttribute : ExceptionFilterAttribute,System.Web.Mvc.IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            ClassLoger.Error(string.Format("{0}发生异常", filterContext.HttpContext.Request.Url.TryToString()), filterContext.Exception);

            JsonResult<string> result = new JsonResult<string>();
            result.code = -1;
            result.msg = string.Format("{0}发生异常{1}", filterContext.HttpContext.Request.Url.TryToString(), filterContext.Exception.Message);
 
            filterContext.HttpContext.Response.Write(JsonHelper.SerializeObject(result));
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            ClassLoger.Error(string.Format("{0}发生异常", context.Request.RequestUri.TryToString()), context.Exception);

            JsonResult<string> result = new JsonResult<string>();
            result.code = -1;
            result.msg = string.Format("{0}发生异常{1}", context.Request.RequestUri.TryToString(), context.Exception.Message);

            //篡改Response  
            context.Response = new HttpResponseMessage(HttpStatusCode.OK);
            
            context.Response.Content = new StringContent(JsonHelper.SerializeObject(result));
        }
    }
}