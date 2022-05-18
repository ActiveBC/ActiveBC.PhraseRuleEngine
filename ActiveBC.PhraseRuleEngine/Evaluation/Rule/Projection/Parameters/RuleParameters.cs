using System;
using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters
{
    public sealed class RuleParameters
    {
        public IReadOnlyDictionary<string, Type> Values { get; }

        public RuleParameters(IReadOnlyDictionary<string, Type> values)
        {
            this.Values = values;
        }
    }
}