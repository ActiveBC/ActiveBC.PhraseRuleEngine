using System.Linq;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection
{
    internal static class ProjectionFactory
    {
        public static object? GetProjectionResult(
            RuleMatchResult result,
            CapturedVariablesParameters capturedVariablesParameters,
            RuleInput input,
            int firstSymbolIndex,
            RuleArguments ruleArguments,
            IRuleProjection projection
        )
        {
            ProjectionArguments arguments = new ProjectionArguments(
                // todo [realtime performance] this must be slow, use Span<> or something
                input.Sequence[firstSymbolIndex..(result.LastUsedSymbolIndex + 1)],
                new CapturedVariablesArguments(
                    capturedVariablesParameters
                        .Values
                        .MapValue(
                            (parameterName, _) =>
                            {
                                if (result.CapturedVariables is null)
                                {
                                    return null;
                                }

                                return result.CapturedVariables.TryGetValue(parameterName, out object? capturedVariable)
                                    ? capturedVariable
                                    : null;
                            }
                        )
                        .ToDictionaryWithKnownCapacity(capturedVariablesParameters.Values.Count)
                ),
                ruleArguments,
                input.RuleSpaceArguments
            );

            return projection.Invoke(arguments);
        }
    }
}