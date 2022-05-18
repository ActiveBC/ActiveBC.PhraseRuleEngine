using System;
using System.Collections.Immutable;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.Common;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing.Automaton.Optimization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Benchmarking.Benchmarks
{
    public class RegexInputProcessorBenchmarks
    {
        private readonly (IRuleMatcher Matcher, string[] Phrases)[] m_automatonWalker_classic;
        private readonly (IRuleMatcher Matcher, string[] Phrases)[] m_automatonWalker_real;

        private readonly Consumer m_consumer = new Consumer();

        public RegexInputProcessorBenchmarks()
        {
            this.m_automatonWalker_classic = DataProvider.Classic.Select(TransformCase).ToArray();
            this.m_automatonWalker_real = DataProvider.Real.Select(TransformCase).ToArray();

            (IRuleMatcher Matcher, string[] Phrases) TransformCase((string Regex, string[] Phrases) @case)
            {
                return (CreateMatcher(@case.Regex), @case.Phrases);

                IRuleMatcher CreateMatcher(string regex)
                {
                    StringInterner stringInterner = new StringInterner();

                    RuleSpaceFactory factory = new RuleSpaceFactory(
                        new []
                        {
                            new MechanicsBundle(
                                "regex",
                                new LoopBasedRegexPatternTokenizer(stringInterner),
                                new RegexProcessorFactory(OptimizationLevel.Max),
                                typeof(RegexGroupToken)
                            )
                        }
                    );

                    const string runeName = "foo";

                    IRuleSpace ruleSpace = factory.CreateWithAliases(
                        Array.Empty<RuleSetToken>(),
                        new []
                        {
                            new RuleToken(
                                null,
                                new ResolvedCSharpTypeToken("string", typeof(string)),
                                runeName,
                                Array.Empty<CSharpParameterToken>(),
                                "regex",
                                factory.PatternTokenizers["regex"].Tokenize(regex, null, false),
                                MatchedInputBasedProjectionToken.Instance
                            ),
                        },
                        ImmutableDictionary<string, IRuleMatcher>.Empty,
                        ImmutableDictionary<string, IRuleSpace>.Empty,
                        ImmutableDictionary<string, Type>.Empty,
                        new LoadedAssembliesProvider()
                    );

                    return ruleSpace[runeName];
                }
            }
        }

        [Benchmark(Baseline = true), BenchmarkCategory("RegexInputProcessor_ClassicCases")]
        public void AutomatonWalker_Classic()
        {
            RunTest(this.m_automatonWalker_classic);
        }

        [Benchmark(Baseline = true), BenchmarkCategory("RegexInputProcessor_RealCases")]
        public void AutomatonWalker_Real()
        {
            RunTest(this.m_automatonWalker_real);
        }

        private void RunTest((IRuleMatcher Matcher, string[] Phrases)[] cases)
        {
            foreach ((IRuleMatcher Matcher, string[] Phrases) @case in cases)
            {
                foreach (string phrase in @case.Phrases)
                {
                    @case
                        .Matcher
                        .MatchAndProject(
                            new RuleInput(
                                phrase.Split(' ', StringSplitOptions.RemoveEmptyEntries),
                                new RuleSpaceArguments(ImmutableDictionary<string, object?>.Empty)
                            ),
                            0,
                            new RuleArguments(ImmutableDictionary<string, object?>.Empty),
                            new RuleSpaceCache()
                        )
                        .Consume(this.m_consumer);
                }
            }
        }
    }
}