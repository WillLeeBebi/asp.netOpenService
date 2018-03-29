using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace liemei.Service.MqttClientHelper
{
    public class MqttAgreement
    {
        /// <summary>
        /// 微信登录结果通知【1.用户授权登录成功；0.用户不同意登录失败】
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="isLogin"></param>
        /// <returns></returns>
        public static string GetWeChatLoginMA(string uuid,bool isLogin)
        {
            if(isLogin)
                return string.Format("WeChatLogin|{0}|{1}", uuid, "1");
            else
                return string.Format("WeChatLogin|{0}|{1}", uuid, "0");
        }
        /// <summary>
        /// 客户端自动更新通知
        /// </summary>
        /// <param name="clientType">客户端类型</param>
        /// <returns></returns>
        public static string GetClientUpdateMA(int clientType,string id)
        {
            return string.Format("ClientUpdate|{0}|{1}",clientType,id);
        }
      
        /// <summary>
        /// 微信支付状态通知
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ispayok"></param>
        /// <returns></returns>
        public static string GetPsychtestWeChartPayState(string id ,bool ispayok)
        {
            if (ispayok)
                return string.Format("WeChartPay|{0}|1",id);
            else
                return string.Format("WeChartPay|{0}|0", id);
        }
    }

    /// <summary>
    /// 关注的主题
    /// </summary>
    public class SubjectItems
    {
        /// <summary>
        /// 所有用户
        /// </summary>
        public const string AllCustomer = "AllCustomer";
        /// <summary>
        /// 一组用户
        /// </summary>
        public const string GroupCustomer = "GroupCustomer";
        /// <summary>
        /// 单独一个用户
        /// </summary>
        public const string OneCustomer = "OneCustomer";
    }
}