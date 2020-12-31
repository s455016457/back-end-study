using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketService
{
    class Program
    {
       static  Core.WebSocketService service;
        static void Main(string[] args)
        {
            var serviceConfig = Config.ConfigHelper.ServiceConfig;
            Console.Title = $"WebSocket服务器【{serviceConfig.Ip}:{serviceConfig.Port}】";
            Console.WriteLine($"版本:{Environment.Version.ToString(4)}");
            ThreadPool.SetMaxThreads(serviceConfig.MaxThreadCount, serviceConfig.MaxThreadCount);
            service = new Core.WebSocketService(serviceConfig.Ip, serviceConfig.Port, serviceConfig.WebSocketSubProtocol);

            service.OnReceiveMessage += Service_OnRegisterMessage;
            service.OnAddWebSocket += Service_OnAddWebSocket;
            service.OnRemoveWebSocket += Service_OnRemoveWebSocket;

            service.Start();

            System.Timers.Timer timer = new System.Timers.Timer(10 * 1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Start();

            Console.ReadLine();
        }

        private static void Service_OnRemoveWebSocket(Core.WebSocketContext webSocketContext)
        {
            Console.WriteLine($"删除WebSocket:{webSocketContext.ID}");
        }

        private static void Service_OnAddWebSocket(Core.WebSocketContext webSocketContext)
        {
            Console.WriteLine($"添加WebSocket:{webSocketContext.ID}");
        }

        private static void Service_OnRegisterMessage(Core.WebSocketContext context, string message)
        {
            var list = service.WebSocketContexts.Where(p => p.ID != context.ID);
            foreach(var item in list)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(p => {
                    Core.WebSocketContext webSocket = p as Core.WebSocketContext;
                    webSocket.SendMessageAsync($"{context.ID}说：{message}").Wait();
                }), item);
                //item.SendMessageAsync($"{context.ID}说：{message}").Wait();
            }
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine($"操作系统内存页字节数【{Environment.SystemPageSize}】");
            Console.WriteLine($"内存使用量【{Environment.WorkingSet}】");
            Console.WriteLine($"客户端连接数量【{service.WebSocketContexts.Count}】");
            int workerThreads = 0, completionPortThreads=0;
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine($"可用线程数量【{workerThreads},{workerThreads}】");
            ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine($"最大可用线程数量【{workerThreads},{workerThreads}】");
            ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine($"最小可用线程数量【{workerThreads},{workerThreads}】");
        }
    }
}
