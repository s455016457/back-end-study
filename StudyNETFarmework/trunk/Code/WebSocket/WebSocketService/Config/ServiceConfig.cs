using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketService.Config
{
    internal class ServiceConfig
    {
        int _maxThreadCount = 0;

        /// <summary>
        /// WebSocket 子协议
        /// <see cref="https://tools.ietf.org/html/rfc6455#section-1.9"/>
        /// </summary>
        public string WebSocketSubProtocol { get; set; }
        /// <summary>
        /// WebSocket 脉搏 单位为秒
        /// </summary>
        public int WebSocketPulse { get; set; }
        /// <summary>
        /// 监听端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Ip地址
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 最大线程数量
        /// </summary>
        public int MaxThreadCount
        {
            get { return _maxThreadCount; }
            set
            {
                if (value < Environment.ProcessorCount)
                    _maxThreadCount = Environment.ProcessorCount;
                else
                _maxThreadCount = value;
            }
        }

        public string Origin { get; set; } = string.Empty;

        public string Extensions { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;

    }
}
