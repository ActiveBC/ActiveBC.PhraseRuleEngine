using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Build.Rule.Projection.Models;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;

namespace ActiveBC.PhraseRuleEngine.Build.Rule.Projection
{
    internal interface IProjectionCompiler
    {
        IRuleProjection CreateProjection(
            string ruleKey,
            IProjectionCompilationData data,
            IAssembliesProvider assembliesProvider
        );

        Dictionary<string, IRuleProjection> CreateProjections(
            IDictionary<string, IProjectionCompilationData> dataByRuleName,
            IAssembliesProvider assembliesProvider,
            int extraCapacity = 0
        );
    }
}