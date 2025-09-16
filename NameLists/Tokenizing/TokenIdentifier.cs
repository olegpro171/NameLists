namespace NameLists.Tokenizing;

internal static class TokenIdentifier
{
    public static TokenType GetTokenType(string token)
    {
        switch (token)
        {
            case "=": return TokenType.EqualsToken;
            case ",": return TokenType.Delimeter;
            case "*": return TokenType.Repeat;
        }
        
        // String literal (single quotes, possibly escaped with '')
        if (RegexConstants.StringRegex.IsMatch(token))
            return TokenType.ValueToken;

        // Integer (only digits, optional leading + or -)
        if (RegexConstants.IntRegex.IsMatch(token))
            return TokenType.ValueToken;

        // Double (decimal point and/or exponent notation)
        if (RegexConstants.DoubleRegex.IsMatch(token))
            return TokenType.ValueToken;

        // Identifiers (default case)
        if (RegexConstants.IdentifierRegex.IsMatch(token))
            return TokenType.Identifier;

        throw new ArgumentException($"Неопознанный токен: {token}");
    }
}