using System.Text;

namespace Homework.ParallelFileReading;

public class FileGenerator
{
    public void GenerateFiles(string folderPath, int fileCount, int fileSize)
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var random = new Random();
        for (int i = 0; i < fileCount; i++)
        {
            var filePath = Path.Combine(folderPath, $"File_{i + 1}.txt");
            var content = new StringBuilder();

            for (int j = 0; j < fileSize; j++)
            {
                content.Append(random.Next(0, 5) == 0 ? ' ' : (char)random.Next('a', 'z' + 1));
            }

            File.WriteAllText(filePath, content.ToString());
        }
    }
}