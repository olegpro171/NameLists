using System.Text.RegularExpressions;
using NameLists.Tokenizing;

namespace NameLists.Tokenizing;

public class NameListTokenizer
{
    public static List<string> Tokenize(string input)
    {
        var tokens = new List<string>();
        foreach (Match m in RegexConstants.TokenRegex.Matches(input))
        {
            tokens.Add(m.Value);
        }
        return tokens;
    }
}