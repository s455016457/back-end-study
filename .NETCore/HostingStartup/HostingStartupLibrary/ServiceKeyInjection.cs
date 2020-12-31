using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;

[assembly:HostingStartup(typeof(HostingStartupLibrary.ServiceKeyInjection))]
namespace HostingStartupLibrary
{
    public class ServiceKeyInjection : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            Dictionary<string, string> dict;
            builder.ConfigureAppConfiguration((builderContext, config) =>
            {
                dict = new Dictionary<string, string>{
                    {"DevAccount_FromLibrary","Dev_111-111"},
                    {"ProdAcction_FromLibrary","Prod_222-222"},
                    {"ConfigurationKey1","From IHostingStartup:比应用程序的配置优先级更高"}
                };
                // 使用内存配置提供程序添加一对服务字符串到应用程序配置中
                config.AddInMemoryCollection(dict);
            });

            dict = new Dictionary<string, string>{
                {"ConfigurationKey2","From IHostingStartup:应用程序的配置的优先级更高"}
            };

            var builtConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(dict)
                .Build();

            builder.UseConfiguration(builtConfig);

            Console.WriteLine($"ServiceKeyInjection 执行：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }
    }
}