using NameLists.Factories;
using static NameLists.Tokenizing.TokenType;

namespace NameLists.Tokenizing;

internal static class TokenRules
{
    private static readonly Dictionary<TokenType, HashSet<TokenType>> ExpectedTokens = new()
    {
        { Identifier, [EqualsToken] },
        { EqualsToken, [ValueToken]},
        { ValueToken, [Delimeter, Repeat] },
        { Repeat, [ValueToken] },
        { Delimeter, [ValueToken, Identifier] }
    };

    public static bool IsValidOrder(TokenType previous, TokenType next)
    {
        return ExpectedTokens.ContainsKey(previous) && ExpectedTokens[previous].Contains(next);
    }
}