using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using WebApi.Extend;
using LoggerService.Interface;

namespace WebApi.App_Start
{
    /// <summary>
    /// 异常筛选器特性
    /// </summary>
    public class ExceptionFilterAttibute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 引发异常事件
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var request = actionExecutedContext.Request;
            var url = request.RequestUri.AbsoluteUri;
            IDictionary<String, String> errMsg = new Dictionary<String, String>();
            String message = GetExceptionMessage(actionExecutedContext.Exception);
            String query = BuildQuery(request);
            errMsg.Add("CODE", "");
            errMsg.Add("MSG", message);

            //1.异常日志记录（正式项目里面一般是用log4net记录异常日志）
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "——" +
                actionExecutedContext.Exception.GetType().ToString() + "：" + actionExecutedContext.Exception.Message + "——堆栈信息：" +
                actionExecutedContext.Exception.StackTrace);
            
                LoggerService.LoggerService.ApplicationLogger.Error(actionExecutedContext.Exception, "执行请求时出现异常！错误信息：{0}，请求参数：{1}", message, query);
                LogRequest(request, false, null);

            //2.返回调用方具体的异常信息
            //if (actionExecutedContext.Exception is NotImplementedException)
            //{
            //    actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented)
            //    {
            //        Content = new StringContent(JsonConvert.SerializeObject(errMsg)),
            //        ReasonPhrase = ExceptionCodes.InternalError
            //    };
            //}
            //else if (actionExecutedContext.Exception is TimeoutException)
            //{
            //    actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.RequestTimeout)
            //    {
            //        Content = new StringContent(JsonConvert.SerializeObject(errMsg)),
            //        ReasonPhrase = ExceptionCodes.InternalError
            //    };
            //}
            ////.....这里可以根据项目需要返回到客户端特定的状态码。如果找不到相应的异常，统一返回服务端错误500
            //else
            //{
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(errMsg)),
                    ReasonPhrase = ""
                };
            //}
            base.OnException(actionExecutedContext);
        }

        /// <summary>
        /// Gets the exception message.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>String.</returns>
        private String GetExceptionMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder(1024);
            GetExceptionMessage(sb, ex);
            return sb.ToString();
        }

        /// <summary>
        /// 获取异常的消息
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="ex"></param>
        private void GetExceptionMessage(StringBuilder sb, Exception ex)
        {
            if (ex != null)
            {
                sb.Append(ex.Message);
                if (ex.InnerException != null)
                {
                    sb.Append(" << ");
                    GetExceptionMessage(sb, ex.InnerException);
                }
            }
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="request">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        private String BuildQuery(HttpRequestMessage request)
        {
            StringBuilder postData = new StringBuilder();
            Boolean hasParam = false;

            foreach (var item in request.GetQueryStrings())
            {
                if (!String.IsNullOrWhiteSpace(item.Key) && !String.IsNullOrWhiteSpace(item.Value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(item.Key);
                    postData.Append("=");
                    postData.Append(Uri.EscapeDataString(item.Value));
                    hasParam = true;
                }
            }

            var context = (HttpContextBase)request.Properties["MS_HttpContext"];
            //定义传统request对象
            var httpRequest = context.Request;
            var ss = httpRequest["DirFilePath"];
            foreach (var name in httpRequest.Form.AllKeys)
            {
                var value = httpRequest.Form[name];
                if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(Uri.EscapeDataString(value));
                    hasParam = true;
                }
            }

            return postData.ToString();
        }

        /// <summary>
        /// 记录请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="success"></param>
        /// <param name="message"></param>
        private void LogRequest(HttpRequestMessage request, Boolean success, String message)
        {
            try
            {
                if (request.RequestUri.IsLoopback)
                {
                    return;
                }
                if (LoggerService.LoggerService.ApiRequestLogger != null)
                {
                    //var log = new HistoryLog();
                    //log.Appkey = request.GetQueryString(Constants.PARAM_APPKEY);
                    //log.RequestUrl = request.RequestUri.AbsoluteUri;
                    //log.IpAddress = GetHostAddress();
                    //log.Method = request.GetQueryString(Constants.PARAM_APPPARAM);
                    //log.Success = success;
                    //log.Remark = message;
                    //LoggerService.LoggerService.ApiRequestLogger.Write(log);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 获取客户端IP地址（无视代理）
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        private string GetHostAddress()
        {
            string userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(userHostAddress))
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                    userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
            }
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.UserHostAddress;
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return String.Empty;
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}