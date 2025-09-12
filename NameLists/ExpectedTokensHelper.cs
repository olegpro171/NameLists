using NameLists.Factories;
using static NameLists.Factories.FieldFactory.TokenType;

namespace NameLists;

public static class ExpectedTokensHelper
{
    public static Dictionary<FieldFactory.TokenType, HashSet<FieldFactory.TokenType>> ExpectedTokens = new()
    {
        { Identifier, [EqualsToken] },
        { EqualsToken, [ValueToken]},
        { ValueToken, [Delimeter, Repeat] },
        { Repeat, [ValueToken] },
        { Delimeter, [ValueToken, Identifier] }
    };

    public static bool IsValidToken(FieldFactory.TokenType previous, FieldFactory.TokenType next)
    {
        return ExpectedTokens.ContainsKey(previous) && ExpectedTokens[previous].Contains(next);
    }
}