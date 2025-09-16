using System.Text.RegularExpressions;
using NameLists.DataStructures;

namespace NameLists.Factories;

internal static partial class NameListFactory
{
    public static NameList CreateFromText(string rawText)
    {
        string variantName = ExtractVariantName(rawText);
        
        List<Block> blocks = BlockFactory.CreateFromText(rawText);
        
        return new NameList(variantName, blocks);
    }

    private static string ExtractVariantName(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        // Split by any newline convention
        var firstLine = text.Split(["\r\n", "\n", "\r"], StringSplitOptions.None)[0];

        if (!firstLine.StartsWith(NameListParameters.NameListIdTag))
            return string.Empty;

        // Remove '*' and trim spaces
        var content = firstLine.TrimStart('#').Trim();

        // Limit to 32 chars if longer
        return content.Length > 32 ? content.Substring(0, 32) : content;
    }

    
}