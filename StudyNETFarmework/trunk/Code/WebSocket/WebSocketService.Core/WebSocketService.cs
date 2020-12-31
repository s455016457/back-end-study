using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Net;
using Common.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace WebSocketService.Core
{
    public delegate void OnWebSocketServiceBeginStart(WebSocketService service, HttpListener httpListener);
    public delegate void OnWebSocketServiceStart(WebSocketService service, HttpListener httpListener);
    public delegate void OnWebSocketServiceStop(WebSocketService service, HttpListener httpListener);
    public delegate void OnWebSocketServiceClose();

    public delegate bool OnAuthentication(HttpListenerContext listenerContext);
    public delegate void OnAddWebSocket(WebSocketContext webSocketContext);
    public delegate void OnRemoveWebSocket(WebSocketContext webSocketContext);
    public class WebSocketService : IDisposable
    {
        /// <summary>
        /// 预留线程数量
        /// 用于程序临时新增的线程使用
        /// 默认值为2
        /// </summary>
        public static int ReservedThreadsCount { get; set; } = 2;
        #region 私有属性
        ILog log = LogManager.GetLogger<WebSocketService>();
        IList<WebSocketContext> webSocketContexts = new List<WebSocketContext>();
        HttpListener HttpListener = null;
        CancellationTokenSource CancellationTokenSource = new CancellationTokenSource(); //可以更改CancellationToken的状态
        CancellationToken cancellationToken;
        #endregion

        #region 成员属性
        /// <summary>
        /// 监听IP地址
        /// </summary>
        public string Ip { get; private set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// 子协议
        /// </summary>
        public string SubProtocol { get; private set; }
        /// <summary>
        /// WebSocket集合
        /// </summary>
        public IReadOnlyList<WebSocketContext> WebSocketContexts
        {
            get { return webSocketContexts.ToList().AsReadOnly(); }
        }
        #endregion

        #region 事件
        /// <summary>
        /// WebSocket服务启动前触发
        /// </summary>
        public event OnWebSocketServiceBeginStart OnWebSocketServiceBeginStart;
        /// <summary>
        /// WebSocket服务启动后触发
        /// </summary>
        public event OnWebSocketServiceStart OnWebSocketServiceStart;
        /// <summary>
        /// WebSocket服务停止后触发，停止接受传入的请求
        /// </summary>
        public event OnWebSocketServiceStop OnWebSocketServiceStop;
        /// <summary>
        /// WebSocket服务关闭，释放服务资源
        /// </summary>
        public event OnWebSocketServiceClose OnWebSocketServiceClose;
        /// <summary>
        /// 当服务管理的WebSocket接收到消息时触发
        /// </summary>
        public event OnReceiveMessageHandler OnReceiveMessage;
        /// <summary>
        /// 当服务添加WebSocket时触发
        /// </summary>
        public event OnAddWebSocket OnAddWebSocket;
        /// <summary>
        /// 当服务删除WebSocket时触发
        /// </summary>
        public event OnRemoveWebSocket OnRemoveWebSocket;
        /// <summary>
        /// 身份验证
        /// </summary>
        public event OnAuthentication OnAuthentication;
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化一个WebSocket服务
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="subProtocol"></param>
        public WebSocketService(string ip, int port, string subProtocol)
        {
            log.Info($"创建WebSocket服务");
            log.Info($"当前操作系统【{Environment.OSVersion.VersionString}】");
            Ip = ip;
            Port = port;
            SubProtocol = subProtocol;
            cancellationToken = CancellationTokenSource.Token;
            if (!HttpListener.IsSupported)
                throw new Exception($"操作系统【{Environment.OSVersion.VersionString}】不支持 HTTP 协议侦听器");
            HttpListener = new HttpListener();
            PrintHttpListenerInfo(HttpListener);
            log.Info($"WebSocket服务创建成功");
        }
        #endregion

        #region 成员方法
        public void Start()
        {
            if (HttpListener.IsListening)
            {
                log.Info("HTTP 协议侦听器已启动");
                return;
            };

            log.Info("开始启动WebSocket服务");

            HttpListener.Prefixes.Add($"http://{Ip}:{Port}/");
            //HttpListener.Prefixes.Add($"https://{Ip}:{Port}/");

            log.Info("支持的资源标识符 (URI) 前缀：");
            foreach (var item in HttpListener.Prefixes)
            {
                log.Info(item);
            }

            if (OnWebSocketServiceBeginStart != null)
                OnWebSocketServiceBeginStart.Invoke(this, HttpListener);

            HttpListener.Start();

            ThreadPool.QueueUserWorkItem(new WaitCallback(p => CreateWebSocket(p as HttpListener)), HttpListener);
            if (OnWebSocketServiceStart != null)
            {
                OnWebSocketServiceStart.Invoke(this, HttpListener);
            }
            log.Info("服务已启动");
        }

        public void Stop()
        {
            if (!HttpListener.IsListening)
            {
                log.Info("HTTP 协议侦听器未启动");
                return;
            }
            log.Info("服务停止，停止接受传入的请求");
            HttpListener.Stop();
            if (OnWebSocketServiceStop != null)
                OnWebSocketServiceStop.Invoke(this, HttpListener);
        }

        public void Close()
        {
            log.Info("服务关闭，释放服务资源");
            Dispose();
            if (OnWebSocketServiceClose != null)
                OnWebSocketServiceClose.Invoke();
        }

        public void Dispose()
        {
            HttpListener.Stop();
            if (webSocketContexts != null)
            {
                foreach (var item in webSocketContexts)
                {
                    item.Dispose();
                }
            }
            webSocketContexts = null;
            if (CancellationTokenSource != null)
            {
                CancellationTokenSource.Cancel();
                CancellationTokenSource.Dispose();
            }
            if (HttpListener != null)
            {
                HttpListener.Close();
                HttpListener = null;
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 监听端口 创建WebSocket
        /// </summary>
        /// <param name="httpListener"></param>
        private void CreateWebSocket(HttpListener httpListener)
        {
            if (!httpListener.IsListening)
                throw new Exception("HttpListener未启动");
            HttpListenerContext listenerContext = httpListener.GetContext();
            
            if (OnAuthentication != null)
            {
                foreach(OnAuthentication item in OnAuthentication.GetInvocationList())
                {
                    if (!item.Invoke(listenerContext))
                    {
                        CreateWebSocket(httpListener);
                        return;
                    }
                }
            }

            if (!listenerContext.Request.IsWebSocketRequest)
            {
                log.Info($"非WebSocket请求：{listenerContext.Request.RequestTraceIdentifier}");
                var byyes = Encoding.UTF8.GetBytes($"非WebSocket请求：{listenerContext.Request.RequestTraceIdentifier}");
                listenerContext.Response.StatusCode =(int) HttpStatusCode.BadRequest;
                listenerContext.Response.ContentEncoding = Encoding.UTF8;
                listenerContext.Response.OutputStream.Write(byyes,0, byyes.Length);
                listenerContext.Response.OutputStream.Flush();
                listenerContext.Response.Close();
                CreateWebSocket(httpListener);
                return;
            }

            WebSocketContext webSocket = null;
            try
            {
                webSocket = new WebSocketContext(listenerContext, SubProtocol);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                CreateWebSocket(HttpListener);
                return;
            }

            log.Info($"成功创建WebSocket：{webSocket.ID}");

            int workerThreads = 0, completionPortThreads = 0;
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            //if (workerThreads <= ReservedThreadsCount + 1 || completionPortThreads <= ReservedThreadsCount + 1)
            //{
            //    /**
            //     * 可用线程小于预留线程数量
            //     * 通知客户端关闭连接
            //     * */
            //    webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "可用线程不足，无法连接").Wait();
            //}
            //else
            //{
            if (OnReceiveMessage != null)
                    webSocket.OnReceiveMessage += OnReceiveMessage;
                webSocket.OnCloseWebSocket += WebSocket_OnCloseWebSocket;
                webSocketContexts.Add(webSocket);
                if (OnAddWebSocket != null)
                    OnAddWebSocket.Invoke(webSocket);

            Task.Factory.StartNew(() =>
            {
                webSocket.ReceiveMessage();
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

                //ThreadPool.QueueUserWorkItem(new WaitCallback(p =>
                //{
                //    (p as WebSocketContext).ReceiveMessageAsync().Wait();
                //}), webSocket);

            //}

            CreateWebSocket(HttpListener);
        }

        private void WebSocket_OnCloseWebSocket(WebSocketContext context)
        {
            webSocketContexts.Remove(context);
            if (OnRemoveWebSocket != null)
                OnRemoveWebSocket.Invoke(context);
            if (context != null)
                context.Dispose();
        }

        /// <summary>
        /// 打印HttpListener的信息
        /// </summary>
        /// <param name="HttpListener"></param>
        private void PrintHttpListenerInfo(HttpListener HttpListener)
        {
            log.Info($"资源分区:{HttpListener.Realm}");
            log.Info($"允许请求在请求队列中停留的时间（以秒为单位）:{HttpListener.TimeoutManager.RequestQueue}");
            log.Info($"响应的最低发送速率（以每秒字节数为单位）:{HttpListener.TimeoutManager.MinSendBytesPerSecond}");
            log.Info($"允许空闲连接的时间（以秒为单位）:{HttpListener.TimeoutManager.IdleConnection.TotalSeconds}");
            log.Info($"允许 HttpListener 分析请求标头的时间（以秒为单位）:{HttpListener.TimeoutManager.HeaderWait.TotalSeconds}");
            log.Info($"允许请求实体正文到达的时间（以秒为单位）:{HttpListener.TimeoutManager.EntityBody.TotalSeconds}");
            log.Info($"允许 HttpListener 在保持连接时侦听完实体正文的时间（以秒为单位）:{HttpListener.TimeoutManager.DrainEntityBody.TotalSeconds}");
            log.Info($"使用 NTLM 时是否需要对使用同一传输控制协议 (TCP) 连接的其他请求进行身份验证:{HttpListener.UnsafeConnectionNtlmAuthentication}");
            log.Info($"应用程序是否接收 System.Net.HttpListener 向客户端发送响应时发生的异常:{HttpListener.IgnoreWriteExceptions}");
        }
        #endregion
    }
}
