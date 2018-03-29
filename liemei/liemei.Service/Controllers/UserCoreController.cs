using liemei.Bll;
using liemei.Common.common;
using liemei.Model;
using liemei.Model.EnumModel;
using liemei.Service.Models;
using liemei.WeChat;
using liemei.WeChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace liemei.Service.Controllers
{
    /// <summary>
    /// 个人中心
    /// </summary>
    public class UserCoreController : BaseController
    {
        UserInfoBll userbll = new UserInfoBll();
        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(User);
        }

        /// <summary>
        /// 提现
        /// </summary>
        /// <returns></returns>
        public ActionResult Withdrawals()
        {
            return View();
        }
        [HttpPost]
        public ActionResult WithdrawalsOrder()
        {
            int total_fee = Request["total_fee"].TryToInt(0);
            if (total_fee<=0)
            {
                return RedirectToAction("WithdrawalsFailed", new { msg = "提现金额不能小于0" });
            }
            //PsychtestBuyLogBll pbuylogbll = new PsychtestBuyLogBll();
            UserWithdrawalsLogBll wilbll = new UserWithdrawalsLogBll();

            //decimal PbuyAmt = pbuylogbll.GetMoneyByUserid(User.ID);
            decimal PbuyAmt = 100;//用户总金额
            decimal withAmt = wilbll.GetWithdrawalsAMT(User.ID);
            if (total_fee>(PbuyAmt- withAmt))
            {
                return RedirectToAction("WithdrawalsFailed", new { msg = "余额不足" });
            }
            WithdrawalsOrderBll wobll = new WithdrawalsOrderBll();
            string OnlineOrder = wobll.GetOnlineOrder();

            WeChatUserBll wuserbll = new WeChatUserBll();
            WeChatUser wuser = wuserbll.GetWeChatUserByUnionID(User.Openid);
            A:
            WxPayData pdata = new WxPayData();
            pdata.SetValue("partner_trade_no", OnlineOrder);
            pdata.SetValue("openid", wuser.ServiceOpenID);
            pdata.SetValue("check_name", "NO_CHECK");
            pdata.SetValue("amount", total_fee*100);
            pdata.SetValue("desc", "提现");
            pdata.SetValue("spbill_create_ip", "192.168.0.1");

            WxPayData result = WxPayApi.transfers(pdata);
            WithdrawalsOrder wo = new Model.WithdrawalsOrder();
            wo.CreateTime = DateTime.Now;
            wo.OnlineOrder = OnlineOrder;
            wo.openid = User.Openid;
            wo.total_fee = total_fee * 100;
            wo.UserID = User.ID;
            wo.wsdesc = "提现";


            if (result.GetValue("return_code").TryToString() != "SUCCESS")
            {
                ClassLoger.Fail("UserCoreController.WithdrawalsOrder用户提现失败", result.GetValue("return_msg").TryToString(), result.GetValue("err_code").TryToString());

                // 系统繁忙，请稍后再试错误。使用原单号以及原请求参数重试，否则可能造成重复支付等资金风险
                if (result.GetValue("err_code").TryToString()== "SYSTEMERROR")
                {
                    goto A;
                }
                wo.result_code = result.GetValue("result_code").TryToString();
                wo.return_code = result.GetValue("return_code").TryToString();
                wobll.AddWithdrawalsOrder(wo);
                return RedirectToAction("WithdrawalsFailed", new { msg = "提现失败" });
            }
            wo.payment_no = result.GetValue("payment_no").TryToString();
            wo.result_code = result.GetValue("result_code").TryToString();
            wo.return_code = result.GetValue("return_code").TryToString();
            wobll.AddWithdrawalsOrder(wo);

            UserWithdrawalsLog uwlog = new UserWithdrawalsLog();
            uwlog.AMT = total_fee;
            uwlog.CreateTime = DateTime.Now;
            uwlog.UserID = User.ID;

            wilbll.AddUserWithdrawalsLog(uwlog);
            return RedirectToAction("WithdrawalsSuccess");
        }
        /// <summary>
        /// 提现成功提示
        /// </summary>
        /// <returns></returns>
        public ActionResult WithdrawalsSuccess()
        {
            return View();
        }
        /// <summary>
        /// 提现失败提示页
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult WithdrawalsFailed(string msg)
        {
            ViewBag.msg = msg;
            return View();
        }


        /// <summary>
        /// 提现明细
        /// </summary>
        /// <returns></returns>
        public ActionResult WithdrawalsLog()
        {
            UserWithdrawalsLogBll bll = new UserWithdrawalsLogBll();
            IList<UserWithdrawalsLog> list = bll.GetUserWithdrawalsLog(User.ID);
            return View(list);
        }
    }
}