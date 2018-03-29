using liemei.Common.cache;
using liemei.Common.common;
using liemei.Dal;
using liemei.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Bll
{
    public class UserInfoBll
    {
        public string RegisterUserInfo(UserInfo _userinfo)
        {
            string id = string.Empty;
            if (_userinfo.BirthDate==DateTime.MinValue || _userinfo.BirthDate== DateTime.MaxValue)
            {
                _userinfo.BirthDate = new DateTime(1990,1,1);
            }
            UserInfo _user = GetUserInfoByOpenID(_userinfo.Openid);
            if (_user != null)
            {
                id = _user.ID;
                _user.UserName = _userinfo.UserName;
                _user.Name = _userinfo.Name;
                _user.Password = _userinfo.Password;
                _user.BirthDate = _userinfo.BirthDate;
                _user.IsAdmin = _userinfo.IsAdmin;
                _user.Sex = _userinfo.Sex;
                UserInfoDal.Ins.UpdateUserinfo(_user);
            }
            else
            {
                id = UserInfoDal.Ins.AddUserInfo(_userinfo);
            }
            if (!string.IsNullOrEmpty(id))
            {
                UserInfoDal.Ins.RegisterPassword(_userinfo.UserName, _userinfo.Password);
                string key = getuserkey(id);
                _userinfo.Password = string.Empty;
                RedisBase.Item_Set(key, _userinfo);

                string openkey = getuseropenkey(_userinfo.Openid);
                RedisBase.Item_Set(openkey, _userinfo);
            }
            return id;
        }
        public string UpdateUserinfo(UserInfo _userinfo)
        {
            string id= UserInfoDal.Ins.UpdateUserinfo(_userinfo);
            if (!string.IsNullOrEmpty(id))
            {
                string key = getuserkey(id);
                if(RedisBase.ContainsKey(key))
                    RedisBase.Item_Remove(key);
                string openkey = getuseropenkey(_userinfo.Openid);
                if (RedisBase.ContainsKey(openkey))
                    RedisBase.Item_Remove(openkey);
                RedisBase.Item_Set(key, _userinfo);
                RedisBase.Item_Set(openkey, _userinfo);
            }
            return id;
        }
        public UserInfo GetUserInfoByOpenID(string OpenID)
        {
            if (string.IsNullOrEmpty(OpenID))
                return null;
            string key = getuseropenkey(OpenID);
            if (RedisBase.ContainsKey(key))
            {
                return RedisBase.Item_Get<UserInfo>(key);
            }
            else
            {
                UserInfo uinfo = UserInfoDal.Ins.GetUserInfoByOpenID(OpenID);
                if (uinfo != null)
                {
                    RedisBase.Item_Set(key, uinfo);
                }
                return uinfo;
            }
        }
        public UserInfo GetUserinfoByID(string userid)
        {
            string key = getuserkey(userid);
            if (RedisBase.ContainsKey(key))
            {
                return RedisBase.Item_Get<UserInfo>(key);
            }
            else
            {
                UserInfo uinfo = UserInfoDal.Ins.GetUserinfoByID(userid);
                if (uinfo!=null)
                {
                    RedisBase.Item_Set(key,uinfo);
                }
                return uinfo;
            }
        }

        /// <summary>
        /// 根据用户ID获取用户人脸识别令牌
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetFaceToken(string userid)
        {
            string token = string.Empty;
            UserAccount ua = UserAccountDal.Ins.GetByUserID(userid);
            if (ua!=null && !ua.FaceToken.IsNull())
            {
                return ua.FaceToken;
            }
            UserInfo user = GetUserinfoByID(userid);
            if (user!=null)
            {
                token = string.Format("{0}_liemei",user.ID).MD5();
                if (ua == null)
                    ua = new UserAccount();
                ua.UserID = userid;
                ua.UserName = user.UserName;
                ua.FaceToken = token;
                UserAccountDal.Ins.Update(ua);
            }
            return token;
        }
        /// <summary>
        /// 根据人脸识别令牌获取用户信息
        /// </summary>
        /// <param name="faceToken"></param>
        /// <returns></returns>
        public UserInfo GetUserinfoByFaceToken(string faceToken)
        {
            UserAccount ua = UserAccountDal.Ins.Get(faceToken);
            if (ua!=null)
            {
                UserInfo user = GetUserinfoByID(ua.UserID);
                return user;
            }
            return null;
        }
        /// <summary>
        /// 根据身份证号获取用户信息
        /// </summary>
        /// <param name="IDNum"></param>
        /// <returns></returns>
        public UserInfo GetUserinfoByIDNum(string IDNum)
        {
            return UserInfoDal.Ins.GetUserinfoByIDNum(IDNum);
        }

        public UserInfo UserLogin(string userName, string password)
        {
            return UserInfoDal.Ins.UserLogin(userName,password);
        }
        public UserInfo GetAdminUserByEnterpriseID(string id)
        {
            return UserInfoDal.Ins.GetAdminUserByEnterpriseID(id);
        }
        public bool ExistsUserName(string username)
        {
            return UserInfoDal.Ins.ExistsUserName(username);
        }
        public bool ExistsUserName(string userid,string username)
        {
            return UserInfoDal.Ins.ExistsUserName(userid,username);
        }
        /// <summary>
        /// 根据用户名称获取用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserInfo GetByUserName(string username)
        {
            return UserInfoDal.Ins.GetByUserName(username);
        }
        /// <summary>
        /// 判断手机号是否已注册
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public bool ExistsTel(string tel)
        {
            return UserInfoDal.Ins.ExistsTel(tel);
        }
        public bool ExistsTel(string userid,string tel)
        {
            return UserInfoDal.Ins.ExistsTel(userid,tel);
        }
        /// <summary>
        /// 判断邮箱是否已注册
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool ExistsEmail(string email)
        {
            return UserInfoDal.Ins.ExistsEmail(email);
        }
        public bool ExistsEmail(string userid,string email)
        {
            return UserInfoDal.Ins.ExistsEmail(userid,email);
        }
        public void DelUserinfo(UserInfo _userinfo)
        {
            UserInfoDal.Ins.DelUserinfo(_userinfo);
            if (!_userinfo.Openid.IsNull())
            {
                string openkey = getuseropenkey(_userinfo.Openid);
                if (RedisBase.ContainsKey(openkey))
                    RedisBase.Item_Remove(openkey);
            }
            string key = getuserkey(_userinfo.ID);
            if (RedisBase.ContainsKey(key))
                RedisBase.Item_Remove(key);
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="tel">手机号</param>
        /// <param name="password">密码</param>
        public void ResetPassword(string tel, string password)
        {
            UserInfoDal.Ins.ResetPassword(tel,password);
        }
        public void ResetPasswordByID(string id, string password)
        {
             UserInfoDal.Ins.ResetPasswordByID(id,password);
        }
      
      

        public IList<UserInfo> GetSystemUserinfo()
        {
            return UserInfoDal.Ins.GetSystemUserinfo();
        }
        #region cache key
        string getuserkey(string userid)
        {
            return string.Format("UserInfoBll/getuserkey/{0}", userid).MD5(); ;
        }

        string getuseropenkey(string openid)
        {
            return string.Format("UserInfoBll/getuseropenkey/{0}",openid);
        }
        #endregion
    }
}
