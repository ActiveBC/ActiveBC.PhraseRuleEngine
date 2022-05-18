using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens
{
    public interface IRuleToken : IToken
    {
        string? Namespace { get; }
        string Name { get; }
        ICSharpTypeToken ReturnType { get; }
        CSharpParameterToken[] RuleParameters { get; }
        IProjectionToken Projection { get; }
    }

    public static class RuleTokenExtensions
    {
        public static string GetFullName(this IRuleToken ruleToken)
        {
            if (ruleToken.Namespace is null || ruleToken.Namespace == "self")
            {
                return ruleToken.Name;
            }

            return $"{ruleToken.Namespace}.{ruleToken.Name}";
        }
    }
}