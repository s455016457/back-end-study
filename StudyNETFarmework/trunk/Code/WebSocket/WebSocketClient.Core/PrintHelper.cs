using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketClient.Core
{
    internal static class PrintHelper
    {

        public static void PrintInfo(string message,ILog log)
        {
            ConsoleWriteLine(ConsoleColor.DarkBlue, ConsoleColor.White, message,log);
            log.Info(message);
        }

        public static void PrintDebug(string message, ILog log)
        {
            ConsoleWriteLine(ConsoleColor.Blue, ConsoleColor.Black, message,log);
            log.Debug(message);
        }

        public static void printError(Exception ex, ILog log)
        {
            ConsoleWriteLine(ConsoleColor.Blue, ConsoleColor.Red, ex,log);
            log.Error(ex);
        }

        static object lock_obj = new object();
        public static void ConsoleWriteLine(ConsoleColor backgroundColor, ConsoleColor foregroundColor, object message, ILog log)
        {
            lock (lock_obj)
            {
                var BackgroundColor = Console.BackgroundColor;
                var ForegroundColor = Console.ForegroundColor;

                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = foregroundColor;

                Console.WriteLine(message);

                Console.BackgroundColor = BackgroundColor;
                Console.ForegroundColor = ForegroundColor;
            }
        }
    }
}
