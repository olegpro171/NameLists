namespace NameLists.Tokenizing;

internal static class ValueTypeIdentifier
{
    public static FieldValueType GetTokenValueType(string rawText)
    {
        // String literal (single quotes, possibly escaped with '')
        if (RegexConstants.StringRegex.IsMatch(rawText))
            return FieldValueType.String;

        // Integer (only digits, optional leading + or -)
        if (RegexConstants.IntRegex.IsMatch(rawText))
            return FieldValueType.Int;
        
        // Double (decimal point and/or exponent notation)
        if (RegexConstants.DoubleRegex.IsMatch(rawText))
            return FieldValueType.Double;

        return FieldValueType.None;
    }
}