using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NetCoreWebAppDemo
{
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">应用程序配置</param>
        /// <param name="webHostEnvironment">web主机环境</param>
        /// <returns></returns>
        public Startup(IConfiguration configuration,IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment=webHostEnvironment;
        }
        /// <summary>
        /// 应用程序配置
        /// </summary>
        /// <value></value>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// web主机环境
        /// </summary>
        public IWebHostEnvironment WebHostEnvironment{get;}

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 配置服务
        /// 该方法在运行时调用
        /// 在该方法中添加服务到服务容器中
        /// </summary>
        /// <param name="services">服务容器</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 应用程序配置
        /// 该方法在运行时调用
        /// 在该方法配置Http请求管道
        /// </summary>
        /// <param name="app">应用程序生成器</param>
        /// <param name="env">Web主机环境</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // 开发人员异常页
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // 异常处理
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // http严格传输安全协议管道，让浏览器强制使用HTTPS与网站进行通信
                app.UseHsts();
            }

            // HTTP请求重定向到HTTPS请求管道
            app.UseHttpsRedirection();
            // 静态文件管道，为当前请求路径启用静态文件服务
            app.UseStaticFiles();
            // 路由管道
            app.UseRouting();
            // 授权管道
            app.UseAuthorization();
            // 将Razor页端点添加到断点路由中
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
