using liemei.WeChat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace liemei.WeChat
{
    public class WeChatXMLMessageHelper
    {
        public static BaseWeChatXMLMessage GetWeChatMsg(XmlDocument xmldom)
        {
            string msgEvent = string.Empty;
            if (xmldom!=null)
            {
                XmlElement rootElement = xmldom.DocumentElement;
                //MsgType    
                XmlNode MsgType = rootElement.SelectSingleNode("MsgType");
                switch (MsgType.InnerText)
                {
                    case "text": //文本消息    
                        break;
                    case "image": //图片    
                        break;
                    case "location": //位置    

                        break;
                    case "link": //链接    
                        break;
                    case "event": //事件推送 支持V4.5+   
                        string MsgEvent = rootElement.SelectSingleNode("Event").InnerText;
                        if (MsgEvent.Equals(MsgTypeEvent.subscribe))
                        {
                            return getSubscribeMsgFromXML(rootElement);
                        }
                        break;
                }
            }
            return null;
        }
        /// <summary>
        /// 解析关注公共号消息
        /// </summary>
        /// <param name="rootElement"></param>
        /// <returns></returns>
        private static SubscribeMsg getSubscribeMsgFromXML(XmlElement rootElement)
        {
            if (rootElement == null)
                return null;
            try
            {
                SubscribeMsg submsg = new SubscribeMsg();
                submsg.CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;
                submsg.Event = rootElement.SelectSingleNode("Event").InnerText;
                submsg.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
                submsg.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;
                submsg.MsgType = rootElement.SelectSingleNode("MsgType").InnerText;
                //submsg.Ticket = rootElement.SelectSingleNode("Ticket").InnerText;
                submsg.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;
                return submsg;
            } catch (Exception ex) { }
            return null;
        }
    }
}
