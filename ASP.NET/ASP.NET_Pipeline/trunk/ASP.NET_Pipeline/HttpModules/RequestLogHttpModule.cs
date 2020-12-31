using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASP.NET_Pipeline.Extensions;

namespace ASP.NET_Pipeline.HttpModules
{
    public class RequestLogHttpModule : IHttpModule
    {
        RequestLog requestLog = null;
        public void Dispose()
        {
            requestLog = null;
        }

        public void Init(HttpApplication context)
        {
            // 系统初始化时执行一次
            context.BeginRequest += Context_BeginRequest;
            context.EndRequest += Context_EndRequest;
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication context = sender as HttpApplication;
            if (requestLog != null) requestLog.EndRequest();
            context.Response.Write($"<h3>RequestLogHttpModule EndRequest 请求结束处理{DateTime.Now.ToString()}</h3>");
            context.Response.Write($"<h3>RequestLogHttpModule EndRequest RequestLog:{requestLog.ToJson()}</h3>");
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication context = sender as HttpApplication;
            requestLog = RequestLog.CreateRequestLog(context.Request);
            context.Response.Write($"<h3>RequestLogHttpModule BeginRequest 请求开始处理{DateTime.Now.ToString()}</h3>");
            context.Response.Write($"<h3>RequestLogHttpModule BeginRequest RequestLog:{requestLog.ToJson()}</h3>");
        }
    }

    public class RequestLog
    {
        public Guid Id { get; private set; }

        public string Url { get; private set; }

        public string Method { get; private set; }

        public DateTime Stamp { get; private set; }

        public bool IsFile { get; private set; }

        public string Ip { get; private set; }

        public long TimeConsuming { get; private set; }
        /// <summary>
        /// 创建一个请求日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static RequestLog CreateRequestLog(HttpRequest request)
        {
            return new RequestLog
            {
                Id=Guid.NewGuid(),
                Url =request.Url.AbsolutePath,
                Method=request.HttpMethod,
                IsFile=request.Url.IsFile,
                Stamp=DateTime.Now,
                Ip=request.GetIp()
            };
        }
        /// <summary>
        /// 请求结束
        /// </summary>
        public void EndRequest()
        {
            this.TimeConsuming = DateTime.Compare(DateTime.Now, Stamp);
        }
    }
}