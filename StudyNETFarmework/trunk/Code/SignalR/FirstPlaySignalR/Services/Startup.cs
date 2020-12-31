using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(FirstPlaySignalR.Services.Startup))]
namespace FirstPlaySignalR.Services
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            // 任何连接或集线器连接和配置都应该在这里
            app.MapSignalR();
        }
    }
}