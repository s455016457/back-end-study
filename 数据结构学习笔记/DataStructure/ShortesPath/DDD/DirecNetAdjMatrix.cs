using System;

namespace ShortesPath.DDD
{
    /// <summary>
    /// 邻接矩阵图
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class DirecNetAdjMatrix<T> : IGraph<T>
    {
        /// <summary>
        /// 图边的数目
        /// </summary>
        public int NumArces { get; private set; }
        /// <summary>
        /// 图顶点集合
        /// </summary>
        private Node<T>[] nodes;
        /// <summary>
        /// 邻接矩阵数组 存放的是顶点到顶点边的权值
        /// </summary>
        private int[,] matrix;

        /// <summary>
        /// 初始化邻接矩阵图
        /// </summary>
        /// <param name="datas"></param>
        public DirecNetAdjMatrix(T[] datas)
        {
            nodes = new Node<T>[datas.Length];
            matrix = new int[datas.Length, datas.Length];
            for(int i = 0; i < datas.Length; i++)
            {
                nodes[i] = new Node<T>(datas[i]);
                for(int j = 0; j < datas.Length; j++)
                {
                    matrix[i, j] = int.MaxValue;
                }
                matrix[i, i] = 0;
            }
        }
        
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Node<T> this[int index]
        {
            get { return nodes[index]; }
            protected set { nodes[index] = value; }
        }
        /// <summary>
        /// 索引器  获取顶点到顶点的权值
        /// </summary>
        /// <param name="index1">顶点1下标</param>
        /// <param name="index2">顶点2下标</param>
        /// <returns></returns>
        public int  this[int index1,int index2]
        {
            get { return matrix[index1, index2]; }
            protected set { matrix[index1, index2] = value; }
        }

        /// <summary>
        /// 顶点为图的订单
        /// </summary>
        /// <param name="node">顶点</param>
        /// <returns></returns>
        public bool IsNode(Node<T> node)
        {
            return IsNode(node.Data);
        }
        /// <summary>
        /// 顶点为图的订单
        /// </summary>
        /// <param name="data">顶点数据</param>
        /// <returns></returns>
        public bool IsNode(T data)
        {
            foreach(Node<T> node in nodes)
            {
                if (node.Data.Equals(data)) return true;
            }
            return false;
        }
        /// <summary>
        /// 获取订单在图中的下标
        /// </summary>
        /// <param name="node">顶点</param>
        /// <returns></returns>
        public int GetIndex(Node<T> node)
        {
            return GetIndex(node.Data);
        }
        /// <summary>
        /// 获取订单在图中的下标
        /// </summary>
        /// <param name="data">顶点数据</param>
        /// <returns></returns>
        public int GetIndex(T data)
        {
            for(int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i].Data.Equals(data)) return i;
            }
            return -1;
        }

        /// <summary>
        /// 删除边
        /// </summary>
        /// <param name="v1">顶点1</param>
        /// <param name="v2">订单2</param>
        public void DelEdge(Node<T> v1, Node<T> v2)
        {
            int v1Index = GetIndex(v1);
            int v2Index = GetIndex(v2);
            if (v1Index < 0)
                throw new Exception("顶点v1不是图的顶点");

            if (v2Index < 0)
                throw new Exception("顶点v2不是图的顶点");

            matrix[v1Index, v2Index] = int.MaxValue;
            NumArces--;
        }
        /// <summary>
        /// 获取边或弧的数目
        /// </summary>
        /// <returns></returns>
        public int GetNumOfEdge()
        {
            return NumArces;
        }
        /// <summary>
        /// 获取顶点数
        /// </summary>
        /// <returns></returns>
        public int GetNumOfVertex()
        {
            return nodes.Length;
        }
        /// <summary>
        /// 判断两个顶点之间是否有边或弧
        /// </summary>
        /// <param name="v1">顶点1</param>
        /// <param name="v2">顶点2</param>
        /// <returns></returns>
        public bool IsEdge(Node<T> v1, Node<T> v2)
        {
            int v1Index = GetIndex(v1);
            int v2Index = GetIndex(v2);
            if (v1Index < 0)
                throw new Exception("顶点v1不是图的顶点");

            if (v2Index < 0)
                throw new Exception("顶点v2不是图的顶点");

            return matrix[v1Index, v2Index] != int.MaxValue;
        }
        /// <summary>
        /// 在两个顶点之间添加权值为v的边或弧
        /// </summary>
        /// <param name="v1">顶点1</param>
        /// <param name="v2">顶点2</param>
        /// <param name="v">权值</param>
        public void SetEdge(Node<T> v1, Node<T> v2, int v)
        {
            int v1Index = GetIndex(v1);
            int v2Index = GetIndex(v2);
            if (v1Index < 0)
                throw new Exception("顶点v1不是图的顶点");

            if (v2Index < 0)
                throw new Exception("顶点v2不是图的顶点");
            matrix[v1Index, v2Index] = v;
            NumArces++;
        }

        /// <summary>
        /// 在两个顶点之间添加权值为v的边或弧
        /// </summary>
        /// <param name="v1">顶点1的数据</param>
        /// <param name="v2">顶点1的数据</param>
        /// <param name="v">权值</param>
        public void SetEdge(T v1, T v2, int v)
        {
            int v1Index = GetIndex(v1);
            int v2Index = GetIndex(v2);
            if (v1Index < 0)
                throw new Exception("顶点v1不是图的顶点");

            if (v2Index < 0)
                throw new Exception("顶点v2不是图的顶点");
            matrix[v1Index, v2Index] = v;
            NumArces++;
        }
    }
}
