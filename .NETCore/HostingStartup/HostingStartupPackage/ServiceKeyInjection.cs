using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

[assembly:HostingStartup(typeof(HostingStartupPackage.ServiceKeyInjection))]
namespace HostingStartupPackage
{
    public class ServiceKeyInjection:IHostingStartup
    {
        public void Configure(IWebHostBuilder builder){
            Dictionary<string, string> dict;
            builder.ConfigureAppConfiguration((builderContext, config) =>
            {
                dict = new Dictionary<string, string>{
                    {"DevAccount_FromPackage","Dev_111-111_Package"},
                    {"ProdAcction_FromPackage","Prod_222-222_Package"},
                    {"ConfigurationPackageKey1","From IHostingStartup:比应用程序的配置优先级更高_Package"}
                };
                // 使用内存配置提供程序添加一对服务字符串到应用程序配置中
                config.AddInMemoryCollection(dict);
            });

            dict = new Dictionary<string, string>{
                {"ConfigurationPackageKey2","From IHostingStartup:应用程序的配置的优先级更高_Package"}
            };

            var builtConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(dict)
                .Build();

            builder.UseConfiguration(builtConfig);

            Console.WriteLine($"ServiceKeyInjection 执行：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }
    }
}