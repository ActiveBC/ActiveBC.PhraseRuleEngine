using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Cache
{
    internal sealed class CachingRuleMatcher : IRuleMatcher
    {
        private readonly int m_id;
        private readonly IRuleMatcher m_source;

        public RuleParameters Parameters => this.m_source.Parameters;
        public RuleMatchResultDescription ResultDescription => this.m_source.ResultDescription;

        public CachingRuleMatcher(int id, IRuleMatcher source)
        {
            this.m_id = id;
            this.m_source = source;
        }

        public RuleMatchResultCollection Match(RuleInput input, int firstSymbolIndex, IRuleSpaceCache cache)
        {
            RuleMatchResultCollection? matchResult = cache.GetResult(this.m_id, input.Sequence, firstSymbolIndex, null);

            if (matchResult is not null)
            {
                return matchResult;
            }

            // todo [code quality] separate cache from IRuleMatcher
            matchResult = this.m_source.Match(input, firstSymbolIndex, cache);

            cache.SetResult(this.m_id, input.Sequence, firstSymbolIndex, null, matchResult);

            return matchResult;
        }

        public RuleMatchResultCollection MatchAndProject(
            RuleInput input,
            int firstSymbolIndex,
            RuleArguments ruleArguments,
            IRuleSpaceCache cache
        )
        {
            RuleMatchResultCollection? matchResult = cache.GetResult(this.m_id, input.Sequence, firstSymbolIndex, ruleArguments.Values);

            if (matchResult is not null)
            {
                return matchResult;
            }

            // todo [code quality] separate cache from IRuleMatcher
            matchResult = this.m_source.MatchAndProject(input, firstSymbolIndex, ruleArguments, cache);

            cache.SetResult(this.m_id, input.Sequence, firstSymbolIndex, ruleArguments.Values, matchResult);

            return matchResult;
        }

        public IEnumerable<string> GetUsedWords()
        {
            return this.m_source.GetUsedWords();
        }
    }
}