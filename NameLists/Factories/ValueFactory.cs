using System.Globalization;
using NameLists.DataStructures;
using NameLists.Tokenizing;


namespace NameLists.Factories;

internal class ValueFactory
{
    public static Value CreateValueFromText(string rawText)
    {
        var valueType = ValueTypeIdentifier.GetTokenValueType(rawText);

        switch (valueType)
        {
            case FieldValueType.String:
                return new StringValue(rawText.TrimStart(NameListParameters.NameListStringEnclosure)
                                                   .TrimEnd(NameListParameters.NameListStringEnclosure));
            
            case FieldValueType.Int:
                return new IntValue(int.Parse(rawText));
            
            case FieldValueType.Double:
                return new DoubleValue(double.Parse(rawText, CultureInfo.InvariantCulture));
            
            default:
                throw new ArgumentException($"Невозможно преобразовать неизвестный тип данных: {rawText}");
        }
    }
}