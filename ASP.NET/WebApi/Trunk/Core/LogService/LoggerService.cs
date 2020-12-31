using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using LoggerService.Implementation;
using LoggerService.Interface;
using System;
using System.IO;

namespace LoggerService
{
    public static class LoggerService
    {
        private static object lock_obj = new object();

        private static Boolean _isStart = false;
        private static Boolean _requestLoggerIsEnable = false;
        private static Boolean _applicationLoggerIsEnable = false;
        private static Boolean _isInitLog4Net = false;

        private static IApiRequestLogger _apiRequestLogger;
        private static IApplicationLogger _applicationLogger;

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="ApiRequestLoggerConnectionString">Api请求日志记录器数据库配置字符串</param>
        public static void Start(String ApiRequestLoggerConnectionString = "")
        {
            if (_isStart) return;

            if (!_isInitLog4Net
                || (!String.IsNullOrWhiteSpace(ApiRequestLoggerConnectionString) && !_requestLoggerIsEnable)
                || !_applicationLoggerIsEnable)
            {
                lock (lock_obj)
                {
                    if (!_isInitLog4Net)
                    {
                        InitLog4Net();
                        _isInitLog4Net = true;
                    }

                    if (!String.IsNullOrWhiteSpace(ApiRequestLoggerConnectionString) && !_requestLoggerIsEnable)
                    {
                        _apiRequestLogger = new ApiRequestLogger(ApiRequestLoggerConnectionString);
                        _requestLoggerIsEnable = true;
                    }

                    if (!_applicationLoggerIsEnable)
                    {
                        _applicationLogger = new Log4NetLogger();
                        _applicationLoggerIsEnable = true;
                    }

                    _isStart = true;
                }
            }
        }

        private static void InitLog4Net()
        {
            // 在应用程序启动时运行的代码
            var configFile = "log4net.config";
            var file = new FileInfo(configFile);
            if (!file.Exists)
            {
                file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile));
            }

            if (file.Exists)
            {
                //使用指定的文件配置log4net，监视文件的更改并在检测到更改时重新加载配置。
                XmlConfigurator.ConfigureAndWatch(file);
            }
            else
            {
                //使用指定的追加器初始化log4net系统。
                BasicConfigurator.Configure(new TraceAppender { Layout = new PatternLayout() });
            }
        }

        public static Boolean IsStart()
        {
            return _isStart;
        }

        public static IApiRequestLogger ApiRequestLogger
        {
            get
            {
                if (!_isStart)
                {
                    throw new Exception("服务未启动，无法获取API请求日志记录器");
                }

                if (!_requestLoggerIsEnable)
                {
                    throw new Exception("API请求日志记录器未启用，无法获取API请求日志记录器");
                }
                return _apiRequestLogger;
            }
        }

        public static IApplicationLogger ApplicationLogger
        {
            get
            {
                if (!_isStart)
                {
                    throw new Exception("服务未启动，无法获取API请求日志记录器");
                }
                return _applicationLogger;
            }
        }
    }
}
