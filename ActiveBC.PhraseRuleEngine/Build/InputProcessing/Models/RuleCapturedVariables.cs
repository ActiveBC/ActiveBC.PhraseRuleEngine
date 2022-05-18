using System;
using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Build.InputProcessing.Models
{
    public sealed class RuleCapturedVariables
    {
        public IReadOnlyDictionary<string, Type> OwnVariables { get; }
        public IReadOnlyCollection<string> ReferencedRules { get; }

        public RuleCapturedVariables(
            IReadOnlyDictionary<string, Type> ownVariables,
            IReadOnlyCollection<string> referencedRules
        )
        {
            this.OwnVariables = ownVariables;
            this.ReferencedRules = referencedRules;
        }
    }
}