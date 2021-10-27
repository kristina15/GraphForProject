
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
                foreach (var v in key.Value)
                {
                    Edge.Add(key.Key);
                    Edge.Add(v.Key);
                }
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


        //найти кратчайшее расстояние между вершинами, их сумма - Индекс Винера
        public int VinerIndex()
        {
            int resultSum = 0;
            foreach (var v in theGraph)
            {
                if (theGraph.Keys.Any(v1 => v1 == v.Key))
                {
                    Console.WriteLine("Данная вершина {0} является искомой", v.Key);
                }
                else
                {
                    foreach (var v1 in theGraph)
                    {
                        if (v1.Key != v.Key)
                        {
                            int result = GetShortest(v.Key, v1.Key, theGraph.Count);

                            resultSum += result;
                        }
                    }
                }
            }
            return resultSum;
        }

        public int GetShortest(int start, int end, int triesCount)
        {
            List<List<int>> routes = new List<List<int>>();
            foreach (var v in theGraph[start])
            {
                routes.Add(new List<int> { start, v.Key });
            }

            int stepCount = 1;

            while (true)
            {
                if (stepCount >= triesCount)
                {
                    return 0;
                }

                var foundRoute = routes.FirstOrDefault(r => r.Any(v => v == end));
                if (foundRoute != null)
                {
                    return foundRoute.Count;
                }

                var newRoutes = new List<List<int>>();
                foreach (var route in routes)
                {
                    foreach (var nextVertex in theGraph[route.Last()])
                    {
                        var newRoute = new List<int>(route);
                        newRoute.Add(nextVertex.Key);
                        newRoutes.Add(newRoute);
                    }
                }
                routes = newRoutes;
                stepCount++;
            }
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
                int j = 0;
                foreach (int v in Edge)
                {
                    foreach (var v1 in theGraph[i])
                    {
                        if (v1.Key == v)
                        {
                            a[i][j] = v1.Value;
                        }

                        else
                        {
                            a[i][j] = 0;
                        }
                    }
                    j += 1;
                }
            }
            return a;
        }

        //determinant
        public int FindDeter()
        {
            int det = 1;
            const double EPS = 1E-9;
            int n = theGraph.Count;
            int[][] a = Matrix();
            int[][] b = new int[n][];
            //проходим по строкам
            for (int i = 0; i < n; ++i)
            {
                //присваиваем k номер строки
                int k = i;
                //идем по строке от i+1 до конца
                for (int j = i + 1; j < n; ++j)
                    //проверяем
                    if (Math.Abs(a[j][i]) > Math.Abs(a[k][i]))
                        //если равенство выполняется то k присваиваем j
                        k = j;
                //если равенство выполняется то определитель приравниваем 0 и выходим из программы
                if (Math.Abs(a[k][i]) < EPS)
                {
                    det = 0;
                    break;
                }
                //меняем местами a[i] и a[k]
                b[0] = a[i];
                a[i] = a[k];
                a[k] = b[0];
                //если i не равно k
                if (i != k)
                    //то меняем знак определителя
                    det = -det;
                //умножаем det на элемент a[i][i]
                det *= a[i][i];
                //идем по строке от i+1 до конца
                for (int j = i + 1; j < n; ++j)
                    //каждый элемент делим на a[i][i]
                    a[i][j] /= a[i][i];
                //идем по столбцам
                for (int j = 0; j < n; ++j)
                    //проверяем
                    if ((j != i) && (Math.Abs(a[j][i]) > EPS))
                        //если да, то идем по k от i+1
                        for (k = i + 1; k < n; ++k)
                            a[j][k] -= a[i][k] * a[j][i];
            }

            return det;
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
                foreach (int vertexForCheck in usedV)
                {
                    foreach (var v in theGraph[vertexForCheck])
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
            return result;
        }

        //Цикломатическое число
        //находится как разница суммы числа компонент связности и числа ребер с числом чершин
        public int AcyclomaticNumber()
        {
            int r = NumberOfConnectedComponents();
            //матрица смежности
            int[][] a = Matrix();

            int sumOfEdges = 0;
            foreach (var ch in a)
            {
                sumOfEdges += 1;
            }

            sumOfEdges /= 2;

            return r + sumOfEdges - theGraph.Count;
        }

        //?????????хроматическое число?????????

        ////вектор степеней 2 порядка - отсортированный словарь для упрощенного поиска
        //public Dictionary<int, Dictionary<int, int>> SortedGraph()
        //{
        //    Dictionary<int, Dictionary<int, int>> result = (Dictionary<int, Dictionary<int, int>>)theGraph.OrderBy(k => k.Key);
        //    foreach (var vert in theGraph)
        //    {
        //        result[vert.Key] = (Dictionary<int, int>)vert.Value.OrderBy(k => k.Key);
        //    }
        //    return result;
        //}


    }
}