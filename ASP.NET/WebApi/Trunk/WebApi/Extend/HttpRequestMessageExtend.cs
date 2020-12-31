using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApi.Extend
{
    /// <summary>
    /// HttpRequestMessage扩展
    /// </summary>
    public static class HttpRequestMessageExtend
    {
        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        private const string OwinContext = "MS_OwinContext";

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            //request.GetOwinContext();

            // Web-hosting. Needs reference to System.Web.dll
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            // Self-hosting. Needs reference to System.ServiceModel.dll. 
            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }

            // Self-hosting using Owin. Needs reference to Microsoft.Owin.dll. 
            if (request.Properties.ContainsKey(OwinContext))
            {
                dynamic owinContext = request.Properties[OwinContext];
                if (owinContext != null)
                {
                    return owinContext.Request.RemoteIpAddress;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取所有键值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetQueryStrings(this HttpRequestMessage request)
        {
            return request.GetQueryNameValuePairs().ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 获取单个键值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetQueryString(this HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();
            if (queryStrings == null)
                return null;

            var match = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, key, true) == 0);
            if (string.IsNullOrEmpty(match.Value))
                return null;

            return match.Value;
        }

        /// <summary>
        /// 依据键获取请求头中值数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetHeader(this HttpRequestMessage request, string key)
        {
            IEnumerable<string> keys = null;
            if (!request.Headers.TryGetValues(key, out keys))
                return null;

            return keys.First();
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookie(this HttpRequestMessage request, string cookieName)
        {
            String CookieValue = String.Empty;
            CookieHeaderValue cookie = request.Headers.GetCookies(cookieName).FirstOrDefault();

            if (cookie != null)
            {
                if (cookie.Expires.HasValue && cookie.Expires.Value < DateTime.Now) throw new Exception("Cookie已过期");
                CookieValue = cookie[cookieName].Value;
            }
            else
            {
                try
                {
                    var cookies = request.Headers.GetValues("Cookie");
                    foreach (var item in cookies)
                    {
                        CookieValue = GetCookie(item, cookieName);
                        if (!String.IsNullOrWhiteSpace(CookieValue))
                        {
                            return CookieValue;
                        }
                    }
                }
                catch (Exception ex)
                { }
            }

            if (String.IsNullOrWhiteSpace(CookieValue))
            {
                throw new Exception("未获取到name为【" + cookieName + "】的Cookie");
            }

            return CookieValue;
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="strCookie">Cookie字符串</param>
        /// <param name="cookieName">Cookie名称</param>
        /// <returns></returns>
        public static String GetCookie(String strCookie, String cookieName)
        {
            String value = String.Empty;
            String[] values = strCookie.Split(';');
            foreach (String item in values)
            {
                string key = String.Format("{0}=", cookieName);
                if (item.TrimStart(' ').IndexOf(key) == 0)
                {
                    return item.Substring(key.Length, item.Length - key.Length);
                }
            }
            return value;
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static CookieHeaderValue GetCookieHeaderValue(this HttpRequestMessage request, string cookieName)
        {
            CookieHeaderValue cookie = request.Headers.GetCookies(cookieName).FirstOrDefault();
            if (cookie == null)
            {
                throw new Exception("未获取到name为【" + cookieName + "】的Cookie");
            }
            if (!cookie.Expires.HasValue) throw new Exception("Cookie未设置到期时间");
            if (cookie.Expires.Value < DateTime.Now) throw new Exception("Cookie已过期");
            return cookie;
        }
    }
}