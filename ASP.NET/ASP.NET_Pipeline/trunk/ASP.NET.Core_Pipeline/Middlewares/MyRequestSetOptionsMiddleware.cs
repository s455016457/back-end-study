using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ASP.NET.Core_Pipeline.Middlewares
{
    public class MyRequestSetOptionsMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine("RequestSetOptionsMiddleware.InvokeAsync");

            // do save request history

            var option = context.Request.Query["option"];
            var newQuery = new Dictionary<string, StringValues>();

            if (string.IsNullOrWhiteSpace(option))
            {
                newQuery.Add("option", "request default query value TestOption");
            }
            foreach (var item in context.Request.Query)
            {
                newQuery.Add(item.Key, new StringValues(WebUtility.HtmlEncode(item.Value)));
            }
            context.Request.Query = new QueryCollection(newQuery);
            await next(context);
        }
    }
}
