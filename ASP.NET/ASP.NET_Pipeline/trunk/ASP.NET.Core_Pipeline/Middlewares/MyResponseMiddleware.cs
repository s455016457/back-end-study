using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NET.Core_Pipeline.Middlewares
{
    public class MyResponseMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine("MyResponseMiddleware.InvokeAsync");
            string str = "This is response middleware message! \n The request query is:\n";
            foreach (var item in context.Request.Query)
            {
                str += $"{item.Key}:{item.Value};\n";
            }

            await context.Response.WriteAsync(str);
            await next.Invoke(context);
        }
    }
}
