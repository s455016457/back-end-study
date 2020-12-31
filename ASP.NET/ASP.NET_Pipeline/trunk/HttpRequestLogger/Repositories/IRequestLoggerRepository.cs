using HttpRequestLogger.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpRequestLogger.Repositories
{
    /// <summary>
    /// 请求日志仓储
    /// </summary>
    public interface IRequestLoggerRepository : IDisposable
    {
        /// <summary>
        /// 添加请求日志
        /// </summary>
        /// <param name="requestLogger"></param>
        void AddRequestLogger(RequestLogger requestLogger);
        /// <summary>
        /// 更新请求日志
        /// </summary>
        /// <param name="requestLogger"></param>
        void UpdateRequestLogger(RequestLogger requestLogger);
        /// <summary>
        /// 保存更新
        /// </summary>
        void SaveChange();
    }
}
