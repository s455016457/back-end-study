using HttpRequestLogger.Entities;
using System;
using System.Web;
using HttpRequestLogger.Repositories;
using Common.Logging;
using HttpRequestLogger.Events;
using HttpRequestLogger.Config;
using System.Reflection;

namespace HttpRequestLogger
{
    /// <summary>
    /// 请求日志Http模组
    /// </summary>
    public class RequestLoggerHttpModule : IHttpModule
    {
        ILog _log = LogManager.GetLogger<RequestLoggerHttpModule>();
        RequestLogger requestLog = null;
        const string _requestLogger_config = "HttpRequestLogger_RequestLoggerConfig";
        HttpRequestLoggerConfig httpRequestLoggerConfig;

        public void Dispose()
        {
            requestLog = null;
        }

        public void Init(HttpApplication context)
        {
            _log.Debug("初始化");
            httpRequestLoggerConfig = GetRequestLoggerConfig();
            context.BeginRequest += Context_BeginRequest;
            context.EndRequest += Context_EndRequest;
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            _log.Debug("开始执行 EndRequest");
            HttpApplication context = sender as HttpApplication;
            if (!RequestLoggerEventHandler.EndRequestLoggerHandler(context.Request, requestLog))
                return;

            try
            {
                if (requestLog != null) requestLog.EndRequest();
                using (var requestLoggerRepository = GetConnection())
                {
                    requestLoggerRepository.UpdateRequestLogger(requestLog);
                    requestLoggerRepository.SaveChange();
                }
                RequestLoggerEventHandler.AlfterEndRequestLoggerHandler(context.Request, requestLog);
            }catch(Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
            _log.Debug("结束执行 EndRequest");
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            _log.Debug("开始执行 BeginRequest");
            HttpApplication context = sender as HttpApplication;
            requestLog = RequestLogger.CreateRequestLog(context.Request);

            if (!RequestLoggerEventHandler.BeginRequestLoggerHandler(context.Request, requestLog))
                return;

            try
            {
                using (var requestLoggerRepository = GetConnection())
                {
                    requestLoggerRepository.AddRequestLogger(requestLog);
                    requestLoggerRepository.SaveChange();
                }
            }catch(Exception ex)
            {
                _log.Error(ex);
                throw ex;
            }
            RequestLoggerEventHandler.AlfterBeginRequestLoggerHandler(context.Request, requestLog);
            _log.Debug("结束执行 BeginRequest");
        }

        private IRequestLoggerRepository GetConnection()
        {
            if (HttpRequestLoggerConfig.CreateRequestLoggerRepositoryFunc != null)
                return HttpRequestLoggerConfig.CreateRequestLoggerRepositoryFunc();

            if (string.IsNullOrEmpty(httpRequestLoggerConfig.RepositoryFullName))
                return new DefaultRequestLoggerRepository();

            object obj = null;
            if (string.IsNullOrEmpty(httpRequestLoggerConfig.RepositoryAssemblyName))
            {
                obj = GetType().Assembly.CreateInstance(httpRequestLoggerConfig.RepositoryFullName);
            }
            else
            {
                var assmebly = Assembly.Load(httpRequestLoggerConfig.RepositoryAssemblyName);
                if (assmebly == null) throw new Exception($"加载程序集【{httpRequestLoggerConfig.RepositoryAssemblyName}】失败");
                obj = assmebly.CreateInstance(httpRequestLoggerConfig.RepositoryFullName);
            }
            if (obj is IRequestLoggerRepository)
                return obj as IRequestLoggerRepository;
            else
                throw new Exception($"【{obj.GetType().FullName}】必须实现【{typeof(IRequestLoggerRepository)}】接口！");
        }

        private HttpRequestLoggerConfig GetRequestLoggerConfig()
        {
            var obj = System.Web.Configuration.WebConfigurationManager.GetWebApplicationSection(_requestLogger_config);
            if (obj == null)
                return new HttpRequestLoggerConfig();
            return obj as HttpRequestLoggerConfig;
        }
    }
}
