using System.Text;
using ReflectorGrid.Processing.NameLists;

namespace NameLists;

public class NameList(string nameListId, List<Block> blocks)
{
    public string NameListId { get; } = nameListId ?? throw new ArgumentNullException(nameof(nameListId));
    public List<Block> Blocks { get; } = blocks ?? throw new ArgumentNullException(nameof(blocks));

    public override string ToString()
    {
        var outputSB = new StringBuilder();
        
        outputSB.AppendLine($"{NameListParameters.NameListIdTag}{NameListId}");
        outputSB.AppendLine();
        foreach (var block in blocks)
        {
            outputSB.AppendLine(block.ToString());
        }
        
        return outputSB.ToString();
    }
}