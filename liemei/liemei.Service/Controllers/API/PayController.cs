using liemei.Bll;
using liemei.Common;
using liemei.Common.common;
using liemei.Common.Models;
using liemei.Model;
using liemei.Model.EnumModel;
using liemei.Service.Filters;
using liemei.Service.Models;
using liemei.WeChat.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace liemei.Service.Controllers.API
{
    /// <summary>
    /// 统一在线支付接口
    /// </summary>
    [ApiActionFilterAttribute]
    public class PayController : ApiController
    {
        /// <summary>
        /// 统一下单接口
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Pay/UnifiedOrder")]
        public JsonResult<PayOrder> Post(UnifiedOrder obj)
        {
            ClassLoger.Info("PayController.Post", obj.PayTyp.ToString(), obj.product_id, obj.productType.TryToString());
            JsonResult<PayOrder> result = new JsonResult<PayOrder>();
            result.code = 1;
            result.msg = "OK";
            PayOrderBll porderbll = new PayOrderBll();
            try
            {
                if (obj.PayTyp == 1)
                {
                   
                    JsApiPay jsApiPay = new JsApiPay();
                    jsApiPay.openid = "";
                    if (!obj.openid.IsNull())
                    {
                        WeChatUserBll wuserbll = new WeChatUserBll();
                        WeChatUser wuser = wuserbll.GetWeChatUserByUnionID(obj.openid);
                        jsApiPay.openid = wuser.ServiceOpenID;
                    } else if (!obj.userid.IsNull())
                    {
                        UserInfoBll ubll = new UserInfoBll();
                        var user = ubll.GetUserinfoByID(obj.userid);
                        if (!user.Openid.IsNull())
                        {
                            WeChatUserBll wuserbll = new WeChatUserBll();
                            WeChatUser wuser = wuserbll.GetWeChatUserByUnionID(user.Openid);
                            jsApiPay.openid = wuser.ServiceOpenID;
                        }
                    }
                    jsApiPay.total_fee = 1;
                    jsApiPay.trade_type = obj.trade_type;
                    if (obj.productType == 1)
                    {
                        //查询商品信息，获取商品价格
                        double Price = 0.1;
                        jsApiPay.total_fee = (Price * 100).TryToInt();
                        jsApiPay.body = "商品名称";
                    }
                    jsApiPay.attach = "北京xx";
                    string out_trade_no = porderbll.GetOnlineOrder(PayTypeEnum.WeChart);
                    jsApiPay.out_trade_no = out_trade_no;
                    jsApiPay.product_id = obj.product_id;

                    ClassLoger.Info("api/Pay/UnifiedOrder", "total_fee", jsApiPay.total_fee.ToString());
                    WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                    PayOrder porder = new PayOrder();

                    if (unifiedOrderResult.GetValue("return_code").TryToString() == "SUCCESS" && unifiedOrderResult.GetValue("result_code").TryToString() == "SUCCESS")
                    {
                        if (unifiedOrderResult.IsSet("code_url"))
                        {
                            string code_url = unifiedOrderResult.GetValue("code_url").TryToString();
                            string filename = string.Format("WXPY{0}.png", code_url.MD5());
                            string filepath = Path.Combine(SystemSet.ResourcesPath, SystemSet.QrCodePic, filename);
                            if (!File.Exists(filepath))
                            {
                                QrCodeHelper.CreateImgCode(code_url, filepath);
                            }
                            porder.code_url = Path.Combine(SystemSet.WebResourcesSite, SystemSet.QrCodePic, filename);
                        }
                        porder.OnlineOrder = out_trade_no;
                        porder.openid = obj.openid;
                        porder.PayState = PayStateEnum.Unpaid;
                        porder.PayType = PayTypeEnum.WeChart;
                        porder.prepay_id = unifiedOrderResult.GetValue("prepay_id").TryToString();
                        porder.product_id = obj.product_id;
                        porder.total_fee = jsApiPay.total_fee;
                        porder.trade_type = obj.trade_type;
                        porder.CreateTime = DateTime.Now;
                        porder.UserID = obj.userid;
                        porder.ID = porderbll.AddPayOrder(porder);
                        result.Result = porder;
                        return result;
                    }
                    else
                    {
                        result.code = 0;
                        result.msg = unifiedOrderResult.GetValue("return_code").TryToString();
                        result.ResultMsg = unifiedOrderResult.GetValue("result_code").TryToString();
                    }
                }
            } catch (Exception ex)
            {
                result.code = -1;
                result.msg = "PayController.unifiedorder:"+ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 微信公共号内调用支付接口的签名
        /// </summary>
        /// <param name="prepay_id">统一下单接口返回的微信平台ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Pay/jssign")]
        public JsonResult<JsPaySign> Get(string prepay_id)
        {
            JsonResult<JsPaySign> result = new JsonResult<JsPaySign>();
            try
            {
                result.code = 1;
                result.msg = "OK";
                JsPaySign jssign = new JsPaySign();
                jssign.nonceStr = Utils.GenerateNonceStr();
                jssign.timestamp = Utils.GenerateTimeStamp();
                jssign.package = prepay_id;
                jssign.signType = "MD5";

                WxPayData jsApiParam = new WxPayData();
                jsApiParam.SetValue("appId", SystemSet.Serviceappid);
                jsApiParam.SetValue("timeStamp", jssign.timestamp);
                jsApiParam.SetValue("nonceStr", jssign.nonceStr);
                jsApiParam.SetValue("package", "prepay_id=" + prepay_id);
                jsApiParam.SetValue("signType", "MD5");
                jssign.paySign = jsApiParam.MakeSign();
                jssign.appid = SystemSet.Serviceappid;
                result.Result = jssign;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.msg = "PayController.PaySign发成了错误:" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 查询支付状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Pay/paystate")]
        public async Task<IHttpActionResult> QueryPayState(string id)
        {
            JsonResult<int> result = new JsonResult<int>();
            if (id.IsNull())
            {
                result.code = 0;
                result.msg = "参数错误";
                result.Result = -1;
                return Ok(result);
            }
            result.code = 1;
            result.msg = "OK";
            result.Result = 0;
            await Task.Run(()=> {
                PayOrderBll bll = new PayOrderBll();
                var order = bll.GetByID(id);
                if (order!=null)
                {
                    if (order.PayState == PayStateEnum.Fail)
                    {
                        result.Result = -1;
                        result.ResultMsg = "支付失败";
                    }
                    if (order.PayState == PayStateEnum.Paid)
                    {
                        result.Result = 1;
                        result.ResultMsg = "支付成功";
                    }
                }
            });
            return Ok(result);
        }
    }
}
