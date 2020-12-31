using Chat.Client.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Client.Core
{
    public delegate void OnReceiveMessage(ChatClient client, ReceiveMessage receiveMessage);
    public class ChatClient:IDisposable
    {
        #region 事件
        public event OnReceiveMessage OnReceiveMessage;
        #endregion

        #region 成员属性
        public WebSocketClient.Core.WebSocketClient WebSocketClient { get; private set; }
        public Guid UserId { get; private set; }
        #endregion

        #region 构造函数
        public ChatClient(Guid UserId,string Password, string Ip, int Port, string SubProtocol)
        {
            this.UserId = UserId;
            WebSocketClient = new WebSocketClient.Core.WebSocketClient(UserId.ToString(), Ip, Port, SubProtocol);
            WebSocketClient.OnReceiveMessage += WebSocketClient_OnReceiveMessage;
            WebSocketClient.SetEncoding(Encoding.UTF8);
            CredentialCache myCache = new CredentialCache();
            myCache.Add(new Uri($"http://{Ip}:{Port}/"), "Basic", new NetworkCredential(UserId.ToString(), Password));
            myCache.Add(new Uri($"http://{Ip}:{Port}/"), "Digest", new NetworkCredential(UserId.ToString(), Password));
            myCache.Add(new Uri($"ws://{Ip}:{Port}/"), "Basic", new NetworkCredential(UserId.ToString(), Password));
            myCache.Add(new Uri($"ws://{Ip}:{Port}/"), "Digest", new NetworkCredential(UserId.ToString(), Password));
            WebSocketClient.ClientWebSocket.Options.Credentials = myCache;
            WebSocketClient.ClientWebSocket.Options.Cookies=new CookieContainer();
            WebSocketClient.ClientWebSocket.Options.Cookies.Add(new Uri($"http://{Ip}:{Port}/"),new Cookie("UId", UserId.ToString()));
        }
        #endregion

        #region 私有属性
        private void WebSocketClient_OnReceiveMessage(WebSocketClient.Core.WebSocketClient webSocketClient, string message)
        {
            if (OnReceiveMessage != null)
                OnReceiveMessage.Invoke(this, Newtonsoft.Json.JsonConvert.DeserializeObject<ReceiveMessage>(message));
        }
        #endregion

        #region 成员方法

        /// <summary>
        /// 打开WebSocket连接
        /// </summary>
        /// <exception cref="Exception">当无法连接到服务器时触发</exception>
        /// <returns></returns>
        public async Task OpenAsync()
        {
            await WebSocketClient.OpenAsync();
            ThreadPool.QueueUserWorkItem(new WaitCallback(p =>
            {
                var client = p as WebSocketClient.Core.WebSocketClient;
                client.ReceiveMessageAsync().Wait();
            }), WebSocketClient);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="webSocketCloseStatus"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task CloseAsync(WebSocketCloseStatus webSocketCloseStatus, string message)
        {
            await WebSocketClient.CloseAsync(webSocketCloseStatus, message);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="receiveId">接收者ID</param>
        /// <param name="receiverType">接收者类别</param>
        /// <param name="messageType">消息类别</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public async Task SendMessageAsync(Guid receiveId,ReceiverType receiverType,MessageType messageType,string message)
        {
            var sendMessage = new SendMessage(Guid.NewGuid(), UserId, receiveId, receiverType, messageType, message);
            await WebSocketClient.SendMassage(Newtonsoft.Json.JsonConvert.SerializeObject(sendMessage));
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="receiveId">接收者ID</param>
        /// <param name="receiverType">接收者类别</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public async Task SendMessageAsync(Guid receiveId, ReceiverType receiverType, string message)
        {
            await SendMessageAsync(receiveId, receiverType, MessageType.message, message);
        }

        public async Task SendMessageReadAsync(ReceiveMessage receiveMessage)
        {
            var sendMessage = new SendMessage(receiveMessage.MessageId, receiveMessage.SenderId, UserId, receiveMessage.ReceiverType, MessageType.readStatusChange, "");
            await WebSocketClient.SendMassage(Newtonsoft.Json.JsonConvert.SerializeObject(sendMessage));
        }
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        public string GetWebSocketStateDescr()
        {
            return WebSocketClient.GetWebSocketStateDescr();
        }

        public void Dispose()
        {
            WebSocketClient.Dispose();
        }
        #endregion
    }
}
