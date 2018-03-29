using liemei.Common.common;
using liemei.Dal;
using liemei.Model;
using liemei.Model.EnumModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Bll
{
    /// <summary>
    /// 支付订单操作
    /// </summary>
    public class PayOrderBll
    {
        public string AddPayOrder(PayOrder porder)
        {
            return PayOrderDal.Ins.AddPayOrder(porder);
        }
        public string UpdatePayOrder(PayOrder porder)
        {
            return PayOrderDal.Ins.UpdatePayOrder(porder);
        }
        public PayOrder GetByID(string id)
        {
            return PayOrderDal.Ins.GetByID(id);
        }
        /// <summary>
        /// 通过微信或支付宝订单ID获取订单
        /// </summary>
        /// <param name="prepay_id"></param>
        /// <returns></returns>
        public PayOrder GetByPrepay_id(string prepay_id)
        {
            return PayOrderDal.Ins.GetByPrepay_id(prepay_id);
        }
        /// <summary>
        /// 通过在线订单号获取订单
        /// </summary>
        /// <param name="onlineOrder"></param>
        /// <returns></returns>
        public PayOrder GetByOnlineOrder(string onlineOrder)
        {
            return PayOrderDal.Ins.GetByOnlineOrder(onlineOrder);
        }
        /// <summary>
        ///生成在线订单号
        /// </summary>
        /// <param name="paytepe"></param>
        /// <returns></returns>
        public string GetOnlineOrder(PayTypeEnum paytepe)
        {
            string result = string.Empty;
            switch (paytepe)
            {
                case PayTypeEnum.Alipay:
                    result = string.Format("Al{0}{1}{2}", Utils.GetUnixTime(), Utils.GenPsw(16, 16), Utils.GetRandom(1000, 9999));
                    break;
                case PayTypeEnum.WeChart:
                    result = string.Format("WX{0}{1}{2}", Utils.GetUnixTime(),Utils.GenPsw(16,16),Utils.GetRandom(1000,9999));
                    break;
            }
            return result;
        }
    }
}
