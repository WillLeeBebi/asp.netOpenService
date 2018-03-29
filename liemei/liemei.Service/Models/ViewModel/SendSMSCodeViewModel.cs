using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace liemei.Service.Models.ViewModel
{
    [Serializable]
    public class SendSMSCodeViewModel
    {
        /// <summary>
        /// 客户端令牌
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string tel { get; set; }

        public double x1 { get; set; }

        public double x2 { get; set; }

        public double x3 { get; set; }

        public double x4 { get; set; }

        public double y1 { get; set; }

        public double y2 { get; set; }

        public double y3 { get; set; }

        public double y4 { get; set; }
        /// <summary>
        /// 0.注册|登录；1.找回密码
        /// </summary>
        public int type { get; set; }
    }
    /// <summary>
    /// 校验图片验证码是否正确
    /// </summary>
    public class CheckPicCodeViewModel
    {
        /// <summary>
        /// 客户端令牌
        /// </summary>
        public string token { get; set; }

        public double x1 { get; set; }

        public double x2 { get; set; }

        public double x3 { get; set; }

        public double x4 { get; set; }

        public double y1 { get; set; }

        public double y2 { get; set; }

        public double y3 { get; set; }

        public double y4 { get; set; }
    }
    [Serializable]
    public class VerCodePicViewModel
    {
        /// <summary>
        /// 图片
        /// </summary>
        public string MainPic { get; set; }
        /// <summary>
        /// 成语内容
        /// </summary>
        public string content { get; set; }
    }
    /// <summary>
    /// 用户基本信息
    /// </summary>
    [Serializable]
    public class UserinfoViewModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 微信用户头像
        /// </summary>
        public string Headimgurl { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 是否是管理员【咨询师】
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 是否是最高级管理员
        /// </summary>
        public bool IsHighestAdmin { get; set; }
        /// <summary>
        /// 是否是后台系统管理员
        /// </summary>
        public bool IsSystemAdmin { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string BirthDate { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 居住地
        /// </summary>
        public string Residence { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }

        public string provincestr { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }

        public string citystr { get; set; }
        /// <summary>
        /// 所属企业
        /// </summary>
        public string EnterpriseID { get; set; }
        /// <summary>
        /// 分组ID
        /// </summary>
        public string GroupID { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 管理员ID
        /// </summary>
        public string AdminUserID { get; set; }
        /// <summary>
        /// 管理员名称
        /// </summary>
        public string AdminUserName { get; set; }
        /// <summary>
        /// 管理员头像
        /// </summary>
        public string AdminHeadimgurl { get; set; }

        public string Openid { get; set; }

        public string IDNum { get; set; }
    }
}