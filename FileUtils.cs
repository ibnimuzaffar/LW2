using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UserInterface;

public static class FileUtils
{
    // проверяет содержит ли путь к файлу некорректные символы
    public static bool IsPathValid(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        // получаем массив недопустимых символов для пути
        char[] invalidChars = Path.GetInvalidPathChars();

        // проверяем, содержит ли путь недопустимые символы
        if (path.IndexOfAny(invalidChars) != -1)
        {
            return false;
        }

        return true;
    }

    // проверяет содержит ли название файла некорректные символы
    public static bool IsFileNameValid(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return false;
        }

        // получаем массив недопустимых символов для пути
        char[] invalidChars = Path.GetInvalidFileNameChars();

        // проверяем, содержит ли путь недопустимые символы
        if (fileName.IndexOfAny(invalidChars) != -1)
        {
            return false;
        }

        return true;
    }

    // проверяет не является ли указанный файл папкой
    public static bool IsFileDirectory(string fileName)
    {
        return (File.GetAttributes(fileName) & FileAttributes.Directory) == FileAttributes.Directory;
    }

    // проверяет доступен ли файл только для чтения
    public static bool IsFileReadOnly(string fileName)
    {
        return (File.GetAttributes(fileName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
    }

    
}

