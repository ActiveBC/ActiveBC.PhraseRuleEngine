using System;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters;

namespace ActiveBC.PhraseRuleEngine.Build.Rule.Projection.Models
{
    internal sealed class BodyBasedProjectionCompilationData : IProjectionCompilationData
    {
        public IReadOnlySet<string> Usings { get; }
        public Type ResultType { get; }
        public ProjectionParameters ProjectionParameters { get; }
        public string Body { get; }

        public BodyBasedProjectionCompilationData(
            IReadOnlySet<string> usings,
            Type resultType,
            ProjectionParameters projectionParameters,
            string body
        )
        {
            this.Usings = usings;
            this.ResultType = resultType;
            this.ProjectionParameters = projectionParameters;
            this.Body = body;
        }
    }
}