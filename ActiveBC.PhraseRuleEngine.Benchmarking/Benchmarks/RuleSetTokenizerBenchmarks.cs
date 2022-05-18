using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Lib.Common;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization;
using BenchmarkDotNet.Attributes;

namespace ActiveBC.PhraseRuleEngine.Benchmarking.Benchmarks
{
    public class RuleSetTokenizerBenchmarks
    {
        private readonly LoopBasedRuleSetTokenizer m_ruleSetTokenizer;

        public RuleSetTokenizerBenchmarks()
        {
            StringInterner stringInterner = new StringInterner();

            this.m_ruleSetTokenizer = new LoopBasedRuleSetTokenizer(
                new Dictionary<string, IPatternTokenizer>()
                {
                    {"peg", new LoopBasedPegPatternTokenizer(stringInterner)},
                    {"regex", new LoopBasedRegexPatternTokenizer(stringInterner)},
                }
            );
        }

        [Benchmark(Baseline = true), BenchmarkCategory("AllCases")]
        [ArgumentsSource(nameof(GetRuleSets))]
        public void RuleSetTokenizer(string ruleSetName)
        {
            this.m_ruleSetTokenizer.Tokenize(DataProvider.MatcherCases.RuleSets[ruleSetName], null, true);
        }

        public static string[] GetRuleSets()
        {
            return new []
            {
                "ner.time",
                "ner.doctor",
            };
        }
    }
}