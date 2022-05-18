using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
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
    public class RuleMatcherBenchmarks
    {
        private readonly Dictionary<string, string[]> m_phrases;

        private readonly IRuleSpace m_ruleSpace;

        private readonly Consumer m_consumer = new Consumer();

        public RuleMatcherBenchmarks()
        {
            this.m_ruleSpace = CreateRuleSpace();
            this.m_phrases = DataProvider.MatcherCases.PhrasesByRule;

            IRuleSpace CreateRuleSpace()
            {
                StringInterner stringInterner = new StringInterner();

                RuleSpaceFactory factory = new RuleSpaceFactory(
                    new []
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

                return factory.CreateWithAliases(
                    new []
                    {
                        factory.RuleSetTokenizer.Tokenize(DataProvider.MatcherCases.RuleSets["ner.time"], "ner.time", true),
                        factory.RuleSetTokenizer.Tokenize(DataProvider.MatcherCases.RuleSets["ner.doctor"], "ner.doctor", true),
                    },
                    Array.Empty<RuleToken>(),
                    ImmutableDictionary<string, IRuleMatcher>.Empty,
                    ImmutableDictionary<string, IRuleSpace>.Empty,
                    ImmutableDictionary<string, Type>.Empty,
                    new LoadedAssembliesProvider()
                );
            }
        }

        [Benchmark(Baseline = true), BenchmarkCategory("AllCases")]
        public void RuleMatcher()
        {
            foreach ((string ruleName, string[] phrase) in this.m_phrases)
            {
                RuleInput ruleInput = new RuleInput(
                    phrase,
                    new RuleSpaceArguments(ImmutableDictionary<string, object?>.Empty)
                );

                this.m_consumer.Consume(
                    this
                        .m_ruleSpace[ruleName]
                        .MatchAndProject(
                            ruleInput,
                            0,
                            new RuleArguments(ImmutableDictionary<string, object?>.Empty),
                            new RuleSpaceCache()
                        )
                );
            }
        }
    }
}