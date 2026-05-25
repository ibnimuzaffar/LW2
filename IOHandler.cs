using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface;
public static class IOHandler
{
    public static (double, double, double) GetPoint()
    {
        double x, y, z;
        try
        {
            Console.Write("Введите координату x: ");
            x = GetDouble();

            Console.Write("Введите координату y: ");
            y = GetDouble();

            Console.Write("Введите координату z: ");
            z = GetDouble();
        }
        catch (FormatException)
        {
            throw new Exception("Некорректный ввод");
        }
        catch (OverflowException)
        {
            throw new Exception("Введенное число не вмещается в тип double");
        }

        return (x, y, z);
    }

    // спрашивает у пользователя параметры сферы
    public static (double, double, double, double) GetSphere()
    {
        double x, y, z, radius;

        Console.WriteLine("Ввод сферы:");

        try
        {
            Console.Write("Введите координату x центра сферы: ");
            x = GetDouble();

            Console.Write("Введите координату y центра сферы: ");
            y = GetDouble();

            Console.Write("Введите координату z центра сферы: ");
            z = GetDouble();

            Console.Write("Введите радиус сферы: ");
            radius = GetDouble();
            if (radius <= 0) throw new Exception("Некорректный радиус");
        }
        catch (FormatException)
        {
            throw new Exception("Некорректный ввод");
        }
        catch (OverflowException)
        {
            throw new Exception("Введенное число не вмещается в тип double");
        }

        return (x, y, z, radius);
    }

    // выводит на консоль координаты точки
    public static void PrintXYZ(double x, double y, double z)
    {
        Console.WriteLine($"x = {x}; y = {y}; z = {z}");
    }

    // выводит на консоль координаты центра сферы и ее радиус
    public static void PrintSphere(double x, double y, double z, double radius)
    {
        Console.WriteLine($"x = {x}; y = {y}; z = {z}; R = {radius}");
    }

    // Спрашивает у пользователя файл, чтобы загрузить данные в программу
    public static string GetFileNameForInput()
    {
        string? fileName;
        
        Console.Write("Введите название файла: ");
        fileName = GetLine();

        if (!File.Exists(fileName)) throw new Exception("Такого файла не существует");

        return fileName;
        
    }

    // спрашивает у пользователя файл, чтобы загрузить в него исходные данные
    public static string GetFileNameForOutput()
    {
        string? fileName;

        Console.Write("Введите название файла: ");
        fileName = GetLine();

        if (!FileUtils.IsPathValid(fileName)) throw new Exception("Путь к файлу содердит недопустимые символы");
        if (!FileUtils.IsFileNameValid(Path.GetFileName(fileName))) throw new Exception("Название файла содержит недопустимые символы");

        if (File.Exists(fileName))
        {
            if (YesOrNo("Указанный файл уже существует. Добавить данные в файл? (да/нет) "))
            {
                if (FileUtils.IsFileReadOnly(fileName)) throw new Exception("Указанный файл доступен только для чтения");
                if (FileUtils.IsFileDirectory(fileName)) throw new Exception("По указанному пути находится папка, а не файл");
                return fileName;
            }
            else
            {
                throw new Exception("");
            }
            
        }
        else
        {
            return fileName;
        }
    }

