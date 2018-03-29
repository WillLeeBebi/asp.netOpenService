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
    public class WeChatTagsAPI
    {
        public static List<WeChatTags> GetAllGroup()
        {
            List<WeChatTags> grouplist = new List<WeChatTags>();
            try
            {
                string access_token = WeChatAccessTokenAPI.GetWeChatAccess_token();
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/tags/get?access_token={0}", access_token);
                string json = HttpUtils.Ins.GET(url);
                if (json.Contains("errcode"))
                {
                    ClassLoger.Fail("WeChatTagsAPI.GetAllGroup", json);
                    return null;
                }
                Dictionary<string, object> dic = JsonHelper.DeserializeObject(json);
                List<Dictionary<string, object>> groups = dic["tags"] as List<Dictionary<string, object>>;
                foreach (var group in groups)
                {
                    WeChatTags info = new WeChatTags();
                    info.count = group["count"].TryToInt(0);
                    info.id = group["id"].TryToInt(0);
                    info.name = group["name"].TryToString();
                    grouplist.Add(info);
                }
            }
            catch (Exception ex)
            {
                ClassLoger.Error("WeChatGroupAPI.GetAllGroup", ex);
            }
            return grouplist;
        }
        /// <summary>
        /// 批量为用户打标签
        /// </summary>
        /// <param name="openIDList"></param>
        /// <param name="tagid"></param>
        /// <returns></returns>
        public static bool batchtagging(List<string> openIDList,int tagid)
        {
            if (openIDList==null || openIDList.Count==0 || tagid==0)
            {
                return false;
            }
            TagUserList tlist = new TagUserList();
            tlist.openid_list = openIDList;
            tlist.tagid = tagid;
            try
            {
                string access_token = WeChatAccessTokenAPI.GetWeChatAccess_token();
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/tags/members/batchtagging?access_token={0}", access_token);
                string posJson = JsonHelper.SerializeObject(tlist);
                string resultJson = HttpUtils.Ins.POST(url, posJson);
                Dictionary<string, object> reslut = JsonHelper.DeserializeObject(resultJson);
                if (reslut["errcode"].TryToInt(0) == 0)
                    return true;
            } catch (Exception ex)
            {
                ClassLoger.Error("WeChatTagsAPI.batchtagging",ex);
            }
            return false;
        }
    }
}
