using System;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.ArgumentsBinding;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result.SelectionStrategy;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Parsers
{
    internal sealed class RuleReferenceParser : IQuantifiableParser
    {
        public Type ResultType => this.m_ruleSpace[KeyInRuleSpace].ResultDescription.ResultType;

        private readonly RuleReferenceToken m_ruleReferenceToken;
        private readonly IResultSelectionStrategy m_resultSelectionStrategy;
        private readonly IRuleSpace m_ruleSpace;

        private IRuleMatcher? m_matcher;
        private IRuleMatcher Matcher => this.m_matcher ??= this.m_ruleSpace[KeyInRuleSpace];

        private string KeyInRuleSpace => this.m_ruleReferenceToken.GetRuleSpaceKey();

        public RuleReferenceParser(
            RuleReferenceToken ruleReferenceToken,
            IResultSelectionStrategy resultSelectionStrategy,
            IRuleSpace ruleSpace
        )
        {
            this.m_ruleReferenceToken = ruleReferenceToken;
            this.m_resultSelectionStrategy = resultSelectionStrategy;
            this.m_ruleSpace = ruleSpace;
        }

        public bool TryParse(
            RuleInput input,
            IRuleSpaceCache cache,
            ref int index,
            out int explicitlyMatchedSymbolsCount,
            out object? result
        )
        {
            RuleMatchResult? matchResult = this
                .Matcher
                .MatchAndProject(
                    input,
                    index,
                    ArgumentsBinder.BindRuleArguments(
                        this.Matcher.Parameters,
                        input.RuleSpaceArguments,
                        this.m_ruleReferenceToken.Arguments.ToArray()
                    ),
                    cache
                )
                .Best(this.m_resultSelectionStrategy);

            if (matchResult is null)
            {
                explicitlyMatchedSymbolsCount = 0;
                result = null;

                return false;
            }

            index = matchResult.LastUsedSymbolIndex + 1;

            explicitlyMatchedSymbolsCount = matchResult.ExplicitlyMatchedSymbolsCount;
            result = matchResult.Result.Value;

            return true;
        }

        public IEnumerable<string> GetUsedWords()
        {
            // rule reference itself doesn't use any words
            yield break;
        }
    }
}