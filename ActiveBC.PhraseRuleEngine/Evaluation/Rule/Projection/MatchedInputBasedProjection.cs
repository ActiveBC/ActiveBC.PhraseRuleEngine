using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection
{
    internal sealed class MatchedInputBasedProjection : IRuleProjection
    {
        public static readonly MatchedInputBasedProjection Instance = new MatchedInputBasedProjection();

        private MatchedInputBasedProjection()
        {
        }

        public object? Invoke(ProjectionArguments projectionArguments)
        {
            return projectionArguments.Input.JoinToString(" ");
        }
    }
}