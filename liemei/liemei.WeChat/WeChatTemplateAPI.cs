using liemei.Common;
using liemei.Common.common;
using liemei.WeChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.WeChat
{
    /// <summary>
    /// 微信模板消息
    /// </summary>
    public class WeChatTemplateAPI
    {
        /// <summary>
        /// 获取模板ID
        /// </summary>
        /// <param name="template_id_short">模板库中模板的编号，有“TM**”和“OPENTMTM**”等形式</param>
        /// <returns></returns>
        public static string GetTemplateID(string template_id_short)
        {
            string templateID = string.Empty;
            try
            {
                string access_token = WeChatAccessTokenAPI.GetWeChatAccess_token();
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token={0}", access_token);
                string json = "{\"template_id_short\":\"" + template_id_short + "\"}";
                string resultJson = HttpUtils.Ins.POST(url,json);
                Dictionary<string, object> reslut = JsonHelper.DeserializeObject(resultJson);
                if (reslut["errcode"].TryToInt(0) == 0)
                    templateID = reslut["template_id"].TryToString();
                else
                {
                    ClassLoger.Fail("WeChatTemplateAPI.GetTemplateID",resultJson);
                }
            } catch (Exception ex)
            {
                ClassLoger.Error("WeChatTemplateAPI.GetTemplateID",ex);
            }
            return templateID;
        }
        /// <summary>
        /// 推送模板消息
        /// </summary>
        /// <param name="templateJson">模板数据json</param>
        public static void SendTemplate(string templateJson)
        {
            string access_token = WeChatAccessTokenAPI.GetWeChatAccess_token();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", access_token);
            string resultJson = HttpUtils.Ins.POST(url,templateJson);
        }
    }
}
