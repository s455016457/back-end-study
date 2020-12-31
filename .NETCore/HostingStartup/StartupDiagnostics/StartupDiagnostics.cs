using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceDescriptorsFactory;

/// <summary>
/// 使用托管启动属性来标识IHostingStartup的实现类
/// </summary>
[assembly:HostingStartup(typeof(StartupDiagnostics.StartupDiagnostics))]
namespace StartupDiagnostics
{
    /// <summary>
    /// 使用托管启动增强配置应用程序
    /// </summary>
    public class StartupDiagnostics:IHostingStartup
    {
        /// <summary>
        /// 使用托管启动增强配置IWebHostBuilder
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(IWebHostBuilder builder){
            Console.WriteLine("StartupDiagnostics.StartupDiagnosticsHostingStartup");

            builder.ConfigureServices(services=>{
                // 创建一个拥有GetServices方法的工厂，该方法可以在中间件中调用，来获取应用程序的服务列表。
                Func<IServiceProvider,IServiceDescriptorsService> factory=provider=>new ServiceDescriptorsService(services);

                // 将工厂注入服务容器中
                services.AddSingleton(factory);

                // 实现一个用于注册两个中间件组件的启动过滤器。
                services.AddSingleton<IStartupFilter, DiagnosticMiddlewareStartupFilter>();                
            });
        }
    }

    /// <summary>
    /// 用于诊断中间件的启动过滤器
    /// </summary>
    public class DiagnosticMiddlewareStartupFilter:IStartupFilter{
        /// <summary>
        /// 提供有关运行应用程序的Web托管环境的信息
        /// </summary>
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// 诊断中间件启动过滤器构造函数
        /// </summary>
        /// <param name="env">提供有关运行应用程序的Web托管环境的信息</param>
        public DiagnosticMiddlewareStartupFilter(IWebHostEnvironment env){
            _env=env;
        }
        /// <summary>
        /// 配置诊断中间件
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next){
            /***
                IApplicationBuilder:提供用于配置应用程序的请求管道的机制
            **/
            return app=>{
                //添加路由
                // 将Microsoft.AspNetCore.Routing.EndpointRoutingMiddleware中间件添加到指定的IApplicationBuilder。
                app.UseRouting();

                /***
                使用终端中间件在请求/services上进行分支。
                中间件使用一个工厂来获取为应用程序注册的服务，并将其输出到一个页面中。
                endpoints:IEndpointRouteBuilder 为应用程序中的路径构建器定义一个约定。路由构建器为应用程序指定路由。
                ***/
                app.UseEndpoints(endpoints=>{
                    // /services请求分支
                    // 添加一个RouteEndpoint到IEndpointRouteBuilder，它为指定的模版匹配Http Get请求
                    // MapGet：第一个参数字符串类型，终端匹配的模版；第二个参数：RequestDelegate(需要一个HttpContext参数)，一个可以处理Http请求的委托。当终端匹配时执行该委托。
                    // context : HttpContext
                    endpoints.MapGet("/services",async context=>{
                        // 非开发模式跳出分支
                        if(!_env.IsDevelopment()){
                    return;
                        }
                        
                        // 构建分支页面
                        var sb = new StringBuilder(@"
                            <!DOCTYPE html><html lang=""en""><head><title>All Service</title>
                            <style>body{font-family:Verdana,Geneval,sans-serif;font-size:.8em}
                            li{padding-button:10px}</style></head></body>
                            <h1>All Services</h1>
                            <ul>
                        ");

                        // context.RequestServices:获取或设置IServiceProvider，它提供对请求的服务容器的访问。
                        // IServiceProvider.GetService  获取指定的服务对象
                        var serviceDescriptorsService = context.RequestServices.GetService<IServiceDescriptorsService>();

                        foreach(var service in serviceDescriptorsService.GetServices()){
                    sb.Append($"<li><b>{service.FullName}<b>({service.Lifetime})");
                    if(!string.IsNullOrEmpty(service.ImplementationType)){
                        sb.Append($"<br>{service.ImplementationType}</li>");
                    }else{
                         sb.Append($"</li>");
                    }
                        }

                        sb.Append("</ul></body></html>");

                        await context.Response.WriteAsync(sb.ToString());
                    });
                });
                
                // 应用程序添加诊断中间件
                app.UseMiddleware<DiagnosticMiddleware>();

                next(app);
            };
        }
    }

    /// <summary>
    /// 诊断中间件
    /// </summary>
    public class DiagnosticMiddleware{
        // 一个可以处理Http请求的委托。当终端匹配时执行该委托
        private readonly RequestDelegate _next;
        // 日志工厂
        private readonly ILoggerFactory _loggerFactory;
        // Web主机环境
        private readonly IWebHostEnvironment _env;
        // 换行字符
        // Enviroment 提供关于当前环境和平台的信息以及操作方法。
        private string cr = Environment.NewLine;

        public DiagnosticMiddleware(RequestDelegate next,ILoggerFactory loggerFactory,IWebHostEnvironment env){
            _next=next;
            _loggerFactory=loggerFactory;
            _env=env;
        }

        public async Task Invoke(HttpContext ctx){
            var path = ctx.Request.Path;

            if(path!="/diag"||!_env.IsDevelopment()){
                await _next(ctx);
            }else{
                var logger = _loggerFactory.CreateLogger("Request");

                logger.LogDebug("Received request:{Method} {Path}",ctx.Request.Method,ctx.Request.Path);

                ctx.Response.ContentType="text/html";

                // 构建页面
                var sb = new StringBuilder(@"
                    <!DOCTYPE html><html lang=""zh-cn""><head><title>Diag</title>
                    <style>body{font-family:Verdana,Geneval,sans-serif;font-size:.8em}
                    li{padding-button:10px}</style></head><body>
                    <h1>Diag</h1>
                    <pre>
                ");

                sb.Append($"{DateTimeOffset.Now}{cr}{cr}");
                sb.Append($"Address:{cr}{cr}");
                sb.Append($"Scheme:{ctx.Request.Scheme}{cr}");
                sb.Append($"Host:{ctx.Request.Headers["Host"]}{cr}");
                sb.Append($"PathBase:{ctx.Request.PathBase.Value}{cr}");
                sb.Append($"Path:{ctx.Request.Path.Value}{cr}");
                sb.Append($"Query:{ctx.Request.QueryString.Value}{cr}{cr}");
                sb.Append($"Connection:{cr}{cr}");
                sb.Append($"RemoteIp:{ctx.Connection.RemoteIpAddress}{cr}");
                sb.Append($"RemotePort:{ctx.Connection.RemotePort}{cr}");
                sb.Append($"LocalIp:{ctx.Connection.LocalIpAddress}{cr}");
                sb.Append($"LocalPort:{ctx.Connection.LocalPort}{cr}");
                sb.Append($"ClientCert:{ctx.Connection.ClientCertificate}{cr}{cr}");
                sb.Append($"Headers:{cr}{cr}");
                
                foreach(var header in ctx.Request.Headers){
                    sb.Append($"{header.Key}:{header.Value}{cr}");
                }

                sb.Append($"{cr}Environment Variables:{cr}{cr}");

                var vars = Environment.GetEnvironmentVariables();

                foreach(var key in vars.Keys.Cast<string>().OrderBy(key=>key,StringComparer.OrdinalIgnoreCase)){
                    sb.Append($"{key}:{vars[key]}{cr}");
                }

                sb.Append("</pre></body></html>");

                await ctx.Response.WriteAsync(sb.ToString());
            }
        }
    }
}
