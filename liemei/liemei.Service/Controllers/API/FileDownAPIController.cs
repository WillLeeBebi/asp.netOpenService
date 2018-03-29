using liemei.Bll;
using liemei.Common;
using liemei.Common.cache;
using liemei.Common.common;
using liemei.Model;
using liemei.Model.EnumModel;
using liemei.Service.Filters;
using liemei.Service.Models;
using liemei.Service.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace liemei.Service.Controllers.API
{
    [RoutePrefix("api/download")]
    [DExceptionFilterAttribute]
    public class FileDownAPIController : ApiController
    {
        /// <summary>
        /// 文件下载接口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("filedown")]
        [HttpGet]
        public async Task<HttpResponseMessage> Getfile(string id,string token)
        {
            
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            if (id.IsNull() || token.IsNull())
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
            ApiUserManager usermanage = new ApiUserManager();
            UserInfo user = usermanage.GetUser(token);
            if (user==null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
            try
            {
                await Task.Run(()=> {
                  
                   
                    string filename = string.Format("{0}.docx",id);
                    //string filePath = WordHelper.GetWordFilePath(filename);
                    //if (File.Exists(filePath))
                    //{
                    //    var stream = new FileStream(filePath, FileMode.Open);
                    //    response.Content = new StreamContent(stream);
                    //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    //    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    //    {
                    //        FileName = filename
                    //    };
                    //    return;
                    //}
                    //StringBuilder sb = new StringBuilder();
                    //string title = "文件名称";
                    //Tuple<string, string> re = WordHelper.CreateWord(title, sb.ToString(), filename);
                    //if (re.Item1.IsNull())
                    //{
                    //    response.StatusCode = HttpStatusCode.NotFound;
                    //    return;
                    //}
                    //var streams = new FileStream(filePath, FileMode.Open);
                    //response.Content = new StreamContent(streams);
                    //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    //{
                    //    FileName = filename
                    //};
                });
            } catch (Exception ex)
            {
                ClassLoger.Error("FileDownAPIController.GetPsychtest",ex);
            }
            return response;
        }
    }
}
