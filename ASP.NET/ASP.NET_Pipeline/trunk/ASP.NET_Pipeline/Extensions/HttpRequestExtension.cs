using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_Pipeline.Extensions
{
    public static class HttpRequestExtension
    {
        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetIp(this HttpRequest request)
        {
            string userHostAddress = request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(userHostAddress))
            {
                if (request.ServerVariables["HTTP_VIA"] != null)
                    userHostAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
            }
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = request.UserHostAddress;
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return string.Empty;
        }

        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}