using System;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters;
using ActiveBC.PhraseRuleEngine.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection
{
    internal sealed class FunctionBasedProjection : IRuleProjection
    {
        private readonly ProjectionParameters m_parameters;
        private readonly Func<object?[], object?> m_function;

        public FunctionBasedProjection(ProjectionParameters parameters, Func<object?[], object?> function)
        {
            this.m_parameters = parameters;
            this.m_function = function;
        }

        public object? Invoke(ProjectionArguments projectionArguments)
        {
            List<object?> arguments = new List<object?>(this.m_parameters.Values.Count);

            foreach ((string variableName, Type type) in this.m_parameters.Values)
            {
                // todo [refactoring] generalize this idea of void rules
                if (type == typeof(void))
                {
                    continue;
                }

                arguments.Add(projectionArguments.Values[variableName]);
            }

            return TryInvokeProjection(arguments.ToArray());
        }

        private object? TryInvokeProjection(object?[] arguments)
        {
            try
            {
                return this.m_function.Invoke(arguments);
            }
            catch (Exception exception)
            {
                throw new RuleMatchException("Projection invocation error", exception);
            }
        }
    }
}