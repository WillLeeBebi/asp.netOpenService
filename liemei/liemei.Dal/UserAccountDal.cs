using liemei.Common.common;
using liemei.Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Dal
{
    public class UserAccountDal
    {
        private static UserAccountDal _Ins;
        public static UserAccountDal Ins
        {
            get
            {
                if (_Ins == null)
                    _Ins = new UserAccountDal();
                return _Ins;
            }
        }

        public string Add(UserAccount userAccount)
        {
            string id = string.Empty;
            try
            {
                using (ISession session = NHibernateSessionFactory.getSession())
                {
                    ITransaction transaction = session.BeginTransaction();
                    session.Save(userAccount);
                    transaction.Commit();
                    id = userAccount.ID;
                }
            } catch (Exception ex)
            {
                ClassLoger.Error("UserAccountDal.Add",ex);
            }
            return id;
        }

        public string Update(UserAccount userAccount)
        {
            string id = string.Empty;
            try
            {
                using (ISession session = NHibernateSessionFactory.getSession())
                {
                    ITransaction transaction = session.BeginTransaction();
                    session.SaveOrUpdate(userAccount);
                    transaction.Commit();
                    id = userAccount.ID;
                }
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserAccountDal.Add", ex);
            }
            return id;
        }

        public UserAccount Get(string username,string password)
        {
            try
            {
                using (ISession session = NHibernateSessionFactory.getSession())
                {
                    string pd = getPassword(password);
                    var list = session.QueryOver<UserAccount>().Where(x=>x.UserName == username && x.Password == pd).List();
                    if (list!=null && list.Count>0)
                    {
                        return list.First();
                    }
                }
            } catch (Exception ex)
            {
                ClassLoger.Error("UserAccountDal.Get",ex);
            }
            return null;
        }
        /// <summary>
        /// 根据面部识别令牌获取用户账户信息
        /// </summary>
        /// <param name="faceToken"></param>
        /// <returns></returns>
        public UserAccount Get(string faceToken)
        {
            try
            {
                using (ISession session = NHibernateSessionFactory.getSession())
                {
                    var list = session.QueryOver<UserAccount>().Where(x => x.FaceToken==faceToken).List();
                    if (list != null && list.Count > 0)
                    {
                        return list.First();
                    }
                }
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserAccountDal.Get", ex);
            }
            return null;
        }

        public UserAccount GetByUserID(string userid)
        {
            try
            {
                using (ISession session = NHibernateSessionFactory.getSession())
                {
                    var list = session.QueryOver<UserAccount>().Where(x => x.UserID == userid).List();
                    if (list != null && list.Count > 0)
                    {
                        return list.First();
                    }
                }
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserAccountDal.Get", ex);
            }
            return null;
        }


        private string getPassword(string password)
        {
            return Utils.MD5(string.Format("{0}_!@#$%^&_xxxx.com", password));
        }

       
    }
}
