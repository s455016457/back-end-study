using System;

namespace WebPrintService
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListenerService hls = new HttpListenerService();
            hls.OnStart(null);
            Console.WriteLine("按任意键退出...");
            Console.Read();
        }
    }
}
