using System.Text.RegularExpressions;

namespace NameLists.Tokenizing;

internal static partial class RegexConstants
{
    public static readonly Regex TokenRegex        = GetTokenRegex();
    public static readonly Regex StringRegex       = GetStringTokenRegex();
    public static readonly Regex IntRegex          = GetIntTokenRegex();
    public static readonly Regex DoubleRegex       = GetDoubleTokenRegex();
    public static readonly Regex IdentifierRegex   = GetIdentifierTokenRegex();
    
    public static bool IsMatch(string input, Regex pattern) => pattern.IsMatch(input);

    
    
    #region RegexConstants
    
    [GeneratedRegex(@"^[A-Za-z_][A-Za-z0-9_]*(\(\s*[A-Za-z0-9_]+(\s*,\s*[A-Za-z0-9_]+)*\s*\))?$", RegexOptions.Compiled)]
    private static partial Regex GetIdentifierTokenRegex();
    
    
    [GeneratedRegex(@"^[+-]?(\d+\.\d*|\.\d+|\d+)([eEdD][+-]?\d+)?$", RegexOptions.Compiled)]
    private static partial Regex GetDoubleTokenRegex();
    
    
    [GeneratedRegex(@"^[+-]?\d+$", RegexOptions.Compiled)]
    private static partial Regex GetIntTokenRegex();
    
    
    [GeneratedRegex(@"^'([^']|'')*'$", RegexOptions.Compiled)]
    private static partial Regex GetStringTokenRegex();
    
    
    [GeneratedRegex(@"('([^']|'')*')|([A-Za-z_][A-Za-z0-9_]*(\(\s*[A-Za-z0-9_]+(\s*,\s*[A-Za-z0-9_]+)*\s*\))?)|([+-]?(?:\d+\.\d*|\.\d+|\d+)(?:[eEdD][+-]?\d+)?)|([=*,/()])|(\*)", RegexOptions.Compiled)]
    private static partial Regex GetTokenRegex();
    
    #endregion
}