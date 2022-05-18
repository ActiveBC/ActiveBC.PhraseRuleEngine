using System;
using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result
{
    public sealed class RuleMatchResultDescription
    {
        public Type ResultType { get; }
        public IReadOnlyDictionary<string, Type> CapturedVariablesTypes { get; }

        public RuleMatchResultDescription(Type resultType, IReadOnlyDictionary<string, Type> capturedVariablesTypes)
        {
            this.ResultType = resultType;
            this.CapturedVariablesTypes = capturedVariablesTypes;
        }
    }
}