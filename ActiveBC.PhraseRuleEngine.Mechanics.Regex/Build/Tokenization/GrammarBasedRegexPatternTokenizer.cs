using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Grammar;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Exceptions;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Grammar;
using IronMeta.Matcher;
using IToken = ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens.IToken;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization
{
    public sealed class GrammarBasedRegexPatternTokenizer : IPatternTokenizer
    {
        public IPatternToken Tokenize(string pattern, string? @namespace, bool caseSensitive)
        {
            if (!caseSensitive)
            {
                throw new RegexPatternTokenizationException(
                    $"Case insensitive PEG tokenization is not supported by {nameof(GrammarBasedRegexPatternTokenizer)}.",
                    pattern
                );
            }

            RegexSyntaxMatcher matcher = new RegexSyntaxMatcher(@namespace, new CSharpSyntaxMatcher());
            MatchResult<char, IToken> result = matcher.GetMatch(pattern.ToList(), matcher.Pattern);

            if (!result.Success)
            {
                throw new RegexPatternTokenizationException("Failed to parse regex pattern.", pattern);
            }

            if (result.Result is null)
            {
                throw new RegexPatternTokenizationException("Root token is null.", pattern);
            }

            if (result.Result is not IPatternToken patternToken)
            {
                throw new RegexPatternTokenizationException($"Root token is of unexpected type '{result.Result.GetType().Name}'.", pattern);
            }

            return patternToken;
        }
    }
}