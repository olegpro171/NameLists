namespace NameLists;

public class Field
{
    public string Identifier { get; }
    public List<Value> Values { get; }
    
    public Field(string identifier, List<Value> values)
    {
        Identifier = identifier;
        Values = values;
    }


    public override string ToString() => ToString(indent: 0);
    public string ToString(int indent)
    {
        if (Values.Count == 0)
            //return $"{Identifier} = ,";
            return string.Empty;

        var parts = new List<string>();
        int count = 1;
        Value current = Values[0];

        for (int i = 1; i < Values.Count; i++)
        {
            if (Values[i].Equals(current))
            {
                count++;
            }
            else
            {
                parts.Add(count > 1 ? $"{count}*{current}" : current.ToString());
                current = Values[i];
                count = 1;
            }
        }
        // add the last run
        parts.Add(count > 1 ? $"{count}*{current}" : current.ToString());
        
        if (indent > 0)
            return $"{Identifier.PadRight(indent)} = {string.Join(", ", parts)},";
        else
            return $"{Identifier} = {string.Join(", ", parts)},";
    }
}