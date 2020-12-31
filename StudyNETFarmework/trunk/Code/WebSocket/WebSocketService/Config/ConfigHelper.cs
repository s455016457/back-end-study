using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace WebSocketService.Config
{
    internal static class ConfigHelper
    {
        private static string ServiceConfig_Key = "WebSocketService_ServiceConfiguration";
        /// <summary>
        /// 获取服务配置
        /// </summary>
        public static ServiceConfig ServiceConfig
        {
            get
            {
                return WebConfigurationManager.GetSection(ServiceConfig_Key) as ServiceConfig;
            }
        }
    }
}
