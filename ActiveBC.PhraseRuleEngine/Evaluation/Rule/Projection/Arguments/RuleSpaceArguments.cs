using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments
{
    public sealed class RuleSpaceArguments
    {
        /// <remarks>
        /// Performance remarks: library performance depends on the way this field is declared.
        /// Please make sure you know what you are doing, when changing this field's declaration.
        /// </remarks>
        public readonly IReadOnlyDictionary<string, object?> Values;

        public RuleSpaceArguments(IReadOnlyDictionary<string, object?> values)
        {
            this.Values = values;
        }
    }
}