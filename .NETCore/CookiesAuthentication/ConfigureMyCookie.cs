using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace CookiesAuthentication
{
    internal class ConfigureMyCookie:IConfigureNamedOptions<CookieAuthenticationOptions>
    {        
        // 可以在这里注入服务
        public ConfigureMyCookie()
        {
        }

        public void Configure(string name, CookieAuthenticationOptions options)
        {
            // 只配置您想要的方案
            if (name == Startup.CookieScheme)
            {
                // options.LoginPath = "/someotherpath";
                options.ExpireTimeSpan=TimeSpan.FromHours(1);
            }
        }

        public void Configure(CookieAuthenticationOptions options)
            => Configure(Options.DefaultName, options);
    }
}