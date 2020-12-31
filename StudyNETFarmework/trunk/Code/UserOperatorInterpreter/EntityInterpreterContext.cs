using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOperatorInterpreter
{
    /// <summary>
    /// 实体解析上下文
    /// </summary>
    public class EntityInterpreterContext
    {
        /// <summary>
        /// 解析数据格式
        /// </summary>
        public string Format { get; private set; }
        /// <summary>
        /// 解析数据json
        /// </summary>
        public object Data { get; private set; }
        /// <summary>
        /// 结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 初始化解析上下文
        /// </summary>
        /// <param name="strFormat">解析数据格式</param>
        /// <param name="data">解析数据json</param>
        public EntityInterpreterContext(string strFormat, object data)
        {
            Format = strFormat;
            Data = data;
        }
    }
}
