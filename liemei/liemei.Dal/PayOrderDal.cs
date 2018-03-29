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
    public class PayOrderDal
    {
        private static PayOrderDal _Ins;
        public static PayOrderDal Ins
        {
            get
            {
                if (_Ins == null)
                    _Ins = new PayOrderDal();
                return _Ins;
            }
        }
        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="porder"></param>
        /// <returns></returns>
        public string AddPayOrder(PayOrder porder)
        {
            string id = string.Empty;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                session.Save(porder);
                transaction.Commit();
                session.Close();
                id = porder.ID;
            }
            catch (Exception ex)
            {
                ClassLoger.Error("PayOrderDal.AddPayOrder", ex);
            }
            return id;
        }

        public string UpdatePayOrder(PayOrder porder)
        {
            string id = string.Empty;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                session.Update(porder);
                transaction.Commit();
                session.Close();
                id = porder.ID;
            }
            catch (Exception ex)
            {
                ClassLoger.Error("PayOrderDal.AddPayOrder", ex);
            }
            return id;
        }

        public PayOrder GetByID(string id)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                PayOrder porder = session.Get<PayOrder>(id);
                session.Close();
                return porder;
            }
            catch (Exception ex)
            {
                ClassLoger.Error("PayOrderDal.GetByID", ex);
            }
            return null;
        }
        /// <summary>
        /// 通过微信或者支付宝线上订单号获取订单信息
        /// </summary>
        /// <param name="prepay_id"></param>
        /// <returns></returns>
        public PayOrder GetByPrepay_id(string prepay_id)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                IList<PayOrder> alist = session.QueryOver<PayOrder>().And(m => m.prepay_id == prepay_id).List();
                session.Close();
                if (alist != null && alist.Count > 0)
                    return alist.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("PayOrderDal.GetByPrepay_id", ex);
            }
            return null;
        }
        /// <summary>
        /// 通过在线订单号获取订单信息
        /// </summary>
        /// <param name="onlineOrder"></param>
        /// <returns></returns>
        public PayOrder GetByOnlineOrder(string onlineOrder)
        {
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                IList<PayOrder> alist = session.QueryOver<PayOrder>().And(m => m.OnlineOrder == onlineOrder).List();
                session.Close();
                if (alist != null && alist.Count > 0)
                    return alist.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("PayOrderDal.GetByOnlineOrder", ex);
            }
            return null;
        }
    }
}
