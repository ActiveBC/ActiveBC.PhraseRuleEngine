using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Rule.Projection.Models;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters;
using ActiveBC.PhraseRuleEngine.Exceptions;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Models;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Types.Formatting;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Build.Rule.Projection
{
    internal sealed class ProjectionCompiler : IProjectionCompiler
    {
        private readonly ITypeFormatter m_typeFormatter;
        private readonly CodeEmitter m_codeEmitter;

        public ProjectionCompiler(ITypeFormatter typeFormatter, CodeEmitter codeEmitter)
        {
            this.m_typeFormatter = typeFormatter;
            this.m_codeEmitter = codeEmitter;
        }

        public IRuleProjection CreateProjection(
            string ruleKey,
            IProjectionCompilationData data,
            IAssembliesProvider assembliesProvider
        )
        {
            return data switch
            {
                BodyBasedProjectionCompilationData bodyBased => CreateFunctionBasedProjection(
                    bodyBased.ProjectionParameters,
                    this.m_codeEmitter.CreateFunction(GetFunctionCreationData(ruleKey, bodyBased), assembliesProvider)
                ),
                MatchedInputBasedProjectionCompilationData => MatchedInputBasedProjection.Instance,
                VoidProjectionCompilationData => VoidProjection.Instance,
                ConstantProjectionCompilationData constant => new ConstantProjection(constant.Constant),
                _ => throw new ArgumentOutOfRangeException(nameof(data))
            };
        }
        public Dictionary<string, IRuleProjection> CreateProjections(
            IDictionary<string, IProjectionCompilationData> dataByRuleName,
            IAssembliesProvider assembliesProvider,
            int extraCapacity = 0
        )
        {
            Dictionary<string, IRuleProjection> projections = new Dictionary<string, IRuleProjection>(dataByRuleName.Count + extraCapacity);
            Dictionary<string, BodyBasedProjectionCompilationData> bodyBasedDataByRuleName = new Dictionary<string, BodyBasedProjectionCompilationData>(dataByRuleName.Count);

            foreach ((string ruleName, IProjectionCompilationData data) in dataByRuleName)
            {
                if (data is BodyBasedProjectionCompilationData bodyBasedData)
                {
                    bodyBasedDataByRuleName.Add(ruleName, bodyBasedData);
                }
                else
                {
                    projections.Add(
                        ruleName,
                        data switch
                        {
                            MatchedInputBasedProjectionCompilationData => MatchedInputBasedProjection.Instance,
                            VoidProjectionCompilationData => VoidProjection.Instance,
                            ConstantProjectionCompilationData constant => new ConstantProjection(constant.Constant),
                            _ => throw new ArgumentOutOfRangeException(nameof(data))
                        }
                    );
                }
            }

            IEnumerable<KeyValuePair<string, IRuleProjection>> bodyBasedProjections = CreateBodyBasedProjections(bodyBasedDataByRuleName, assembliesProvider);

            foreach ((string ruleName, IRuleProjection projection) in bodyBasedProjections)
            {
                projections.Add(ruleName, projection);
            }

            return projections;
        }

        private IEnumerable<KeyValuePair<string, IRuleProjection>> CreateBodyBasedProjections(
            IReadOnlyDictionary<string, BodyBasedProjectionCompilationData> dataByRuleName,
            IAssembliesProvider assembliesProvider
        )
        {
            if (dataByRuleName.Count == 0)
            {
                return ImmutableDictionary<string, IRuleProjection>.Empty;
            }

            try
            {
                IReadOnlyDictionary<string, FunctionCreationData> functionCreationDataByName = dataByRuleName
                    .MapValue(GetFunctionCreationData)
                    .ToDictionaryWithKnownCapacity(dataByRuleName.Count);

                return this
                    .m_codeEmitter
                    .CreateFunctions(functionCreationDataByName, assembliesProvider)
                    .MapValue(
                        (ruleKey, function) => CreateFunctionBasedProjection(
                            dataByRuleName[ruleKey].ProjectionParameters,
                            function
                        )
                    );
            }
            catch (Exception exception)
            {
                throw new RuleBuildException("Projection compilation error", exception);
            }
        }

        private FunctionCreationData GetFunctionCreationData(string ruleKey, BodyBasedProjectionCompilationData data)
        {
            return new FunctionCreationData(
                data.Usings,
                this.m_typeFormatter.GetStringRepresentation(data.ResultType),
                ruleKey.Replace('.', '_'),
                data
                    .ProjectionParameters
                    .Values
                    .WhereValue(parameterType => parameterType != typeof(void))
                    .Select(
                        parameterPair => new VariableCreationData(
                            parameterPair.Key,
                            this.m_typeFormatter.GetStringRepresentation(parameterPair.Value)
                        )
                    ),
                data.Body
            );
        }

        private static IRuleProjection CreateFunctionBasedProjection(
            ProjectionParameters projectionParameters,
            Func<object?[], object?> function
        )
        {
            return new FunctionBasedProjection(projectionParameters, function);
        }
    }
}
