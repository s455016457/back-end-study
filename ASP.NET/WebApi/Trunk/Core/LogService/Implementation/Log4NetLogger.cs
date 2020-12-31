using log4net;
using LoggerService.Interface;
using System;

namespace LoggerService.Implementation
{
    public class Log4NetLogger : IApplicationLogger
    {
        /// <summary>
        /// The _log
        /// </summary>
        private readonly ILog _log;
        private static String _loggerName = "ApplicationLogger";

        /// <summary>
        /// 初始化一个 <see cref="Log4netLogger"/> 实例.
        /// </summary>
        public Log4NetLogger()
        {
            _log = log4net.LogManager.GetLogger(_loggerName);
        }

        /// <summary>
        /// 初始化一个 <see cref="Log4netLogger"/> 实例.
        /// </summary>
        /// <param name="log">log4net log实例.</param>
        public Log4NetLogger(ILog log)
        {
            _log = log;
        }

        /// <summary>
        /// 获取一个值,该值指示是否启用 debug 等级的日志记录.
        /// </summary>
        /// <value>如果已启用debug等级的日志记录标示为<c>true</c>; 否则为<c>false</c>.</value>
        public Boolean IsDebugEnabled
        {
            get { return _log.IsDebugEnabled; }
        }

        /// <summary>
        /// 获取一个值,该值指示是否启用 information 等级的日志记录.
        /// </summary>
        /// <value>如果已启用information等级的日志记录标示为<c>true</c>; 否则为<c>false</c>.</value>
        public Boolean IsInfoEnabled
        {
            //get { return _log.IsInfoEnabled; }
            get { return true; }
        }

        /// <summary>
        /// 获取一个值,该值指示是否启用 warn 等级的日志记录.
        /// </summary>
        /// <value>如果已启用warn等级的日志记录标示为<c>true</c>; 否则为<c>false</c>.</value>
        public Boolean IsWarnEnabled
        {
            get { return _log.IsWarnEnabled; }
        }

        /// <summary>
        /// 获取一个值,该值指示是否启用 error 等级的日志记录.
        /// </summary>
        /// <value>如果已启用error等级的日志记录标示为<c>true</c>; 否则为<c>false</c>.</value>
        public Boolean IsErrorEnabled
        {
            get { return _log.IsErrorEnabled; }
        }

        /// <summary>
        /// 获取一个值,该值指示是否启用 fatal 等级的日志记录.
        /// </summary>
        /// <value>如果已启用fatal等级的日志记录标示为<c>true</c>; 否则为<c>false</c>.</value>
        public Boolean IsFatalEnabled
        {
            get { return _log.IsFatalEnabled; }
        }

        /// <summary>
        /// 写入一个debug等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        public void Debug(String message)
        {
            if (_log.IsDebugEnabled)
            {
                _log.Debug(message);
            }
        }

        /// <summary>
        /// 写入一个debug等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        /// <param name="exception">此日志消息的异常信息.</param>
        public void Debug(Exception exception, String message)
        {
            if (_log.IsDebugEnabled)
            {
                _log.Debug(message, exception);
            }
        }

        /// <summary>
        /// 写入一个info等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        public void Info(String message)
        {
            if (_log.IsInfoEnabled)
            {
                _log.Info(message);
            }
        }

        /// <summary>
        /// 写入一个info等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        /// <param name="exception">此日志消息的异常信息.</param>
        public void Info(Exception exception, String message)
        {
            if (_log.IsInfoEnabled)
            {
                _log.Info(message, exception);
            }
        }

        /// <summary>
        /// 写入一个warning等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        public void Warn(String message)
        {
            if (_log.IsWarnEnabled)
            {
                _log.Warn(message);
            }
        }

        /// <summary>
        /// 写入一个warning等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        /// <param name="exception">此日志消息的异常信息.</param>
        public void Warn(Exception exception, String message)
        {
            if (_log.IsWarnEnabled)
            {
                _log.Warn(message, exception);
            }
        }

        /// <summary>
        /// 写入一个error等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        public void Error(String message)
        {
            if (_log.IsErrorEnabled)
            {
                _log.Error(message);
            }
        }

        /// <summary>
        /// 写入一个error等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        /// <param name="exception">此日志消息的异常信息.</param>
        public void Error(Exception exception, String message)
        {
            if (_log.IsErrorEnabled)
            {
                _log.Error(message, exception);
            }
        }

        /// <summary>
        /// 写入一个fatal等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        public void Fatal(String message)
        {
            if (_log.IsFatalEnabled)
            {
                _log.Fatal(message);
            }
        }

        /// <summary>
        /// 写入一个fatal等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        /// <param name="exception">此日志消息的异常信息.</param>
        public void Fatal(Exception exception, String message)
        {
            if (_log.IsFatalEnabled)
            {
                _log.Fatal(message, exception);
            }
        }
    }
}
