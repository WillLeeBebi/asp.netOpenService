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
    /// <summary>
    /// 微信提现信息
    /// </summary>
    public class WithdrawalsOrderBll
    {
        public string AddWithdrawalsOrder(WithdrawalsOrder wo)
        {
            return WithdrawalsOrderDal.Ins.AddWithdrawalsOrder(wo);
        }

        public string UpdateWithdrawalsOrder(WithdrawalsOrder wo)
        {
            return WithdrawalsOrderDal.Ins.UpdateWithdrawalsOrder(wo);
        }
        /// <summary>
        /// 根据在线订单号获取提现订单
        /// </summary>
        /// <param name="OnlineOrder"></param>
        /// <returns></returns>
        public WithdrawalsOrder GetWithdrawalsOrderByOnlineOrder(string OnlineOrder)
        {
            return WithdrawalsOrderDal.Ins.GetWithdrawalsOrderByOnlineOrder(OnlineOrder);
        }
        /// <summary>
        ///生成在线订单号
        /// </summary>
        /// <param name="paytepe"></param>
        /// <returns></returns>
        public string GetOnlineOrder()
        {
           return string.Format("WO{0}{1}{2}", Utils.GetUnixTime(), Utils.GenPsw(16, 16), Utils.GetRandom(1000, 9999));
        }
    }
}
