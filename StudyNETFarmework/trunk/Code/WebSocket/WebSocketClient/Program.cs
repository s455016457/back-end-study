using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace WebSocketClient
{
    static class Program
    {
        static string ip = "127.0.0.1", subProtocol = "chat";
        static int port = 8181;
        static string name = $"Name_{Guid.NewGuid()}", OldWebSocketStateDescr=string.Empty;
        static Core.WebSocketClient webSocketClient;
        static void Main(string[] args)
        {
            Console.Title = "WebStocket客户端";
            webSocketClient = new Core.WebSocketClient(name,ip,port, subProtocol);
            webSocketClient.SetEncoding(Encoding.UTF8);

            webSocketClient.OnReceiveMessage += WebSocketClient_OnReceiveMessage;
            webSocketClient.OnCloseWebSocketClient += WebSocketClient_OnCloseWebSocketClient;
            ListeningClientWebSocketStatus();
            string key = Console.ReadLine();
            CommandHandler(key);

        }

        private static void WebSocketClient_OnCloseWebSocketClient(Core.WebSocketClient webSocketClient, WebSocketCloseStatus webSocketCloseStatus, string statusDescription)
        {
            Console.WriteLine($"WebSocket上下文已关闭，关闭原因【{statusDescription}】");
        }

        private static void WebSocketClient_OnReceiveMessage(Core.WebSocketClient webSocketClient, string message)
        {
            Console.WriteLine(message);
        }

        static void CommandHandler(string key)
        {
            if (key.Equals("Open", StringComparison.CurrentCultureIgnoreCase))
            {
                try
                {
                    webSocketClient.OpenAsync().Wait();
                }catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    key = Console.ReadLine();
                    CommandHandler(key);
                    return;
                }
                ThreadPool.QueueUserWorkItem(new WaitCallback(p =>
                {
                    (p as Core.WebSocketClient).ReceiveMessageAsync().Wait();
                }), webSocketClient);
            }
            else if (key.Equals("Close", StringComparison.CurrentCultureIgnoreCase))
            {
                webSocketClient.CloseAsync(WebSocketCloseStatus.EndpointUnavailable,"客户端断开连接！").Wait();
            }
            else if (key.Equals("Exit", StringComparison.CurrentCultureIgnoreCase))
            {
                webSocketClient.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "客户端断开连接！").Wait();
                Environment.Exit(0);
            }
            else
            {
                webSocketClient.SendMassage(key).Wait();
            }

            key = Console.ReadLine();
            CommandHandler(key);
        }

        static void ListeningClientWebSocketStatus()
        {
            System.Timers.Timer timer = new System.Timers.Timer(100);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (webSocketClient == null) return;
            if (OldWebSocketStateDescr == webSocketClient.GetWebSocketStateDescr()) return;
            OldWebSocketStateDescr = webSocketClient.GetWebSocketStateDescr();
            Console.WriteLine(OldWebSocketStateDescr);
        }
    }
}
