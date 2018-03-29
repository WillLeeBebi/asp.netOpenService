using liemei.Bll;
using liemei.Common.common;
using liemei.Common.Models;
using liemei.Model;
using liemei.Model.EnumModel;
using liemei.Service.Filters;
using liemei.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace liemei.Service.Controllers.API
{
    /// <summary>
    /// 客户端查看微信用户扫码登录状态的接口
    /// </summary>
    [DExceptionFilterAttribute]
    [ApiActionFilterAttribute]
    public class ClientWeChatLoginStateController : ApiController
    {
        WeChatLoginBll logBll = new WeChatLoginBll();
        UserInfoBll userinfoBll = new UserInfoBll();
        // GET: api/ClientWeChatLoginState/5
        [CustomAuthenticationFilter(IsSigna = true, Signpara = new string[] { "lockCode" })]
        public JsonResult<UserInfo> Get(string uuid)
        {
            JsonResult<UserInfo> result = new JsonResult<UserInfo>();
            result.code = 0;
            result.msg = "OK";
            WeChatLogin loginList = logBll.GetWeChatLoginByUUID(uuid);
            if (loginList != null)
            {
                UserInfo userinfo = userinfoBll.GetUserInfoByOpenID(loginList.Openid);
                if (userinfo == null)
                {
                    userinfo = new UserInfo();
                    userinfo.Headimgurl = loginList.Headimgurl;
                    userinfo.Nickname = loginList.Nickname;
                    userinfo.Openid = loginList.Openid;
                    userinfo.Sex = (SexEnum)loginList.Sex.TryToInt(0);
                    //userinfo.UserName = loginList.Nickname;
                    userinfo.ID = userinfoBll.UpdateUserinfo(userinfo);
                }
                ApiUserManager userManager = new ApiUserManager(ActionContext);
                if (userinfo.UserName.IsNull())
                {
                    userinfo.UserName = userinfo.Nickname;
                }
                result.code = 1;
                result.Result = userinfo;
                result.ResultMsg = userManager.GetUserToken(userinfo);
            }
            return result;
        }
    }
}