using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Model
{
    /// <summary>
    /// 用户账号相关
    /// </summary>
    [Serializable]
    public class UserAccount
    {
        public virtual string ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual string UserID { get; set; }
        /// <summary>
        /// 人脸识别秘钥
        /// </summary>
        public virtual string FaceToken { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; set; }
    }
}
