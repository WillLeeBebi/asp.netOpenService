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
    public class WeChatUserDal
    {
        private ISessionFactory sessionFactory;

        private static WeChatUserDal _Ins;
        public static WeChatUserDal Ins
        {
            get
            {
                if (_Ins == null)
                {
                    _Ins = new WeChatUserDal();
                }
                return _Ins;
            }
        }

        public string AddWeChatUser(WeChatUser user)
        {
            string id = string.Empty;

            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                session.Save(user);
                transaction.Commit();
                session.Close();
                id = user.ID;
            }
            catch (Exception ex)
            {
                ClassLoger.Error("WeChatUserDal.AddClientUpdate", ex);
            }
            return id;
        }

        public WeChatUser GetWeChatUserByUnionID(string UnionID)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                IList<WeChatUser> wlist = session.QueryOver<WeChatUser>().And(m => m.UnionID == UnionID).List();
                session.Close();
                if (wlist != null)
                    return wlist.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("WeChatUserDal.GetWeChatUserByUnionID", ex);
            }
            return null;
        }

        public void UpdateWeChatUser(WeChatUser user)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                session.SaveOrUpdate(user);
                transaction.Commit();
                session.Close();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("WeChatUserDal.UpdateWeChatUser", ex);
            }
        }
    }
}
