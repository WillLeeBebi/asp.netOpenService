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
    public class WithdrawalsOrderDal
    {
        private static WithdrawalsOrderDal _Ins;

        public static WithdrawalsOrderDal Ins
        {
            get
            {
                if (_Ins==null)
                {
                    _Ins = new WithdrawalsOrderDal();
                }
                return _Ins;
            }
        }

        public string AddWithdrawalsOrder(WithdrawalsOrder wo)
        {
            string id = string.Empty;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                session.Save(wo);
                transaction.Commit();
                session.Close();
                id = wo.ID;
            }
            catch (Exception ex)
            {
                ClassLoger.Error("WithdrawalsOrderDal.AddWithdrawalsOrder", ex);
            }
            return id;
        }

        public string UpdateWithdrawalsOrder(WithdrawalsOrder wo)
        {
            string id = string.Empty;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                session.SaveOrUpdate(wo);
                transaction.Commit();
                session.Close();
                id = wo.ID;
            }
            catch (Exception ex)
            {
                ClassLoger.Error("WithdrawalsOrderDal.UpdateWithdrawalsOrder", ex);
            }
            return id;
        }
        /// <summary>
        /// 根据在线订单号获取订单
        /// </summary>
        /// <param name="OnlineOrder"></param>
        /// <returns></returns>
        public WithdrawalsOrder GetWithdrawalsOrderByOnlineOrder(string OnlineOrder)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                IList<WithdrawalsOrder> wolist = session.QueryOver<WithdrawalsOrder>().And(m => m.OnlineOrder == OnlineOrder).List();
                session.Close();
                if (wolist != null && wolist.Count > 0)
                    return wolist.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("WithdrawalsOrderDal.GetWithdrawalsOrderByOnlineOrder", ex);
            }
            return null;
        }
    }
}
