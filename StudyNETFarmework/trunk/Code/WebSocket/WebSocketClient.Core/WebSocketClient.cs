using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketClient.Core
{
    public delegate void OnReceiveMessage(WebSocketClient webSocketClient, string message);
    public delegate void OnCloseWebSocketClient(WebSocketClient webSocketClient, WebSocketCloseStatus webSocketCloseStatus, string statusDescription);
    public class WebSocketClient:IDisposable
    {
        #region 成员属性
        public ClientWebSocket ClientWebSocket { get; private set; }
        public string Ip { get; private set; }
        public int Port { get; private set; }
        public string SubProtocol { get; private set; } = "chat";
        public string Name { get; set; }
        public Encoding Encoding { get; private set; } = Encoding.GetEncoding("gb2312");
        #endregion

        #region 私有属性
        private ILog log = LogManager.GetLogger<WebSocketClient>();
        private CancellationTokenSource CancellationTokenSource = new CancellationTokenSource(); //可以更改CancellationToken的状态
        private CancellationToken cancellationToken;
        #endregion

        #region 事件
        /// <summary>
        /// 当接收到服务器端消息时触发
        /// </summary>
        public event OnReceiveMessage OnReceiveMessage;
        /// <summary>
        /// 当WebSocket关闭时触发
        /// </summary>
        public event OnCloseWebSocketClient OnCloseWebSocketClient;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造WebSocket客户端
        /// </summary>
        /// <param name="Name">客户端名称</param>
        /// <param name="Ip">服务器IP地址</param>
        /// <param name="Port">服务器端口</param>
        /// <param name="SubProtocol">子协议</param>
        public WebSocketClient(string Name,string Ip, int Port,string SubProtocol)
        {
            this.Name = Name;
            this.Ip = Ip;
            this.Port = Port;
            this.SubProtocol = SubProtocol;
            ClientWebSocket = new ClientWebSocket();
            PrintHelper.PrintInfo("创建WebSocket客户端", log);
        }
        #endregion

        #region 成员方法
        /// <summary>
        /// 设置WebSocket编码
        /// </summary>
        /// <param name="encoding"></param>
        public void SetEncoding(Encoding encoding)
        {
            Encoding = encoding;
            Console.WriteLine($"encoding.BodyName:{encoding.BodyName}");
            Console.WriteLine($"encoding.EncodingName:{encoding.EncodingName}");
            Console.WriteLine($"encoding.WebName:{encoding.WebName}");
            Console.WriteLine($"encoding.EncodingName:{encoding.HeaderName}");
        }
        /// <summary>
        /// 打开WebSocket连接
        /// </summary>
        /// <exception cref="Exception">当无法连接到服务器时触发</exception>
        /// <returns></returns>
        public async Task OpenAsync()
        {
            if (ClientWebSocket.State == WebSocketState.Open)
            {
                PrintHelper.PrintInfo("WebSocket客户端已打开，不能再次打开", log);
                return;
            }
            if (ClientWebSocket.State == WebSocketState.Closed)
            {
                ClientWebSocket = new ClientWebSocket();
            }
            InitWebSocketOptions();
            await ClientWebSocket.ConnectAsync(new Uri($"ws://{Ip}:{Port}/"), cancellationToken);
            PrintHelper.PrintInfo("WebSocket客户端与服务器握手成功！", log);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public async Task SendMassage(string str)
        {
            if (ClientWebSocket.State != WebSocketState.Open)
            {
                PrintHelper.PrintInfo("WebSocket客户端与服务器未握手成功，不能发送消息！", log);
                return;
            }
            await ClientWebSocket.SendAsync(new ArraySegment<byte>(Encoding.GetBytes(str)), WebSocketMessageType.Binary, true, cancellationToken);
        }

        /// <summary>
        /// 异步递归接受消息
        /// </summary>
        /// <returns></returns>
        public async Task ReceiveMessageAsync()
        {
            ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(new byte[1024]);
            CancellationToken cancel = new CancellationToken();
            WebSocketReceiveResult webSocketReceiveResult;
            try
            {
                webSocketReceiveResult = await ClientWebSocket.ReceiveAsync(receiveBuffer, cancel);
            }
            catch (Exception ex)
            {
                PrintHelper.printError(ex, log);
                await CloseAsync(WebSocketCloseStatus.ProtocolError, "WebSocket 连接异常" + GetExceptionMessge(ex));
                return;
            }
            byte[] bytes = new byte[webSocketReceiveResult.Count];
            Array.Copy(receiveBuffer.Array, bytes, webSocketReceiveResult.Count);
            if (!webSocketReceiveResult.CloseStatus.HasValue)
            {
                if (OnReceiveMessage != null)
                {
                    OnReceiveMessage.Invoke(this, Encoding.GetString(bytes));
                }
                await ReceiveMessageAsync();
            }
            else
            {
                PrintHelper.PrintInfo($"连接被关闭：服务器关闭码【{webSocketReceiveResult.CloseStatus.Value}】，原因【{webSocketReceiveResult.CloseStatusDescription}】", log);
                await CloseAsync(webSocketReceiveResult.CloseStatus.Value, webSocketReceiveResult.CloseStatusDescription);
            }
        }

        /// <summary>
        /// 关闭WebSocket
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync(WebSocketCloseStatus webSocketCloseStatus, string message)
        {
            if (ClientWebSocket.State == WebSocketState.Open || ClientWebSocket.State == WebSocketState.CloseReceived)
            {
                string statusDescription = message;
                statusDescription = statusDescription.Length > 61 ? statusDescription.Substring(0, 61) : statusDescription;
                PrintHelper.PrintInfo($"关闭WebSocket", log);
                await ClientWebSocket.CloseAsync(webSocketCloseStatus, statusDescription, cancellationToken);
            }
            if (ClientWebSocket.State != WebSocketState.Closed && OnCloseWebSocketClient != null)
                OnCloseWebSocketClient.Invoke(this, webSocketCloseStatus, message);
            CancellationTokenSource.Cancel();
        }

        public string GetWebSocketStateDescr()
        {
            switch (ClientWebSocket.State)
            {
                case WebSocketState.None:
                    return "None 保留将来使用";
                case WebSocketState.Aborted:
                    return "Aborted 保留将来使用";
                case WebSocketState.Closed:
                    return " WebSocket 关闭握手正常完成。";
                case WebSocketState.CloseReceived:
                    return "从远端接收到关闭消息";
                case WebSocketState.CloseSent:
                    return "关闭消息发送到远端";
                case WebSocketState.Connecting:
                    return "连接正在协商与远程终结点在握手。";
                case WebSocketState.Open:
                    return "在 HTTP 握手完成后，初始状态。";
                default:
                    return $"未知状态【{ClientWebSocket.State}】";
            }
        }
        #endregion

        protected virtual void InitWebSocketOptions()
        {
            ClientWebSocket.Options.AddSubProtocol(SubProtocol);
            ClientWebSocket.Options.SetRequestHeader("Sec-WebSocket-Name", Name);
            ClientWebSocket.Options.SetRequestHeader("charset", Encoding.HeaderName);
            //ClientWebSocket.Options.SetRequestHeader("charset", Encoding.EncodingName);
        }

        private string GetExceptionMessge(Exception ex)
        {
            string message = ex.Message;
            if (ex.InnerException != null)
                message += " => " + GetExceptionMessge(ex.InnerException);
            return message;
        }

        public void Dispose()
        {
            CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "客户端关闭！").Wait(); ;
            ClientWebSocket.Dispose();
            ClientWebSocket = null;
        }
    }
}
