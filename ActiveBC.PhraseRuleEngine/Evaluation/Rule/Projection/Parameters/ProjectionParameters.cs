using System;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters
{
    internal sealed class ProjectionParameters
    {
        public const string InputParameterName = "input";

        private readonly Type m_inputParameter;
        private readonly CapturedVariablesParameters m_capturedVariablesParameters;
        private readonly RuleParameters m_ruleParameters;
        private readonly RuleSpaceParameters m_ruleSpaceParameters;

        private IReadOnlyDictionary<string, Type>? m_values;
        public IReadOnlyDictionary<string, Type> Values => this.m_values ??= Array
            .Empty<KeyValuePair<string, Type>>()
            .Append(new KeyValuePair<string, Type>(InputParameterName, this.m_inputParameter))
            .Concat(this.m_capturedVariablesParameters.Values)
            .Concat(this.m_ruleParameters.Values)
            .Concat(this.m_ruleSpaceParameters.Values)
            .ToDictionaryWithKnownCapacity(
                1 +
                this.m_capturedVariablesParameters.Values.Count +
                this.m_ruleParameters.Values.Count +
                this.m_ruleSpaceParameters.Values.Count
            );

        public ProjectionParameters(
            Type inputParameter,
            CapturedVariablesParameters capturedVariablesParameters,
            RuleParameters ruleParameters,
            RuleSpaceParameters ruleSpaceParameters
        )
        {
            this.m_inputParameter = inputParameter;
            this.m_capturedVariablesParameters = capturedVariablesParameters;
            this.m_ruleParameters = ruleParameters;
            this.m_ruleSpaceParameters = ruleSpaceParameters;
        }
    }
}