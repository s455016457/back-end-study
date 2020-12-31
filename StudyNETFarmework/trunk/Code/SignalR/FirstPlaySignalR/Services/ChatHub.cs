using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstPlaySignalR.Services
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // 调用 broadcastMessage 方法更新客户端.
            Console.WriteLine($"服务器发送消息：{name}：{message}");
            Clients.All.broadcastMessage(name, message);
        }
    }
}