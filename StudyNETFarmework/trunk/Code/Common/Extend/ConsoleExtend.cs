using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extend
{
    /// <summary>
    /// console扩展
    /// </summary>
    public static class ConsoleExtend
    {
        static object lock_obj = new object();
        /// <summary>
        /// 在控制台输出一行带背景与字体颜色的数据
        /// </summary>
        /// <param name="backgroundColor">这行数据背景颜色</param>
        /// <param name="foregroundColor">这行数据字体颜色</param>
        /// <param name="format">字符串格式</param>
        /// <param name="param">字符串参数</param>
        public static void WriteLine(ConsoleColor backgroundColor, ConsoleColor foregroundColor, String format, params object[] param)
        {
            lock (lock_obj)
            {
                var rawBackgroundColor = Console.BackgroundColor;
                var rawForegroundColor = Console.ForegroundColor;
                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = foregroundColor;
                try
                {
                    Console.WriteLine(format, param);
                }
                finally
                {
                    Console.BackgroundColor = rawBackgroundColor;
                    Console.ForegroundColor = rawForegroundColor;
                }
            }
        }
        /// <summary>
        /// 在控制台输出一行带字体颜色的数据
        /// </summary>
        /// <param name="foregroundColor">这行数据字体颜色</param>
        /// <param name="format">字符串格式</param>
        /// <param name="param">字符串参数</param>
        public static void WriteLine(ConsoleColor foregroundColor, String format, params object[] param)
        {
            lock (lock_obj)
            {
                var rawColor = Console.ForegroundColor;
                Console.ForegroundColor = foregroundColor;
                try
                {
                    Console.WriteLine(format, param);
                }
                finally
                {
                    Console.ForegroundColor = rawColor;
                }
            }
        }

        public static void WriteLineError(String format, params object[] param)
        {
            WriteLine(ConsoleColor.Red, ConsoleColor.White, format, param);
        }

        public static void WriteLineRandomDebug(String format, params object[] param)
        {
            lock (lock_obj)
            {
                Random random = new Random();
                switch (random.Next(1, 4))
                {
                    case 1:
                        WriteLineDebug(format, param);
                        return;
                    case 2:
                        WriteLineDebug1(format, param);
                        return;
                    case 3:
                        WriteLineDebug2(format, param);
                        return;
                    default:
                        WriteLineDebug(format, param);
                        return;

                }
            }
        }

        static int nextDebugCount=1;
        public static void WriteLineNextDebug(String format, params object[] param)
        {
            lock (lock_obj)
            {
                if (nextDebugCount >= 4) nextDebugCount = 1;
                switch (nextDebugCount++)
                {
                    case 1:
                        WriteLineDebug(format, param);
                        return;
                    case 2:
                        WriteLineDebug1(format, param);
                        return;
                    case 3:
                        WriteLineDebug2(format, param);
                        return;
                    default:
                        WriteLineDebug(format, param);
                        return;
                }
            }
        }

        public static void WriteLineDebug(String format, params object[] param)
        {
            WriteLine(ConsoleColor.Yellow, ConsoleColor.Black, format, param);
        }

        public static void WriteLineDebug1(String format, params object[] param)
        {
            WriteLine(ConsoleColor.DarkYellow, ConsoleColor.White, format, param);
        }

        public static void WriteLineDebug2(String format, params object[] param)
        {
            WriteLine(ConsoleColor.DarkMagenta, ConsoleColor.White, format, param);
        }

        public static void WriteLineInfo(String format, params object[] param)
        {
            WriteLine(ConsoleColor.Green, ConsoleColor.White, format, param);
        }

        public static void WriteLineRed(String format, params object[] param)
        {
            WriteLine(ConsoleColor.Red, format, param);
        }

        public static void WriteLineWhite(String format, params object[] param)
        {
            WriteLine(ConsoleColor.White, format, param);
        }

        public static void WriteLineBlack(String format, params object[] param)
        {
            WriteLine(ConsoleColor.Black, format, param);
        }

        public static void WriteLineBlue(String format, params object[] param)
        {
            WriteLine(ConsoleColor.Blue, format, param);
        }

        public static void WriteLineYellow(String format, params object[] param)
        {
            WriteLine(ConsoleColor.Yellow, format, param);
        }

        public static void WriteLineDarkMagenta(String format, params object[] param)
        {
            WriteLine(ConsoleColor.DarkMagenta, format, param);
        }
    }
}
