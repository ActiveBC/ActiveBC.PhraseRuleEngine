using ActiveBC.PhraseRuleEngine.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Grammar;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Exceptions;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Grammar;
using IronMeta.Matcher;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization
{
    public sealed class GrammarBasedPegPatternTokenizer : IPatternTokenizer
    {
        public IPatternToken Tokenize(string pattern, string? @namespace, bool caseSensitive)
        {
            if (!caseSensitive)
            {
                throw new PegPatternTokenizationException(
                    $"Case insensitive PEG tokenization is not supported by {nameof(GrammarBasedPegPatternTokenizer)}.",
                    pattern
                );
            }

            PegSyntaxMatcher matcher = new PegSyntaxMatcher(@namespace, new CSharpSyntaxMatcher());

            MatchResult<char, IToken> result = matcher.GetMatch(pattern, matcher.Pattern);

            if (!result.Success)
            {
                throw new PegPatternTokenizationException("Failed to parse PEG pattern.", pattern);
            }

            if (result.Result is null)
            {
                throw new PegPatternTokenizationException("Root token is null.", pattern);
            }

            if (result.Result is not IPatternToken patternToken)
            {
                throw new PegPatternTokenizationException($"Root token is of unexpected type '{result.Result.GetType().Name}'.", pattern);
            }

            return patternToken;
        }
    }
}