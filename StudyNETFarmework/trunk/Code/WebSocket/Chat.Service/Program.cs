using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service
{
    public class Program
    {
        static void Main(string[] args)
        {
            Core.ChatService chatService = new Core.ChatService("127.0.0.1", 8081);
            chatService.SetCreateRepositoryFunc(() => new ChatServiceRepository());
            chatService.SetGetUidFunc(p =>
            {
                string uid= p.CookieCollection["UId"].Value;
                return Guid.Parse(uid);
            });

            chatService.Start();
            Console.WriteLine("服务已启动");
            Console.ReadLine();
        }
    }
}
