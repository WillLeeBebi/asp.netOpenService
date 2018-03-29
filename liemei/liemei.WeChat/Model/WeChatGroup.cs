using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.WeChat.Model
{
    /// <summary>
    /// 微信用户分组
    /// </summary>
    [Serializable]
    public class WeChatGroup
    {
        public int id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 分组内的用户数量
        /// </summary>
        public int count { get; set; }
    }
}
