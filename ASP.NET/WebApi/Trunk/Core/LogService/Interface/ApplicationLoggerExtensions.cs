using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService.Interface
{
    public static class ApplicationLoggerExtensions
    {
        /// <summary>
        /// 写入一个debug等级的日志记录.
        /// </summary>
        /// <param name="logger">日志记录器.</param>
        /// <param name="format">待格式化的消息字符串.</param>
        /// <param name="args">格式化参数列表.</param>
        public static void Debug(this IApplicationLogger logger, String format, params Object[] args)
        {
            logger.Debug(String.Format(format, args));
        }

        /// <summary>
        /// 写入一个info等级的日志记录.
        /// </summary>
        /// <param name="logger">日志记录器.</param>
        /// <param name="format">待格式化的消息字符串.</param>
        /// <param name="args">格式化参数列表.</param>
        public static void Info(this IApplicationLogger logger, String format, params Object[] args)
        {
            logger.Info(String.Format(format, args));
        }

        /// <summary>
        /// 写入一个warning等级的日志记录.
        /// </summary>
        /// <param name="logger">日志记录器.</param>
        /// <param name="format">待格式化的消息字符串.</param>
        /// <param name="args">格式化参数列表.</param>
        public static void Warn(this IApplicationLogger logger, String format, params Object[] args)
        {
            logger.Warn(String.Format(format, args));
        }

        /// <summary>
        /// 写入一个error等级的日志记录.
        /// </summary>
        /// <param name="logger">日志记录器.</param>
        /// <param name="format">待格式化的消息字符串.</param>
        /// <param name="args">格式化参数列表.</param>
        public static void Error(this IApplicationLogger logger, String format, params Object[] args)
        {
            logger.Error(String.Format(format, args));
        }

        /// <summary>
        /// 写入一个fatal等级的日志记录.
        /// </summary>
        /// <param name="logger">日志记录器.</param>
        /// <param name="format">待格式化的消息字符串.</param>
        /// <param name="args">格式化参数列表.</param>
        public static void Fatal(this IApplicationLogger logger, String format, params Object[] args)
        {
            logger.Fatal(String.Format(format, args));
        }


        /// <summary>
        /// 写入一个debug等级的日志记录.
        /// </summary>
        /// <param name="logger">日志记录器.</param>
        /// <param name="exception">要记录的异常.</param>
        /// <param name="format">待格式化的消息字符串.</param>
        /// <param name="args">格式化参数列表.</param>
        public static void Debug(this IApplicationLogger logger, Exception exception, String format, params Object[] args)
        {
            logger.Debug(exception, String.Format(format, args));
        }

        /// <summary>
        /// 写入一个info等级的日志记录.
        /// </summary>
        /// <param name="logger">日志记录器.</param>
        /// <param name="exception">要记录的异常.</param>
        /// <param name="format">待格式化的消息字符串.</param>
        /// <param name="args">格式化参数列表.</param>
        public static void Info(this IApplicationLogger logger, Exception exception, String format, params Object[] args)
        {
            logger.Info(exception, String.Format(format, args));
        }

        /// <summary>
        /// 写入一个warning等级的日志记录.
        /// </summary>
        /// <param name="logger">日志记录器.</param>
        /// <param name="exception">要记录的异常.</param>
        /// <param name="format">待格式化的消息字符串.</param>
        /// <param name="args">格式化参数列表.</param>
        public static void Warn(this IApplicationLogger logger, Exception exception, String format, params Object[] args)
        {
            logger.Warn(exception, String.Format(format, args));
        }

        /// <summary>
        /// 写入一个error等级的日志记录.
        /// </summary>
        /// <param name="logger">日志记录器.</param>
        /// <param name="exception">要记录的异常.</param>
        /// <param name="format">待格式化的消息字符串.</param>
        /// <param name="args">格式化参数列表.</param>
        public static void Error(this IApplicationLogger logger, Exception exception, String format, params Object[] args)
        {
            logger.Error(exception, String.Format(format, args));
        }

        /// <summary>
        /// 写入一个fatal等级的日志记录.
        /// </summary>
        /// <param name="logger">日志记录器.</param>
        /// <param name="exception">要记录的异常.</param>
        /// <param name="format">待格式化的消息字符串.</param>
        /// <param name="args">格式化参数列表.</param>
        public static void Fatal(this IApplicationLogger logger, Exception exception, String format, params Object[] args)
        {
            logger.Fatal(exception, String.Format(format, args));
        }
    }
}
