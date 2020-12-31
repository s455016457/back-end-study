using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET.Core_Pipeline.Middlewares
{
    /// <summary>
    /// 请求日志中间件
    /// </summary>
    public class RequestLogerMiddleware : IMiddleware
    {        
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine(" RequestLogerMiddleware: do loger request");
            return next.Invoke(context);
        }
    }
}
