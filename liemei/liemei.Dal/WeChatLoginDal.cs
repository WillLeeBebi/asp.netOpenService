
using liemei.Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using liemei.Common.common;

namespace liemei.Dal
{
    public class WeChatLoginDal
    {
        private ISessionFactory sessionFactory;

        private static WeChatLoginDal _ins;
        public static WeChatLoginDal Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new WeChatLoginDal();
                return _ins;
            }
        }


        /// <summary>
        /// 添加微信扫码登录记录
        /// </summary>
        /// <param name="_weChatLogin"></param>
        /// <returns></returns>
        public bool AddWeChatLogin(WeChatLogin _weChatLogin)
        {
            bool flag = true;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                session.Save(_weChatLogin);
                transaction.Commit();
                session.Close();
            } catch (Exception ex)
            {
                flag = false;
                ClassLoger.Error("WeChatLoginDal.AddWeChatLogin",ex);
                ClassLoger.Error("WeChatLoginDal.AddWeChatLogin", ex.InnerException.Message);
            }
            return flag;
        }

        public bool UpdateWeChatLogin(WeChatLogin _weChatLogin)
        {
            bool flag = true;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                session.SaveOrUpdate(_weChatLogin);
                transaction.Commit();
                session.Close();
            }
            catch (Exception ex)
            {
                flag = false;
                ClassLoger.Error("WeChatLoginDal.AddWeChatLogin", ex);
            }
            return flag;
        }
        /// <summary>
        /// 根据全局id获取登录记录
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public WeChatLogin GetWeChatLoginByUUID(string uuid)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                IList<WeChatLogin> wlist = session.QueryOver<WeChatLogin>().And(m => m.UUID == uuid).List();
                session.Close();
                if (wlist != null)
                    return wlist.FirstOrDefault();
            } catch (Exception ex)
            {
                ClassLoger.Error("WeChatLoginDal.GetWeChatLoginByUUID", ex);
            }
            return null;
        }
    }
}
