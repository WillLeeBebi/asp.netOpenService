using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.WeChat.Model
{
    /// <summary>
    /// 微信公共号调用统一下单接口支付签名
    /// </summary>
    [Serializable]
    public class JsPaySign
    {
        /// <summary>
        /// 支付签名时间戳，注意微信jssdk中的所有使用timestamp字段均为小写。但最新版的支付后台生成签名使用的timeStamp字段名需大写其中的S字符
        /// </summary>
        public string timestamp { get; set; }
        /// <summary>
        /// 支付签名随机串，不长于 32 位
        /// </summary>
        public string nonceStr { get; set; }
        /// <summary>
        /// 统一支付接口返回的prepay_id参数值，提交格式如：prepay_id=***）
        /// </summary>
        public string package { get; set; }
        /// <summary>
        /// 签名方式，默认为'SHA1'，使用新版支付需传入'MD5'
        /// </summary>
        public string signType { get; set; }
        /// <summary>
        ///  支付签名
        /// </summary>
        public string paySign { get; set; }

        public string appid { get; set; }
    }
}
