using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace liemei.Service.Controllers.API.ueditor
{
    [RoutePrefix("api/ueditor")]
    public class UEditorApiController : ApiController
    {
        public IHttpActionResult Index(string action)
        {
            switch (action)
            {
                case "config":
                    return Ok(Config.Items);
                case "uploadimage":
                    break;
            }
            return Ok();
        }
    }
}
