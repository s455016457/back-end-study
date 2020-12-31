using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    /// <summary>
    /// 值API
    /// </summary>
    public class ValuesController : ApiController
    {
       /// <summary>
       /// 获取值
       /// </summary>
       /// <returns>值集合</returns>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 根据ID获取值
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// POST请求，传入值列表
        /// </summary>
        /// <param name="value">值列表</param>
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// Put请求，传入ID，值列表
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="value">值列表</param>
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        /// Delete请求，根据Id删除值
        /// </summary>
        /// <param name="id">ID</param>
        public void Delete(int id)
        {
        }
    }
}
