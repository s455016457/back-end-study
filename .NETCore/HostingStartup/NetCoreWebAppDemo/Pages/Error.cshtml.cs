using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace NetCoreWebAppDemo.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        /// <summary>
        /// 当前请求在跟踪日志中的标示符
        /// </summary>
        /// <value></value>
        public string RequestId { get; set; }
        /// <summary>
        /// 显示请求Id
        /// </summary>
        /// <returns></returns>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            /**
            Activity：表示带有用于日志记录的上下文的操作
            Activity：当前线程的当前操作
            Activity.Current.Id:获取特定于特定请求的标识符
            HttpContext：单个HTTP请求的上下文
            HttpContext.TraceIdentifier：获取跟踪日志中表示此请求的惟一标识符
            **/
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
