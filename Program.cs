using UserInterface;
using MathMethods;
using System.ComponentModel;

namespace LW1_Program;


// переделать ввод с консоли, остальное ок
class Program
{
    static void Main(string[] args)
    {
        const string info = """
            Программа создана для лабораторной работы №2 варианта №2 РПС
            Задание: В трехмерном пространстве заданы точки (тройками x, y, z)
            и сфера (центр и радиус).Напишите программу, выводящую точки (их координаты),
            которые попадают в заданную пользователем сферу.
            Автор: студент группы 443 Акпаров Шукрулло
            """;

        Menu menu = new Menu(0, 7);

        Sphere sphere = new Sphere();
        List<Point> points = new List<Point>();
        List<Point> innerPoints = new List<Point>();

        while (true)
        {
            Console.Write(info + "\n");

            switch (menu.PrintAndGetChoice())
            {
                // ввод с консоли
                case MenuItem.InputFromConsole:
                    // ввод точек
                    // после каждой введенной точки предлагается завершить ввод и начать ввод сферы
                    // при некорректном вводе выводит сообщение и возвращает введенное количество точек
                    Console.WriteLine("Ввод точек:");
                    while (true)
                    {
                        try
                        {
                            (double x, double y, double z) pointTuple = IOHandler.GetPoint();
                            points.Add(new Point(pointTuple.x, pointTuple.y, pointTuple.z));
                            Console.WriteLine($"Точка x = {pointTuple.x} y = {pointTuple.y} z = {pointTuple.z} была добавлена");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        if (!IOHandler.CheckForEnterKey("Нажмите Enter, чтобы завершить ввод точек или любую другую клавишу, чтобы продолжить.")) continue;

                        break;
                    }

                    Console.WriteLine($"Введено {points.Count} точек");
                    
                    while (true)
                    { 
                        // ввод сферы
                        try
                        {
                            (double X, double Y, double Z, double R) = IOHandler.GetSphere();
                            sphere = new Sphere(new Point(X, Y, Z), R);
                            Console.WriteLine($"Введена сфера с центром x = {sphere.Center.X} y = {sphere.Center.Y} z = {sphere.Center.Z} и R = {sphere.Radius}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        if (IOHandler.CheckForEnterKey("Нажмите Enter, чтобы выйти в главное меню")) break;
                    }

                    if (points.Count != 0 && sphere.Radius != 0)
                    {
                        innerPoints = sphere.GetInnerPoints(points);
                        menu.IsDataLoaded = true;
                    }

                    break;
                // ввод с файла
                case MenuItem.LoadFromFile:
                    while (true)
                    {
                        try
                        {
                            string fileName = IOHandler.GetFileNameForInput();

                            (double, double, double)[] pointTuples;
                            (double X, double Y, double Z, double R) sphereTuple;
                            (pointTuples, sphereTuple) = IOHandler.ReadSphereAndPoints(fileName);
                            
                            // преобразуем массив кортежей в список объектов типа Points
                            foreach ((double X, double Y, double Z) pointTuple in pointTuples)
                            {
                                points.Add(new Point(pointTuple.X, pointTuple.Y, pointTuple.Z));
                            }

                            sphere = new Sphere(new Point(sphereTuple.X, sphereTuple.Y, sphereTuple.Z), sphereTuple.R);

                            innerPoints = sphere.GetInnerPoints(points);
                            menu.IsDataLoaded = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        if (!IOHandler.CheckForEnterKey()) continue;

                        break;

                    }
                    break;
                // вывод введённых данных в консоль
                case MenuItem.ShowInputDataAndResult:
                    while(true)
                    {
                        try
                        {
                            Console.WriteLine("Точки:");
                            foreach (Point point in points)
                            {
                                Console.Write("\t");
                                IOHandler.PrintXYZ(point.X, point.Y, point.Z);
                            }

                            Console.WriteLine("Сфера:");
                            Console.Write("\t");
                            IOHandler.PrintSphere(sphere.Center.X, sphere.Center.Y, sphere.Center.Z, sphere.Radius);

                            Console.WriteLine("Точки, находящиеся внутри сферы:");

                            if (innerPoints.Count == 0)
                            {
                                Console.WriteLine("Точек внутри сферы нет");
                            }
                            else
                            {
                                foreach (Point point in innerPoints)
                                {
                                    Console.Write("\t");
                                    IOHandler.PrintXYZ(point.X, point.Y, point.Z);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        if (IOHandler.CheckForEnterKey("Нажмите Enter для выхода в главное меню")) break;
                    }
                    break;
                // сохранение исходных данных в файл
                case MenuItem.SaveInputToFile:
                    while (true)
                    {
                        try
                        {
                            (double x, double y, double z)[] pointTuples = new (double x, double y, double z)[points.Count];
                            string fileName = IOHandler.GetFileNameForOutput();

                            // преобразуем  список объектов типа Points в массив кортежей
                            for (int i = 0; i < points.Count; i++)
                            {
                                pointTuples[i] = (points[i].X, points[i].Y, points[i].Z);
                            }

                            IOHandler.WritePoints(fileName, pointTuples);
                            IOHandler.PrintFullPath(fileName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        if (!IOHandler.CheckForEnterKey()) continue;

                        break;
                    }
                    break;
                // сохранение результата в файл
                case MenuItem.SaveResultToFile:
                    while (true)
                    {
                        try
                        {
                            (double x, double y, double z)[] innerPointTuples = new (double x, double y, double z)[points.Count];
                            string fileName = IOHandler.GetFileNameForOutput();

                            // преобразуем  список объектов типа Points в массив кортежей
                            for (int i = 0; i < points.Count; i++)
                            {
                                innerPointTuples[i] = (points[i].X, points[i].Y, points[i].Z);
                            }

                            IOHandler.WritePoints(fileName, innerPointTuples);
                            IOHandler.PrintFullPath(fileName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        if (!IOHandler.CheckForEnterKey()) continue;

                        break;

                    }
                    break;
                case MenuItem.Exit:
                    return;
            }

            Console.Clear();
        }
    }

}
