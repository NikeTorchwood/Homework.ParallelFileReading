using System.Diagnostics;

namespace Homework.ParallelFileReading;
public class Program
{
    static async Task Main(string[] args)
    {
        string folderPath;
        int fileCount;
        int fileSizeInSimbols;
        bool isNum;
        do
        {
            Console.WriteLine("Задайте название папки, в которую мы генерируем файлы:");
            folderPath = Console.ReadLine();
        } while (string.IsNullOrEmpty(folderPath));

        do
        {
            Console.WriteLine("Задайте количество генерируемых файлов:");
            isNum = int.TryParse(Console.ReadLine(), out fileCount);
        } while (!isNum && fileCount <= 0);

        do
        {
            Console.WriteLine("Задайте количество символов:");
            isNum = int.TryParse(Console.ReadLine(), out fileSizeInSimbols);
        } while (!isNum && fileSizeInSimbols <= 0);

        var generator = new FileGenerator();
        generator.GenerateFiles(folderPath, fileCount, fileSizeInSimbols);
        Console.WriteLine($"Сгенерировано {fileCount} файлов в папке {Path.GetFullPath(folderPath)}.");

        Console.WriteLine("Нажмите любую клавишу чтобы начать считать");
        Console.ReadKey();

        var files = Directory.GetFiles(folderPath).Take(3).ToArray();
        Console.WriteLine("Считаем пробелы в первых трех файлах:");
        var stopwatch = Stopwatch.StartNew();

        int totalSpaces = await CountSpacesInFilesAsync(files);

        stopwatch.Stop();
        Console.WriteLine($"Общее количество пробелов в первых 3 файлах: {totalSpaces}.");
        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс.");

        Console.WriteLine("\n\nНажмите любую клавишу чтобы начать считать");
        Console.ReadKey();

        Console.WriteLine("Считаем пробелы в файлах, в созданной папке");
        stopwatch.Restart();

        totalSpaces = await CountSpacesInFolderAsync(folderPath);

        stopwatch.Stop();
        Console.WriteLine($"Общее количество пробелов во всех файлах папки: {totalSpaces}.");
        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс.");

        while (true)
        {
            Console.WriteLine("\nВведите \"Y\" для того, чтобы удалить эти файлы и папку.");
            Console.WriteLine("Введите \"N\" для того, чтобы завершить программу без удаления.");
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Y)
            {
                DeleteFolder(folderPath);
                return;
            }
            if (key.Key == ConsoleKey.N)
            {
                return;
            }
        }

    }

    private static void DeleteFolder(string folderPath)
    {
        Directory.Delete(folderPath, true);
    }

    static async Task<int> CountSpacesInFilesAsync(string[] filePaths)
    {
        var tasks = filePaths.Select(async filePath =>
        {
            var content = await File.ReadAllTextAsync(filePath);
            return content.Count(c => c == ' ');
        });

        var results = await Task.WhenAll(tasks);
        return results.Sum();
    }

    static async Task<int> CountSpacesInFolderAsync(string folderPath)
    {
        var filePaths = Directory.GetFiles(folderPath);
        return await CountSpacesInFilesAsync(filePaths);
    }
}
