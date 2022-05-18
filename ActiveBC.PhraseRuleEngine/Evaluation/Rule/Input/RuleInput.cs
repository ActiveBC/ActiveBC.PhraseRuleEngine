using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input
{
    /// <remarks>
    /// Performance remarks: library performance depends on the way the fields in this class are declared.
    /// Please make sure you know what you are doing, when changing any of the fields declaration.
    /// </remarks>
    public sealed class RuleInput
    {
        public readonly string[] Sequence;
        public readonly RuleSpaceArguments RuleSpaceArguments;

        public RuleInput(string[] input, RuleSpaceArguments ruleSpaceArguments)
        {
            this.Sequence = input;
            this.RuleSpaceArguments = ruleSpaceArguments;
        }
    }
}