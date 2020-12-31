using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShortesPath.DDD;

namespace ShortesPath
{
    class Program
    {
        static void Main(string[] args)
        {
            var v = int.MaxValue;
            Console.WriteLine("计算溢出，造成负数");
            Console.WriteLine("var v = int.MaxValue;");
            Console.WriteLine($"v+1：{v+1}");

            DirecNetAdjMatrix<string> adjMatrix = new DirecNetAdjMatrix<string>(new string[] { "A", "B", "C", "D", "E", "F" });

            adjMatrix.SetEdge("A", "C", 5);
            adjMatrix.SetEdge("A", "D", 30);
            adjMatrix.SetEdge("B", "A", 2);
            adjMatrix.SetEdge("B", "E", 8);
            adjMatrix.SetEdge("C", "B", 15);
            adjMatrix.SetEdge("C", "F", 7);
            adjMatrix.SetEdge("E", "D", 4);
            adjMatrix.SetEdge("F", "D", 10);
            adjMatrix.SetEdge("F", "E", 18);

            PrintDirecNetAdjMatrix(adjMatrix);

            bool[,] pathMatricArr = new bool[adjMatrix.GetNumOfVertex(), adjMatrix.GetNumOfVertex()];
            int[] shortPathArr = new int[adjMatrix.GetNumOfVertex()];

            adjMatrix.Dijkstra(ref pathMatricArr, ref shortPathArr, new Node<string>("A"));

            Console.WriteLine();
            Console.WriteLine("最短路径长度：");
            PrintArray(shortPathArr);
            Console.WriteLine();
            Console.WriteLine("路线：");
            PrintArray(pathMatricArr, shortPathArr.Length);
        }

        static void PrintDirecNetAdjMatrix<T>(DirecNetAdjMatrix<T> adjMatrix)
        {
            Console.WriteLine("图顶点数组：");
            StringBuilder sb = new StringBuilder();
            for(int i= 0;i<adjMatrix.GetNumOfVertex();i++)
            {
                sb.AppendFormat("  {0}  ", adjMatrix[i].Data);
            }
            Console.WriteLine(sb.ToString());

            Console.WriteLine();
            Console.WriteLine("图权值邻接表矩阵：");

            for (int i = 0; i < adjMatrix.GetNumOfVertex(); i++)
            {
                sb = new StringBuilder();
                for (int j = 0; j < adjMatrix.GetNumOfVertex(); j++)
                {
                    sb.AppendFormat("  {0}  ", adjMatrix[i, j].ToString().PadLeft(6, ' ').PadRight(10, ' '));
                }
                Console.WriteLine(sb.ToString());
            }
        }

        static void PrintArray<T>(T[] ts)
        {
            StringBuilder sb = new StringBuilder();
            foreach(T t in ts)
            {
                sb.AppendFormat("  {0}  ", t);
            }
            Console.WriteLine(sb.ToString());
        }

        static void PrintArray<T>(T[,] ts, int length)
        {
            for (int i = 0; i < length; i++)
            {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < length; j++)
                {
                    sb.AppendFormat("  {0}  ", ts[i, j].ToString().PadRight(5,' '));
                }
                Console.WriteLine(sb.ToString());
            }
        }
    }
}
