using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService.Interface
{
    public interface IApplicationLogger
    {
        /// <summary>
        /// 获取一个值,该值指示是否启用 debug 等级的日志记录.
        /// </summary>
        /// <value>如果已启用debug等级的日志记录标示为<c>true</c>; 否则为<c>false</c>.</value>
        Boolean IsDebugEnabled { get; }

        /// <summary>
        /// 获取一个值,该值指示是否启用 information 等级的日志记录.
        /// </summary>
        /// <value>如果已启用information等级的日志记录标示为<c>true</c>; 否则为<c>false</c>.</value>
        Boolean IsInfoEnabled { get; }

        /// <summary>
        /// 获取一个值,该值指示是否启用 warn 等级的日志记录.
        /// </summary>
        /// <value>如果已启用warn等级的日志记录标示为<c>true</c>; 否则为<c>false</c>.</value>
        Boolean IsWarnEnabled { get; }

        /// <summary>
        /// 获取一个值,该值指示是否启用 error 等级的日志记录.
        /// </summary>
        /// <value>如果已启用error等级的日志记录标示为<c>true</c>; 否则为<c>false</c>.</value>
        Boolean IsErrorEnabled { get; }

        /// <summary>
        /// 获取一个值,该值指示是否启用 fatal 等级的日志记录.
        /// </summary>
        /// <value>如果已启用fatal等级的日志记录标示为<c>true</c>; 否则为<c>false</c>.</value>
        Boolean IsFatalEnabled { get; }

        /// <summary>
        /// 写入一个debug等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        void Debug(String message);

        /// <summary>
        /// 写入一个debug等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        /// <param name="exception">此日志消息的异常信息.</param>
        void Debug(Exception exception, String message);

        /// <summary>
        /// 写入一个info等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        void Info(String message);

        /// <summary>
        /// 写入一个info等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        /// <param name="exception">此日志消息的异常信息.</param>
        void Info(Exception exception, String message);

        /// <summary>
        /// 写入一个warning等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        void Warn(String message);

        /// <summary>
        /// 写入一个warning等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        /// <param name="exception">此日志消息的异常信息.</param>
        void Warn(Exception exception, String message);

        /// <summary>
        /// 写入一个error等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        void Error(String message);

        /// <summary>
        /// 写入一个error等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        /// <param name="exception">此日志消息的异常信息.</param>
        void Error(Exception exception, String message);

        /// <summary>
        /// 写入一个fatal等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        void Fatal(String message);

        /// <summary>
        /// 写入一个fatal等级的日志记录.
        /// </summary>
        /// <param name="message">日志消息.</param>
        /// <param name="exception">此日志消息的异常信息.</param>
        void Fatal(Exception exception, String message);
    }
}
