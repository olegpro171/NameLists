using System.Text;
using NameLists.Factories;
using ReflectorGrid.Processing.NameLists;

namespace NameLists;

public class NameList
{
    public string NameListId { get; }
    public List<Block> Blocks { get; } = [];
    
    
    
    
    public static NameList CreateFromText(string rawText) => NameListFactory.CreateFromText(rawText);
    public NameList(string nameListId)
    {
        this.NameListId = nameListId;
    }
    
    public NameList(string nameListId, List<Block> blocks) : this(nameListId)
    {
        Blocks = new List<Block>(blocks.Count);
        blocks.ForEach(AddBlock);
    }

    
    
    
    public void AddBlock(Block block)
    {
        if (Blocks.Any(x => x.BlockId == block.BlockId))
            throw new ArgumentException($"Блок с идентификатором {block.BlockId} уже существует в неймлисте {NameListId}");
        Blocks.Add(block);
    }
    
    public override string ToString()
    {
        var outputSB = new StringBuilder();
        outputSB.AppendLine($"{NameListParameters.NameListIdTag}{NameListId}");
        outputSB.AppendLine();
        foreach (var block in Blocks)
        {
            outputSB.AppendLine(block.ToString());
        }
        return outputSB.ToString();
    }
}