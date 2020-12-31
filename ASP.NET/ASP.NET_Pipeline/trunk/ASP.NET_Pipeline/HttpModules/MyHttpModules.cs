using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_Pipeline.HttpModules
{
    public class MyHttpModules : IHttpModule
    {
        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
            context.AuthenticateRequest += Context_AuthenticateRequest;
            context.AuthorizeRequest += Context_AuthorizeRequest;
            context.ResolveRequestCache += Context_ResolveRequestCache;
            context.PostMapRequestHandler += Context_PostMapRequestHandler;
            context.AcquireRequestState += Context_AcquireRequestState;
            context.PreRequestHandlerExecute += Context_PreRequestHandlerExecute;
            context.UpdateRequestCache += Context_UpdateRequestCache;
            context.EndRequest += Context_EndRequest;
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication context = sender as HttpApplication;
            context.Response.Write($"<h3>EndRequest  在 ASP.NET 响应请求时作为 HTTP 执行管线链中的最后一个事件发生。{DateTime.Now.ToString()}</h3>");
        }

        private void Context_UpdateRequestCache(object sender, EventArgs e)
        {
            HttpApplication context = sender as HttpApplication;
            context.Response.Write($"<h3>UpdateRequestCache 当 ASP.NET 执行完事件处理程序以使缓存模块存储将用于从缓存为后续请求提供服务的响应时发生。{DateTime.Now.ToString()}</h3>");
        }

        private void Context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication context = sender as HttpApplication;
            context.Response.Write($"<h3>PreRequestHandlerExecute 恰好在 ASP.NET 开始执行事件处理程序（例如，某页或某个 XML Web services）前发生。{DateTime.Now.ToString()}</h3>");
        }

        private void Context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpApplication context = sender as HttpApplication;
            context.Response.Write($"<h3>AcquireRequestState 当 ASP.NET 获取与当前请求关联的当前状态（如会话状态）时发生。{DateTime.Now.ToString()}</h3>");
        }

        private void Context_PostMapRequestHandler(object sender, EventArgs e)
        {
            HttpApplication context = sender as HttpApplication;
            context.Response.Write($"<h3>PostMapRequestHandler 在 ASP.NET 已将当前请求映射到相应的事件处理程序时发生。{DateTime.Now.ToString()}</h3>");
        }

        private void Context_ResolveRequestCache(object sender, EventArgs e)
        {
            HttpApplication context = sender as HttpApplication;
            context.Response.Write($"<h3>ResolveRequestCache 处理缓存{DateTime.Now.ToString()}</h3>");
        }

        private void Context_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpApplication context = sender as HttpApplication;
            context.Response.Write($"<h3>AuthorizeRequest 请求授权{DateTime.Now.ToString()}</h3>");
        }

        private void Context_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication context = sender as HttpApplication;
            context.Response.Write($"<h3>AuthenticateRequest 请求认证{DateTime.Now.ToString()}</h3>");
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication context = sender as HttpApplication;
            context.Response.Write($"<h3>BeginRequest 请求前处理{DateTime.Now.ToString()}</h3>");
        }
    }
}