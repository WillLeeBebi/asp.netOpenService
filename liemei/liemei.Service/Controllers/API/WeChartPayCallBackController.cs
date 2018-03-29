using liemei.Bll;
using liemei.Common;
using liemei.Common.common;
using liemei.Common.Models;
using liemei.Model;
using liemei.Service.MqttClientHelper;
using liemei.WeChat;
using liemei.WeChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace liemei.Service.Controllers
{
    /// <summary>
    /// 微信支付回调接口
    /// </summary>
    public class WeChartPayCallBackController : Controller
    {
        [HttpPost]
        public void Index()
        {
            try
            {
                WxPayData notifyData = GetNotifyData(Request, Response);
                ClassLoger.Info("WeChartPayCallBack.Index", notifyData.ToJson());
                //检查支付结果中transaction_id是否存在
                if (!notifyData.IsSet("transaction_id"))
                {
                    //若transaction_id不存在，则立即返回结果给微信支付后台
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "支付结果中微信订单号不存在");
                    ClassLoger.Fail("WeChartPayCallBackController.Index:支付结果中微信订单号不存在", notifyData.ToXml());
                    Response.Write(res.ToXml());
                    Response.End();
                }
                string transaction_id = notifyData.GetValue("transaction_id").ToString();
                //查询订单，判断订单真实性
                if (!QueryOrder(transaction_id))
                {
                    //若订单查询失败，则立即返回结果给微信支付后台
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "订单查询失败");
                    ClassLoger.Fail(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                    Response.Write(res.ToXml());
                    Response.End();
                }//查询订单成功
                else
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(p=> {
                        string out_trade_no = notifyData.GetValue("out_trade_no").ToString();
                        PayOrderBll porderBll = new PayOrderBll();
                        PayOrder porder = porderBll.GetByOnlineOrder(out_trade_no);
                        string mqttmsg = string.Empty;
                        if (notifyData.GetValue("result_code").ToString() == "SUCCESS")
                        {
                            porder.PayState = Model.EnumModel.PayStateEnum.Paid;
                            mqttmsg = MqttAgreement.GetPsychtestWeChartPayState(out_trade_no, true);
                        }
                        else
                        {
                            porder.PayState = Model.EnumModel.PayStateEnum.Fail;
                            if (notifyData.IsSet("err_code_des"))
                                porder.PayStateMsg = notifyData.GetValue("err_code_des").TryToString();
                            mqttmsg = MqttAgreement.GetPsychtestWeChartPayState(out_trade_no, false);
                        }
                        porderBll.UpdatePayOrder(porder);
                        MqttPublishClient.Ins.PublishOneUser(porder.ID, mqttmsg);
                    }),null);

                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "SUCCESS");
                    res.SetValue("return_msg", "OK");
                    ClassLoger.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
                    Response.Write(res.ToXml());
                    Response.End();
                }
            } catch (Exception ex)
            {
                ClassLoger.Error("WeChartPayCallBackController.Index",ex);
            }

        }

        private WxPayData GetNotifyData(HttpRequestBase Request, HttpResponseBase Response)
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream s = Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                ClassLoger.Error("WeChartPayCallBackController.GetNotifyData:Sign check error", res.ToXml());
                Response.Write(res.ToXml());
                Response.End();
            }
            return data;
        }
      
        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="transaction_id"></param>
        /// <returns></returns>
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
