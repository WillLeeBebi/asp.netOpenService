using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Model
{
    /// <summary>
    /// 微信扫码登录日志
    /// </summary>
    [Serializable]
    public class WeChatLogin
    {
        public virtual string ID { get; set; }
        /// <summary>
        /// 全局唯一用户ID(临时,本次扫码登录有效)
        /// </summary>
        public virtual string UUID { get; set; }
        /// <summary>
        /// 微信用户唯一标示
        /// </summary>
        public virtual string Openid { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string Nickname { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public virtual string Sex { get; set; }
        /// <summary>
        /// 微信扫码状态0.未授权；1.已授权;-1.拒绝授权
        /// </summary>
        public virtual int State { get; set; }
        /// <summary>
        /// 微信用户头像
        /// </summary>
        public virtual string Headimgurl { get; set; }
        /// <summary>
        /// 登录的日期
        /// </summary>
        public virtual string LoginData { get; set; }

        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 客户端加密锁编码
        /// </summary>
        public virtual string LockCode { get; set; }
    }
}
