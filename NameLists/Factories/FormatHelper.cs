using ReflectorGrid.Processing.NameLists;

namespace NameLists.Factories;

internal static class FormatHelper
{
    public static string RemoveCommentLines(string content)
    {
        var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var nonCommentLines = new List<string>();

        foreach (var line in lines)
        {
            // Trim the line and check if it starts with '#'
            if (!line.TrimStart().StartsWith(NameListParameters.NameListCommentTag))
            {
                nonCommentLines.Add(line);
            }
        }

        return string.Join(Environment.NewLine, nonCommentLines);
    }
}