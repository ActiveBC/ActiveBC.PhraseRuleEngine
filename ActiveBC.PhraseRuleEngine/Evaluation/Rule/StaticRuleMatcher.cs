using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule
{
    public sealed class StaticRuleMatcher<TResult> : IRuleMatcher
    {
        private readonly Func<IEnumerable<string>> m_usedWordsProvider;
        private readonly Func<object?[], IEnumerable<(TResult Result, int LastUsedSymbolIndex)>> m_ruleEvaluator;

        public RuleParameters Parameters { get; }
        public RuleMatchResultDescription ResultDescription { get; }

        public StaticRuleMatcher(
            Func<IEnumerable<string>> usedWordsProvider,
            Func<object?[], IEnumerable<(TResult Result, int LastUsedSymbolIndex)>> ruleEvaluator,
            RuleParameters parameters
        )
        {
            this.m_usedWordsProvider = usedWordsProvider;
            this.m_ruleEvaluator = ruleEvaluator;
            this.Parameters = parameters;
            this.ResultDescription = new RuleMatchResultDescription(
                typeof(TResult),
                ImmutableDictionary<string, Type>.Empty
            );
        }

        public RuleMatchResultCollection Match(RuleInput input, int firstSymbolIndex, IRuleSpaceCache cache)
        {
            return MatchAndProject(input, firstSymbolIndex, RuleArguments.Empty, cache);
        }

        public RuleMatchResultCollection MatchAndProject(
            RuleInput input,
            int firstSymbolIndex,
            RuleArguments ruleArguments,
            IRuleSpaceCache cache
        )
        {
            return new RuleMatchResultCollection(
                Evaluate(input, firstSymbolIndex, ruleArguments)
                    .Select(
                        evaluationResult => new RuleMatchResult(
                            input.Sequence,
                            firstSymbolIndex,
                            evaluationResult.LastUsedSymbolIndex,
                            null,
                            evaluationResult.LastUsedSymbolIndex - firstSymbolIndex + 1,
                            null,
                            new Lazy<object?>(() => evaluationResult.Result)
                        )
                    )
            );
        }

        public IEnumerable<string> GetUsedWords()
        {
            return this.m_usedWordsProvider();
        }

        private IEnumerable<(TResult Result, int LastUsedSymbolIndex)> Evaluate(
            RuleInput input,
            int firstSymbolIndex,
            RuleArguments ruleArguments
        )
        {
            if (firstSymbolIndex >= input.Sequence.Length)
            {
                return Enumerable.Empty<(TResult Result, int LastUsedSymbolIndex)>();
            }

            // todo [realtime performance] we can use more efficient way of creating this variable
            object?[] arguments = new object?[] { input.Sequence, firstSymbolIndex }
                .Concat(ruleArguments.Values.Values)
                .ToArray();

            return this.m_ruleEvaluator(arguments);
        }
    }
}