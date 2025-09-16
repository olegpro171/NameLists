using System.Text;

namespace Helpers;

public static class TextFileReader
{
    public static string ReadTextFromFile(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            throw new ArgumentNullException(nameof(filename), "Указан пустой путь");

        try
        {
            // Try UTF-8 first
            string text = File.ReadAllText(filename, Encoding.UTF8);

            // If text contains � replacement characters, try Windows-1251
            if (text.Contains('�'))
            {
                text = File.ReadAllText(filename, Encoding.GetEncoding(1251));
            }

            return text;
        }
        catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
        {
            throw new DirectoryNotFoundException($"Не найден файл по указанной директории: {filename}", ex);
        }
    }
}




