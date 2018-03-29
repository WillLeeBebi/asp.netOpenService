using liemei.Dal;
using liemei.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Bll
{
    public class UserWithdrawalsLogBll
    {
        /// <summary>
        /// 添加用户提现记录
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public string AddUserWithdrawalsLog(UserWithdrawalsLog log)
        {
            return UserWithdrawalsLogDal.Ins.AddUserWithdrawalsLog(log);
        }
        /// <summary>
        /// 根据用户ID获取用户总提现金额
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetWithdrawalsAMT(string userid)
        {
            return UserWithdrawalsLogDal.Ins.GetWithdrawalsAMT(userid);
        }
        /// <summary>
        /// 获取用户提现记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IList<UserWithdrawalsLog> GetUserWithdrawalsLog(DateTime startTime, DateTime endTime,string userid, int pageIndex, int pageSize, out long count)
        {
            return UserWithdrawalsLogDal.Ins.GetUserWithdrawalsLog(startTime,endTime,userid, pageIndex,pageSize,out count);
        }

        public IList<UserWithdrawalsLog> GetUserWithdrawalsLog(string userid)
        {
            return UserWithdrawalsLogDal.Ins.GetUserWithdrawalsLog(userid);
        }
    }
}
