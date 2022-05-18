using System;
using ActiveBC.PhraseRuleEngine.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Build.Rule.Source
{
    internal sealed class RuleTokenBasedRuleSource : IRuleSource
    {
        private readonly RuleToken m_rule;
        private readonly IInputProcessorFactory m_inputProcessorFactory;
        private readonly RuleParameters m_ruleParameters;
        private readonly CapturedVariablesParameters m_capturedVariablesParameters;
        private readonly RuleMatchResultDescription m_resultDescription;
        private readonly IRuleProjection m_ruleProjection;

        public RuleTokenBasedRuleSource(
            RuleToken rule,
            IInputProcessorFactory inputProcessorFactory,
            RuleParameters ruleParameters,
            CapturedVariablesParameters capturedVariablesParameters,
            RuleMatchResultDescription resultDescription,
            IRuleProjection ruleProjection
        )
        {
            this.m_rule = rule;
            this.m_inputProcessorFactory = inputProcessorFactory;
            this.m_ruleParameters = ruleParameters;
            this.m_capturedVariablesParameters = capturedVariablesParameters;
            this.m_resultDescription = resultDescription;
            this.m_ruleProjection = ruleProjection;
        }

        public IRuleMatcher GetRuleMatcher(in IRuleSpace ruleSpace)
        {
            try
            {
                return new RuleMatcher(
                    this.m_inputProcessorFactory.Create(this.m_rule.Pattern, ruleSpace),
                    this.m_ruleParameters,
                    this.m_capturedVariablesParameters,
                    this.m_resultDescription,
                    this.m_ruleProjection
                );
            }
            catch (Exception exception)
            {
                throw new RuleBuildException(
                    $"Cannot create rule matcher for rule '{this.m_rule.GetFullName()}'.",
                    exception
                );
            }
        }
    }
}