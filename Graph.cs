using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    /// <summary>
    ///Class representing oriented weighted graph
    /// </summary>
    /// 

    public class Graph
    {
        public int n, m, HPower; // n - vertices count,m - edges count,HPower - graph power (-1 if not homogenic)
        public Edge[] edges;
        public Vertex[] verteses;
        public int[,] AdjMatr;
        public int[,] IncMatr;
        public int[,] DistMatr;
        public int[,] ReachMatr;
        public int[] Excs;
        public bool Homogen;
        public bool Oriented;
        public bool Cyclic;
        public List<string> catalogCycles = new List<string>();//List of primitive cycles in form of string,like "1-2-1"

        public Graph(string file, bool oriented)
        {
            InputFromFile(file);
            Oriented = oriented;
        }

        private int[,] FillAdjacencyMatrix()
        {
            int[,] AdjMatr = new int[n, n];
            for (int i = 0; i < m; i++)
            {
                AdjMatr[edges[i].n1, edges[i].n2] = edges[i].w;
            }
            return AdjMatr;
        }

        private int[,] FillIncidenceMatrix()
        {
            int[,] IncMatr = new int[n, m];
            for (int i = 0; i < m; i++)
                IncMatr[edges[i].n1, i] = -1;
            for (int i = 0; i < m; i++)
                IncMatr[edges[i].n2, i] = 1;
            for (int i = 0; i < m; i++)
                if (edges[i].n1 == edges[i].n2) IncMatr[edges[i].n1, i] = 2;
            return IncMatr;
        }

        private int[,] FillDistanceMatrix()
        {
            int[,] DistMatr = new int[n, n];

            return DistMatr;
        }

        private void SymmetrizeMatrix(int[,] Matrix)
        {
            for (int i = 0; i < Matrix.GetLength(0); i++)
                for (int j = i + 1; j < Matrix.GetLength(1); j++) Matrix[j, i] = Matrix[i, j];
        }

        private bool[,] ConvertToBool(int[,] Matrix)
        {
            int n = Matrix.GetLength(0);
            int m = Matrix.GetLength(1);
            bool[,] BMatr = new bool[n, m];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    BMatr[i, j] = Convert.ToBoolean(Matrix[i, j]);

            return BMatr;
        }

        private int[,] ConvertToInt(bool[,] Matrix)
        {
            int n = Matrix.GetLength(0);
            int m = Matrix.GetLength(1);
            int[,] IMatr = new int[n, m];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    IMatr[i, j] = Convert.ToInt32(Matrix[i, j]);

            return IMatr;
        }

        private int GetRowMaximum(int[,] Matr, int r)
        {
            int max = 0;
            for (int i = 0; i < Matr.GetLength(0); i++) if (Matr[r, i] > max) max = Matr[r, i];
            return max;
        }

        private T[,] TransposeMatrix<T>(T[,] Matrix)
        {
            int n = Matrix.GetLength(0);
            int m = Matrix.GetLength(1);
            T[,] TMatr = new T[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    TMatr[i, j] = Matrix[j, i];
            return TMatr;
        }

        private int[,] MultiplyMatrix(int[,] m1, int[,] m2)
        {
            int n = m1.GetLength(0);
            int m = m2.GetLength(1);
            int[,] resultMatrix = new int[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    resultMatrix[i, j] = 0;
                    for (int k = 0; k < m; k++)
                    {
                        resultMatrix[i, j] += m1[i, k] * m2[k, j];
                    }
                }
            }
            return resultMatrix;
        }

        private int[,] MatrixPower(int[,] Matrix, int p)
        {
            int n = Matrix.GetLength(0);
            int m = Matrix.GetLength(1);
            int[,] PMatr = new int[n, m];
            Array.Copy(Matrix, PMatr, Matrix.Length);
            for (int i = 1; i < p; i++) PMatr = MultiplyMatrix(PMatr, Matrix);
            return PMatr;
        }

        private void InputFromFile(string path)
        {
            int c = 0;
            string line;
            StreamReader file = new StreamReader(path);
            line = file.ReadLine();
            n = Int32.Parse(line[0].ToString());
            m = Int32.Parse(line.Split(' ')[1].ToString());
            edges = new Edge[m];
            verteses = new Vertex[n];
            InitializeVerteses();
            string[] rawlist = new string[m];
            while ((line = file.ReadLine()) != null)
            {
                rawlist[c] = line;
                c++;

            }
            for (c = 0; c < rawlist.GetLength(0); c++)
            {
                int n1, n2, w;
                n1 = Int32.Parse(rawlist[c].Split(' ')[0]);//Проверок на наличие числа в строке нет специально,так как ошибки ввода обработаются исключением
                n2 = Int32.Parse(rawlist[c].Split(' ')[1]);//А стандартные значения лучше не задавать,потом труднее искать ошибки :)
                w = Int32.Parse(rawlist[c].Split(' ')[2]);
                verteses[n1 - 1].adjances.Add(n2 - 1);
                edges[c] = new Edge(n1 - 1, n2 - 1, w);
            }
            file.Close();
        }

        private void InitializeVerteses()
        {
            for (int i = 0; i < verteses.GetLength(0); i++) verteses[i] = new Vertex(i);
        }


        /// <summary>
        /// Dijkstra's algorithm for shortest path search
        /// </summary>
        /// <param name="s">Start vertex</param>
        /// <param name="f">End vertex</param>
        /// <returns>String with shortest path,separated by '-',or empty string if path doesn't exsist</returns>
        public String Dijkstra(int s, int f)
        {
            List<List<Edge>> g = new List<List<Edge>>();//алгоритм работает с структурой следующего вида - список g списков ребер которые выходят из g[i] вершины 
            for (int i = 0; i < n; i++)//Формируем список списков g
            {
                List<Edge> ledges = new List<Edge>();//Список ребер
                foreach (Edge e in edges)
                    if (e.n1 == i)
                        ledges.Add(e);
                g.Add(ledges);
            }

            bool[] used = new bool[n];
            int[] d = new int[n];
            int[] p = new int[n];//Массив предков для восстановления пути
            for (int i = 0; i < n; i++)//Заполнение массива максимальными значениями
                d[i] = Int32.MaxValue;

            d[s] = 0;
            for (int i = 0; i < n; i++)//Сам алгоритм
            {
                var v = -1;
                for (int j = 0; j < n; j++)
                    if (!used[j] && (v == -1 || d[j] < d[v]))
                        v = j;
                if (d[v] == Int32.MaxValue)
                    break;
                used[v] = true;

                for (int j = 0; j < g[v].Count; j++)
                {
                    int to = g[v][j].n2, len = g[v][j].w;
                    if (d[v] + len < d[to])
                    {
                        d[to] = d[v] + len;
                        p[to] = v;
                    }
                }
            }
            //Формируем стек пути
            Stack<int> path = new Stack<int>();
            for (int v = f; v != s; v = p[v])
                path.Push(v);
            path.Push(s);
            path.Reverse();
            String spath = String.Empty;//Сам путь в виде строки
            foreach (int node in path)
                spath += node + 1 + " ";

            return spath;
        }

    }
}
