using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Model
{
    [Serializable]
    public class WeChatUser
    {
        public virtual string ID { get; set; }
        /// <summary>
        /// 微信开放平台OPenID
        /// </summary>
        public virtual string PlatformOpenID { get; set; }
        /// <summary>
        /// 微信服务号OpenID
        /// </summary>
        public virtual string ServiceOpenID { get; set; }
        /// <summary>
        /// 微信全局ID
        /// </summary>
        public virtual string UnionID { get; set; }
    }
}
