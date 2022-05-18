using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Tests.Helpers
{
    public class DummyRuleSpace : IRuleSpace
    {
        public static readonly DummyRuleSpace Instance = new DummyRuleSpace();

        private DummyRuleSpace()
        {
        }

        public IReadOnlyDictionary<string, Type> RuleSpaceParameterTypesByName => ImmutableDictionary<string, Type>.Empty;
        public IReadOnlyDictionary<string, Type> RuleResultTypesByName => ImmutableDictionary<string, Type>.Empty;
        public IReadOnlyDictionary<string, IRuleMatcher> RuleMatchersByName => ImmutableDictionary<string, IRuleMatcher>.Empty;

        public IRuleMatcher this[string ruleName]
        {
            get => throw new Exception();
            set => throw new Exception();
        }
    }
}