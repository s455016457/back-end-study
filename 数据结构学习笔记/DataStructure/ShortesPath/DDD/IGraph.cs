using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortesPath.DDD
{
    /// <summary>
    /// 图接口
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public interface IGraph<T>
    {
        /// <summary>
        /// 获取顶点数
        /// </summary>
        /// <returns></returns>
        int GetNumOfVertex();
        /// <summary>
        /// 获取边或弧的数目
        /// </summary>
        /// <returns></returns>
        int GetNumOfEdge();
        /// <summary>
        /// 在两个顶点之间添加权值为v的边或弧
        /// </summary>
        /// <param name="v1">顶点1</param>
        /// <param name="v2">顶点2</param>
        /// <param name="v">权值</param>
        void SetEdge(Node<T> v1, Node<T> v2, int v);
        /// <summary>
        /// 删除两个顶点之间的边或弧
        /// </summary>
        /// <param name="v1">顶点1</param>
        /// <param name="v2">顶点2</param>
        void DelEdge(Node<T> v1, Node<T> v2);
        /// <summary>
        /// 判断两个顶点之间是否有边或弧
        /// </summary>
        /// <param name="v1">顶点1</param>
        /// <param name="v2">顶点2</param>
        /// <returns></returns>
        bool IsEdge(Node<T> v1, Node<T> v2);
    }
}
