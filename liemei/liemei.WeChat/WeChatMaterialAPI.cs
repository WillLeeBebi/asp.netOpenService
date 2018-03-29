using liemei.Common.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.WeChat
{
    /// <summary>
    /// 素材管理
    /// </summary>
    public class WeChatMaterialAPI
    {
        /// <summary>
        /// 获取所有素材
        /// </summary>
        /// <returns></returns>
        public static string GetmaterialList()
        {
            string jsonresult = string.Empty;
            try
            {
                string access_token = WeChatAccessTokenAPI.GetWeChatAccess_token();
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token={0}", access_token);
                string postjson = "{\"type\":\"news\",\"offset\":0,\"count\":10}";
                jsonresult = HttpUtils.Ins.POST(url, postjson);
            } catch (Exception ex)
            {
                ClassLoger.Error("WeChatMaterialAPI.GetmaterialList",ex);
            }
            return jsonresult;
        }
    }
}
