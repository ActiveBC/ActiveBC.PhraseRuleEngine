using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;

namespace ActiveBC.PhraseRuleEngine.Build.Rule.Source
{
    internal sealed class MatcherBasedRuleSource : IRuleSource
    {
        private readonly IRuleMatcher m_matcher;

        public MatcherBasedRuleSource(IRuleMatcher matcher)
        {
            this.m_matcher = matcher;
        }

        public IRuleMatcher GetRuleMatcher(in IRuleSpace ruleSpace)
        {
            return this.m_matcher;
        }
    }
}