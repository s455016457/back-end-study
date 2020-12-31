using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketService.Config;
using WebSocketService.Events;

namespace WebSocketService
{
    /// <summary>
    /// WebSocket上下文
    /// </summary>
    public class WebSocketContext : IDisposable
    {
        /// <summary>
        /// ID 唯一标示
        /// </summary>
        public Guid ID { get; protected set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Encoding { get; protected set; }
        /// <summary>
        /// WebSocket
        /// </summary>
        public WebSocket WebSocket { get; private set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; private set; }
        /// <summary>
        /// 子协议
        /// 服务器提供的服务的标识符。
        /// 如果服务器提供多个服务，那么值应该是派生自客户端握手中给出的资源名称在GET方法的“Request-URI”<see cref="https://tools.ietf.org/html/rfc2616">[RFC2616]</see> 中。
        /// 如果请求的服务不可用，服务器必须发送一个适当的HTTP错误代码（例如404 Not Found）和中止WebSocket握手。
        /// </summary>
        public string Subprotocol { get; private set; }
        /// <summary>
        /// 扩展性
        /// </summary>
        public string Extensions { get; private set; }
        /// <summary>
        /// 起源
        /// 服务器可以使用此信息作为决定是否接受传入连接的一部分。
        /// 如果服务器不验证源，它将接受来自任何地方的连接。
        /// 如果服务器不希望接受这个连接，它必须返回一个适当的HTTP错误代码
        /// (例如。并终止本节描述的WebSocket握手。
        /// 有关详细信息，<see cref="https://tools.ietf.org/html/rfc6455#section-10">请参阅第10节</see>。
        /// </summary>
        public string Origin { get; private set; }
        /// <summary>
        /// Key 客户端握手中的头字段 包括base64编码的值
        /// </summary>
        public string Key { get; private set; }
        /// <summary>
        /// 客户端试图与之通信的WebSocket协议的版本。
        /// 如果此版本与服务器理解的版本不匹配，服务器必须中止本节中描述的WebSocket握手，
        /// 并且发送一个适当的HTTP错误代码(如需要升级426)和一个指示服务器能够理解的版本的|Sec-WebSocket-Version|报头字段。
        /// </summary>
        public string Version { get; private set; }
        /// <summary>
        /// 客户端使用的http版本
        /// </summary>
        public string ProtocolVersion { get; private set; }

        public CancellationToken cancel { get; private set; } = new CancellationToken();

        protected WebSocketContext(WebSocket webSocket, Guid id, string name, Encoding encoding)
        {
            WebSocket = webSocket;
            ID = id;
            Name = name;
            Encoding = encoding;
        }
        /// <summary>
        /// 创建WebSocket上下文
        /// </summary>
        /// <param name="httpListenerContext"></param>
        /// <returns></returns>
        public static async Task<WebSocketContext> CreateAsync(HttpListenerContext httpListenerContext)
        {
            var request = httpListenerContext.Request;
            Console.WriteLine("接收到WebSocket握手请求，ID：" + request.RequestTraceIdentifier);

            var name = request.Headers["Sec-WebSocket-Name"] ?? string.Empty;
            var Subprotocol = request.Headers["Sec-WebSocket-Protocol"] ?? string.Empty;
            var Extensions = request.Headers["Sec-WebSocket-Extensions"] ?? string.Empty;
            var Version = request.Headers["Sec-WebSocket-Version"] ?? string.Empty;

            var serviceConfig = ConfigHelper.ServiceConfig;
            var encoding = request.ContentEncoding ?? Encoding.UTF8;
            HttpListenerWebSocketContext wsContext = null;
            try
            {
                wsContext = await httpListenerContext.AcceptWebSocketAsync(serviceConfig.WebSocketSubProtocol
                    , TimeSpan.FromSeconds(serviceConfig.WebSocketPulse));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (Subprotocol.Length > 0)
                {
                    wsContext = await httpListenerContext.AcceptWebSocketAsync(Subprotocol.Split(',')[0], TimeSpan.FromSeconds(serviceConfig.WebSocketPulse));
                }
                else
                {
                    wsContext = await httpListenerContext.AcceptWebSocketAsync(null, TimeSpan.FromSeconds(serviceConfig.WebSocketPulse));
                }
                await wsContext.WebSocket.CloseAsync(WebSocketCloseStatus.ProtocolError, GetExceptionMessage(ex), new CancellationToken());
                return null;
            }

            if (!serviceConfig.Extensions.Contains(Extensions))
            {
                Console.WriteLine($"请求ID{ request.RequestTraceIdentifier}服务器不支持该扩展，服务器支持的扩展" + serviceConfig.Extensions);
                await wsContext.WebSocket.CloseAsync(WebSocketCloseStatus.MandatoryExtension, "服务器不支持该扩展，服务器支持的扩展" + serviceConfig.Extensions, new CancellationToken());
                return null;
            }
            if (!serviceConfig.Version.Contains(Version))
            {
                Console.WriteLine($"请求ID{ request.RequestTraceIdentifier}服务器不支持该版本，服务器支持的版本" + serviceConfig.Version);
                await wsContext.WebSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "服务器不支持该版本，服务器支持的版本" + serviceConfig.Version, new CancellationToken());
                return null;
            }
            var WebSocket = wsContext.WebSocket;

            var model = new WebSocketContext(WebSocket, request.RequestTraceIdentifier, name, encoding)
            {
                Subprotocol = request.Headers["Sec-WebSocket-Protocol"],
                Extensions = request.Headers["Sec-WebSocket-Extensions"],
                ResourceName = request.ServiceName,
                Version = request.Headers["Sec-WebSocket-Version"],
                ProtocolVersion = request.ProtocolVersion.ToString(2)
            };
            Console.WriteLine("创建WebSocket：" + model.ID);
            return model;
        }
        /// <summary>
        /// 接收消息
        /// </summary>
        public async Task ReceiveMessageAsync()
        {
            ArraySegment<byte> abuf = new ArraySegment<byte>(new byte[10240]);
            byte[] buf = new byte[10240];
            WebSocketReceiveResult wsdata = null;
            string message = string.Empty;
            try
            {
                try
                {
                    // 开始异步接收数据
                    wsdata = await WebSocket.ReceiveAsync(abuf, cancel);
                }
                catch (WebSocketException ex)
                {
                    if (ex.InnerException is HttpListenerException)
                    {
                        Console.WriteLine($"{ID}客户端断开连接");
                        await WebSocketCloseAsync(WebSocketCloseStatus.ProtocolError, GetExceptionMessage(ex));
                        return;
                    }
                    else
                    {
                        Console.WriteLine($"{ID}WebSocket 连接异常。原因：{GetExceptionMessage(ex)}");
                        await WebSocketCloseAsync(WebSocketCloseStatus.ProtocolError, GetExceptionMessage(ex));
                        return;
                    }
                }

                if (wsdata.CloseStatus.HasValue)
                {
                    Console.WriteLine($"{ID}接受到关闭消息");
                    await WebSocketCloseAsync(wsdata.CloseStatus.Value, wsdata.CloseStatusDescription);
                }
                else
                {
                    byte[] bRec = new byte[wsdata.Count];
                    Array.Copy(abuf.ToArray(), bRec, wsdata.Count);
                    if (WebSocketContextHandler.BeforeReceiveHandler(this, abuf))
                    {
                        message = Encoding.UTF8.GetString(bRec);
                        WebSocketContextHandler.AflterReceiveHandler(this, message);
                    }
                    Console.WriteLine($"{ID}接受到消息:{message}");
                    await ReceiveMessageAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(string message)
        {
            if (WebSocketContextHandler.BeforeSendHandler(this, message))
            {
                Console.WriteLine($"{ID}发送消息:{message}");
                await WebSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, cancel);
                WebSocketContextHandler.AflterSendHandler(this, message);
            }
        }

        /// <summary>
        /// WebSocket关闭
        /// </summary>
        /// <param name="webSocketCloseStatus">指示关闭 WebSocket 连接的原因</param>
        /// <param name="statusDescription">指定一个可读的解释为连接关闭的原因。</param>
        public async Task WebSocketCloseAsync(WebSocketCloseStatus webSocketCloseStatus, string statusDescription)
        {
            Console.WriteLine($"{ID}WebSocket关闭");
            string message = statusDescription;

            message = message.Length > 61 ? message.Substring(0, 61) : message;

            await WebSocket.CloseAsync(webSocketCloseStatus, message, new CancellationToken());
            WebSocketContextHandler.CloseWebStocketHandler(this, webSocketCloseStatus, statusDescription);
        }

        public void Dispose()
        {
            switch (WebSocket.State)
            {
                case WebSocketState.Connecting:
                case WebSocketState.Open:
                    WebSocketCloseAsync(WebSocketCloseStatus.EndpointUnavailable, "在服务器或客户端将变得不可用").Wait();
                    WebSocket.Dispose();
                    WebSocket = null;
                    break;
                case WebSocketState.CloseReceived:
                case WebSocketState.CloseSent:
                case WebSocketState.Closed:
                    WebSocket.Dispose();
                    break;
                default:
                    break;
            }
        }

        private static string GetExceptionMessage(Exception ex)
        {
            string message = ex.Message;

            if (ex.InnerException != null)
            {
                message += $" => {GetExceptionMessage(ex.InnerException)}";
            }

            return message;
        }
    }
}
