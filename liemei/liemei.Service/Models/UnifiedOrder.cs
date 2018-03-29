using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace liemei.Service.Models
{
    [Serializable]
    public class UnifiedOrder
    {
        /// <summary>
        /// 1.微信；2.支付宝
        /// </summary>
        public int PayTyp { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public string product_id { get; set; }
        /// <summary>
        /// 商品类型：1.
        /// </summary>
        public int productType { get; set; }
        /// <summary>
        /// 支付类型:JSAPI，NATIVE，APP
        /// JSAPI：微信公共号内发起支付
        /// NATIVE：生成二维码，扫码支付
        /// APP：手机app内发起微信支付
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 客户openid，如果为微信公共号发起支付，openid必须有
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string userid { get; set; }
    }
}