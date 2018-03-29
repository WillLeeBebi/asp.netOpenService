using liemei.Service.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using liemei.Common.Models;
using liemei.Bll;
using liemei.Service.Models;
using liemei.Model;
using liemei.Service.Models.ViewModel;
using liemei.Common.common;
using liemei.Common;
using System.IO;
using System.Text;
using liemei.Common.cache;
using System.Threading.Tasks;
using liemei.Service.WeChatAPIHelper;
using liemei.WeChat;
using liemei.Model.EnumModel;

namespace liemei.Service.Controllers.API
{
    /// <summary>
    /// 用户账户相关接口
    /// </summary>
    [RoutePrefix("api/account")]
    [DExceptionFilterAttribute]
    [ApiActionFilterAttribute]
    public class AccountController : ApiController
    {
        /// <summary>
        /// 获取验证码，有效时间10分钟
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("vercode")]
        public async Task<IHttpActionResult> VerCodePic()
        {
            JsonResult<VerCodePicViewModel> result = new JsonResult<VerCodePicViewModel>();
            result.code = 1;
            result.msg = "OK";
            await Task.Run(()=> {
                try
                {
                    ClassLoger.Info("VerCodePic", "开始获取成语");
                    cy_dictBll cybll = new cy_dictBll();
                    IList<cy_dict> cylist = cybll.GetAllcy_dict();
                    ClassLoger.Info("VerCodePic", cylist.Count.ToString());
                    int i = Utils.GetRandom(0, cylist.Count - 1);
                    ClassLoger.Info("VerCodePic", i.ToString());
                    cy_dict cy = cylist[i];
                    ClassLoger.Info("VerCodePic成语：", cy.chengyu);
                    VerCodePicViewModel vcvm = new VerCodePicViewModel();

                    string sourcePic = FileHelper.GetVerCodePicResource();
                    if (sourcePic.IsNull() || !File.Exists(sourcePic))
                    {
                        sourcePic = @"E:\WebResources\images\VerCodePicSource\1.jpg";
                    }
                    ClassLoger.Info("VerCodePic图片", sourcePic);
                    VerCodePic codepic = FileHelper.GetVerCodePic(cy.chengyu, sourcePic);
                    vcvm.content = cy.chengyu;
                    vcvm.MainPic = codepic.PicURL;
                    result.Result = vcvm;

                    string key = cookieKey();
                    RedisBase.Item_Set(key, codepic);
                    RedisBase.ExpireEntryAt(key, DateTime.Now.AddMinutes(10));
                    result.ResultMsg = key;
                }
                catch (Exception ex)
                {
                    ClassLoger.Error("AccountController.VerCodePic", ex);
                    result.code = -1;
                    result.msg = "AccountController.VerCodePic发生异常:" + ex.Message;
                }
            });
            return Ok(result);
        }
        /// <summary>
        /// 发送短信接口
        /// </summary>
        /// <param name="sendSmsCode"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("smscode")]
        public async Task<IHttpActionResult> SendSmSCode([FromBody]SendSMSCodeViewModel sendSmsCode)
        {
            JsonResult<string> result = new JsonResult<string>();
            result.code = 1;
            result.msg = "OK";
            if (sendSmsCode == null)
            {
                result.code = 0;
                result.msg = "参数错误";
                return Ok(result);
            }
            string requestTimeskey = "AccountController.SendSmSCode";
            double times = RedisBase.SortedSet_ZSCORE<string>(requestTimeskey, sendSmsCode.token);
            //if (times > 0)
            //{
            //    result.code = 0;
            //    result.msg = "验证码已失效";
            //    return Ok(result);
            //}
            
            if (string.IsNullOrEmpty(sendSmsCode.token) || !RedisBase.ContainsKey(sendSmsCode.token))
            {
                result.code = 0;
                result.msg = "验证码已过期";
                return Ok(result);
            }
            if (sendSmsCode.type==0 && !sendSmsCode.tel.IsMobile())
            {
                result.code = 0;
                result.msg = "invalid tel";
                return Ok(result);
            }
            await Task.Run(()=> {
                RedisBase.SortedSet_Zincrby<string>(requestTimeskey, sendSmsCode.token, 1);
                RedisBase.SortedSet_SetExpire(requestTimeskey, DateTime.Now.AddMinutes(10));

                VerCodePic codepic = RedisBase.Item_Get<VerCodePic>(sendSmsCode.token);
                if (Math.Abs(codepic.Font1.X - sendSmsCode.x1) > 0.5 || Math.Abs(codepic.Font1.Y - sendSmsCode.y1) > 0.5
                    || Math.Abs(codepic.Font2.X - sendSmsCode.x2) > 0.5 || Math.Abs(codepic.Font2.Y - sendSmsCode.y2) > 0.5
                    || Math.Abs(codepic.Font3.X - sendSmsCode.x3) > 0.5 || Math.Abs(codepic.Font3.Y - sendSmsCode.y3) > 0.5
                    || Math.Abs(codepic.Font4.X - sendSmsCode.x4) > 0.5 || Math.Abs(codepic.Font4.Y - sendSmsCode.y4) > 0.5)
                {
                    result.code = 0;
                    result.msg = "验证码错误";
                }
                else
                {
                    //产生短信验证码
                    int code = Utils.GetRandom(100000, 999999);
                    string key = cookieKey();
                    RedisBase.Item_Set(key, code.TryToString());
                    RedisBase.ExpireEntryAt(key, DateTime.Now.AddMinutes(10));
                    string tel = sendSmsCode.tel;
                    if (sendSmsCode.type==1)
                    {
                        tel = RedisBase.Item_Get<string>(sendSmsCode.tel);
                    }
                    bool isSend = Utils.SendSMSVCode(code.TryToString(), tel);
                    if (isSend)
                    {
                        result.Result = key;
                    }
                    else
                    {
                        result.code = 0;
                        result.msg = "发送失败";
                    }
                }
            });
            return Ok(result);
        }

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="tel">手机号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("sendsms")]
        [ApiActionFilterAttribute(CheckClient = ClientEnum.WindowsClient)]
        public async Task<IHttpActionResult> SendSmS(string tel)
        {
            JsonResult<string> result = new JsonResult<string>();
            result.code = 1;
            result.msg = "OK";
            if (tel.IsNull())
            {
                result.code = 0;
                result.msg = "手机号不能为空";
                return Ok(result);
            }
            if (!tel.IsMobile())
            {
                result.code = 0;
                result.msg = "请输入正确的手机号";
                return Ok(result);
            }

            //产生短信验证码
            int code = Utils.GetRandom(100000, 999999);
            string key = cookieKey();
            RedisBase.Item_Set(key, code.TryToString());
            RedisBase.ExpireEntryAt(key, DateTime.Now.AddMinutes(10));
            bool isSend = Utils.SendSMSVCode(code.TryToString(), tel);
            if (isSend)
            {
                result.Result = key;
            }
            else
            {
                result.code = 0;
                result.msg = "发送失败";
            }
            return Ok(result);
        }

        /// <summary>
        /// 校验短信验证码是否正确
        /// </summary>
        /// <param name="token"></param>
        /// <param name="telcode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("checksmscode")]
        public async Task<IHttpActionResult> CheckSmsCode(string token,string telcode)
        {
            JsonResult<string> result = new JsonResult<string>();
            if (token.IsNull() || !RedisBase.ContainsKey(token))
            {
                result.code = 0;
                result.msg = "invalid token";
                return Ok(result);
            }
            string value = RedisBase.Item_Get<string>(token);
            if (telcode.IsNull() || value != telcode)
            {
                result.code = 0;
                result.msg = "验证码错误";
                return Ok(result);
            }
            result.code = 1;
            result.msg = "验证码正确";
            return Ok(result);
        }
        /// <summary>
        /// 校验图片验证码是否正确
        /// </summary>
        /// <param name="piccode"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("checkpiccode")]
        public async Task<IHttpActionResult> CheckPicCode([FromBody]CheckPicCodeViewModel piccode)
        {
            JsonResult<bool> result = new JsonResult<bool>();
            result.code = 1;
            result.msg = "OK";
            if (piccode == null)
            {
                result.Result = false;
                result.ResultMsg = "参数错误";
                return Ok(result);
            }
            
            if (string.IsNullOrEmpty(piccode.token) || !RedisBase.ContainsKey(piccode.token))
            {
                result.Result = false;
                result.ResultMsg = "验证码已过期";
                return Ok(result);
            }
            result.Result = await Task.Run<bool>(() => {
                bool flag = false;
                VerCodePic codepic = RedisBase.Item_Get<VerCodePic>(piccode.token);
                if (Math.Abs(codepic.Font1.X - piccode.x1) > 0.5 || Math.Abs(codepic.Font1.Y - piccode.y1) > 0.5
                    || Math.Abs(codepic.Font2.X - piccode.x2) > 0.5 || Math.Abs(codepic.Font2.Y - piccode.y2) > 0.5
                    || Math.Abs(codepic.Font3.X - piccode.x3) > 0.5 || Math.Abs(codepic.Font3.Y - piccode.y3) > 0.5
                    || Math.Abs(codepic.Font4.X - piccode.x4) > 0.5 || Math.Abs(codepic.Font4.Y - piccode.y4) > 0.5)
                {
                    flag = false;
                    result.ResultMsg = "验证码错误";
                }
                else
                {
                    flag = true;
                    result.ResultMsg = "验证码正确";
                }
                return flag;
            });
            return Ok(result);
        }
        /// <summary>
        /// 判断用户信息是否完善
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("isperfect")]
        [ApiActionFilterAttribute(IsCache =true,IsLogin =true)]
        public async Task<IHttpActionResult> IsPerfect()
        {
            JsonResult<bool> result = new JsonResult<bool>();
            result.code = 1;
            result.msg = "OK";
            
            ApiUserManager userManager = new ApiUserManager(ActionContext);
            if (userManager.User.Name.IsNull() || userManager.User.BirthDate.IsNull() || userManager.User.Sex == Model.EnumModel.SexEnum.Nolimit)
            {
                result.Result = false;
            }
            else
            {
                result.Result = true;
            }
            return Ok(result);
        }

        /// <summary>
        /// 找回密码第一步验证用户名返回手机号
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("checkusername")]
        public async Task<IHttpActionResult> ResetPassword01_CheckUserName(string username)
        {
            JsonResult<string> result = new JsonResult<string>();
            result.code = 0;
            result.msg = "OK";
            if (username.IsNull())
            {
                result.Result = "用户名不能为空";
                return Ok(result);
            }
            result.Result = await Task.Run<string>(()=> {
                string tel = string.Empty;
                UserInfoBll ubll = new UserInfoBll();
                UserInfo userinfo = ubll.GetByUserName(username);
                if (userinfo!=null)
                {
                    result.code = 1;
                    result.msg = "OK";
                    if (!string.IsNullOrEmpty(userinfo.Telephone) && userinfo.Telephone.IsMobile())
                    {
                        tel = string.Format("{0}*****{1}", userinfo.Telephone.Substring(0, 3),userinfo.Telephone.Substring(8));
                        string cachekey = cookieKey();
                        RedisBase.Item_Set(cachekey,userinfo.Telephone);
                        RedisBase.ExpireEntryAt(cachekey,DateTime.Now.AddMinutes(10));
                        result.ResultMsg = cachekey;
                        return tel;
                    }
                }
                return tel;
            });
            return Ok(result);
        }

        /// <summary>
        /// 校验手机号是否已注册
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("existstel")]
        public async Task<IHttpActionResult> ExistsTel(string tel)
        {
            JsonResult<bool> result = new JsonResult<bool>();
            if (!tel.IsMobile())
            {
                result.code = 0;
                result.msg = "请输入正确的手机号";
            }
            result.code = 1;
            result.msg = "OK";
            result.Result = await Task.Run<bool>(()=>{
                bool flag = false;
                UserInfoBll userbll = new UserInfoBll();
                flag = userbll.ExistsTel(tel);
                return flag;
            });
            return Ok(result);
        }

        /// <summary>
        /// 校验邮箱是否已注册
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("existsemail")]
        public async Task<IHttpActionResult> ExistsEmail(string email)
        {
            JsonResult<bool> result = new JsonResult<bool>();
            if (!email.IsEmail())
            {
                result.code = 0;
                result.msg = "邮箱格式不正确";
                return Ok(result);
            }
            result.code = 1;
            result.msg = "OK";
            result.Result = await Task.Run<bool>(()=> {
                bool flag = false;
                UserInfoBll userbll = new UserInfoBll();
                flag = userbll.ExistsEmail(email);
                return flag;
            });
            return Ok(result);
        }

        /// <summary>
        /// 校验用户名是否已注册
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("existsusername")]
        public async Task<IHttpActionResult> ExistsUserName(string username)
        {
            JsonResult<bool> result = new JsonResult<bool>();
            if (username.IsNull())
            {
                result.code = 0;
                result.msg = "用户名不能为空";
                return Ok(result);
            }

            result.code = 1;
            result.msg = "OK";
            result.Result = await Task.Run<bool>(()=> {
                bool flag = false;
                UserInfoBll userbll = new UserInfoBll();
                flag = userbll.ExistsUserName(username);
                return flag;
            });
            return Ok(result);
        }
        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("userinfo")]
        [ApiActionFilterAttribute(IsLogin = true)]
        public async Task<IHttpActionResult> Userinfo()
        {
            JsonResult<UserinfoViewModel> result = new JsonResult<UserinfoViewModel>();
            result.code = 1;
            result.msg = "OK";
            await Task.Run(()=> {
                ApiUserManager userManager = new ApiUserManager(ActionContext);
                UserinfoViewModel uv = new UserinfoViewModel();
                uv.Age = "0";
                if (userManager.User.BirthDate!= DateTime.MaxValue && userManager.User.BirthDate!= DateTime.MinValue)
                {
                    uv.Age = (DateTime.Now.Year - userManager.User.BirthDate.Year).TryToString();
                }
                uv.BirthDate = userManager.User.BirthDate.ToString("yyyy-MM-dd");
                uv.city = userManager.User.city;
                uv.Email = userManager.User.Email;
                uv.EnterpriseID = userManager.User.EnterpriseID;
                uv.GroupID = userManager.User.GroupID;
                if (!uv.GroupID.IsNull())
                {
                    UserGroupBll groupbll = new UserGroupBll();
                    var group = groupbll.GetByID(uv.GroupID);
                    if (group!=null)
                    {
                        uv.GroupName = group.GroupName;
                    }
                }
                uv.Headimgurl = userManager.User.Headimgurl;
                uv.ID = userManager.User.ID;
                uv.IsAdmin = userManager.User.IsAdmin;
                uv.IsHighestAdmin = userManager.User.IsHighestAdmin;
                uv.IsSystemAdmin = userManager.User.IsSystemAdmin;
                uv.Name = userManager.User.Name;
                uv.Nickname = userManager.User.Nickname;
                uv.province = userManager.User.province;
                uv.Residence = userManager.User.Residence;
                uv.Sex = ((int)userManager.User.Sex).TryToString();
                uv.Telephone = userManager.User.Telephone;
                uv.UserName = userManager.User.UserName;
                uv.Openid = userManager.User.Openid;
                if (!userManager.User.IDNum.IsNull())
                {
                    uv.IDNum = $"{userManager.User.IDNum.Substring(0, 4)}**********{userManager.User.IDNum.Substring(14)}";
                }
                

                result.Result = uv;
            });
            return Ok(result);
        }
        /// <summary>
        /// 获取绑定微信的二维码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("bindwechart")]
        [ApiActionFilterAttribute(IsLogin = true)]
        public async Task<IHttpActionResult> BindingWeChartCQ()
        {
            JsonResult<string> result = new JsonResult<string>();
            result.code = 0;
            result.msg = "OK";
            await Task.Run(()=> {
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

                    //图片记录进缓存，定期清理
                    string key = CacheKey.GetQrCodeKey(DateTime.Now);
                    RedisBase.List_Add<string>(key, filePath);
                    RedisBase.List_SetExpire(key, DateTime.Now.AddDays(2));

                    ApiUserManager userManager = new ApiUserManager(ActionContext);
                    string bindkey = string.Format("bind_{0}",uuid);
                    RedisBase.Item_Set(bindkey, userManager.User);
                    RedisBase.ExpireEntryAt(bindkey,DateTime.Now.AddHours(1));
                }
            });
            return Ok(result);
        }

        string cookieKey()
        {
            return string.Format("Client-{0}-{1}", Utils.GetUnixTime(), Utils.GetRandom(10000, 99999));
        }
    }
}
