using liemei.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace liemei.Service.MqttClientHelper
{
    public class MqttPublishClient
    {
        private MqttClient _client;
        private static MqttPublishClient _Ins;
        public static MqttPublishClient Ins
        {
            get
            {
                if (_Ins == null)
                    _Ins = new MqttPublishClient();
                return _Ins;
            }
        }
        public MqttPublishClient()
        {
            //创建客户端
            _client = _client = new MqttClient(SystemSet.MqttServiceIP);
            _client.MqttMsgPublished += new MqttClient.MqttMsgPublishedEventHandler(_client_MqttMsgPublished);
            _client.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(_client_MqttMsgPublishReceived);
            _client.MqttMsgSubscribed += new MqttClient.MqttMsgSubscribedEventHandler(_client_MqttMsgSubscribed);
            _client.MqttMsgUnsubscribed += new MqttClient.MqttMsgUnsubscribedEventHandler(_client_MqttMsgUnsubscribed);
            
            //链接服务器
            string clientId = "WebServiec" + Guid.NewGuid().ToString();
            _client.Connect(clientId, SystemSet.MqttUserName, SystemSet.MqttPassword);
        }
        public void PublishAllClient(string msg)
        {
            //订阅主题/频道
            _client.Subscribe(new string[] { SubjectItems.AllCustomer.ToString() }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            //在该主题/频道中发送消息
            _client.Publish(SubjectItems.AllCustomer.ToString(), Encoding.UTF8.GetBytes(msg));
        }
        /// <summary>
        /// 向一组客户端推送消息【放弃的方法，不兼容web】
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="msg"></param>
        public void PublishGroupClient(string groupID,string msg)
        {
            string strSub = string.Format("Group|{1}",groupID);
            //订阅主题/频道
            _client.Subscribe(new string[] { strSub }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            //在该主题/频道中发送消息
            _client.Publish(strSub, Encoding.UTF8.GetBytes(msg));
        }
        /// <summary>
        /// 向单独一个客户端推送消息【放弃的方法，不兼容web】
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="msg"></param>
        public void PublishOneClient(string clientID,string msg)
        {
            string strSub = string.Format("client|{0}", clientID);
            //订阅主题/频道
            _client.Subscribe(new string[] { strSub }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            //在该主题/频道中发送消息
            _client.Publish(strSub, Encoding.UTF8.GetBytes(msg));
        }
        /// <summary>
        /// 向具体用户推送消息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="msg">消息内容</param>
        public void PublishOneUser(string userid,string msg)
        {
            string strSub = string.Format("user_{0}", userid);
            //在该主题/频道中发送消息
            _client.Publish(strSub, Encoding.UTF8.GetBytes(msg));
        }
        /// <summary>
        /// 向一组用户推送消息
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="msg"></param>
        public void PublishGroupUser(string groupid,string msg)
        {
            string strSub = string.Format("userGroup_{0}", groupid);
            //在该主题/频道中发送消息
            _client.Publish(strSub, Encoding.UTF8.GetBytes(msg));
        }

        private void _client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
        }

        private void _client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
        }

        private void _client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
        }

        private void _client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
        }
    }
}