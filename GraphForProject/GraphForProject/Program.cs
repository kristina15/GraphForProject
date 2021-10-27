using RightGraph;
using System;

namespace GraphForProject
{
    class Program
    {
        static void Main(string[] args)
        {
            bool ves = true;

            Graph g1 = new Graph("C:/Users/krisF/OneDrive/Документы/GitHub/GraphForProject/GraphForProject/GraphForProject/inputGraph1.txt", ves);
            Graph g2 = new Graph("C:/Users/krisF/OneDrive/Документы/GitHub/GraphForProject/GraphForProject/GraphForProject/inputGraph2.txt", ves);

            Console.WriteLine("1 Эвристический алгоритм: ");
            var g1VinerIndex = g1.VinerIndex();
            var g1RandicIndex = g1.RandicIndex();
            var g1NumberConnectedComponents = g1.NumberOfConnectedComponents();
            var g1Cyclomatic = g1.CyclomaticNumber();

            Console.WriteLine($"G1: \n{g1.ToStringGraph(true)} " +
                $"\n Индекс Винера: {g1VinerIndex} " +
                $"\n Индекс Рандича: {g1RandicIndex} " + 
                $"\n Число компонент связности: {g1NumberConnectedComponents}" +
                $"\n Цикломатическое число: {g1Cyclomatic} " +
                $"\n");

            var g2VinerIndex = g2.VinerIndex();
            var g2RandicIndex = g2.RandicIndex();
            var g2NumberConnectedComponents = g2.NumberOfConnectedComponents();
            var g2Cyclomatic = g2.CyclomaticNumber();

            Console.WriteLine($"G1: \n{g2.ToStringGraph(true)} " +
                $"\n Индекс Винера: {g2VinerIndex} " +
                $"\n Индекс Рандича: {g2RandicIndex} " +
                $"\n Число компонент связности: {g2NumberConnectedComponents}" +
                $"\n Цикломатическое число: {g2Cyclomatic} " +
                $"\n");

            if (g1VinerIndex == g2VinerIndex && (int)g1RandicIndex == (int)g2RandicIndex && g1Cyclomatic == g2Cyclomatic && g1NumberConnectedComponents == g2NumberConnectedComponents)
            {
                Console.WriteLine("Результат: Ожидаем изоморфизм");
            }
            else
            {
                Console.WriteLine("Изоморфизма нет");
            }

            Console.WriteLine("\n2 Эвристический алгоритм: ");
            var g1MinCode = g1.MiniCode();
            var g1MaxiCode = g1.MaxiCode();

            Console.WriteLine($"G1: \n Мин-код: {g1MinCode} " +
                $"\n Макс-код: {g1MaxiCode}");

            var g2MinCode = g2.MiniCode();
            var g2MaxiCode = g2.MaxiCode();

            Console.WriteLine($"G2: \n Мин-код: {g2MinCode} " +
                $"\n Макс-код: {g2MaxiCode}");

            if (g1MinCode == g2MinCode && g1MaxiCode == g2MaxiCode)
            {
                Console.WriteLine("\nРезультат: Ожидаем изоморфизм");
            }
            else
            {
                Console.WriteLine("\nИзоморфизма нет");
            }

            Console.ReadLine();

        }
    }
}
