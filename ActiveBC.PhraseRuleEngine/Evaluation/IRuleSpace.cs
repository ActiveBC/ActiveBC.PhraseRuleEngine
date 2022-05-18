using System;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;

namespace ActiveBC.PhraseRuleEngine.Evaluation
{
    public interface IRuleSpace
    {
        IReadOnlyDictionary<string, Type> RuleSpaceParameterTypesByName { get; }
        IReadOnlyDictionary<string, Type> RuleResultTypesByName { get; }
        IReadOnlyDictionary<string, IRuleMatcher> RuleMatchersByName { get; }
        IRuleMatcher this[string ruleName] { get; set; }
    }
}