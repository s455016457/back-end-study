using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortesPath.DDD
{
    public static class DirecNetAdjMatrixExtend
    {
        /// <summary>
        /// 狄克斯特拉算法的实现
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="adjMatrix">邻接矩阵图</param>
        /// <param name="pathMatricArr">从源点到某个顶点的最短路径上的顶点数组</param>
        /// <param name="shortPathArr">从源点到各个顶点的最短路径的长度数组</param>
        /// <param name="n">起始顶点</param>
        public static void Dijkstra<T>(this DirecNetAdjMatrix<T> adjMatrix, ref bool[,] pathMatricArr, ref int[] shortPathArr, Node<T> n)
        {
            // 获取图的顶点数量
            int vertexNum = adjMatrix.GetNumOfVertex();
            // 某顶点的最短路径是否已经找到，true未已找到，false为未找到
            bool[] final = new bool[adjMatrix.GetNumOfVertex()];

            // 开始顶点下标
            int startNodeIndex = adjMatrix.GetIndex(n);

            // 初始化
            for (int i = 0; i < vertexNum; i++)
            {
                final[i] = false;
                shortPathArr[i] = adjMatrix[startNodeIndex, i];
                for (int j = 0; j < vertexNum; j++)
                {
                    pathMatricArr[i, j] = false;
                }
                // 起点到各个点的最小距离
                // shortPathArr 中存在最短路径，设置顶点i到开始顶点已存在最短路径
                //if (shortPathArr[i] != 0 && shortPathArr[i] < int.MaxValue)
                //{
                //    pathMatricArr[i, startNodeIndex] = true;
                //    pathMatricArr[i, i] = true;
                //}
            }

            // 设置n为开始顶点
            shortPathArr[startNodeIndex] = 0;
            final[startNodeIndex] = true;   // 设置起始顶点已找到最短路径

            //处理从源点到其余顶点的最短路径
            for (int i = 0; i < vertexNum; i++)
            {
                // 从 i出发 最小路径的顶点
                int index = 0, pathValue = int.MaxValue;

                for (int j = 0; j < vertexNum; j++)
                {
                    // 如果该顶点已是最短路径，则跳过
                    if (final[j]) continue;
                    if (shortPathArr[j] > 0 && shortPathArr[j] < pathValue)
                    {
                        pathValue = shortPathArr[j];
                        index = j;
                    }
                }

                // 源点到顶点index的路径长度最小
                final[index] = true;

                // 计算从index出发各个路线的权值，并更新权值数组和路线数组
                for (int j = 0; j < vertexNum; j++)
                {
                    // 如果该顶点已是最短路径，则跳过
                    if (final[j]) continue;

                    int v = pathValue + adjMatrix[index, j];
                    if (v > 0 && v < shortPathArr[j])// 必须要判断v>0，防止溢出变成负数
                    {
                        shortPathArr[j] = v;
                        // 将index顶点经过的路径复制到j顶点中
                        for (int k = 0; k < vertexNum; k++)
                        {
                            pathMatricArr[j, k] = false;
                            //pathMatricArr[j, k] = pathMatricArr[index, k];
                        }
                        //Console.WriteLine($"顶点J【{j}】过顶点index【{index}】");
                        // 从原点到j订单经过index订单
                        pathMatricArr[j, index] = true;
                    }
                }
            }
        }
    }
}
