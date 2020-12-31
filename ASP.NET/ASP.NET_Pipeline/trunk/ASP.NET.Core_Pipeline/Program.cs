using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASP.NET.Core_Pipeline
{
    public class Program
    {
        public static Dictionary<string, string> arrayDict = new Dictionary<string, string>
        {
            {"array:entries:0", "value0"},
            {"array:entries:1", "value1"},
            {"array:entries:2", "value2"},
            {"array:entries:4", "value4"},
            {"array:entries:5", "value5"}
        };

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                // 配置App配置文件
                .ConfigureAppConfiguration(config =>
                {
                    // 设置配置文件路径
                    config.SetBasePath(AppContext.BaseDirectory);
                    // 内存配置
                    config.AddInMemoryCollection(arrayDict);
                    // json文件配置
                    config.AddJsonFile("json_array.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile("starship.json", optional: false, reloadOnChange: true);
                    // xml文件配置
                    config.AddXmlFile("tvshow.xml", optional: true, reloadOnChange: true);
                    // 添加一个Microsoft.Extensions.Configuration.IConfigurationProvider
                    // 它从命令行中读取配置
                    config.AddCommandLine(args);
                })
                .UseStartup<Startup>()
                .ConfigureLogging(log =>
                {
                    log.ClearProviders();
                    log.AddConsole();
                    log.AddDebug();
                });
    }
}
