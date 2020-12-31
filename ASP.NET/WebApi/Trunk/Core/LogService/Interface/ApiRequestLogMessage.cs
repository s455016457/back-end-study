using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService.Interface
{
    public class ApiRequestLogMessage
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// appkey
        /// </summary>
        public String Appkey { get; set; }

        /// <summary>
        /// 发送请求的IP地址
        /// </summary>
        public String IpAddress { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public String RequestUrl { get; set; }

        /// <summary>
        /// 调用的API方法
        /// </summary>
        public String Method { get; set; }

        /// <summary>
        /// 调用是否成功
        /// </summary>
        public Boolean Success { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime CreatedStamp { get {return CreatedStamp; } private set { value = DateTime.Now; } }
    }
}
