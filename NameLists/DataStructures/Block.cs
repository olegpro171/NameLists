using System.Text;

namespace NameLists.DataStructures;

public class Block
{
    public string BlockId { get; }
    public List<Field> Fields { get; }


    
    public Block(string blockId)
    {
        BlockId = blockId;
    }
    public Block(string blockId, List<Field> fields) : this(blockId)
    {
        Fields = new List<Field>(fields.Count);
        fields.ForEach(AddField);
    }

    
    
    public void AddField(Field field)
    {
        if (Fields.Any(x => x.Identifier == field.Identifier))
            throw new ArgumentException($"Поле с идентификатором {field.Identifier} уже существует в блоке {BlockId}");
        Fields.Add(field);
    }
    
    public override string ToString()
    {
        // расчет самого длинного имени поля для выравнивания
        var identCount = Fields.Max(x => x.Identifier.Length);
        
        var outputSB = new StringBuilder();
        
        outputSB.AppendLine($"{NameListParameters.BlockIdTag}{BlockId}");
        foreach (var field in Fields)
        {
            if (NameListParameters.BaseIdent > 0)
                outputSB.Append(' ', NameListParameters.BaseIdent);
            outputSB.AppendLine($"{field.ToString(identCount)}");
        }
        outputSB.AppendLine($"{NameListParameters.BlockIdTag}{NameListParameters.BlockEndKeyword}");
        
        return outputSB.ToString();
    }
}