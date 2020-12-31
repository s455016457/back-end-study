using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using Unity_MVC.App_Start;
using Unity;
using Unity_MVC.DDD.Services.Interfaces;

namespace Unity_MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        internal static ISingletonService SingletonService;
        ILog log = LogManager.GetLogger<MvcApplication>();
        protected void Application_Start()
        {
            log.Info($"系统物理文件地址：{HttpRuntime.AppDomainAppPath}");
            log.Info($"系统虚拟文件地址：{HttpRuntime.AppDomainAppVirtualPath}");
            log.Info($"系统应用程序标识：{HttpRuntime.AppDomainAppId}");
            log.Info($"系统实例所在应用程序域的域标识：{HttpRuntime.AppDomainId}");
            log.Info($"系统Bin文件夹地址：{HttpRuntime.BinDirectory}");
            log.Info($"IIS版本：{HttpRuntime.IISVersion}");
            log.Info($" .NET Framework 版本：{HttpRuntime.TargetFramework}");
            log.Info($" 程序域基目录：{AppDomain.CurrentDomain.BaseDirectory}");
            if (Site != null)
            {
                log.Info($" 站点名称：{Site.Name}");
                log.Info($" 站点为设计模式：{Site.DesignMode}");
            }

            AreaRegistration.RegisterAllAreas();
            log.Debug("Application_Start: 注册 ASP.NET MVC 应用程序中的所有区域。");
            UnityConfig.RegisterComponents();                           // <----- Add this line
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            log.Debug("Application_Start: 注册 默认路由。");

            log.Debug("Application_Start: 设置控制器工厂类。");
            // 设置控制器工厂类
            ControllerBuilder.Current.SetControllerFactory(typeof(MyControllerFactory));

            log.Debug("Application_Start: 设置过滤器");

            GlobalFilters.Filters.Add(new App_Start.Filters.ActionFilter());
            GlobalFilters.Filters.Add(new App_Start.Filters.AuthenticationFilter());
            GlobalFilters.Filters.Add(new App_Start.Filters.AuthorizationFilter());
            GlobalFilters.Filters.Add(new App_Start.Filters.ResultFilter());
            GlobalFilters.Filters.Add(new App_Start.Filters.MyActionFilterAttribute());
        }

        public override void Init()
        {
            log.Debug("MvcApplication.Int");
            log.Debug($"MvcApplication.Int：已注册的HttpModule{new JavaScriptSerializer().Serialize(Modules)}");
            log.Debug($"MvcApplication.Int：已注册的委托事件{new JavaScriptSerializer().Serialize(Events)}");

            PreSendRequestHeaders += MvcApplication_PreSendRequestHeaders;
            MapRequestHandler += MvcApplication_MapRequestHandler;
            Disposed += MvcApplication_Disposed;
            BeginRequest += MvcApplication_BeginRequest;
            AuthenticateRequest += MvcApplication_AuthenticateRequest;
            PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
            AuthorizeRequest += MvcApplication_AuthorizeRequest;
            PostAuthorizeRequest += MvcApplication_PostAuthorizeRequest;
            ResolveRequestCache += MvcApplication_ResolveRequestCache;
            PostResolveRequestCache += MvcApplication_PostResolveRequestCache;
            PreSendRequestContent += MvcApplication_PreSendRequestContent;
            PostMapRequestHandler += MvcApplication_PostMapRequestHandler;
            PostLogRequest += MvcApplication_PostLogRequest;
            RequestCompleted += MvcApplication_RequestCompleted;
            PostAcquireRequestState += MvcApplication_PostAcquireRequestState;
            PreRequestHandlerExecute += MvcApplication_PreRequestHandlerExecute;
            PostRequestHandlerExecute += MvcApplication_PostRequestHandlerExecute;
            ReleaseRequestState += MvcApplication_ReleaseRequestState;
            PostReleaseRequestState += MvcApplication_PostReleaseRequestState;
            UpdateRequestCache += MvcApplication_UpdateRequestCache;
            PostUpdateRequestCache += MvcApplication_PostUpdateRequestCache;
            LogRequest += MvcApplication_LogRequest;
            AcquireRequestState += MvcApplication_AcquireRequestState;
            EndRequest += MvcApplication_EndRequest;
            Error += MvcApplication_Error;
            base.Init();
            log.Debug($"MvcApplication.Int：已注册的HttpModule{new JavaScriptSerializer().Serialize(Modules)}");
            log.Debug($"MvcApplication.Int：已注册的委托事件{new JavaScriptSerializer().Serialize(Events)}");

            SingletonService = UnityConfig.container.Resolve<ISingletonService>();

            SingletonService.ToDoSomeThing();
        }

        #region application event
        private void MvcApplication_Error(object sender, EventArgs e)
        {
            log.Debug("Error：当引发未处理的异常时发生");
        }

        private void MvcApplication_EndRequest(object sender, EventArgs e)
        {
            log.Debug("EndRequest： 作为执行的 HTTP 管道链中的最后一个事件发生，当 ASP.NET 的请求做出响应。");
        }

        private void MvcApplication_AcquireRequestState(object sender, EventArgs e)
        {
            LogDebug("AcquireRequestState：  当 ASP.NET 获取与当前的请求相关联的当前状态 （例如，会话状态）。");
        }

        private void MvcApplication_LogRequest(object sender, EventArgs e)
        {
            LogDebug("LogRequest：ASP.NET 执行当前请求的任何日志记录之前发生。");
        }

        private void MvcApplication_PostUpdateRequestCache(object sender, EventArgs e)
        {
            LogDebug("PostUpdateRequestCache：当 ASP.NET 完成更新的缓存模块和存储用于为从缓存中的后续请求提供服务的响应时发生。");
        }

        private void MvcApplication_UpdateRequestCache(object sender, EventArgs e)
        {
            LogDebug("UpdateRequestCache：当 ASP.NET 完成执行事件处理程序，以便让缓存模块存储将用于为从缓存中的后续请求提供服务的响应时发生。");
        }

        private void MvcApplication_PostReleaseRequestState(object sender, EventArgs e)
        {
            LogDebug("PostReleaseRequestState：当 ASP.NET 已完成执行所有请求事件处理程序和存储数据的请求状态时发生。");
        }

        private void MvcApplication_ReleaseRequestState(object sender, EventArgs e)
        {
            LogDebug("ReleaseRequestState：ASP.NET 完成执行所有请求事件处理程序后发生。 此事件会导致状态模块保存当前的状态数据。");
        }

        private void MvcApplication_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            LogDebug("PostRequestHandlerExecute:当 ASP.NET 事件处理程序 （例如，一个页面或 XML Web 服务） 完成执行时发生。");
        }

        private void MvcApplication_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            LogDebug("PreRequestHandlerExecute: ASP.NET 开始执行事件处理程序 （例如，一个页面或 XML Web 服务） 之前发生。");
        }

        private void MvcApplication_PostAcquireRequestState(object sender, EventArgs e)
        {
            LogDebug("PostAcquireRequestState:获取与当前的请求相关联的请求状态 （例如，会话状态） 时发生。");
        }

        private void MvcApplication_RequestCompleted(object sender, EventArgs e)
        {
            LogDebug("RequestCompleted:ASP.NET 已释放与请求相关联的托管的对象时发生。");
        }

        private void MvcApplication_PostLogRequest(object sender, EventArgs e)
        {
            LogDebug("PostLogRequest:当 ASP.NET 已完成处理的事件处理程序时发生 System.Web.HttpApplication.LogRequest 事件。");
        }

        private void MvcApplication_PostMapRequestHandler(object sender, EventArgs e)
        {
            LogDebug("PostMapRequestHandler:ASP.NET 已映射到相应的事件处理程序的当前请求时出现。");
        }

        private void MvcApplication_PreSendRequestContent(object sender, EventArgs e)
        {
            LogDebug("PreSendRequestContent:ASP.NET 将内容发送到客户端之前发生。");
        }

        private void MvcApplication_PostResolveRequestCache(object sender, EventArgs e)
        {
            LogDebug("PostResolveRequestCache:   ASP.NET 将绕过当前事件处理程序的执行，并允许缓存模块以处理从缓存请求时发生。");
        }

        private void MvcApplication_ResolveRequestCache(object sender, EventArgs e)
        {
            LogDebug("ResolveRequestCache:在 ASP.NET 完成授权事件以使缓存模块从缓存中为请求提供服务后发生，从而绕过事件处理程序（例如某个页或 XML Web services）的执行。");
        }

        private void MvcApplication_PostAuthorizeRequest(object sender, EventArgs e)
        {
            LogDebug("PostAuthorizeRequest:当前请求的用户已被授权时发生");
        }

        private void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        {
            LogDebug("AuthorizeRequest:安全模块已验证用户身份验证时发生");
        }

        private void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        {
            LogDebug("PostAuthenticateRequest:当安全模块已建立的用户标识时出现");
        }

        private void MvcApplication_AuthenticateRequest(object sender, EventArgs e)
        {
            LogDebug("AuthenticateRequest:当安全模块已建立的用户标识时出现");
        }

        private void MvcApplication_BeginRequest(object sender, EventArgs e)
        {
            LogDebug("BeginRequest: 作为执行的 HTTP 管道链中的第一个事件发生，当 ASP.NET 的请求做出响应", "BeginRequest");

            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine($"GetOutputCacheProviderName:{GetOutputCacheProviderName(Context)}");
            stringBuilder.AppendLine($"GetVaryByCustomString:{GetVaryByCustomString(Context, "browser")}");
        }

        private void MvcApplication_Disposed(object sender, EventArgs e)
        {
            LogDebug("Disposed:应用程序销毁时触发");
        }

        private void MvcApplication_MapRequestHandler(object sender, EventArgs e)
        {
            LogDebug("MapRequestHandler:应用程序映射请求处理程(RequestHandler)序时触发");
        }

        private void MvcApplication_PreSendRequestHeaders(object sender, EventArgs e)
        {
            LogDebug("PreSendRequestHeaders:ASP.NET 将 HTTP 请求头发送到客户端之前触发。");
        }
        #endregion

        private void LogDebug(object message, string typeName = "")
        {
            if (!string.IsNullOrWhiteSpace(typeName))
                log.Debug($"=====================BEGIN {typeName}=============================");

            log.Debug(message);

            if (!string.IsNullOrWhiteSpace(typeName))
                log.Debug($"=====================END {typeName}=============================");

            if (Context == null || Context.Response == null)
                return;
            if (Context.Response.OutputStream.CanWrite)
            {
                var array = Context.Response.HeaderEncoding.GetBytes($"<div class='log'>{message.ToString()}</div>");
                Context.Response.OutputStream.Write(array, 0, array.Length);
            }
        }

        public override void Dispose()
        {
            LogDebug("Dispose 应用程序注销");
            base.Dispose();
        }
    }
}
