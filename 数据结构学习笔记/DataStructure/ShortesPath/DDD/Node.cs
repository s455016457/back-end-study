using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortesPath.DDD
{
    /// <summary>
    /// 图顶点
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class Node<T>
    {
        /// <summary>
        /// 顶点数据
        /// </summary>
        public T Data { get; private set; }
        
        public Node(T data)
        {
            Data = data;
        }
    }
}
