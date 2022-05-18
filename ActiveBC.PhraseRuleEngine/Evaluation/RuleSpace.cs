using System;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Exceptions;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Evaluation
{
    internal sealed class RuleSpace : IRuleSpace
    {
        private readonly Dictionary<string, IRuleMatcher> m_ruleMatchersByName;
        public IReadOnlyDictionary<string, IRuleMatcher> RuleMatchersByName => m_ruleMatchersByName;
        private readonly Dictionary<string, Type> m_ruleResultTypesByName;
        public IReadOnlyDictionary<string, Type> RuleResultTypesByName => m_ruleResultTypesByName;

        public IReadOnlyDictionary<string, Type> RuleSpaceParameterTypesByName { get; }

        public IRuleMatcher this[string ruleName]
        {
            get => ResolveRule(ruleName);
            set => AddRule(ruleName, value);
        }

        public RuleSpace(
            IReadOnlyDictionary<string, Type> ruleSpaceParameterTypesByName,
            Dictionary<string, Type> ruleResultTypesByName,
            Dictionary<string, IRuleMatcher> ruleMatchersByName
        )
        {
            this.RuleSpaceParameterTypesByName = ruleSpaceParameterTypesByName;
            this.m_ruleResultTypesByName = ruleResultTypesByName;
            this.m_ruleMatchersByName = ruleMatchersByName;
        }

        private void AddRule(string ruleName, IRuleMatcher value)
        {
            this.m_ruleMatchersByName.Add(ruleName, value);
            this.m_ruleResultTypesByName.Add(ruleName, value.ResultDescription.ResultType);
        }

        private IRuleMatcher ResolveRule(string ruleName)
        {
            if (this.RuleMatchersByName.TryGetValue(ruleName, out IRuleMatcher? ruleMatcher))
            {
                return ruleMatcher;
            }

            throw new RuleMatchException(
                $"Rule space doesn't contain rule {ruleName}. " +
                $"Available rules are: {this.RuleMatchersByName.Keys.JoinToString(", ")}."
            );
        }
    }
}