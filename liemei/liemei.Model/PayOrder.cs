using liemei.Model.EnumModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Model
{
    /// <summary>
    /// 支付订单
    /// </summary>
    [Serializable]
    public class PayOrder
    {
        public virtual string ID { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public virtual PayTypeEnum PayType { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual string UserID { get; set; }
        /// <summary>
        /// 在线订单号
        /// </summary>
        public virtual string OnlineOrder { get; set; }
        /// <summary>
        /// 金额[单位分]
        /// </summary>
        public virtual int total_fee { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public virtual string trade_type { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public virtual string product_id { get; set; }
        /// <summary>
        /// 用户openid【trade_type=JSAPI时（即公众号支付），此参数必传，此参数为微信用户在商户对应appid下的唯一标识】
        /// </summary>
        public virtual string openid { get; set; }
        /// <summary>
        /// 微信或支付宝上订单编号【支付生成的预支付会话标识，用于后续接口调用中使用，该值有效期为2小时】
        /// </summary>
        public virtual string prepay_id { get; set; }
        /// <summary>
        /// 二维码链接
        /// </summary>
        public virtual string code_url { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public virtual PayStateEnum PayState { get; set; }
        /// <summary>
        /// 订单状态说明
        /// </summary>
        public virtual string PayStateMsg { get; set; }

        public virtual DateTime CreateTime { get; set; }
    }
}
