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

    public override string ToString()
    {
        if (Values.Count == 0)
            return $"{Identifier} = ,";

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

        return $"{Identifier} = {string.Join(", ", parts)},";
    }
}