using System;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Reflection;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule
{
    public interface IRuleMatcher : IUsedWordsProvider
    {
        RuleParameters Parameters { get; }
        RuleMatchResultDescription ResultDescription { get; }
        RuleMatchResultCollection Match(RuleInput input, int firstSymbolIndex, IRuleSpaceCache cache);
        RuleMatchResultCollection MatchAndProject(
            RuleInput input,
            int firstSymbolIndex,
            RuleArguments ruleArguments,
            IRuleSpaceCache cache
        );
    }

    public static class RuleMatcherExtensions
    {
        public static RuleMatchResultCollection MatchAll(
            this IRuleMatcher matcher,
            RuleInput input,
            int firstSymbolIndex,
            IRuleSpaceCache cache
        )
        {
            return EvaluateAll(
                startIndex => matcher.Match(input, startIndex, cache),
                input,
                firstSymbolIndex
            );
        }

        public static RuleMatchResultCollection MatchAndProjectAll(
            this IRuleMatcher matcher,
            RuleInput input,
            int firstSymbolIndex,
            RuleArguments ruleArguments,
            IRuleSpaceCache cache
        )
        {
            return EvaluateAll(
                startIndex => matcher.MatchAndProject(input, startIndex, ruleArguments, cache),
                input,
                firstSymbolIndex
            );
        }

        private static RuleMatchResultCollection EvaluateAll(
            Func<int, RuleMatchResultCollection> evaluator,
            RuleInput input,
            int firstSymbolIndex
        )
        {
            if (firstSymbolIndex == input.Sequence.Length)
            {
                return evaluator(firstSymbolIndex);
            }

            List<RuleMatchResultCollection> results = new List<RuleMatchResultCollection>(input.Sequence.Length);

            for (int symbolIndex = firstSymbolIndex; symbolIndex < input.Sequence.Length; symbolIndex++)
            {
                results.Add(evaluator(symbolIndex));
            }

            return results.Merge();
        }
    }
}