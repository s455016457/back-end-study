using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HttpRequestLogger.Events
{
    public delegate bool BeginRequestLogger(HttpRequest request,Entities.RequestLogger requestLogger);
    public delegate void AlfterBeginRequestLogger(HttpRequest request, Entities.RequestLogger requestLogger);
    public delegate bool EndRequestLogger(HttpRequest request, Entities.RequestLogger requestLogger);
    public delegate void AlfterEndRequestLogger(HttpRequest request, Entities.RequestLogger requestLogger);

    /// <summary>
    /// 请求日志时间处理者
    /// </summary>
    public static class RequestLoggerEventHandler
    {
        /// <summary>
        /// 在开始请求日志前执行 返回false 则停止继续执行
        /// </summary>
        public static event BeginRequestLogger BeginRequestLoggerEvent;
        /// <summary>
        /// 在开始请求日志后执行
        /// </summary>
        public static event AlfterBeginRequestLogger AlfterBeginRequestLoggerEvent;
        /// <summary>
        /// 在结束请求日志前执行 返回false 则停止继续执行
        /// </summary>
        public static event EndRequestLogger EndRequestLoggerEvent;
        /// <summary>
        /// 在结束请求日志后执行
        /// </summary>
        public static event AlfterEndRequestLogger AlfterEndRequestLoggerEvent;

        /// <summary>
        /// 执行 在开始请求日志前执行 事件 返回false 则停止继续执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestLogger"></param>
        internal static bool BeginRequestLoggerHandler(HttpRequest request, Entities.RequestLogger requestLogger)
        {
            if (BeginRequestLoggerEvent == null) return true;
            foreach (BeginRequestLogger beginRequest in BeginRequestLoggerEvent.GetInvocationList())
            {
                if (!beginRequest.Invoke(request, requestLogger))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 执行 在开始请求日志后执行 事件
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestLogger"></param>
        internal static void AlfterBeginRequestLoggerHandler(HttpRequest request, Entities.RequestLogger requestLogger)
        {
            if (AlfterBeginRequestLoggerEvent == null) return;
            AlfterBeginRequestLoggerEvent(request, requestLogger);
        }

        /// <summary>
        /// 执行 在结束请求日志前执行 事件 返回false 则停止继续执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestLogger"></param>
        internal static bool EndRequestLoggerHandler(HttpRequest request, Entities.RequestLogger requestLogger)
        {
            if (EndRequestLoggerEvent == null) return true;
            foreach (EndRequestLogger endRequest in EndRequestLoggerEvent.GetInvocationList())
            {
                if (!endRequest.Invoke(request, requestLogger))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 执行 在结束请求日志前执行 事件
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestLogger"></param>
        internal static void AlfterEndRequestLoggerHandler(HttpRequest request, Entities.RequestLogger requestLogger)
        {
            if (AlfterEndRequestLoggerEvent == null) return;
            AlfterEndRequestLoggerEvent(request, requestLogger);
        }
    }
}
