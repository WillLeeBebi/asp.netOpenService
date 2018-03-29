using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Model.EnumModel
{
    /// <summary>
    /// 支付类型
    /// </summary>
    public enum PayTypeEnum
    {
        /// <summary>
        /// 微信支付
        /// </summary>
        WeChart=1,
        /// <summary>
        /// 支付宝支付
        /// </summary>
        Alipay=2
    }
    public enum PayStateEnum
    {
        /// <summary>
        /// 支付失败
        /// </summary>
        Fail = -1,
        /// <summary>
        /// 未支付
        /// </summary>
        Unpaid = 0,
        /// <summary>
        /// 已付款
        /// </summary>
        Paid = 1,
    }
}
