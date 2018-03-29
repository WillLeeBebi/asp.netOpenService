using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Model
{
    /// <summary>
    /// 微信提现订单
    /// </summary>
    [Serializable]
    public class WithdrawalsOrder
    {
        public virtual string ID { get; set; }
        /// <summary>
        /// 在线订单号
        /// </summary>
        public virtual string OnlineOrder { get; set; }
        /// <summary>
        /// 总金额【分】
        /// </summary>
        public virtual int total_fee { get; set; }

        public virtual string openid { get; set; }

        public virtual string UserID { get; set; }
        /// <summary>
        /// 提现说明
        /// </summary>
        public virtual string wsdesc { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 微信订单号
        /// </summary>
        public virtual string payment_no { get; set; }

        public virtual string result_code { get; set; }

        public virtual string return_code { get; set; }

        public virtual string err_code { get; set; }

        public virtual string err_code_des { get; set; }
    }
}
