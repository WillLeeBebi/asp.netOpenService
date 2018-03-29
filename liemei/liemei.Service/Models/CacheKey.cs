using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace liemei.Service.Models
{
    public class CacheKey
    {
        /// <summary>
        /// 获取微信登录二维码图片缓存KEY
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetQrCodeKey(DateTime dt)
        {
            return "QrCodeImage" + dt.ToString("yyyy-MM-dd");
        }
    }
}