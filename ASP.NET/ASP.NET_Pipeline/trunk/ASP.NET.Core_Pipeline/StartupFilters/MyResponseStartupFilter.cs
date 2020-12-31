using ASP.NET.Core_Pipeline.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

namespace ASP.NET.Core_Pipeline.StartupFilters
{
    public class MyResponseStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                builder.UseMiddleware<MyResponseMiddleware>();
                next.Invoke(builder);
            };
        }
    }
}
