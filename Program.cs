using System;
using System.IO;
using System.Runtime.InteropServices;

class Programm
{
    public static void Main(string[] args)
    {
        //long size = 0;
        DirectoryInfo dirInfo = new DirectoryInfo(@"/Users/Kolesnikov_aa/desktop/8Module");
        long size = LengthFiles(dirInfo);                                   // Вызов метода расчета исходного объема файлов

        if (dirInfo.Exists)                                                 // Проверка пути
            LengthFiles(dirInfo);                                           // Повторный вызов метода расчета объема файлов (после удаления файлов)
            Console.WriteLine("\n\tДиректорий: " + dirInfo + "");
            long size1 = size;
            Console.WriteLine("\n\t" + size1 + " байт - исходный размер директория");

            DeleteFiles(dirInfo);                                           // Вызов метода удаления файлов

            LengthFiles(dirInfo);                                           // Повторный вызов метода расчета объема файлов (после удаления файлов)
            long size2 = size;
            Console.WriteLine("\n\t" + size2 + " байт - размер директория после очистки");
            
            long size3 = size2 - size1;
            Console.WriteLine("\n\t" + size3 + " байт - очищено");
    }

    /// <summary>
    /// Рекурсия - расчет объема файлов в директории
    /// </summary>
    /// <param name="dirInfo"></param>
    /// <returns></returns>
    public static long LengthFiles(DirectoryInfo dirInfo)
    {
        long size = 0;

        FileInfo[] fff = dirInfo.GetFiles();
        foreach (FileInfo ff in fff)                                        // Перебираем файлы в директории
        {
            size += ff.Length;                                              // Считаем объем, суммируем
        }

        DirectoryInfo[] ddd = dirInfo.GetDirectories();
        foreach (DirectoryInfo dd in ddd)                                   // Перебираем директории
        {
            try
            {
                size += LengthFiles(dd);                                    // Считаем объем, суммируем
                dirInfo = dd;
            }
            catch (Exception e)                                             // Проверка исключений
            {
                Console.WriteLine(e.Message);
            }
        }
        return size;
    }

    /// <summary>
    /// Рекурсия - удаление файлов, сохраненых более 30 минут назад
    /// </summary>
    /// <param name="dirInfo"></param>
    public static void DeleteFiles(DirectoryInfo dirInfo)
    {
        FileInfo[] fff = dirInfo.GetFiles();
        foreach (FileInfo ff in fff)                                        // Перебираем файлы в директории
        {
            DateTime lastWriteTime = File.GetLastWriteTime("" + ff);        // Время последнего сохранения // На "ff" - ругался, получилось через такую конструкцию
            TimeSpan timeSpan = TimeSpan.FromMinutes(30);                   // "Контрольный интервал"
            DateTime minTime = DateTime.Now.Subtract(timeSpan);             // Допустимое время (текущее время - 30 минут)

            if (lastWriteTime < minTime)                                    // Если время последнего сохранения < допустимого
            {
                File.Delete("" + ff);
            }
        }

        DirectoryInfo[] ddd = dirInfo.GetDirectories();
        foreach (DirectoryInfo dd in ddd)                                   // Перебираем директории
        {
            try
            {
                dirInfo = dd;
                DeleteFiles(dirInfo);
            }
            catch (Exception e)                                             // Проверка исключений
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
