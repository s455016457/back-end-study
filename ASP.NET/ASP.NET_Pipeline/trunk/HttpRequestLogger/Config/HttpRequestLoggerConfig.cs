using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpRequestLogger.Config
{
    public class HttpRequestLoggerConfig
    {
        /// <summary>
        /// 资源库全名
        /// </summary>
        public string RepositoryFullName { get; set; }
        /// <summary>
        /// 资源库所在程序集
        /// </summary>
        public string RepositoryAssemblyName { get; set; }
        /// <summary>
        /// 获取或设置资源库创建方法
        /// </summary>
        public static Func<Repositories.IRequestLoggerRepository> CreateRequestLoggerRepositoryFunc { get; set; }
    }
}
