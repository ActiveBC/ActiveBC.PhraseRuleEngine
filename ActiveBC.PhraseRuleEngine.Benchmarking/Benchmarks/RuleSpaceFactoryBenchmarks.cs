using System;
using System.Collections.Immutable;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result.SelectionStrategy;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;
using ActiveBC.PhraseRuleEngine.Lib.Common;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing.Automaton.Optimization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace ActiveBC.PhraseRuleEngine.Benchmarking.Benchmarks
{
    public class RuleSpaceFactoryBenchmarks
    {
        private readonly RuleSpaceFactory m_ruleSpaceFactory;

        private readonly (RuleSetToken[] RuleSets, RuleToken[] Rules)[] m_cases;

        private readonly Consumer m_consumer = new Consumer();

        public RuleSpaceFactoryBenchmarks()
        {
            this.m_ruleSpaceFactory = CreateFactory();

            this.m_cases = new []
            {
                (
                    new []
                    {
                        this.m_ruleSpaceFactory.RuleSetTokenizer.Tokenize(DataProvider.MatcherCases.RuleSets["ner.time"], "ner.time", true),
                        this.m_ruleSpaceFactory.RuleSetTokenizer.Tokenize(DataProvider.MatcherCases.RuleSets["ner.doctor"], "ner.doctor", true),
                    },
                    Array.Empty<RuleToken>()
                )
            };

            RuleSpaceFactory CreateFactory()
            {
                StringInterner stringInterner = new StringInterner();

                return new RuleSpaceFactory(
                    new[]
                    {
                        new MechanicsBundle(
                            "peg",
                            new LoopBasedPegPatternTokenizer(stringInterner),
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
                        ),
                        new MechanicsBundle(
                            "regex",
                            new LoopBasedRegexPatternTokenizer(stringInterner),
                            new RegexProcessorFactory(OptimizationLevel.Max),
                            typeof(RegexGroupToken)
                        )
                    }
                );
            }
        }

        [Benchmark(Baseline = true), BenchmarkCategory("AllCases")]
        public void RuleSpaceFactory()
        {
            foreach ((RuleSetToken[] ruleSets, RuleToken[] rules) in this.m_cases)
            {
                this.m_consumer.Consume(
                    this.m_ruleSpaceFactory.CreateWithAliases(
                        ruleSets,
                        rules,
                        ImmutableDictionary<string, IRuleMatcher>.Empty,
                        ImmutableDictionary<string, IRuleSpace>.Empty,
                        ImmutableDictionary<string, Type>.Empty,
                        new LoadedAssembliesProvider()
                    )
                );
            }
        }
    }
}