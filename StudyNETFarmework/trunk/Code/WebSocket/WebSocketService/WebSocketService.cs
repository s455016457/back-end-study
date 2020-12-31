using System;
using System.Collections.Concurrent;
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
    public static class WebSocketService
    {
        /// <summary>
        /// 已启动
        /// </summary>
        public static bool IsStart { get; private set; }
        static object lock_ojb = new object();
        static ConcurrentDictionary<Guid,WebSocketContext> webSocketContexts = new ConcurrentDictionary<Guid,WebSocketContext>();
        static ServiceConfig serviceConfig = ConfigHelper.ServiceConfig;
        static HttpListener httpListener;
        
        static WebSocketService()
        {
            httpListener = new HttpListener();
            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            httpListener.Prefixes.Add($"http://{serviceConfig.Ip}:{serviceConfig.Port}/");
            ThreadPool.SetMaxThreads(serviceConfig.MaxThreadCount, serviceConfig.MaxThreadCount);
            WebSocketContextHandler.CloseWebStocketEvent += WebSocketContextHandler_CloseWebStocketEvent;
        }

        private static void WebSocketContextHandler_CloseWebStocketEvent(WebSocketContext context, WebSocketCloseStatus webSocketCloseStatus, string closeStatusDescription)
        {
            Console.WriteLine($"从服务中删除WebSocket{context.ID}");
            int count = 1;
            WebSocketContext model = null;
            while (!webSocketContexts.TryRemove(context.ID, out model))
            {
                if (count++ >= 99)
                {
                    throw new Exception($"尝试从{webSocketContexts}中删除{context}失败99次！");
                }
            }
            Console.WriteLine($"成功从服务中删除WebSocket{context.ID}");
            model.Dispose();
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public static void Start()
        {
            Console.WriteLine("服务已启动");
            httpListener.Start();
            Console.WriteLine($"开始监听：http://{serviceConfig.Ip}:{serviceConfig.Port}/");

            ThreadPool.QueueUserWorkItem(new WaitCallback(p =>
            {
                var httpListener = p as HttpListener;
                var context = httpListener.GetContextAsync().Result;
                while (true)
                {
                    WebSocketContext webSocketContext = WebSocketContext.CreateAsync(context).Result;
                    if (webSocketContext == null)
                    {
                        context = httpListener.GetContextAsync().Result;
                        continue;
                    }
                    int count = 1, workerThreads=0, completionPortThreads=0;
                    ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
                    if (workerThreads <= 5 || completionPortThreads <= 5)
                    {
                        /**
                         * 可用线程为0
                         * 通知客户端关闭连接
                         * 等待下个传入请求
                         * 跳出本次循环
                         * */
                        webSocketContext.WebSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "可用线程不足，无法连接",new CancellationToken()).Wait();
                        context = httpListener.GetContextAsync().Result;
                        continue;

                    }
                    while (!webSocketContexts.TryAdd(webSocketContext.ID, webSocketContext))
                    {
                        if (count++ >= 99)
                        {
                            throw new Exception($"尝试将{webSocketContext}添加进{webSocketContexts}失败99次！");
                        }
                    }
                    Console.WriteLine($"客户端【{webSocketContext.ID} {webSocketContext.Name}】创建连接");
                    ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
                    {
                        WebSocketContext webSocketContext1 = x as WebSocketContext;
                        webSocketContext1.ReceiveMessageAsync().Wait();
                    }), webSocketContext);
                    context = httpListener.GetContextAsync().Result;
                }
            }), httpListener);
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public static void Stop()
        {
            httpListener.Stop();
            WebSocketDisposable();
        }

        /// <summary>
        /// 获取系统启动后经过的毫秒数
        /// </summary>
        /// <returns></returns>
        public static int GetTickCount()
        {
            return Environment.TickCount;
        }

        /// <summary>
        /// 获取映射到进程上下文的物理内存量
        /// </summary>
        /// <returns></returns>
        public static long GetWorkingSet()
        {
            return Environment.WorkingSet;
        }

        public static IReadOnlyList<WebSocketContext> GetWebSocketContexts()
        {
            return webSocketContexts.Values.ToList();
        }

        static void WebSocketDisposable()
        {
            foreach (var webSocket in webSocketContexts.ToArray())
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(p =>
                {
                    int count = 1;
                    WebSocketContext webSocketContext = p as WebSocketContext;
                    webSocketContext.Dispose();
                    while (!webSocketContexts.TryRemove(webSocketContext.ID,out webSocketContext))
                    {
                        if (count++ >= 99)
                        {
                            throw new Exception($"尝试从{webSocketContexts}中删除{webSocketContext}失败99次！");
                        }
                    }
                }), webSocket);
            }
        }
    }
}
