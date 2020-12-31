using Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HttpRequestLogger.Entities
{
    [Table("RequestLogger")]
    public class RequestLogger
    {
        ILog log = LogManager.GetLogger<RequestLogger>();
        protected RequestLogger()
        {
            log.Debug("初始化 RequestLogger");
        }

        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [Column("Id")]
        public virtual Guid Id { get; protected set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        [Column("Url")]
        public virtual string Url { get; protected set; }
        /// <summary>
        /// 请求方法
        /// </summary>
        [Column("Method")]
        public virtual string Method { get; protected set; }
        /// <summary>
        /// 请求时间戳
        /// </summary>
        [Column("Stamp")]
        public virtual DateTime Stamp { get; protected set; }
        /// <summary>
        /// 指定的URL是否为文件URL
        /// </summary>
        [Column("IsFile")]
        public virtual bool IsFile { get; protected set; }
        /// <summary>
        /// Ip地址
        /// </summary>
        [Column("Ip")]
        public virtual string Ip { get; protected set; }
        /// <summary>
        /// 请求耗时
        /// </summary>
        [Column("TimeConsuming")]
        public virtual double TimeConsuming { get; protected set; }
        /// <summary>
        /// 创建一个请求日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static RequestLogger CreateRequestLog(HttpRequest request)
        {   
            var model = new RequestLogger
            {
                Id = Guid.NewGuid(),
                Url = request.Url.AbsolutePath,
                Method = request.HttpMethod,
                IsFile = request.Url.IsFile,
                Stamp = DateTime.Now,
            };

            model.Ip = model.GetIp(request);
            return model;
        }
        /// <summary>
        /// 请求结束
        /// </summary>
        public virtual void EndRequest()
        {
            log.Debug("EndRequest");
            TimeConsuming = (DateTime.Now - Stamp).TotalMilliseconds;
        }

        /// <summary>
        /// 获取请求IP地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual string GetIp(HttpRequest request)
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
            if (!string.IsNullOrEmpty(userHostAddress) && this.IsIP(userHostAddress))
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
        protected virtual bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}
