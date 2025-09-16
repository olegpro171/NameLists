using System.Data;
using System.Globalization;
using NameLists.DataStructures;
using NameLists.Tokenizing;

namespace NameLists.Factories;

internal static partial class FieldFactory
{
    public static List<Field> CreateFromText(string rawText)
    {
        List<Field> fields = [];
        
        List<string> tokens = NameListTokenizer.Tokenize(rawText);

        string id = string.Empty;
        List<string> values = new List<string>();
        TokenType? previousTokenType = null;
        TokenType? currentTokenType = null;
        
        for (int i = 0; i < tokens.Count; i++)
        {
            string currentToken = tokens[i];
            
            if (currentTokenType is not null)
                previousTokenType = currentTokenType;
            
            currentTokenType = TokenIdentifier.GetTokenType(currentToken);
            
            if (previousTokenType is not null && !TokenRules.IsValidOrder((TokenType)previousTokenType, (TokenType)currentTokenType))
                throw new DataException("Неожиданный токен: " + currentToken);
            else if (previousTokenType is null && currentTokenType != TokenType.Identifier)
                throw new DataException("Неожиданный токен: " + currentToken);
            
            switch (currentTokenType)
            {
                case TokenType.Identifier:
                    if (id != string.Empty)
                        fields.Add(CreateField(id, values));
                    values.Clear();
                    id = currentToken;
                    continue;
                
                case TokenType.ValueToken:
                case TokenType.Repeat:
                    values.Add(currentToken);
                    continue;
                
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
    
    

    private static Field CreateField(string id, List<string> textValues)
    {
        List<string> finalValuesList = [];
        int i = 0;
        while (i < textValues.Count)
        {
            // Check if this value is followed by a repeat symbol
            if (i + 1 < textValues.Count && TokenIdentifier.GetTokenType(textValues[i + 1]) == TokenType.Repeat)
            {
                // The current value must be an integer
                if (ValueTypeIdentifier.GetTokenValueType(textValues[i]) != FieldValueType.Int)
                    throw new DataException(
                        $"Знак повторения должен находиться после целочисленной переменной: {textValues[i]}");
                
                int count;
                if (!int.TryParse(textValues[i], out count))
                    throw new DataException(
                        $"Некорректное значение количества повторов: {textValues[i]}");
                
                // There must be a value after the repeat symbol
                if (i + 2 >= textValues.Count)
                    throw new DataException("Знак повторения не сопровождается значением для повторения.");
                
                string repeatedValue = textValues[i + 2];
                for (int j = 0; j < count; j++)
                {
                    finalValuesList.Add(repeatedValue);
                }
                i += 3; // Skip count, '*', and repeated value
            }
            else
            {
                finalValuesList.Add(textValues[i]);
                i += 1;
            }
        }
        
        List<Value> typedValues = finalValuesList.Select(ValueFactory.CreateValueFromText).ToList();
        
        return new Field(id, typedValues);
    }
}