using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Tests.Helpers
{
    internal sealed class RuleSpaceSource<TPatternToken>
        where TPatternToken : IPatternToken
    {
        private readonly RuleSpaceFactory m_factory;
        private readonly IReadOnlyDictionary<string, (string Definition, TPatternToken Token)> m_rules;
        private readonly IReadOnlyCollection<Type> m_staticRuleContainers;

        private IRuleSpace? m_ruleSpace;
        public IRuleSpace RuleSpace => this.m_ruleSpace ??= CreateRuleSpace();

        public RuleSpaceSource(
            RuleSpaceFactory factory,
            IReadOnlyDictionary<string, (string Definition, TPatternToken Token)> rules,
            IReadOnlyCollection<Type> staticRuleContainers
        )
        {
            this.m_factory = factory;
            this.m_rules = rules;
            this.m_staticRuleContainers = staticRuleContainers;
        }

        private IRuleSpace CreateRuleSpace()
        {
            return this.m_factory.CreateWithAliases(
                Array.Empty<RuleSetToken>(),
                this
                    .m_rules
                    .MapValue(
                        (ruleName, rulePattern) => new RuleToken(
                            null,
                            new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                            ruleName,
                            Array.Empty<CSharpParameterToken>(),
                            "regex",
                            rulePattern.Token,
                            VoidProjectionToken.Instance
                        )
                    )
                    .SelectValues()
                    .ToArray(),
                this.m_staticRuleContainers
                    .Select(this.m_factory.StaticRuleFactory.ConvertStaticRuleContainerToRuleMatchers)
                    .MergeWithKnownCapacity(this.m_staticRuleContainers.Count),
                ImmutableDictionary<string, IRuleSpace>.Empty,
                ImmutableDictionary<string, Type>.Empty,
                new LoadedAssembliesProvider()
            );
        }
    }
}