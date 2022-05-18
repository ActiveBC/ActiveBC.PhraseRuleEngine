using System;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments
{
    public sealed class ProjectionArguments
    {
        public IReadOnlyCollection<string> Input { get; }
        private readonly CapturedVariablesArguments m_capturedVariablesArguments;
        private readonly RuleArguments m_ruleArguments;
        private readonly RuleSpaceArguments m_ruleSpaceArguments;

        private IReadOnlyDictionary<string, object?>? m_values;
        public IReadOnlyDictionary<string, object?> Values => this.m_values ??= Array
            .Empty<KeyValuePair<string, object?>>()
            .Append(new KeyValuePair<string, object?>(ProjectionParameters.InputParameterName, this.Input))
            .Concat(this.m_capturedVariablesArguments.Values)
            .Concat(this.m_ruleArguments.Values)
            .Concat(this.m_ruleSpaceArguments.Values)
            .ToDictionaryWithKnownCapacity(
                1 +
                this.m_capturedVariablesArguments.Values.Count +
                this.m_ruleArguments.Values.Count +
                this.m_ruleSpaceArguments.Values.Count
            );

        public ProjectionArguments(
            IReadOnlyCollection<string> input,
            CapturedVariablesArguments capturedVariablesArguments,
            RuleArguments ruleArguments,
            RuleSpaceArguments ruleSpaceArguments
        )
        {
            this.Input = input;
            this.m_capturedVariablesArguments = capturedVariablesArguments;
            this.m_ruleArguments = ruleArguments;
            this.m_ruleSpaceArguments = ruleSpaceArguments;
        }
    }
}