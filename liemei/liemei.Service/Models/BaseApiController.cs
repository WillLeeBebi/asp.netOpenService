using liemei.Model;
using liemei.Service.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace liemei.Service.Models
{
    [DExceptionFilterAttribute]
    [ApiActionFilterAttribute(IsLogin = true)]
    public class BaseApiController: ApiController
    {
        private UserInfo _User;
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public UserInfo User
        {
            get
            {
                if (_User==null)
                {
                    ApiUserManager userManager = new ApiUserManager(this.ActionContext);
                    _User = userManager.User;
                }
                return _User;
            }
        }
    }
}