using liemei.Bll;
using liemei.Common.cache;
using liemei.Common.common;
using liemei.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;

namespace liemei.Service.Models
{
    public class ApiUserManager
    {
        private HttpActionContext actionContext;
        public ApiUserManager(HttpActionContext actionContext)
        {
            this.actionContext = actionContext;
        }

        public ApiUserManager() { }
        private UserInfo _User;
        /// <summary>
        /// 当前用户
        /// </summary>
        public UserInfo User
        {
            get
            {
                if (_User==null)
                {
                    string key = GetKey();
                    if (!string.IsNullOrEmpty(key) && RedisBase.ContainsKey(key))
                    {
                        _User = RedisBase.Item_Get<UserInfo>(key);
                    }
                }
                return _User;
            }
        }

        string GetKey()
        {
            if (actionContext.Request.Headers.Contains("Authorization"))
            {
                string base64Code = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
                //code结构为：userid-UserAgent.MD5()-随机数-时间戳
                string code = EncryptUtil.UnBase64(base64Code);
                string[] para = code.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                string key = (para[0] + para[1] + para[3]).MD5();
                return key;
            }
            return string.Empty;
        }
        public UserInfo GetUser(string token)
        {
            //code结构为：userid-UserAgent.MD5()-随机数-时间戳
            string code = EncryptUtil.UnBase64(token);
            string[] para = code.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            string key = (para[0] + para[1] + para[3]).MD5();
            if (!string.IsNullOrEmpty(key) && RedisBase.ContainsKey(key))
            {
                return RedisBase.Item_Get<UserInfo>(key);
            }
            return null;
        }
        /// <summary>
        /// 用户是否已经登录
        /// </summary>
        /// <returns></returns>
        public bool ExistsLogin()
        {
            string base64Code = string.Empty;
            if (actionContext.Request.Headers.Contains("Authorization"))
            {
                base64Code = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
            }
            if (base64Code.IsNull())
            {
                return false;
            }
            //code结构为：userid-UserAgent.MD5()-随机数-时间戳
            string code = EncryptUtil.UnBase64(base64Code);
            string[] para = code.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            if (para.Length != 4)
            {
                return false;
            }
            string key = (para[0] + para[1] + para[3]).MD5();
            if (!RedisBase.ContainsKey(key))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 用户登录返回令牌
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetUserToken(UserInfo user)
        {
            string uagin = actionContext.Request.Headers.UserAgent.TryToString().MD5();
            string rm = Utils.GenPsw(11,11);
            long time = Utils.GetUnixTime();
            string code = string.Format("{0}-{1}-{2}-{3}", user.ID, uagin, rm, time);
            string token = EncryptUtil.Base64(code);
            string key = (user.ID + uagin + time).MD5();
            RedisBase.Item_Set(key,user);
            RedisBase.ExpireEntryAt(key,DateTime.Now.AddDays(2));
            return token;
        }
        /// <summary>
        /// 刷新当前用户信息【修改用户信息后刷新用户信息到缓存中】
        /// </summary>
        public void RefreshUser()
        {
            string key = GetKey();
            if (RedisBase.ContainsKey(key))
            {
                RedisBase.Item_Set(key,User);
            }
        }
    }
}