    // считывает с указанного файла коэффициенты и интервал
    public static ((double, double, double)[], (double, double, double, double)) ReadSphereAndPoints(string fileName)
    {
        List<(double X, double Y , double Z)> points = new List<(double, double, double)>();
        (double X, double Y, double Z, double R) sphere = (0, 0, 0, 0);
        bool hasSphere = false;

        char[] separators = new char[] { ' ', '=' };

        // получаем массив строк
        string[] lines = File.ReadAllLines(fileName);

        // разбираем каждую строку
        foreach (string line in lines)
        {
            string[] splitted = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (splitted.Length < 9 &&
                splitted.Contains("R") &&
                splitted.Contains("x") &&
                splitted.Contains("y") &&
                splitted.Contains("z"))
            {
                if (hasSphere == true) throw new Exception("В файле не может быть 2 или более сферы");

                // получаем число, следующее после "x"
                double x = Double.Parse(splitted[(Array.FindIndex(splitted, (part) => part == "x")) + 1]);

                // получаем число, следующее после "y"
                double y = Double.Parse(splitted[(Array.FindIndex(splitted, (part) => part == "y")) + 1]);

                // получаем число, следующее после "z"
                double z = Double.Parse(splitted[(Array.FindIndex(splitted, (part) => part == "z")) + 1]);

                // получаем число, следующее после "R"
                double radius = Double.Parse(splitted[(Array.FindIndex(splitted, (part) => part == "R")) + 1]);

                if (radius <= 0) throw new Exception("Некорректный радиус сферы");

                sphere = (x, y, z, radius);

                hasSphere = true;
            }
            else if (splitted.Length < 7 &&
                splitted.Contains("x") &&
                splitted.Contains("y") &&
                splitted.Contains("z"))
            {
                // получаем число, следующее после "x"
                double x = Double.Parse(splitted[(Array.FindIndex(splitted, (part) => part == "x")) + 1]);

                // получаем число, следующее после "y"
                double y = Double.Parse(splitted[(Array.FindIndex(splitted, (part) => part == "y")) + 1]);

                // получаем число, следующее после "z"
                double z = Double.Parse(splitted[(Array.FindIndex(splitted, (part) => part == "z")) + 1]);

                points.Add((x, y, z));
            }
        }

        // обработка ошибок
        if (hasSphere == false && points.Count == 0)
        {
            throw new Exception("В файле не обнаружено данных");
        }
        else if (hasSphere == false)
        {
            throw new Exception("В файле не обнаружено ниодной сферы");
        }
        else if (points.Count == 0)
        {
            throw new Exception("В файле не обнаружено ниодной точки");
        }

        // вывод считанной информации
        Console.WriteLine($"Было добавлено:\n\t {points.Count} точек\n\t сфера с центром в ({sphere.X}, {sphere.Y}, {sphere.Z}) и R = {sphere.R}");

        return (points.ToArray(), sphere);
    }

    // записывает в указанный файл данные о сфере
    public static void WriteSphere(
        string fileName,
        (double x, double y, double z, double R) sphere)
    {
        File.AppendAllText(fileName, $"x = {sphere.x} y = {sphere.y} z = {sphere.z} R = {sphere.R}");
    }

    // записывает в указанный файл данные о точках
    public static void WritePoints(
        string fileName,  
        (double, double, double)[] points)
    {
        foreach ((double x, double y, double z) point in points)
        {
            File.AppendAllText(fileName, $"x = {point.x} y = {point.y} z = {point.z}");
        }
    }

    // записывает в указанный файл корни, коэффициенты и интервал
    public static void WriteInnerPoints(
        string fileName,
        (double?, double?) roots,
        (double a, double b, double c) coefs,
        (double begin, double end) interval)
    {
        string text = $"\na = {coefs.a} b = {coefs.b} c = {coefs.c} d = {interval.begin} e = {interval.end} ";

        if (roots.Item1 == null && roots.Item2 == null)
        {
            text += "корней нет";
        }
        else if (roots.Item1 != null && roots.Item2 == null)
        {
            text += $"один корень x = {roots.Item1}";
        }
        else
        {
            text += $"два корня x1 = {roots.Item1} x2 = {roots.Item2}";
        }


        File.AppendAllText(fileName, text);
    }

    // выводит полный путь к файлу в консоль
    public static void PrintFullPath(string fileName)
    {
        Console.WriteLine("Файл был сохранен по адресу: " + Path.GetFullPath(fileName));
    }

    // считывает непустую строку с консоли
    private static string GetLine()
    {
        while (true)
        {
            string? input = Console.ReadLine();
            if (input != "" && input != null) return input;
        }
    }

    // считывает число с плавающей точкой с консоли
    private static double GetDouble()
    {
        while (true)
        {
            string? input = Console.ReadLine();
            if (input != "" && input != null) return Double.Parse(input);
        }        
    }

    // спрашивает пользователя, возвращает true или false, в зависимости от ответа
    private static bool YesOrNo(string message)
    {
        string? response;
        while (true)
        {
            Console.Write(message);
            response = GetLine();

            if (response.ToLower() == "да") return true;
            if (response.ToLower() == "нет") return false;

            Console.WriteLine("Некорректный ответ");
        }
    }

    // возвращает true, если была нажата клавиша Enter, в противном случае возваращает false
    public static bool CheckForEnterKey(
        string message = "Нажмите Enter для выхода в меню или любую другую клавишу, чтобы повторить ввод.")
    {
        Console.WriteLine(message);

        // Считываем нажатую клавишу, не отображая её в консоли
        ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

        // Проверяем, была ли нажата клавиша Enter
        return keyInfo.Key == ConsoleKey.Enter;
    }
}

