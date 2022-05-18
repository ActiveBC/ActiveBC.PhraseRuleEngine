using System;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result.SelectionStrategy;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.Common;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Tests.Helpers
{
    internal sealed class RuleSpaceSource
    {
        private static readonly RuleSpaceFactory s_factory = new RuleSpaceFactory(
            new[]
            {
                new MechanicsBundle(
                    "peg",
                    new LoopBasedPegPatternTokenizer(new StringInterner()),
                    new PegProcessorFactory(
                        new CombinedStrategy(
                            new IResultSelectionStrategy[]
                            {
                                new MaxExplicitSymbolsStrategy(),
                                new MaxProgressStrategy(),
                            }
                        )
                    ),
                    typeof(PegGroupToken)
                )
            }
        );

        private readonly IReadOnlyDictionary<string, (string Definition, PegGroupToken Token)> m_rules;

        private IRuleSpace? m_ruleSpace;
        public IRuleSpace RuleSpace => this.m_ruleSpace ??= CreateRuleSpace();

        public RuleSpaceSource(IReadOnlyDictionary<string, (string Definition, PegGroupToken Token)> rules)
        {
            this.m_rules = rules;
        }

        private IRuleSpace CreateRuleSpace()
        {
            return s_factory.CreateWithAliases(
                Array.Empty<RuleSetToken>(),
                this
                    .m_rules
                    .MapValue(
                        (ruleName, rulePattern) => new RuleToken(
                            null,
                            new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                            ruleName,
                            Array.Empty<CSharpParameterToken>(),
                            "peg",
                            rulePattern.Token,
                            VoidProjectionToken.Instance
                        )
                    )
                    .SelectValues()
                    .ToArray(),
                new Dictionary<string, IRuleMatcher>(),
                new Dictionary<string, IRuleSpace>(),
                new Dictionary<string, Type>(),
                new LoadedAssembliesProvider()
            );
        }
    }
}