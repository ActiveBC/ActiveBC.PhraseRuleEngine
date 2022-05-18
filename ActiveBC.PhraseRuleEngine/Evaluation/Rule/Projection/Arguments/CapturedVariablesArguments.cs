using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments
{
    // todo [realtime performance] see if we can just derive from dictionary
    public sealed class CapturedVariablesArguments
    {
        /// <remarks>
        /// Performance remarks: library performance depends on the way this field is declared.
        /// Please make sure you know what you are doing, when changing this field's declaration.
        /// </remarks>
        public readonly IReadOnlyDictionary<string, object?> Values;

        public CapturedVariablesArguments(IReadOnlyDictionary<string, object?> values)
        {
            this.Values = values;
        }
    }
}