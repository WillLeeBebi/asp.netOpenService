using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Model
{
    /// <summary>
    /// 用户提现日志
    /// </summary>
    [Serializable]
    public class UserWithdrawalsLog
    {
        public virtual string ID { get; set; }
        /// <summary>
        /// 提现用户ID
        /// </summary>
        public virtual string UserID { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public virtual decimal AMT { get; set; }
        /// <summary>
        /// 提现时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
    }
}
