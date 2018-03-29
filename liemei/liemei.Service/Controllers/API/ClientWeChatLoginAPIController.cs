using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using liemei.Common.Models;
using liemei.Service.WeChatAPIHelper;
using liemei.Common.common;
using liemei.Common;
using liemei.Model;
using liemei.Bll;
using liemei.Service.Filters;
using liemei.Common.cache;
using liemei.Service.Models;
using System.Threading;
using liemei.WeChat;

namespace liemei.Service.Controllers.API
{
    [DExceptionFilterAttribute]
    [ApiActionFilterAttribute]
    public class ClientWeChatLoginAPIController : ApiController
    {
        WeChatLoginBll loginbll = new WeChatLoginBll();
        // GET api/<controller>
        /// <summary>
        /// 线下客户端获取微信登录二维码
        /// </summary>
        /// <returns></returns>
        public JsonResult<string> Get(string lockCode)
        {
            JsonResult<string> result = new JsonResult<string>();
            result.code = 0;
            result.msg = "OK";
            string uuid = Utils.GetWeChatUUID();
            string long_url = WeChateSiteHelper.getCRContent(uuid);
            string cqContent = WeChatAccessTokenAPI.GetShortURL(long_url);
            if (string.IsNullOrEmpty(cqContent))
            {
                cqContent = long_url;
            }
            string fileName = string.Format("{0}.png", uuid);
            string filePath = FileHelper.GetPicFilePath(fileName);
            if (QrCodeHelper.CreateImgCode(cqContent, filePath))
            {
                result.code = 1;
                result.Result = FileHelper.GetPicFileURL(fileName);
                result.ResultMsg = uuid;

                ThreadPool.QueueUserWorkItem(new WaitCallback(p=> {
                    //图片记录进缓存，定期清理
                    string key = CacheKey.GetQrCodeKey(DateTime.Now);
                    RedisBase.List_Add<string>(key, filePath);
                    RedisBase.List_SetExpire(key, DateTime.Now.AddDays(2));

                    //记录日志
                    WeChatLogin login = new WeChatLogin();
                    login.State = 0;
                    login.UUID = uuid;
                    login.LoginData = DateTime.Now.ToString("yyyy-MM-dd");
                    login.CreateTime = DateTime.Now;
                    login.LockCode = lockCode;
                    SaveWeChatLogin(login);
                }),null);
               
            }
           

            return result;
        }

        private async void SaveWeChatLogin(WeChatLogin login)
        {
            await loginbll.AddWeChatLogin(login);
        }
    }
}