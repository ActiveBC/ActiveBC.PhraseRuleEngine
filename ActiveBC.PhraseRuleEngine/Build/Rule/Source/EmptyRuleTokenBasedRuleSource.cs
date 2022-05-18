using System;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Build.Rule.Source
{
    internal sealed class EmptyRuleTokenBasedRuleSource : IRuleSource
    {
        private readonly EmptyRuleToken m_rule;
        private readonly RuleParameters m_ruleParameters;
        private readonly RuleMatchResultDescription m_resultDescription;

        public EmptyRuleTokenBasedRuleSource(
            EmptyRuleToken rule,
            RuleParameters ruleParameters,
            RuleMatchResultDescription resultDescription
        )
        {
            this.m_rule = rule;
            this.m_ruleParameters = ruleParameters;
            this.m_resultDescription = resultDescription;
        }

        public IRuleMatcher GetRuleMatcher(in IRuleSpace ruleSpace)
        {
            try
            {
                return new EmptyRuleMatcher(this.m_ruleParameters, this.m_resultDescription);
            }
            catch (Exception exception)
            {
                throw new RuleBuildException(
                    $"Cannot create empty rule matcher for rule '{this.m_rule.GetFullName()}'.",
                    exception
                );
            }
        }
    }
}