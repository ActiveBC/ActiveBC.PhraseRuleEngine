using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection
{
    internal interface IRuleProjection
    {
        object? Invoke(ProjectionArguments projectionArguments);
    }
}