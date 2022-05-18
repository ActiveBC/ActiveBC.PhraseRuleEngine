using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection
{
    internal sealed class VoidProjection : IRuleProjection
    {
        public static readonly VoidProjection Instance = new VoidProjection();

        private VoidProjection()
        {
        }

        public object? Invoke(ProjectionArguments projectionArguments)
        {
            return null;
        }
    }
}