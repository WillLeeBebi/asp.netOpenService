using liemei.Common.common;
using liemei.Model;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Dal
{
    public class UserWithdrawalsLogDal
    {
        private static UserWithdrawalsLogDal _Ins;
        public static UserWithdrawalsLogDal Ins
        {
            get
            {
                if (_Ins == null)
                    _Ins = new UserWithdrawalsLogDal();
                return _Ins;
            }
        }

        public string AddUserWithdrawalsLog(UserWithdrawalsLog log)
        {
            string id = string.Empty;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ITransaction transaction = session.BeginTransaction();
                session.Save(log);
                transaction.Commit();
                session.Close();
                id = log.ID;
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserWithdrawalsLogDal.AddUserWithdrawalsLog", ex);
            }
            return id;
        }
        /// <summary>
        /// 根据用户ID获取已经提现的金额
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetWithdrawalsAMT(string userid)
        {
            decimal amt = 0;
            try
            {
                ISession session = NHibernateSessionFactory.getSession();
                ICriteria criteria = session.CreateCriteria(typeof(UserWithdrawalsLog));
                criteria.Add(Expression.Eq("UserID", userid));
                criteria.SetProjection(Projections.Sum("AMT"));
                amt = criteria.UniqueResult().TryToDecimal();
                session.Close();
                return amt;
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserWithdrawalsLogDal.GetWithdrawalsAMT", ex);
            }
            return amt;
        }
        /// <summary>
        /// 获取用户提现记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IList<UserWithdrawalsLog> GetUserWithdrawalsLog(DateTime startTime,DateTime endTime, string userid,int pageIndex,int pageSize,out long count)
        {
            count = 0;
            try
            {
                using (ISession session = NHibernateSessionFactory.getSession())
                {
                    count = session.QueryOver<UserWithdrawalsLog>().Where(x=>x.UserID == userid && x.CreateTime > startTime && x.CreateTime < endTime).RowCount();

                    var list = session.QueryOver<UserWithdrawalsLog>().Where(x => x.UserID == userid && x.CreateTime > startTime && x.CreateTime < endTime)
                        .OrderBy(x => x.CreateTime).Desc
                        .Skip((pageIndex - 1) * pageSize).Take(pageSize).List();
                    return list;
                }
            }
            catch (Exception ex)
            {
                ClassLoger.Error("UserWithdrawalsLogDal.GetUserWithdrawalsLog", ex);
            }
            return null;
        }

        public IList<UserWithdrawalsLog> GetUserWithdrawalsLog(string userid)
        {
            try
            {
                using (ISession session = NHibernateSessionFactory.getSession())
                {
                    var list = session.QueryOver<UserWithdrawalsLog>().Where(x => x.UserID == userid).OrderBy(x => x.CreateTime).Desc.List();
                    return list;
                }
            } catch (Exception ex)
            {
                ClassLoger.Error("UserWithdrawalsLogDal.GetUserWithdrawalsLog", ex);
            }
            return null;
        }
    }
}
