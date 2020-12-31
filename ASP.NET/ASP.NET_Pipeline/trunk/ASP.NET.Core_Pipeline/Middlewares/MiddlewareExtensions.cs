using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET.Core_Pipeline.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMyRequestSetOptionsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyRequestSetOptionsMiddleware>();
        }
        public static IApplicationBuilder UseMyResponseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyResponseMiddleware>();
        }
        /// <summary>
        /// 使用请求日志中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequestLogerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLogerMiddleware>();
        }
    }
}
