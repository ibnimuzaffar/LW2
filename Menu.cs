using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface;

// перечисление пунктов меню
public enum MenuItem
{
    Exit,
    InputFromConsole,
    LoadFromFile,
    ShowInputDataAndResult,
    SaveInputToFile,
    SaveResultToFile
}

// класс представляющий консольное меню
public class Menu
{
    // показывает загружал ли пользователь данные в программу
    public bool IsDataLoaded { get; set; }

    // номер столбца с которого начинается вывод меню
    private int Left { get; }
    // номер строки с которого начинается вывод меню
    private int Top { get; }

    private const string guide = "Используйте стрелки вверх, вниз и Enter для выбора пункта меню.";

    // список строк для вывода, если пользователь ввел данныые в программу
    private List<string> menuLinesWhenDataIsLoaded = new List<string>()
    {
        "Выйти из программы",
        "Ввести данные с консоли",
        "Загрузить данные с файла",
        "Показать введённые данные и результат",
        "Сохранить исходные данные в файл",
        "Сохранить результат в файл"
    };

    // список строк для вывода, если пользователь не ввел данные в программу
    private List<string> menuLinesWhenDataIsNotLoaded = new List<string>()
    {
        "Выйти из программы",
        "Ввести данные с консоли",
        "Загрузить данные с файла"
    };

    public Menu(int left, int top)
    {
        this.IsDataLoaded = false;
        this.Left = left;
        this.Top = top;
    }

    // выводит меню на экран и возвращает выбор пользователя
    public MenuItem PrintAndGetChoice()
    {
        // установка текущего меню в зависимости от того ввел пользователь данные или нет
        List<string> currentMenu = IsDataLoaded ? menuLinesWhenDataIsLoaded : menuLinesWhenDataIsNotLoaded;

        // индекс пункта меню, который при приведении к MenuItem отображает выбранный пользователем пункт меню
        int choiceIndex = 0;

        // хранит нажатую пользователем клавишу
        ConsoleKeyInfo pressedKeyInfo;

        Console.CursorVisible = false;

        do
        {
            // установка курсора в указанную позицию
            Console.SetCursorPosition(Left, Top);

            // вывод меню в консоль
            DisplayMenu(currentMenu, choiceIndex);

            Console.WriteLine("\n" + guide);

            // получение нажатой пользователем клавишу
            pressedKeyInfo = Console.ReadKey(intercept: true);

            // обработка нажатие клавиши
            if (pressedKeyInfo.Key == ConsoleKey.DownArrow)
            {
                choiceIndex = (choiceIndex == currentMenu.Count - 1) ? currentMenu.Count - 1 : choiceIndex + 1; 
            }
            else if (pressedKeyInfo.Key == ConsoleKey.UpArrow)
            {
                choiceIndex = (choiceIndex == 0) ? 0 : choiceIndex - 1;
            }

            ClearMenuArea(currentMenu.Count);
        }
        while (pressedKeyInfo.Key != ConsoleKey.Enter);

        Console.CursorVisible = true;
        return (MenuItem)choiceIndex;
    }

    // выводит меню в консоль
    private void DisplayMenu(List<string> currentMenuLines, int choiceIndex)
    {
        // установка курсора в указанную позицию
        Console.SetCursorPosition(Left, Top);

        // вывод меню в консоль
        for (int i = 0; i < currentMenuLines.Count; i++)
        {
            if (i == choiceIndex)
            {
                // вывод выбранного пункта 
                Console.WriteLine(" > " + currentMenuLines[i]);
            }
            else
            {
                // вывод невыбранного пункта
                Console.WriteLine(currentMenuLines[i]);
            }
        }
    }

    private void ClearMenuArea(int menuLinesCount)
    {
        for (int i = 0; i < menuLinesCount + 2; i++)
        {
            Console.SetCursorPosition(this.Left, this.Top + i);
            Console.WriteLine(new string(' ', Console.WindowWidth));
        }

        Console.SetCursorPosition(this.Left, this.Top);
    }
}

