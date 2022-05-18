using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Models
{
    public sealed class PegInputProcessorDataCollector
    {
        public int ExplicitlyMatchedSymbolsCount { get; set; }
        public Dictionary<string, object?> CapturedVariables { get; }

        public PegInputProcessorDataCollector()
        {
            this.ExplicitlyMatchedSymbolsCount = 0;
            this.CapturedVariables = new Dictionary<string, object?>();
        }
    }
}