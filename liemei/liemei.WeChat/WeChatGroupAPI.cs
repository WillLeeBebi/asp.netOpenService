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
    public class WeChatGroupAPI
    {
        /// <summary>
        /// 创建微信用户分组
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static WeChatGroup CreateGroup(string groupName)
        {
            try
            {
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/groups/create?access_token={0}", SystemSet.Token);
                string postJson = "{\"group\":{\"name\":\""+ groupName + "\"}}";
                string resultJson = HttpUtils.Ins.POST(url, postJson);
                if (resultJson.Contains("errcode"))
                {
                    ClassLoger.Fail("WeChatGroupAPI.CreateGroup", resultJson);
                    return null;
                }
                Dictionary<string, object> dic = JsonHelper.DeserializeObject(resultJson);
                Dictionary<string, object> groupdic = dic["group"] as Dictionary<string, object>;
                WeChatGroup group = new WeChatGroup();
                group.id = groupdic["id"].TryToInt(0);
                group.name = groupdic["name"].TryToString();
                return group;
            } catch (Exception ex)
            {
                ClassLoger.Error("WeChatGroupAPI.CreateGroup",ex);
            }
            return null;
        }
        /// <summary>
        /// 获取所有微信用户分组
        /// </summary>
        /// <returns></returns>
        public static List<WeChatGroup> GetAllGroup()
        {
            List<WeChatGroup> grouplist = new List<WeChatGroup>();
            try
            {
                string access_token = WeChatAccessTokenAPI.GetWeChatAccess_token();
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/groups/get?access_token={0}", access_token);
                string json = HttpUtils.Ins.GET(url);
                if (json.Contains("errcode"))
                {
                    ClassLoger.Fail("WeChatGroupAPI.GetAllGroup", json);
                    return null;
                }
                Dictionary<string, object> dic = JsonHelper.DeserializeObject(json);
                List<Dictionary<string, object>> groups = dic["groups"] as List<Dictionary<string, object>>;
                foreach (var group in groups)
                {
                    WeChatGroup info = new WeChatGroup();
                    info.count = group["count"].TryToInt(0);
                    info.id = group["id"].TryToInt(0);
                    info.name = group["name"].TryToString();
                    grouplist.Add(info);
                }
            } catch (Exception ex)
            {
                ClassLoger.Error("WeChatGroupAPI.GetAllGroup",ex);
            }
            return grouplist;
        }
        /// <summary>
        /// 根据用户OpenID获取用户所在的分组
        /// </summary>
        /// <param name="openID"></param>
        /// <returns></returns>
        public static int GetUserGroupID(string openID)
        {
            string access_token = WeChatAccessTokenAPI.GetWeChatAccess_token();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/groups/getid?access_token={0}", access_token);
            string json = "{\"openid\":\"" + openID + "\"}";
            string resultJson = HttpUtils.Ins.POST(url,json);
            if (resultJson.Contains("errcode"))
            {
                ClassLoger.Fail("WeChatGroupAPI.GetUserGroupID",json);
                return 0;
            }
            Dictionary<string, object> result = JsonHelper.DeserializeObject(resultJson);
            return result["groupid"].TryToInt(0);
        }
        /// <summary>
        /// 把用户移动到指定分组
        /// </summary>
        /// <param name="openID">用户openid</param>
        /// <param name="groupid">分组ID</param>
        /// <returns></returns>
        public static bool UpdateUserGroup(string openID,int groupid)
        {
            string access_token = WeChatAccessTokenAPI.GetWeChatAccess_token();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token={0}", access_token);
            string json = "{\"openid\":\"" + openID + "\",\"to_groupid\":" + groupid + "}";
            string resultJson = HttpUtils.Ins.POST(url,json);
            Dictionary<string, object> reslut = JsonHelper.DeserializeObject(resultJson);
            if (reslut["errcode"].TryToInt(0) == 0)
                return true;
            else
            {
                ClassLoger.Fail("WeChatGroupAPI.UpdateUserGroup", resultJson);
            }
            return false;
        }
    }
}
