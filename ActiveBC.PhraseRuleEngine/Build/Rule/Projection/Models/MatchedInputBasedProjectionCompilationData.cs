namespace ActiveBC.PhraseRuleEngine.Build.Rule.Projection.Models
{
    internal sealed class MatchedInputBasedProjectionCompilationData : IProjectionCompilationData
    {
        public static readonly MatchedInputBasedProjectionCompilationData Instance = new MatchedInputBasedProjectionCompilationData();

        private MatchedInputBasedProjectionCompilationData()
        {
        }
    }
}