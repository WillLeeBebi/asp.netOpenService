using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace liemei.WeChat.Model
{
    //实体
    public class WxEvent
    {
        /// <summary>
        /// 接收人
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 发送人
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// 事件
        /// </summary>
        public string Event { get; set; }

        public string EventKey { get; set; }
    }

    public class WxTextMsg
    {
        /// <summary>
        /// 接收人
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 发送人
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public long CreateTime { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
    public class XmlEntityExchange<T> where T : new()
    {
        /// <summary>
        /// 将XML转换为对象
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T ConvertXml2Entity(string xml)
        {
            XmlDocument doc = new XmlDocument();
            PropertyInfo[] propinfos = null;
            doc.LoadXml(xml);
            XmlNodeList nodelist = doc.SelectNodes("/xml");
            T entity = new T();
            foreach (XmlNode node in nodelist)
            {
                //初始化propertyinfo
                if (propinfos == null)
                {
                    Type objtype = entity.GetType();
                    propinfos = objtype.GetProperties();
                }
                //填充entity类的属性
                foreach (PropertyInfo pi in propinfos)
                {
                    XmlNode cnode = node.SelectSingleNode(pi.Name);
                    pi.SetValue(entity, Convert.ChangeType(cnode.InnerText, pi.PropertyType), null);
                }
            }
            return entity;
        }

        /// <summary>
        /// 构造微信消息
        /// </summary>
        /// <param name="t">对象实体</param>
        /// <returns>返回微信消息xml格式</returns>
        public static string ConvertEntity2Xml(T t)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<xml>");
            Type objtype = t.GetType();
            //填充entity类的属性
            foreach (PropertyInfo pi in objtype.GetProperties())
            {
                object obj = pi.GetValue(t);
                string value = obj == null ? "" : obj.ToString();
                if (pi.PropertyType.Name.ToLower() == "int64")
                    builder.Append("<" + pi.Name + ">" + value + "</" + pi.Name + ">");
                else
                    builder.Append("<" + pi.Name + "><![CDATA[" + value + "]]></" + pi.Name + ">");
            }
            builder.Append("</xml>");
            return builder.ToString();
        }
    }
}
