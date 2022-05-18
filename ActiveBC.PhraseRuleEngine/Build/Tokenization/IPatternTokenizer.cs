using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Build.Tokenization
{
    public interface IPatternTokenizer
    {
        IPatternToken Tokenize(string pattern, string? @namespace, bool caseSensitive);
    }
}