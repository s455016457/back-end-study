using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ASP.NET.Core_Pipeline.Middlewares;
using Microsoft.Extensions.Configuration;
using ASP.NET.Core_Pipeline.configeEntities;
using ASP.NET.Core_Pipeline.Extensions;

namespace ASP.NET.Core_Pipeline
{
    public class Startup
    {
        private readonly IConfiguration _config;
        Starship starship = new Starship();
        JsonArray jsonArray = new JsonArray();
        TvShow tvshow = new TvShow();

        public Startup(IConfiguration config)
        {
            _config = config;

            _config.GetSection("json_array").Bind(jsonArray);
            _config.GetSection("starship").Bind(starship);
            _config.GetSection("tvshow").Bind(tvshow);

            Console.WriteLine("Startup:");
            ConsoleExtensions.WriteObject(jsonArray, "jsonArray");
            ConsoleExtensions.WriteObject(starship, "starship");
            ConsoleExtensions.WriteObject(tvshow, "tvshow");

            Console.WriteLine(jsonArray.ToJson());
            Console.WriteLine(starship.ToJson());
            Console.WriteLine(tvshow.ToJson());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        // 添加服务到容器
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加自定义中间件服务   请求
            services.AddTransient<MyRequestSetOptionsMiddleware>();
            // 添加自定义中间件服务   响应
            services.AddTransient<MyResponseMiddleware>();
            services.AddTransient<RequestLogerMiddleware>();
            // 添加MVC中间件服务
            services.AddMvc();
            // 添加session中间件服务
            services.AddSession();
            // 添加响应缓存中间件服务
            services.AddResponseCaching();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // 这个方法在运行时调用，使用这个方法去配置http请求管道
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            /**
             *  检查当前主机环境名是否为 Microsoft.AspNetCore.Hosting.EnvironmentName.Development.
             *  
             * */
            if (env.IsDevelopment())
            {
                /**
                 * 当应用程序运行在开发环境中
                 * 使用Developer Exception Page 报告App运行时异常
                 * 使用Database Error Page 报告Database运行时异常
                 * */
                // 从管道中捕获同步和异步异常，并生成异常Html响应
                app.UseDeveloperExceptionPage();
                // 可以从使用EntityFarmeWork迁移解决的管道中捕获与数据库相关的同步和异步异常。
                // 当这些异常发生时，将生成一个HTML响应，其中包含解决问题的可能操作的详细信息。
                app.UseDatabaseErrorPage();
            }
            else
            {
                /***
                 * 当应用程序没有运行在开发环境中
                 * 使用异常处理中间件捕获下面中间件的异常。
                 * */
                app.UseExceptionHandler("error");
                /**
                 * 使用HTTP严格传输安全协议(HSTS)中间件。
                 * */
                app.UseHsts();
            }
            // 使用HTTPS 重定向中间件去重定向 HTTP 请求到 HTTPS.
            app.UseHttpsRedirection();
            /**
             * Return static files and end the pipeline.
             * 返回静态文件并结束管道
             * */
            app.UseStaticFiles();

            /** 
             * Use Cookie Policy Middleware to conform to EU General Data Protection Regulation (GDPR) regulations.
             * 使用Cookie策略中间件以符合欧盟通用数据保护法规(GDPR)的规定。
             * */
            app.UseCookiePolicy();
            /**
             * Authenticate before the user accesses secure resources.
             * 在用户访问安全资源之前进行身份验证。
             * 添加Microsoft.AspNetCore.Authentication.AuthenticationMiddleware 到指定的 Microsoft.AspNetCore.Builder.IApplicationBuilder
             * 它支持身份验证功能。
             * */
            app.UseAuthentication();
            // If the app uses session state, call Session Middleware after Cookie 
            // Policy Middleware and before MVC Middleware.
            // 如果App使用了session状态，则在MVC中间件之前，cookie中间件之后调用session中间件
            app.UseSession();
            // Add MVC to the request pipeline.
            // 添加MVC到请求管道中
            app.UseMvc();



            // 添加响应缓存中间件
            app.UseResponseCaching();
            // 添加MVC到请求管道中，使用默认路由器 例如: '{controller=Home}/{action=Index}/{id?}'
            app.UseMvcWithDefaultRoute();

            /**
             * Map 扩展用作约定来创建管道分支。 Map* 基于给定请求路径的匹配项来创建请求管道分支。
             * 如果请求路径以给定路径开头，则执行分支。
             * */
            app.Map("/map1", HandleMapTest1);
            app.Map("/map2", HandleMapTest2);
            app.Map("/map/map1", HandleMapTest1);
            app.Map("/map/map2", HandleMapTest2);



            // 请求路径包含branch参数则执行
            app.MapWhen(context => context.Request.Query.ContainsKey("branch"),
                              HandleBranch);

            /***
             * Map 支持嵌套，例如：
             * */
            //app.Map("/level1", level1App => {
            //    level1App.Map("/level2a", level2AApp => {
            //        // "/level1/level2a" processing
            //    });
            //    level1App.Map("/level2b", level2BApp => {
            //        // "/level1/level2b" processing
            //    });
            //});

            // 用 Use 将多个请求委托链接在一起。
            // next 参数表示管道中的下一个委托。 可通过不调用 next 参数使管道短路。
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("This is first request delegate! \n");
                await next.Invoke();
            });

            // 使用自定义中间件
            app.UseRequestLogerMiddleware();
            app.UseMyRequestSetOptionsMiddleware();
            app.UseMyResponseMiddleware();

            app.Run(async (context) =>
            {
                Console.WriteLine("Hello World!");
                await context.Response.WriteAsync("Hello World!");
            });

        }

        private static void HandleMapTest1(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 1");
            });
        }
        private static void HandleMapTest2(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 2");
            });
        }
        private static void HandleBranch(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var branchVer = context.Request.Query["branch"];
                await context.Response.WriteAsync($"Branch used = {branchVer}");
            });
        }
    }
}
