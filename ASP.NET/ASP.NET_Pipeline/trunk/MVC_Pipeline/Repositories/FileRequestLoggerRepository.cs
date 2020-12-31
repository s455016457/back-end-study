using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using HttpRequestLogger.Entities;
using System.Timers;

namespace MVC_Pipeline.Repositories
{
    public class FileRequestLoggerRepository : HttpRequestLogger.Repositories.IRequestLoggerRepository
    {
        static string fileUrl;

        static List<RequestLogger> requestLoggers = new List<RequestLogger>();
        static Timer timer;
        static object lock_object = new object();

        public FileRequestLoggerRepository()
        {
            if (fileUrl == null)
            {
                var dirctoryUrl = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, "Data");
                if (!Directory.Exists(dirctoryUrl))
                    Directory.CreateDirectory(dirctoryUrl);
                fileUrl = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, "Data", "RequestLoggerRepository.data");
                if (!File.Exists(fileUrl))
                    File.Create(fileUrl);
            }
            if (timer == null)
            {
                timer = new Timer(10 * 1000);
                timer.Elapsed += FileRequestLoggerRepository.Timer_Elapsed;
                timer.Start();
            }
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            // 此处有未知异常，会导致应用程序重启

            //lock (lock_object)
            //{
            //    try
            //    {
            //        var requestLoggerArray = requestLoggers.ToArray();
            //        requestLoggers.Clear();
            //        StringBuilder context = new StringBuilder();
            //        foreach (var item in requestLoggerArray)
            //        {
            //            context.AppendLine(RequestLoggerToString(item));
            //        }
            //        File.AppendAllText(fileUrl, context.ToString());
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //}
        }

        private static string RequestLoggerToString(RequestLogger requestLogger) {
            return ($"AddRequestLogger : {requestLogger.Id}    {requestLogger.Url}    {requestLogger.Method}" +
                    $"    {requestLogger.Ip}    {requestLogger.IsFile}    {requestLogger.Stamp.ToString("yyyy-MM-dd HH:mm:ss zzzz")}" +
                    $"    {requestLogger.TimeConsuming}");
        }

        public void AddRequestLogger(RequestLogger requestLogger)
        {
            requestLoggers.Add(requestLogger);
        }

        public void Dispose()
        {
            //fileUrl = null;
            //context = null;
        }

        public void SaveChange()
        {
        }

        public void UpdateRequestLogger(RequestLogger requestLogger)
        {
            var model = requestLoggers.FirstOrDefault(p => p.Id.Equals(requestLogger.Id));
            if (model != null)
                model.EndRequest();
            else
            {
                requestLogger.EndRequest();
                AddRequestLogger(requestLogger);
            }
        }
    }
}