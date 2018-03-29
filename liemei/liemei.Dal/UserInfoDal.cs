using liemei.Common.common;
using liemei.Model;
using liemei.Model.EnumModel;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Dal
{
    public class UserInfoDal
    {
        private static UserInfoDal _ins;

        public static UserInfoDal Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new UserInfoDal();
                return _ins;
            }
        }


        public string AddUserInfo(UserInfo _userinfo)
        {
            string id = string.Empty;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                session.Save(_userinfo);
                transaction.Commit();
                session.Close();
                id = _userinfo.ID;
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.AddUserInfo", ex);
            }
            return id;
        }

        public string UpdateUserinfo(UserInfo _userinfo)
        {
            string id = string.Empty;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                session.SaveOrUpdate(_userinfo);
                transaction.Commit();
                session.Close();
                id = _userinfo.ID;
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.UpdateUserinfo", ex);
            }
            return id;
        }
        /// <summary>
        /// 根据用户微信ID获取用户信息
        /// </summary>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByOpenID(string OpenID)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                IList<UserInfo> wlist = session.QueryOver<UserInfo>().And(m => m.Openid == OpenID && m.State==0).List();
                session.Close();
                if (wlist != null)
                    return wlist.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.GetWeChatLoginByUUID", ex);
            }
            return null;
        }
        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public UserInfo GetUserinfoByID(string userid)
        {
            try
            {
                if (userid.IsNull())
                {
                    return null;
                }
                ISession session = NHibernateSessionFactory.getSession();
                UserInfo a = session.Get<UserInfo>(userid);
                session.Close();
                if (a!=null &&  a.State == -1)
                    return null;
                return a;
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.GetUserinfoByID", ex);
            }
            return null;
        }
        /// <summary>
        /// 根据企业ID获取最高级管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserInfo GetAdminUserByEnterpriseID(string id)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                IList<UserInfo> wlist = session.QueryOver<UserInfo>().And(m => m.EnterpriseID==id && m.IsHighestAdmin==true && m.State==0).OrderBy(x=>x.CreateTime).Desc.List();
                session.Close();
                if (wlist != null)
                    return wlist.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.GetAdminUserByEnterpriseID", ex);
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
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                IList<UserInfo> wlist = session.QueryOver<UserInfo>().And(m => m.IDNum == IDNum && m.State == 0).List();
                session.Close();
                if (wlist != null)
                    return wlist.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.GetAdminUserByEnterpriseID", ex);
            }
            return null;
        }

        /// <summary>
        /// 注册密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        public void RegisterPassword(string username,string password)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                string sql = string.Format("update UserInfo set Password='{0}' where UserName='{1}';", getPassword(password), username);
                session.CreateSQLQuery(sql).AddEntity(typeof(UserInfo)).ExecuteUpdate();
                session.Flush();
                transaction.Commit();
                session.Close();
            }catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.RegisterPassword",ex);
            }
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="tel">手机号</param>
        /// <param name="password">新密码</param>
        public void ResetPassword(string tel,string password)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                string sql = string.Format("update UserInfo set Password='{0}' where Telephone='{1}';", getPassword(password), tel);
                session.CreateSQLQuery(sql).AddEntity(typeof(UserInfo)).ExecuteUpdate();
                session.Flush();
                transaction.Commit();
                session.Close();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.ResetPassword", ex);
            }
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        public void ResetPasswordByID(string id,string password)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                string sql = string.Format("update UserInfo set Password='{0}' where ID='{1}';", getPassword(password), id);
                session.CreateSQLQuery(sql).AddEntity(typeof(UserInfo)).ExecuteUpdate();
                session.Flush();
                transaction.Commit();
                session.Close();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.ResetPasswordByID", ex);
            }
        }
        /// <summary>
        /// 用户名密码登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserInfo UserLogin(string userName,string password)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                string sql = string.Format("select * from  UserInfo  where State=0 and  Password='{0}' and  UserName='{1}';", getPassword(password), userName);
                IList<UserInfo> wlist = session.CreateSQLQuery(sql).AddEntity(typeof(UserInfo)).List<UserInfo>();
                if (wlist == null || wlist.Count==0)
                {
                    sql = string.Format("select * from  UserInfo  where State=0 and   Password='{0}' and  Telephone='{1}';", getPassword(password), userName);
                    wlist = session.CreateSQLQuery(sql).AddEntity(typeof(UserInfo)).List<UserInfo>();
                }
                if (wlist == null || wlist.Count == 0)
                {
                    sql = string.Format("select * from  UserInfo  where State=0 and  Password='{0}' and  Email='{1}';", getPassword(password), userName);
                    wlist = session.CreateSQLQuery(sql).AddEntity(typeof(UserInfo)).List<UserInfo>();
                }
                session.Flush();
                transaction.Commit();
                session.Close();
                if (wlist != null)
                    return wlist.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.UserLogin", ex);
            }
            return null;
        }

        /// <summary>
        /// 判断用户名是否已经存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool ExistsUserName(string username)
        {
            bool flag = false;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                flag = session.QueryOver<UserInfo>().Where(m => m.UserName == username).RowCount()>0;
                session.Close();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.ExistsUserName", ex);
            }
            return flag;
        }

        public UserInfo GetByUserName(string username)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                var userlist = session.QueryOver<UserInfo>().Where(m => m.UserName == username && m.State==0).OrderBy(x=>x.CreateTime).Desc.List();
                session.Close();
                if (userlist != null && userlist.Count > 0)
                    return userlist.First();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.ExistsUserName", ex);
            }
            return null;
        }

        public bool ExistsUserName(string userid,string username)
        {
            bool flag = false;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                flag = session.QueryOver<UserInfo>().Where(m => m.UserName == username && m.ID!=userid).RowCount() > 0;
                session.Close();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.ExistsUserName", ex);
            }
            return flag;
        }
        /// <summary>
        /// 判断手机号是否已注册
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public bool ExistsTel(string tel)
        {
            bool flag = false;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                flag = session.QueryOver<UserInfo>().Where(m => m.Telephone == tel).RowCount() > 0;
                session.Close();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.ExistsTel", ex);
            }
            return flag;
        }
        public bool ExistsTel(string userid,string tel)
        {
            bool flag = false;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                flag = session.QueryOver<UserInfo>().Where(m => m.Telephone == tel && m.ID!=userid).RowCount() > 0;
                session.Close();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.ExistsTel", ex);
            }
            return flag;
        }
        /// <summary>
        /// 判断邮箱是否已经注册过
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool ExistsEmail(string email)
        {
            bool flag = false;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                flag = session.QueryOver<UserInfo>().Where(m => m.Email == email).RowCount() > 0;
                session.Close();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.ExistsEmail", ex);
            }
            return flag;
        }
        public bool ExistsEmail(string userid,string email)
        {
            bool flag = false;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                flag = session.QueryOver<UserInfo>().Where(m => m.Email == email && m.ID!=userid).RowCount() > 0;
                session.Close();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.ExistsEmail", ex);
            }
            return flag;
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="_userinfo"></param>
        public void DelUserinfo(UserInfo _userinfo)
        {
            try
            {
                _userinfo.State = -1;
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                session.Update(_userinfo);
                transaction.Commit();
                session.Close();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.AddUserInfo", ex);
            }
        }
        /// <summary>
        /// 处理原始密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string getPassword(string password)
        {
            return Utils.MD5(string.Format("{0}_!@#$%^&_xxx.com", password));
        }

        public IList<UserInfo> GetSystemUserinfo()
        {
            try
            {
                using (ISession session = NHibernateSessionFactory.getSession())
                {
                    var list = session.QueryOver<UserInfo>().Where(x => x.IsSystemAdmin == true && x.State==0).OrderBy(x => x.CreateTime).Asc.List();
                    return list;
                }
            } catch (Exception ex)
            {
                ClassLoger.Error("UserInfoDal.GetSystemUserinfo",ex);
            }
            return null;
        }
        
    }
}
