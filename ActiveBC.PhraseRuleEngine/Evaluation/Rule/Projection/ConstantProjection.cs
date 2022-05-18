using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection
{
    internal sealed class ConstantProjection : IRuleProjection
    {
        private readonly object? m_constant;

        public ConstantProjection(object? constant)
        {
            this.m_constant = constant;
        }

        public object? Invoke(ProjectionArguments projectionArguments)
        {
            return this.m_constant;
        }
    }
}