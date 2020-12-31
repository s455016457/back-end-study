using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

namespace CookiesAuthentication
{
    public class Startup
    {
        public const string CookieScheme = "MySchemeName";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // 添加身份验证服务，设置默认的cookie方案
            services.AddAuthentication(CookieScheme)
                .AddCookie(CookieScheme,options=>{
                    options.AccessDeniedPath="/account/denied";
                    options.LoginPath="/account/login";
                });

            // 演示如何定制cookie选项的特定实例，而且他还可以使用其他服务。
            services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, ConfigureMyCookie>();

            services.AddAuthorization(Options=>{
                Options.DefaultPolicy=new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder(CookieScheme).RequireAuthenticatedUser().Build();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            // 添加身份验证中间件
            app.UseAuthentication();
            // 添加授权中间件
            app.UseAuthorization();

            //app.

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
