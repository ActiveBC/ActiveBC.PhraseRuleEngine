using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Build.Tokenization
{
    public interface IRuleSetTokenizer
    {
        RuleSetToken Tokenize(string ruleSet, string? @namespace, bool caseSensitive);
    }
}