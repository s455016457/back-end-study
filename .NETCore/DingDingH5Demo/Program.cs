using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DingDingH5Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 创建主机生成器，生成并运行主机
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args) //创建默认生成器
                // 配置Web主机默认属性
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // 指定web主机要使用的启动类型。
                    webBuilder.UseStartup<Startup>();
                });
    }
}
