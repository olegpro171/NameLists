using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using ReflectorGrid.Processing.NameLists;

namespace NameLists.Factories;

public static partial class FieldFactory
{
    public enum TokenType
    {
        EqualsToken,
        Delimeter,
        Identifier,
        ValueToken,
        Repeat,
    };
    
    public enum ValueType
    {
        None,
        Int,
        Double,
        String,
    };
    
    private static readonly Regex TokenRegex = new Regex(
        @"('([^']|'')*')" +            // String literal (supports '' escape for single quote)
        @"|([A-Za-z_][A-Za-z0-9_]*(\(\s*[A-Za-z0-9_]+(\s*,\s*[A-Za-z0-9_]+)*\s*\))?)" + // Identifiers, optionally with parenthesized argument list
        @"|([0-9]*\.?[0-9]+(?:[eEdD][+-]?[0-9]+)?)" + // Numbers
        @"|([=*,/()])" +               // Operators & punctuation
        @"|(\*)",                      // Multiplication / repeat symbol
        RegexOptions.Compiled);

    private static readonly Regex StringPattern     = new Regex(@"^'([^']|'')*'$",                               RegexOptions.Compiled);
    private static readonly Regex IntPattern        = new Regex(@"^[+-]?\d+$",                                   RegexOptions.Compiled);
    private static readonly Regex DoublePattern     = new Regex(@"^[+-]?(\d+\.\d*|\.\d+|\d+)([eEdD][+-]?\d+)?$", RegexOptions.Compiled);
    private static readonly Regex IdentifierPattern = new(
        @"^[A-Za-z_][A-Za-z0-9_]*(\(\s*[A-Za-z0-9_]+(\s*,\s*[A-Za-z0-9_]+)*\s*\))?$",
        RegexOptions.Compiled);

    private static bool IsMatch(string input, Regex pattern) => pattern.IsMatch(input);

    
    public static List<Field> CreateFromText(string rawText)
    {
        List<Field> fields = [];
        
        List<string> tokens = Tokenize(rawText);

        string id = string.Empty;
        List<string> values = new List<string>();
        TokenType? previousTokenType = null;
        TokenType? tokenType = null;
        
        for (int i = 0; i < tokens.Count; i++)
        {
            string token = tokens[i];
            
            if (tokenType is not null)
                previousTokenType = tokenType;
            
            tokenType = GetTokenType(token);
            
            
            if (previousTokenType is not null && !ExpectedTokensHelper.IsValidToken((TokenType)previousTokenType, (TokenType)tokenType))
                throw new DataException("Неожиданный токен: " + token);
            else if (previousTokenType is null && tokenType != TokenType.Identifier)
                throw new DataException("Неожиданный токен: " + token);
            
            switch (tokenType)
            {
                case TokenType.Identifier:
                {
                    if (id != string.Empty)
                    {
                        fields.Add(CreateField(id, values));
                    }
                    
                    values.Clear();
                    id = token;
                    
                    continue;
                }
                
                case TokenType.ValueToken:
                {
                    values.Add(token);
                    continue;
                }
                
                case TokenType.Repeat:
                {
                    values.Add(token);
                    continue;
                }
                
                default:
                    continue;
            }
        }

        if (values.Count == 0)
        {
            throw new DataException($"Идентификатор без заданного значения в конце файла: {id}");
        }
        else
        {
            fields.Add(CreateField(id, values));
        }
        
        return fields;
    }
    
    public static List<string> Tokenize(string input)
    {
        var tokens = new List<string>();
        foreach (Match m in TokenRegex.Matches(input))
        {
            tokens.Add(m.Value);
        }
        return tokens;
    }

    private static Field CreateField(string id, List<string> textValues)
    {
        List<string> multipliedValues = new();
        int i = 0;
        while (i < textValues.Count)
        {
            // Check if this value is followed by a repeat symbol
            if (i + 1 < textValues.Count && GetTokenType(textValues[i + 1]) == TokenType.Repeat)
            {
                // The current value must be an integer
                if (GetTokenValueType(textValues[i]) != ValueType.Int)
                {
                    throw new DataException(
                        $"Знак повторения должен находиться после целочисленной переменной: {textValues[i]}");
                }
                int count;
                if (!int.TryParse(textValues[i], out count))
                {
                    throw new DataException(
                        $"Некорректное значение количества повторов: {textValues[i]}");
                }
                // There must be a value after the repeat symbol
                if (i + 2 >= textValues.Count)
                {
                    throw new DataException("Знак повторения не сопровождается значением для повторения.");
                }
                string repeatedValue = textValues[i + 2];
                for (int j = 0; j < count; j++)
                {
                    multipliedValues.Add(repeatedValue);
                }
                i += 3; // Skip count, '*', and repeated value
            }
            else
            {
                multipliedValues.Add(textValues[i]);
                i += 1;
            }
        }
        List<Value> typedValues = multipliedValues.Select(x => GetValue(x)).ToList();
        return new Field(id, typedValues);
    }

    private static Value GetValue(string text)
    {
        // String literal (single quotes, possibly escaped with '')
        if (IsMatch(text, StringPattern))
            return new StringValue(text.TrimStart(NameListParameters.NameListStringEnclosure).TrimEnd(NameListParameters.NameListStringEnclosure));

        // Integer (only digits, optional leading + or -)
        if (IsMatch(text, IntPattern))
            return new IntValue(int.Parse(text));

        // Double (decimal point and/or exponent notation)
        if (IsMatch(text, DoublePattern))
            return new DoubleValue(double.Parse(text, CultureInfo.InvariantCulture));
        
        throw new ArgumentException($"Невозможно преобразовать неизвестный тип данных: {text}");
    }
    
    private static TokenType GetTokenType(string token)
    {
        switch (token)
        {
            case "=": return TokenType.EqualsToken;
            case ",": return TokenType.Delimeter;
            case "*": return TokenType.Repeat;
            default: break;
        }
        
        // String literal (single quotes, possibly escaped with '')
        if (IsMatch(token, StringPattern))
            return TokenType.ValueToken;

        // Integer (only digits, optional leading + or -)
        if (IsMatch(token, IntPattern))
            return TokenType.ValueToken;

        // Double (decimal point and/or exponent notation)
        if (IsMatch(token, DoublePattern))
            return TokenType.ValueToken;

        // Identifiers (default case)
        if (IsMatch(token, IdentifierPattern))
            return TokenType.Identifier;

        throw new ArgumentException($"Неопознанный токен: {token}");
    }

    private static ValueType GetTokenValueType(string token)
    {
        // String literal (single quotes, possibly escaped with '')
        if (IsMatch(token, StringPattern))
            return ValueType.String;

        // Integer (only digits, optional leading + or -)
        if (IsMatch(token, IntPattern))
            return ValueType.Int;
        
        // Double (decimal point and/or exponent notation)
        if (IsMatch(token, DoublePattern))
            return ValueType.Double;

        return ValueType.None;
        
        throw new ArgumentException($"Неизвестный тип данных: {token}", nameof(token));
    }
}