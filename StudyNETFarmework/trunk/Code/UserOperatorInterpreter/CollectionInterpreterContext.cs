using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOperatorInterpreter
{
    /// <summary>
    /// 集合解析上下文
    /// </summary>
    public class CollectionInterpreterContext
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
        public List<string> Result { get; } = new List<string>();
        /// <summary>
        /// 初始集合解析上下文
        /// </summary>
        /// <param name="strFormat">解析数据格式</param>
        /// <param name="data">解析数据json</param>
        public CollectionInterpreterContext(string strFormat, object data)
        {
            Format = strFormat;
            Data = data;
        }
    }
}
