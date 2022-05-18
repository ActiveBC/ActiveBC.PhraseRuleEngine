using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;

namespace ActiveBC.PhraseRuleEngine.Tests.Helpers
{
    internal sealed class RuleSetContainer
    {
        public string Definition { get; }
        public RuleSetToken Token { get; }
        public IReadOnlyCollection<MechanicsBundle> MechanicsCollection { get; }

        private IRuleSpace? m_ruleSpace;
        public IRuleSpace RuleSpace
        {
            get
            {
                return this.m_ruleSpace ??= new RuleSpaceFactory(this.MechanicsCollection).CreateWithAliases(
                    new[] { this.Token },
                    Array.Empty<RuleToken>(),
                    ImmutableDictionary<string, IRuleMatcher>.Empty,
                    ImmutableDictionary<string, IRuleSpace>.Empty,
                    ImmutableDictionary<string, Type>.Empty,
                    new LoadedAssembliesProvider()
                );
            }
        }

        public RuleSetContainer(
            string definition,
            RuleSetToken token,
            IReadOnlyCollection<MechanicsBundle> mechanicsCollection
        )
        {
            this.Definition = definition;
            this.Token = token;
            this.MechanicsCollection = mechanicsCollection;
        }
    }
}