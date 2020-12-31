using Common.Logging;
using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketService.Core
{
    public delegate void OnReceiveMessageHandler(WebSocketContext context, string message);
    public delegate void OnCloseWebSocket(WebSocketContext context);

    /// <summary>
    /// WebSocket上下文
    /// </summary>
    public class WebSocketContext : IDisposable
    {
        ILog log = LogManager.GetLogger<WebSocketContext>();

        #region 成员属性
        /// <summary>
        /// id
        /// </summary>
        public Guid ID { get; private set; }
        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Encoding { get; private set; }
        public HttpListenerWebSocketContext HttpListenerWebSocketContext { get; private set; }
        /// <summary>
        /// 接收缓冲区的大小（以字节为单位）。
        /// </summary>
        public int ReceiveBufferSize { get; set; } = 1024;
        #endregion

        #region 私有属性
        private CancellationTokenSource CancellationTokenSource = new CancellationTokenSource(); //可以更改CancellationToken的状态
        private CancellationToken cancellationToken;
        #endregion

        #region 事件
        /// <summary>
        /// 当接受到消息时触发
        /// </summary>
        public event OnReceiveMessageHandler OnReceiveMessage;
        public event OnCloseWebSocket OnCloseWebSocket;
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化WebSocket上下文
        /// </summary>
        /// <param name="listenerContext"></param>
        /// <param name="SubProtocol"></param>
        public WebSocketContext(HttpListenerContext listenerContext, string SubProtocol)
        {
            cancellationToken = CancellationTokenSource.Token;
            ID = listenerContext.Request.RequestTraceIdentifier;
            HttpListenerWebSocketContext = listenerContext.AcceptWebSocketAsync(SubProtocol).Result;
            Encoding = listenerContext.Request.ContentEncoding;
            var charset = listenerContext.Request.Headers["charset"];
            if (charset != null && charset.Length > 0)
            {
                try
                {
                    Encoding = Encoding.GetEncoding(charset);
                }
                catch (ArgumentException ex)
                {
                    CloseAsync(WebSocketCloseStatus.InvalidMessageType, ex.Message).Wait();
                    throw ex;
                }
            }
        }
        #endregion

        #region 成员方法
        /// <summary>
        /// 递归 异步接收消息
        /// </summary>
        /// <param name="httpListenerWebSocketContext"></param>
        /// <returns></returns>
        public async Task ReceiveMessageAsync()
        {
            WebSocket webSocket = HttpListenerWebSocketContext.WebSocket;

            if (webSocket.State != WebSocketState.Open)
                throw new Exception("Http未握手成功，接受消息！");
            
            var byteBuffer = WebSocket.CreateServerBuffer(ReceiveBufferSize);
            WebSocketReceiveResult receiveResult = null;
            try
            {
                receiveResult = await webSocket.ReceiveAsync(byteBuffer, cancellationToken);
            }
            catch (WebSocketException ex)
            {
                if (ex.InnerException is HttpListenerException)
                {
                    log.Error(ex);
                    await CloseAsync(WebSocketCloseStatus.ProtocolError, "客户端断开连接" + ex.Message);
                    return;
                }
                else
                {
                    log.Error(ex);
                    await CloseAsync(WebSocketCloseStatus.ProtocolError, "WebSocket 连接异常" + ex.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                await CloseAsync(WebSocketCloseStatus.ProtocolError, "客户端断开连接" + ex.Message);
                return;
            }
            if (receiveResult.CloseStatus.HasValue)
            {
                log.Info("接受到关闭消息！");
                await CloseAsync(receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription);
                return;
            }

            byte[] bytes = new byte[receiveResult.Count];
            Array.Copy(byteBuffer.Array, bytes, bytes.Length);

            string message = Encoding.GetString(bytes);
            log.Info($"{ID}接收到消息：{message}");

            if (OnReceiveMessage != null)
                OnReceiveMessage.Invoke(this, message);

            if (!cancellationToken.IsCancellationRequested)
                await ReceiveMessageAsync();
        }
        /// <summary>
        /// 递归 同步接收消息
        /// </summary>
        /// <param name="httpListenerWebSocketContext"></param>
        /// <returns></returns>
        public void ReceiveMessage()
        {
            WebSocket webSocket = HttpListenerWebSocketContext.WebSocket;

            if (webSocket.State != WebSocketState.Open)
                throw new Exception("Http未握手成功，接受消息！");
            
            var byteBuffer = WebSocket.CreateServerBuffer(ReceiveBufferSize);
            WebSocketReceiveResult receiveResult = null;
            try
            {
                receiveResult = webSocket.ReceiveAsync(byteBuffer, cancellationToken).Result;
            }
            catch (WebSocketException ex)
            {
                if (ex.InnerException is HttpListenerException)
                {
                    log.Error(ex);
                    CloseAsync(WebSocketCloseStatus.ProtocolError, "客户端断开连接" + ex.Message).Wait(TimeSpan.FromSeconds(20));
                    return;
                }
                else
                {
                    log.Error(ex);
                    CloseAsync(WebSocketCloseStatus.ProtocolError, "WebSocket 连接异常" + ex.Message).Wait(TimeSpan.FromSeconds(20));
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                CloseAsync(WebSocketCloseStatus.ProtocolError, "客户端断开连接" + ex.Message).Wait(TimeSpan.FromSeconds(20));
                return;
            }
            if (receiveResult.CloseStatus.HasValue)
            {
                log.Info("接受到关闭消息！");
                CloseAsync(receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription).Wait(TimeSpan.FromSeconds(20));
                return;
            }

            byte[] bytes = new byte[receiveResult.Count];
            Array.Copy(byteBuffer.Array, bytes, bytes.Length);

            string message = Encoding.GetString(bytes);
            log.Info($"{ID}接收到消息：{message}");

            if (OnReceiveMessage != null)
                OnReceiveMessage.Invoke(this, message);

            if (!cancellationToken.IsCancellationRequested)
                ReceiveMessage();
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(string message)
        {
            WebSocket webSocket = HttpListenerWebSocketContext.WebSocket;
            var bytes = Encoding.GetBytes(message);
            log.Info($"{ID}发送消息:{message}");
            await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Binary, true, cancellationToken);
        }
        /// <summary>
        /// 关闭WebSocket
        /// </summary>
        /// <param name="closeStatus"></param>
        /// <param name="statusDescription"></param>
        /// <returns></returns>
        public async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription)
        {
            if (HttpListenerWebSocketContext.WebSocket.State == WebSocketState.Open || HttpListenerWebSocketContext.WebSocket.State == WebSocketState.CloseReceived)
            {
                string message = statusDescription;
                message = message.Length > 61 ? message.Substring(0, 61) : message;
                log.Info($"{ID}关闭WebSocket");
                await HttpListenerWebSocketContext.WebSocket.CloseAsync(closeStatus, message, cancellationToken);
            }
            CancellationTokenSource.Cancel();
            if (OnCloseWebSocket != null)
                OnCloseWebSocket.Invoke(this);
        }
        /// <summary>
        /// 释放WebSocket的资源
        /// </summary>
        public void Dispose()
        {
            if (HttpListenerWebSocketContext.WebSocket.State == WebSocketState.Open)
            {
                CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "服务器正在关闭").Wait();
            }
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
        }
        #endregion
    }
}
