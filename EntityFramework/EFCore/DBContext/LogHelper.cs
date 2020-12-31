using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.DBContext
{
    internal class LogHelper
    {
        private static string LogPath;
        private static string AppPath;

        private static LogHelper log;

        private LogHelper()
        {
            LogPath = "Log";
            AppPath = AppDomain.CurrentDomain.BaseDirectory;

        }

        private static Object lock_obj = new Object();

        internal static LogHelper Create()
        {
            if (log == null)
            {
                lock (lock_obj)
                {
                    if (log == null)
                    {
                        log = new LogHelper();
                    }
                }
            }
            return log;
        }

        /// <summary>
        /// 写日志
        /// 线程安全的
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        internal Boolean WriteLog(String Message, params String[] objs)
        {
            return WriteLog(String.Format(Message, objs));
        }

        /// <summary>
        /// 写日志
        /// 线程安全的
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        internal Boolean WriteLog(String Message)
        {
            var FullPath = Path.Combine(AppPath, LogPath);
            DateTime dateTime = DateTime.Now;
            string FullFilePath = Path.Combine(FullPath, CreateFileName());
            lock (lock_obj)
            {
                if (!Directory.Exists(FullPath))
                {
                    Directory.CreateDirectory(FullPath);
                }
                File.AppendAllText(FullFilePath, DateTime.Now.ToString("dd HH:mm:ss") + "：" + Message + System.Environment.NewLine);
            }
            return true;
        }

        /// <summary>
        /// 写日志
        /// 线程安全的
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        internal Boolean WriteErrorLog(Exception ex)
        {
            StringBuilder strb = new StringBuilder();
            strb.AppendLine("============================= " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " =================================");
            strb.AppendLine(ex.Message);
            strb.AppendLine(ex.Source);
            strb.AppendLine(ex.HelpLink);
            strb.AppendLine(ex.StackTrace);
            strb.AppendLine(ex.TargetSite == null ? String.Empty : ex.TargetSite.ToString());
            strb.AppendLine("==============================================================");
            strb.AppendLine("");
            return WriteErrorLog(strb.ToString());
        }

        internal Boolean WriteErrorLog(String Message)
        {
            var FullPath = Path.Combine(AppPath, LogPath);
            DateTime dateTime = DateTime.Now;
            string FullFilePath = Path.Combine(FullPath, CreateErrorFileName());
            lock (lock_obj)
            {
                if (!Directory.Exists(FullPath))
                {
                    Directory.CreateDirectory(FullPath);
                }
                File.AppendAllText(FullFilePath, DateTime.Now.ToString("dd HH:mm:ss") + "：" + Message + System.Environment.NewLine);
            }
            return true;
        }

        private String CreateFileName()
        {
            DateTime dateTime = DateTime.Now;
            return String.Format("{0}.log", dateTime.ToString("yyyyMM"));
        }
        private String CreateErrorFileName()
        {
            DateTime dateTime = DateTime.Now;
            return String.Format("{0}_error.log", dateTime.ToString("yyyyMM"));
        }
    }
}
