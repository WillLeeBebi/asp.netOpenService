using liemei.Model.EnumModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Model
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual string ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName { get; set; }
        /// <summary>
        /// 微信唯一ID
        /// </summary>
        public virtual string Openid { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string Nickname { get; set; }
        /// <summary>
        /// 微信用户头像
        /// </summary>
        public virtual string Headimgurl { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public virtual SexEnum Sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsAdmin { get; set; }
        /// <summary>
        /// 是否是最高级管理员
        /// </summary>
        public virtual bool IsHighestAdmin { get; set; }
        /// <summary>
        /// 是否是后台系统管理员
        /// </summary>
        public virtual bool IsSystemAdmin { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public virtual DateTime BirthDate { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Telephone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 密码单独处理，不在此处显示，仅用于注册时数据传递
        /// </summary>
        public virtual string Password { get; set; }
       
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 居住地
        /// </summary>
        public virtual string Residence { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public virtual string province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public virtual string city { get; set; }
        /// <summary>
        /// 所属企业
        /// </summary>
        public virtual string EnterpriseID { get; set; }
        /// <summary>
        /// 分组ID
        /// </summary>
        public virtual string GroupID { get; set; }
        /// <summary>
        /// 0.正常；-1.删除
        /// </summary>
        public virtual int State { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public virtual string IDNum { get; set; }
    }
}
