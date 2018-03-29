using liemei.Common;
using liemei.Common.common;
using liemei.Common.Models;
using liemei.Service.Filters;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace liemei.Service.Controllers.API
{
    /// <summary>
    /// 分享统一接口
    /// </summary>
    [RoutePrefix("api/share")]
    [DExceptionFilterAttribute]
    [ApiActionFilterAttribute]
    public class ShareAPIController : ApiController
    {
        /// <summary>
        /// 获取微信分享二维码图片接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("evaluation")]
        public async Task<IHttpActionResult> EvaluationResultShare(string id)
        {
            JsonResult<string> result = new JsonResult<string>();
            result.code = 1;
            result.msg = "OK";
            await Task.Run(()=> {
                string filename = string.Format("ER{0}.png", id);
                string filepath = Path.Combine(SystemSet.ResourcesPath, SystemSet.QrCodePic, filename);
                if (!File.Exists(filepath))
                {
                    string dir = Path.GetDirectoryName(filepath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string qrcode = ShareQrCodeContent.EvaluationResultShareQrCodeContent(id);
                    QrCodeHelper.CreateImgCode(qrcode, filepath);
                }
                result.Result = string.Format("{0}/{1}/{2}", SystemSet.WebResourcesSite, SystemSet.QrCodePic, filename);
            });
            return Ok(result);
        }

    }
}