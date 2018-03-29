using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.WeChat.Model
{
    /// <summary>
    /// 标签下的用户
    /// </summary>
    [Serializable]
    public class TagUserList
    {
        /// <summary>
        /// 用户标签
        /// </summary>
        public List<string> openid_list { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>
        public int tagid { get; set; }
    }
}
