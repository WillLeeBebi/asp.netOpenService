using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.WeChat.Model
{
    public class MsgTypeEvent
    {
        /// <summary>
        /// 订阅
        /// </summary>
        public static string subscribe
        {
            get { return  "subscribe"; }
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        public static readonly string unsubscribe = "unsubscribe";
        /// <summary>
        /// 菜单点击
        /// </summary>
        public static readonly string CLICK = "CLICK";
        /// <summary>
        /// 二维码
        /// </summary>
        public static readonly string scan = "scan";

        /// <summary>
        /// 群发消息成功
        /// </summary>
        public static readonly string MASSSENDJOBFINISH = "MASSSENDJOBFINISH";

        /// <summary>
        /// 模板消息成功
        /// </summary>
        public static readonly string TEMPLATESENDJOBFINISH = "TEMPLATESENDJOBFINISH";
    }
}
