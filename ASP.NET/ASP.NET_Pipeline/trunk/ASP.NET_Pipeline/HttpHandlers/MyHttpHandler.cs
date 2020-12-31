using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_Pipeline.HttpHandlers
{
    public class MyHttpHandler : IHttpHandler
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write($"<h4>MyHttpHandler 我的http处理程序{DateTime.Now.ToString()}</h4>");
            foreach(DictionaryEntry item in context.Cache)
            {
                context.Response.Write($"<h4>MyHttpHandler 获取当前应用程序域的Cache {item.ToString()};{item.Key}:{item.Value}</h4>");
            }

            foreach (var item in context.Request.Cookies)
            {
                context.Response.Write($"<h4>MyHttpHandler 当前请求的cookie:{item}:{context.Request.Cookies[item.ToString()].Value}</h4>");
            }

            foreach (var item in context.Request.QueryString)
            {
                context.Response.Write($"<h4>MyHttpHandler 当前请求的QueryString:{item}:{context.Request.QueryString[item.ToString()]}</h4>");
            }
        }
    }
}