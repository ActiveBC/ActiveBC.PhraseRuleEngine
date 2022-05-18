using System;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Grammar;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Exceptions;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Grammar;
using IronMeta.Matcher;

namespace ActiveBC.PhraseRuleEngine.Build.Tokenization
{
    internal sealed class GrammarBasedRuleSetTokenizer : IRuleSetTokenizer
    {
        private readonly IReadOnlyDictionary<string, IPatternTokenizer> m_patternTokenizers;

        public GrammarBasedRuleSetTokenizer(IReadOnlyDictionary<string, IPatternTokenizer> patternTokenizers)
        {
            this.m_patternTokenizers = patternTokenizers;
        }

        public RuleSetToken Tokenize(string ruleSet, string? @namespace, bool caseSensitive)
        {
            RuleSetSyntaxMatcher matcher = new RuleSetSyntaxMatcher(
                @namespace,
                this.m_patternTokenizers,
                new CSharpSyntaxMatcher(),
                caseSensitive
            );

            MatchResult<char, IToken> result;

            try
            {
                result = matcher.GetMatch(ruleSet, matcher.RuleSet);
            }
            catch (PhraseRuleEngineTokenizationException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new PhraseRuleEngineTokenizationException("Failed to parse rule set", exception, ruleSet);
            }

            if (!result.Success)
            {
                throw new PhraseRuleEngineTokenizationException("Failed to parse rule set", ruleSet);
            }

            if (result.Result is null)
            {
                throw new PhraseRuleEngineTokenizationException("Root token is null", ruleSet);
            }

            if (result.Result is not RuleSetToken ruleSetToken)
            {
                throw new PhraseRuleEngineTokenizationException($"Root token is of unexpected type '{result.Result.GetType().Name}'", ruleSet);
            }

            return ruleSetToken;
        }
    }
}