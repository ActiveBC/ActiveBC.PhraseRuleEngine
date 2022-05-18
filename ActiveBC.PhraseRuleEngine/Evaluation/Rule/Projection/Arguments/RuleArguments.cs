using System.Collections.Generic;
using System.Collections.Immutable;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments
{
    // todo [realtime performance] see if we can just derive from dictionary
    public sealed class RuleArguments
    {
        public static readonly RuleArguments Empty = new RuleArguments(ImmutableDictionary<string, object?>.Empty);

        /// <remarks>
        /// Performance remarks: library performance depends on the way this field is declared.
        /// Please make sure you know what you are doing, when changing this field's declaration.
        /// </remarks>
        public readonly IReadOnlyDictionary<string, object?> Values;

        public RuleArguments(IReadOnlyDictionary<string, object?> values)
        {
            this.Values = values;
        }
    }
}