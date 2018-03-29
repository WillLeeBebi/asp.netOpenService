using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Model
{
    /// <summary>
    /// 用户分组
    /// </summary>
    [Serializable]
    public class UserGroup
    {
        /// <summary>
        /// 分组ID
        /// </summary>
        public virtual string ID { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public virtual string GroupName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 上级分组ID
        /// </summary>
        public virtual string PGroupID { get; set; }
        /// <summary>
        /// 所属企业ID
        /// </summary>
        public virtual string EnterpriseID { get; set; }
    }
}
