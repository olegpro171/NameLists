using System.Text.RegularExpressions;

namespace NameLists.Factories;

public static partial class BlockFactory
{
    private static readonly Regex blockPattern = GetBlockRegex();
    
    public static List<Block> CreateFromText(string rawText)
    {
        List<Block> blocks = [];
        
        var cleanText = FormatHelper.RemoveCommentLines(rawText);
        var matches = blockPattern.Matches(cleanText);
        
        foreach (Match match in matches)
        {
            string blockName = match.Groups["blockName"].Value.Trim();
            string blockContent = match.Groups["blockContent"].Value.Trim();
            
            var fields = FieldFactory.CreateFromText(blockContent);
            
            blocks.Add(new Block(blockName, fields));
        }
        
        
        return blocks;
    }
    
    
    
    [GeneratedRegex(@"\&(?<blockName>\w+)(?<blockContent>[\s\S]*?)(?:&END)", RegexOptions.Multiline)]
    private static partial Regex GetBlockRegex();
}