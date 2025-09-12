using System.Text;
using ReflectorGrid.Processing.NameLists;

namespace NameLists;

public class Block(string blockId, List<Field> fields)
{
    public string BlockId { get; } = blockId ?? throw new ArgumentNullException(nameof(blockId));
    public List<Field> Fields { get; } = fields ?? throw new ArgumentNullException(nameof(fields));

    public override string ToString()
    {
        var outputSB = new StringBuilder();
        
        outputSB.AppendLine($"{NameListParameters.NameListIdTag}{BlockId}");
        foreach (var field in Fields)
        {
            outputSB.AppendLine($"  {field.ToString()}");
        }
        outputSB.AppendLine($"{NameListParameters.NameListIdTag}{NameListParameters.BlockEndKeyword}");
        
        return outputSB.ToString();
    }
}