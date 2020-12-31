using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Example1.DDD.Repostory;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Example1
{
    public class Startup
    {
        public Startup(IConfiguration configuration
            ,IWebHostEnvironment webEnvironment
            ,IHostEnvironment environment)
        {
            Configuration = configuration;
            WebHostEnvironment=webEnvironment;
           HostEnvironment=environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment{get;}
        public IHostEnvironment HostEnvironment{get;}
        
        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 运行时调用，使用该方法添加服务到集合
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加控制器和视图
            services.AddControllersWithViews();
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}执行ConfigureServices");
            
            // using Microsoft.EntityFrameworkCore; 添加数据库配置
            services.AddDbContext<MyDbContext>(options =>{
                options.UseSqlServer(Configuration.GetConnectionString("MyContext"));
                });

            // 添加身份验证提供程序
            // using Microsoft.AspNetCore.Authentication.Cookies;
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    // using Microsoft.AspNetCore.Http;
                    options.LoginPath = new PathString("/Account/Login");
                    options.LogoutPath = new PathString("/Account/Logout");
                    options.AccessDeniedPath = new PathString("/Account/AccessDenied");
                
                    options.ReturnUrlParameter = "ReturnUrl";

                    // 配置中的设置覆盖系统内置的设置
                    Configuration.Bind("CookieSettings", options);
                
                    // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-3.1#react-to-back-end-changes                    
                    // options.EventsType = typeof(CustomCookieAuthenticationEvents);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 运行时调用，使用这个方法配置Http请求管道
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory">日志工厂</param>
        /// <param name="logger"></param>
        public void Configure(IApplicationBuilder app
            , IWebHostEnvironment env
            ,ILoggerFactory loggerFactory
            ,ILogger<Startup> logger)
        {
            loggerFactory.AddLog4Net();

            Console.WriteLine("执行Configure");
            logger.LogDebug("执行Configure");
            logger.LogInformation("执行Configure");

            LogHostInformation(logger);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // 使用身份验证
            app.UseAuthentication();
            // 使用授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void LogHostInformation(ILogger<Startup> logger){
            logger.LogDebug($"Configuration.AsEnumerable()：{Newtonsoft.Json.JsonConvert.SerializeObject(Configuration.AsEnumerable())}");

            logger.LogDebug($"WebHostEnvironment.ApplicationName:{WebHostEnvironment.ApplicationName}");
            logger.LogDebug($"WebHostEnvironment..ContentRootFileProvider.GetType().FullName:{WebHostEnvironment.ContentRootFileProvider.GetType().FullName}");
            logger.LogDebug($"WebHostEnvironment.ContentRootPath:{WebHostEnvironment.ContentRootPath}");
            logger.LogDebug($"WebHostEnvironment.EnvironmentName:{WebHostEnvironment.EnvironmentName}");
            logger.LogDebug($"WebHostEnvironment.IsDevelopment():{WebHostEnvironment.IsDevelopment()}");
            logger.LogDebug($"WebHostEnvironment.IsEnvironment(WebHostEnvironment.EnvironmentName):{WebHostEnvironment.IsEnvironment(WebHostEnvironment.EnvironmentName)}");
            logger.LogDebug($"WebHostEnvironment.IsProduction():{WebHostEnvironment.IsProduction()}");
            logger.LogDebug($"WebHostEnvironment.IsStaging():{WebHostEnvironment.IsStaging()}");
            logger.LogDebug($"WebHostEnvironment.WebRootFileProvider.GetType().FullName:{WebHostEnvironment.WebRootFileProvider.GetType().FullName}");
            logger.LogDebug($"WebHostEnvironment.WebRootPath:{WebHostEnvironment.WebRootPath}");

            logger.LogDebug($"HostEnvironment.ApplicationName：{HostEnvironment.ApplicationName}");
            logger.LogDebug($"HostEnvironment.ContentRootFileProvider{HostEnvironment.ContentRootFileProvider.GetType().FullName}");
            logger.LogDebug($"HostEnvironment.ContentRootPath{HostEnvironment.ContentRootPath}");
            logger.LogDebug($"HostEnvironment.EnvironmentName{HostEnvironment.EnvironmentName}");
            logger.LogDebug($"HostEnvironment.IsDevelopment(){HostEnvironment.IsDevelopment()}");
            logger.LogDebug($"HostEnvironment.IsEnvironment(HostEnvironment.EnvironmentName){HostEnvironment.IsEnvironment(HostEnvironment.EnvironmentName)}");
            logger.LogDebug($"HostEnvironment.IsProduction(){HostEnvironment.IsProduction()}");
            logger.LogDebug($"HostEnvironment.IsStaging():{HostEnvironment.IsStaging()}");
        }
    }
}
