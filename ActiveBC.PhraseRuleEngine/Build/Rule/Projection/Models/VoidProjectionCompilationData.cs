namespace ActiveBC.PhraseRuleEngine.Build.Rule.Projection.Models
{
    internal sealed class VoidProjectionCompilationData : IProjectionCompilationData
    {
        public static readonly VoidProjectionCompilationData Instance = new VoidProjectionCompilationData();

        private VoidProjectionCompilationData()
        {
        }
    }
}