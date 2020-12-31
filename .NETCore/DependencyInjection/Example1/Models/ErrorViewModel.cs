using System;
using System.IO;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Example1.Controllers;

namespace Example1.Models
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorViewModel
    {
        public string RequestId { get; private set; }
        public string ExceptionMessage { get; private set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public ErrorViewModel(HttpContext httpContext,ILogger<HomeController> logger){
            RequestId = Activity.Current?.Id ?? httpContext.TraceIdentifier;


            var exceptionHandlerPathFeature =
                httpContext.Features.Get<IExceptionHandlerPathFeature>();

            ExceptionMessage=exceptionHandlerPathFeature?.Error.Message??String.Empty;
            
            // 当发生错误时， Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware.Log已记录错误信息，无需手动配置错误信息记录
            // logger.LogError(exceptionHandlerPathFeature?.Error,ExceptionMessage);
            
            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            {
                ExceptionMessage += "File error thrown";
            }
            if (exceptionHandlerPathFeature?.Path == "/index")
            {
                ExceptionMessage += " from home page";
            }
        }
    }
}
