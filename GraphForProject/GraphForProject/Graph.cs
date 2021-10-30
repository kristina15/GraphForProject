
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RightGraph
{
    public class Graph
    {
        //Словарь «Вершина – Ребро» - на его основе создается граф
        Dictionary<int, Dictionary<int, int>> theGraph = new Dictionary<int, Dictionary<int, int>>();

        //Лист ребер
        List<int> Edge = new List<int>();

        public bool CheckVertex(int vertex)
        {
            foreach (var i in theGraph.Keys)
            {
                if (i == vertex)
                    return true;
            }
            return false;
        }
        public bool CheckEdge(int from, int to, int edge)
        {
            foreach (var i in theGraph)
            {
                if (i.Key == from)
                    foreach (var j in i.Value)
                    {
                        if (j.Key == to && j.Value == edge)
                            return true;
                    }
            }
            return false;
        }
        public bool CheckEdge(int from, int to)
        {
            foreach (var i in theGraph)
            {
                if (i.Key == from)
                    foreach (var j in i.Value)
                    {
                        if (j.Key == to)
                            return true;
                    }
            }
            return false;
        }

        //Конструкторы
        //По умолчанию
        public Graph()
        {
            Dictionary<int, Dictionary<int, int>> theGraph = new Dictionary<int, Dictionary<int, int>>();

            ToEdgeList();
        }

        //Создание графа через лист вершин
        public Graph(List<int> vertexes, bool isOriented, bool isves, List<int> ves)
        {
            if (isves == false)
            {
                for (int i = 0; i < vertexes.Count; i++)
                {
                    if (vertexes.IndexOf(vertexes[i], 0, i) == -1)
                    {
                        int v = vertexes[i];
                        AddVertex(v);
                    }

                }

                for (int k = 0; k < vertexes.Count - 1; k += 2)
                {

                    theGraph[vertexes[k]].Add(vertexes[k + 1], 1);
                    theGraph[vertexes[k + 1]].Add(vertexes[k], 1);
                }
            }
            else
            {
                for (int i = 0; i < vertexes.Count; i++)
                {
                    if (vertexes.IndexOf(vertexes[i], 0, i) == -1)
                    {
                        int v = vertexes[i];
                        AddVertex(v);
                    }
                }

                for (int k = 0; k < vertexes.Count - 1; k += 2)
                {
                    theGraph[vertexes[k]].Add(vertexes[k + 1], ves[k / 2]);
                    theGraph[vertexes[k + 1]].Add(vertexes[k], ves[k / 2]);
                }
            }
            ToEdgeList();
        }

        //Создание графа из файла
        public Graph(string FileName, bool isves)
        {
            StreamReader file = new StreamReader(FileName);
            string[] vertex;
            string[] StrFromFile = file.ReadToEnd().Split('\n');
            //Создать все вершины
            foreach (var s in StrFromFile)
            {
                vertex = s.Split(' ');
                int v = int.Parse(vertex[0]);
                AddVertex(v);
            }

            if (isves)
            {
                foreach (var s in StrFromFile)
                {
                    vertex = s.Split(' ');
                    int from = int.Parse(vertex[0]);

                    for (int k = 1; k < vertex.Length; k += 2)
                    {
                        int to = int.Parse(vertex[k]);
                        int weight = int.Parse(vertex[k + 1]);

                        theGraph[from].Add(to, weight);

                        theGraph[to].Add(from, weight);
                    }

                }
            }
            else
            {
                foreach (var s in StrFromFile)
                {
                    vertex = s.Split(' ');
                    int from = int.Parse(vertex[0]);

                    for (int k = 1; k < vertex.Length; k += 2)
                    {
                        int to = int.Parse(vertex[k]);

                        theGraph[from].Add(to, 1);

                        theGraph[to].Add(from, 1);
                    }

                }
            }

            file.Close();
            ToEdgeList();
        }

        //Конструктор-копия
        public Graph(Graph g)
        {
            var newOne = new Dictionary<int, Dictionary<int, int>>();
            foreach (var item in g.theGraph)
            {
                newOne.Add(item.Key, item.Value);
            }

            this.theGraph = newOne;
        }

        //Создание листа ребер из графа
        void ToEdgeList()
        {
            Edge.Clear();
            foreach (var key in theGraph)
                Edge.Add(key.Key);
        }

        //Красивый вывод
        public string ToStringGraph(bool isves)
        {
            if (this != null)
            {
                string result = "";
                if (isves)
                {
                    foreach (var key in theGraph)
                    {
                        string s = key.Key.ToString() + " : ";
                        foreach (var v in key.Value)
                            s += v.Key.ToString() + " (вес " + v.Value.ToString() + ") ";
                        result += s + Environment.NewLine;
                    }
                }
                else
                {
                    foreach (var key in theGraph)
                    {
                        string s = key.Key.ToString() + " : ";
                        foreach (var v in key.Value)
                            s += v.Key.ToString() + " ";
                        result += s + Environment.NewLine;
                    }
                }
                return result;
            }
            return "";
        }

        //Сохранение в файл
        public void Save(bool isves, string FileName = "Auto.txt")
        {
            StreamWriter sw = new StreamWriter(FileName);
            if (isves)
            {
                foreach (var key in theGraph)
                {
                    string s = key.Key.ToString() + " : ";
                    foreach (var v in key.Value)
                        s += v.Key.ToString() + " (вес " + v.Value.ToString() + ") ";
                    sw.WriteLine(s);
                }
            }
            else
            {
                foreach (var key in theGraph)
                {
                    string s = key.Key.ToString() + " : ";
                    foreach (var v in key.Value)
                        s += v.Key.ToString() + " ";
                    sw.WriteLine(s);
                }
            }

            sw.Close();
        }

        //Добавление вершины        
        public void AddVertex(int vertex)
        {
            if (CheckVertex(vertex) == false)
            {
                theGraph.Add(vertex, new Dictionary<int, int>());
            }
            else
            {
                Console.WriteLine("Данная вершина уже существует!");
            }
        }

        //Добавление ребра в неориентированный граф
        public void AddEdgeInNotOriented(int from, int to, int edge)
        {
            if (from != to)
            {
                var reverseEdge = new Dictionary<int, int> { { from, edge } };

                if ((CheckVertex(from) == false) || (CheckVertex(to) == false))
                {
                    Console.WriteLine("Не существует вершин(ы) для данного ребра!");
                }
                else if ((CheckEdge(from, to, edge) == true) || (CheckEdge(from, to, edge) == true))
                {
                    Console.WriteLine("Данное ребро уже существует!");
                    if (CheckEdge(from, to, edge) == false)
                    {
                        foreach (var item in theGraph)
                        {
                            if (from == item.Key)
                                foreach (var item1 in item.Value)
                                {
                                    if (item1.Key == to)
                                    {
                                        item.Value[to] = edge;
                                        break;
                                    }
                                }
                            if (to == item.Key)
                                foreach (var item1 in item.Value)
                                {
                                    if (item1.Key == from)
                                    {
                                        item.Value[from] = edge;
                                        break;
                                    }
                                }
                        }
                    }
                }
                else
                {
                    foreach (var item in theGraph)
                    {
                        if (from == item.Key)
                            item.Value.Add(to, edge);
                        if (to == item.Key)
                            item.Value.Add(from, edge);
                    }
                }
            }
            else
            {
                Console.WriteLine("Нельзя добавить петлю");
            }
        }


        //Удаление ребра из неориентированного графа
        public void DeleteEdgeFromNotOriented(int from, int to)
        {
            if (CheckEdge(from, to) == false)
            {
                Console.WriteLine("Данное ребро не существует!");
            }
            else
            {
                foreach (var v in theGraph)
                {
                    if (v.Key != from) continue;
                    foreach (var e in v.Value)
                    {
                        if (e.Key == to)
                        {
                            v.Value.Remove(e.Key);
                            break;
                        }
                    }
                }

                foreach (var v in theGraph)
                {
                    if (v.Key != to) continue;
                    foreach (var e in v.Value)
                    {
                        if (e.Key == from)
                        {
                            v.Value.Remove(e.Key);
                            break;
                        }
                    }
                }

            }
        }

        //Удаление вершины
        public void DeleteVertex(int ID)
        {
            int vertex = ID;

            if (CheckVertex(vertex) == false)
            {
                Console.WriteLine("Данной вершины не существует!");
            }
            else
            {
                foreach (var v in theGraph)
                    foreach (var e in v.Value)
                        if (e.Key == ID)
                        {
                            v.Value.Remove(e.Key);
                            break;
                        }
                foreach (var v in theGraph)
                    if (v.Key == ID)
                    {
                        theGraph.Remove(v.Key);
                        break;
                    }
            }
        }

        #region Brute-Force Algorithm
        public bool GetBruteForce(Graph g2)
        {
            int[][] matrix = Matrix();
            if (!Compare(matrix, g2))
            {
                for (int i = 0; i < matrix.Length; i++)
                {
                    for (int j = 0; j < matrix.Length; j++)
                    {
                        if (i != j)
                        {
                            Swap(ref matrix[i], ref matrix[j]);
                            if (Compare(matrix, g2))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            var rotateMatrix = RotatedMatrix(matrix);
            if(!Compare(rotateMatrix, g2))
            {
                for (int i = 0; i < rotateMatrix.Length; i++)
                {
                    for (int j = 0; j < rotateMatrix.Length; j++)
                    {
                        if (i != j)
                        {
                            Swap(ref rotateMatrix[i], ref rotateMatrix[j]);
                            if (Compare(rotateMatrix, g2))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return true;
        }

        private void Swap(ref int[] str1, ref int[] str2)
        {
            int[] temp = new int[str1.Length];
            for (int i = 0; i < str1.Length; i++)
            {
                temp[i] = str1[i];
            }

            for (int i = 0; i < str1.Length; i++)
            {
                str1[i] = str2[i];
            }

            for (int i = 0; i < temp.Length; i++)
            {
                str2[i] = temp[i];
            }
        }

        private bool Compare(int[][] matrix, Graph g2)
        {
            var matrix2 = g2.Matrix();
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if(matrix[i][j] != matrix2[i][j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private int[][] RotatedMatrix(int[][] matrix)
        {
            int[][] matrixCopy = new int[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
            {
                matrixCopy[i] = matrix[i];
            }

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    matrixCopy[i][j] = matrix[j][i];
                }
            }

            return matrixCopy;
        }
        #endregion

        #region First Evristic
        //найти кратчайшее расстояние между вершинами, их сумма - Индекс Винера
        public int VinerIndex()
        {
            int resultSum = 0;
            int[][] res = Floyd();
            for (int i = 0; i < res[0].Length; ++i)
            {
                for (int j = 0; j < res[0].Length; ++j)
                {
                    if (res[i][j] != int.MaxValue)
                    {
                        resultSum += res[i][j];
                    }
                }
            }
            return resultSum;
        }

        public int[][] Floyd()
        {
            int i, j, k;
            //создаем массив а
            int size = Edge.Count;
            int[][] a = Matrix();

            for (k = 0; k < size; k++)
            {
                for (i = 0; i < size; i++)
                {
                    for (j = 0; j < size; j++)
                    {
                        long distance = (long)a[i][k] + (long)a[k][j];
                        if ((long)a[i][j] > distance)
                        {
                            a[i][j] = (int)distance;
                        }
                    }
                }
            }

            return a;//в качестве результата возвращаем массив кратчайших путей между
        }

        //Индекс Рандича
        public double RandicIndex()
        {
            //составляем список степеней вешин
            Dictionary<int, int> degreeList = new Dictionary<int, int>();
            foreach (var v in theGraph)
            {
                int count = 0;
                foreach (var v1 in v.Value)
                {
                    count += 1;
                }
                degreeList.Add(v.Key, count);
            }

            double result = 1;
            foreach (var v in degreeList)
            {
                foreach (var v1 in degreeList)
                {
                    if (v.Key != v1.Key)
                    {
                        result += 1 / Math.Sqrt(v.Value * v1.Value);
                    }
                }
            }

            return result;
        }


        //построение матрицы смежности и поиск ее определителя

        //matrix
        public int[][] Matrix()
        {
            int n = theGraph.Count;
            int[][] a = new int[n][];
            for (int i = 0; i < n; ++i)
            {
                a[i] = new int[n];
            }

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (theGraph[Edge[i]].ContainsKey(Edge[j]))
                    {
                        a[i][j] = theGraph[Edge[i]][Edge[j]];
                    }
                    else if (i != j)
                    {
                        a[i][j] = int.MaxValue;
                    }
                    else
                    {
                        a[i][j] = 0;
                    }
                }
            }
            return a;
        }



        //число компонент связности графа
        public int NumberOfConnectedComponents()
        {
            int result = 1;

            //использованные вершины
            List<int> usedV = new List<int>();
            //неиспользованные вершины
            List<int> notUsedV = new List<int>();

            foreach (var v in theGraph)
                notUsedV.Add(v.Key);

            //выбираем начальную вершину
            usedV.Add(notUsedV[0]);

            notUsedV.Remove(usedV[0]);

            for (int i = 0; i < theGraph.Count; ++i)
            {
                bool newComp = false;

                //ищем компоненты связности
                for (int j = 0; j < theGraph.Count; ++j)
                {
                    if (notUsedV.Count < j)
                    {
                        foreach (var v in theGraph[notUsedV[j]])
                        {
                            //если проходимся по вершине, заносим в использованные, удаляем из неиспользованных
                            if (!usedV.Any(d => d == v.Key))
                            {
                                usedV.Add(v.Key);
                                notUsedV.Remove(v.Key);
                            }
                            else
                            {
                                newComp = true;
                            }
                        }

                        if (newComp)
                        {
                            result += 1;
                        }
                    }
                }

            }
            return result;
        }

        //Цикломатическое число
        //находится как разница суммы числа компонент связности и числа ребер с числом чершин
        public int CyclomaticNumber()
        {
            int r = NumberOfConnectedComponents();
            //матрица смежности
            int[][] a = Matrix();

            int sumOfEdges = 0;
            foreach (var ch in a)
            {
                foreach (var val in ch)
                {
                    if (val != 0 && val != int.MaxValue)
                    {
                        sumOfEdges += 1;
                    }
                }
            }

            sumOfEdges /= 2;

            return r + sumOfEdges - theGraph.Count;
        }
        #endregion

        #region Second Evristic

        //перевод числа в двоичную систему
        public string ToBin(long x)
        {
            string s3 = "";
            long x2 = x;
            while (x2 > 0)
            {
                s3 = x2 % 2 + s3;
                x2 /= 2;
            }
            if (s3 == "")
                s3 = "0";

            return s3;
        }

        static List<List<int>> Permute(int[] nums)
        {
            var list = new List<List<int>>();
            //передаем список чисел, start - номер вставляемого элемента в очередную вариацию, end определяет количество чисел в перестановке, list - список вариаций 
            DoPermute(nums, 0, nums.Length - 1, ref list);
            return list;
        }


        static void DoPermute(int[] nums, int start, int end, ref List<List<int>> list)
        {
            if (start == end)
            {
                // мы получили один из n! вариантов перестановок
                list.Add(new List<int>(nums));
            }
            else
            {
                for (var i = start; i <= end; ++i)
                {
                    //составляем n! перестановок за счет попарной замены чисел, получаем рекурсивный алгоритм получения списка перестановок
                    Swap(ref nums[start], ref nums[i]);
                    DoPermute(nums, start + 1, end, ref list);
                    Swap(ref nums[start], ref nums[i]);
                }
            }

        }

        static void Swap(ref int a, ref int b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        //мини-код графа - минимальное двоичное значение выражения, вычисляемого за счет перестановки элементов
        public long MiniCode()
        {
            //для каждой комбинации найти значение выражения и найти минимальное среди них

            List<List<int>> permutations = Permute(Edge.ToArray());

            long min = long.MaxValue;

            foreach (var p in permutations)
            {
                Edge = p;
                int[][] matr = Matrix();

                List<int> values = new List<int>();

                for (int i = 1; i < matr[0].Length; i++)
                {
                    for (int j = 0; i > j; j++)
                    {
                        values.Add(matr[j][i]);
                    }
                }

                long resultValue = 0;
                int k = 1;
                foreach (var v in values)
                {
                    if (v != int.MaxValue)
                        resultValue += v * k;
                    k *= 2;
                }

                if (resultValue < min)
                    min = resultValue;

            }

            return min;
        }

        //ищем макси-код матрицы смежности
        public long MaxiCode()
        {
            //для каждой комбинации найти значение выражения и найти минимальное среди них
            List<List<int>> permutations = Permute(Edge.ToArray());

            long max = long.MinValue;

            foreach (var p in permutations)
            {
                Edge = p;
                int[][] matr = Matrix();

                List<int> values = new List<int>();

                for (int i = 1; i < matr[0].Length; i++)
                {
                    for (int j = 0; i > j; j++)
                    {
                        values.Add(matr[j][i]);
                    }
                }

                long resultValue = 0;
                int k = 1;
                foreach (var v in values)
                {
                    if (v != int.MaxValue)
                        resultValue += v * k;
                    k *= 2;
                }

                if (resultValue > max)
                    max = resultValue;

            }

            return max;
        }
    }
    #endregion
}
