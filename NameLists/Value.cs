using System.Globalization;

namespace NameLists;

public abstract record Value
{
}

public record IntValue(int Data) : Value
{
    public override string ToString()
    {
        return Data.ToString();
    }
}

public record DoubleValue(double Data) : Value
{
    public override string ToString()
    {
        if (Helpers.DoubleUtils.CountDigits(Data) <= 7)
        {
            return Data.ToString("0.0#####", CultureInfo.InvariantCulture);;
        }

        return Data.ToString("0.0#####E0", CultureInfo.InvariantCulture);
    }
}

public record StringValue(string Data) : Value
{
    public override string ToString()
    {
        return $"'{Data}'";
    }
}
