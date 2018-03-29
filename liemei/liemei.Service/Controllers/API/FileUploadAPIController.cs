using liemei.Common;
using liemei.Common.common;
using liemei.Service.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace liemei.Service.Controllers.API
{
    /// <summary>
    /// 文件上传接口
    /// </summary>
    [DExceptionFilterAttribute]
    [ApiActionFilterAttribute]
    public class FileUploadAPIController : ApiController
    {

        //POST api/values
       [HttpPost]
        public Task<Hashtable> Post()
        {

            // 检查是否是 multipart/form-data 
            if (!Request.Content.IsMimeMultipartContent("form-data"))
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            //文件保存目录路径 
            string SaveTempPath = Path.Combine(SystemSet.ResourcesPath, SystemSet.PicPath);
            if (!Directory.Exists(SaveTempPath))
            {
                Directory.CreateDirectory(SaveTempPath);
            }
            // 设置上传目录 
            var provider = new MultipartFormDataStreamProvider(SaveTempPath);
            var queryp = Request.GetQueryNameValuePairs();//获得查询字符串的键值集合 
            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<Hashtable>(o =>
                {
                    Hashtable hash = new Hashtable();
                    try
                    {
                        hash["code"] = 0;
                        hash["msg"] = "上传出错";
                        var file = provider.FileData[0];//provider.FormData 
                        string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                        FileInfo fileinfo = new FileInfo(file.LocalFileName);
                        //最大文件大小 
                        int maxSize = 10000000;
                        if (fileinfo.Length <= 0)
                        {
                            hash["code"] = 0;
                            hash["msg"] = "请选择上传文件。";
                        }
                        else if (fileinfo.Length > maxSize)
                        {
                            hash["code"] = 0;
                            hash["msg"] = "上传文件大小超过限制。";
                        }
                        else
                        {
                            string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                            //定义允许上传的文件扩展名 
                            String fileTypes = "gif,jpg,jpeg,png,bmp,xlsx";
                            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
                            {
                                hash["code"] = 0;
                                hash["msg"] = "上传文件扩展名是不允许的扩展名。";
                            }
                            else
                            {
                                string newFileName = Utils.GetFileMD5(file.LocalFileName);
                                string allfilename = Path.Combine(SaveTempPath, newFileName + fileExt);
                                if (Path.GetExtension(file.LocalFileName).Equals(".xlsx"))
                                {
                                    string filepath = Path.Combine(SystemSet.ResourcesPath, SystemSet.ExcelFilePath);
                                    if (!Directory.Exists(filepath))
                                        Directory.CreateDirectory(filepath);
                                    allfilename = Path.Combine(filepath, newFileName + fileExt);
                                }
                                if (!File.Exists(allfilename))
                                {
                                    fileinfo.CopyTo(allfilename, true);
                                }

                                fileinfo.Delete();
                                hash["code"] = 1;
                                hash["msg"] = "上传成功";
                                hash["picurl"] = Path.Combine(SystemSet.WebResourcesSite, SystemSet.PicPath, newFileName + fileExt);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ClassLoger.Error("FileUploadAPIController.POST", ex);
                    }
                    return hash;
                });

            return task;
        }
    }
}