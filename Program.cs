using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApplication1
{
    class Program
    {
        const string menu_text = "1.Найти кратчайший путь\n2.Перезапуск\n3.Выход";
        static Graph graph;
        static void Main(string[] args)
        {
            SetGraph();
            UserMenu();
        }

        static void SetGraph()//Ввод графа с файла
        {
            System.Console.WriteLine("Введите название файла с графом:");
            try
            {
                graph = new Graph(System.Console.ReadLine(), true);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Исключение:" + e.Message);
                Restart();
            }
        }

        static void Restart()//Перезапуск программы
        {
            SetGraph();
            UserMenu();
        }

        static void UserMenu()//Вывод меню
        {
            int i = 0;
            bool restart = false;
            while (!restart)
            {
                System.Console.Clear();
                System.Console.WriteLine(menu_text);
                if (Int32.TryParse(System.Console.ReadLine(), out i))
                    switch (i)
                    {
                        case 1:
                            PrintDist();
                            break;
                        case 2:
                            Restart();
                            break;
                        case 3:
                           System.Environment.Exit(1);
                            break;
                        default:
                            System.Console.WriteLine("Try again");
                            UserMenu();
                            break;

                    }
                System.Console.ReadKey();
            }
        }

        static void PrintDist()
        {
            System.Console.WriteLine("Введите номера двух вершин для вывода расстояния");
            int v1 = Int32.Parse(System.Console.ReadLine().ToString()) - 1;
            int v2 = Int32.Parse(System.Console.ReadLine().ToString()) - 1;
            //System.Console.WriteLine("Расстояние между вершинами:{0}", graph.DistMatr[v1, v2]);
            System.Console.WriteLine("Путь:" + graph.Dijkstra(v1, v2));
        
        }
    }
}
