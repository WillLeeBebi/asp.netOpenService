using liemei.Common;
using liemei.Common.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.WeChat
{
    /// <summary>
    /// 关于微信公共号菜单接口
    /// </summary>
    public class WeChatMenuAPI
    {
        /// <summary>
        /// 创建基础自定义菜单
        /// </summary>
        /// <returns></returns>
        public static bool CreateBaseMenu()
        {
            string access_token = WeChatAccessTokenAPI.GetWeChatAccess_token();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", access_token);
            string json = SystemSet.BaseMenu;
            string resultJson = HttpUtils.Ins.POST(url,json);
            Dictionary<string, object> reslut = JsonHelper.DeserializeObject(resultJson);
            if (reslut["errcode"].TryToInt(0) == 0)
            {
                return true;
            }
            else
            {
                ClassLoger.Fail("WeChatMenuAPI.CreateBaseMenu",resultJson);
            }
            return false;
        }
        /// <summary>
        /// 创建管理员微信公共号菜单
        /// </summary>
        /// <returns></returns>
        public static bool CreateAdminMenu()
        {
            string access_token = WeChatAccessTokenAPI.GetWeChatAccess_token();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/addconditional?access_token={0}", access_token);
            string json = SystemSet.AdminMenu;
            string resultJson = HttpUtils.Ins.POST(url, json);
            Dictionary<string, object> reslut = JsonHelper.DeserializeObject(resultJson);
            ClassLoger.Info("WeChatMenuAPI.CreateAdminMenu", resultJson);
            if (resultJson.Contains("errcode"))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 创建普通用户菜单
        /// </summary>
        /// <returns></returns>
        public static bool CreateUserMenu()
        {
            string access_token = WeChatAccessTokenAPI.GetWeChatAccess_token();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/addconditional?access_token={0}", access_token);
            string json = SystemSet.UserMenu;
            string resultJson = HttpUtils.Ins.POST(url, json);
            ClassLoger.Info("WeChatMenuAPI.CreateUserMenu", resultJson);
            Dictionary<string, object> reslut = JsonHelper.DeserializeObject(resultJson);
            if (resultJson.Contains("errcode"))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 创建演示用户菜单
        /// </summary>
        /// <returns></returns>
        public static bool CreateDemoMenu()
        {
            string access_token = WeChatAccessTokenAPI.GetWeChatAccess_token();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/addconditional?access_token={0}", access_token);
            string json = SystemSet.DemoMenu;
            string resultJson = HttpUtils.Ins.POST(url, json);
            ClassLoger.Info("WeChatMenuAPI.CreateDemoMenu", resultJson);
            Dictionary<string, object> reslut = JsonHelper.DeserializeObject(resultJson);
            if (resultJson.Contains("errcode"))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 删除自定义菜单
        /// </summary>
        /// <returns></returns>
        public static bool DeleteAllMenu()
        {
            string access_token = WeChatAccessTokenAPI.GetWeChatAccess_token();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", access_token);
           
            string resultJson = HttpUtils.Ins.GET(url);
            ClassLoger.Info("WeChatMenuAPI.DeleteAllMenu", resultJson);
            Dictionary<string, object> reslut = JsonHelper.DeserializeObject(resultJson);
            if (reslut["errcode"].TryToInt(0) != 0)
            {
                return false;
            }
            return true;
        }
    }
}
