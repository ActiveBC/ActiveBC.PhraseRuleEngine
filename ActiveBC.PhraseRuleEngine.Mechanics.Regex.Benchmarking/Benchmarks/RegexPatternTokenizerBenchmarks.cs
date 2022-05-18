using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.Common;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Benchmarking.Benchmarks
{
    public class RegexPatternTokenizerBenchmarks
    {
        private readonly string[] m_cases_classic;
        private readonly string[] m_cases_real;

        private readonly LoopBasedRegexPatternTokenizer m_regexPatternTokenizer;

        private readonly Consumer m_consumer = new Consumer();

        public RegexPatternTokenizerBenchmarks()
        {
            this.m_cases_classic = DataProvider.Classic.Select(TransformCase).ToArray();
            this.m_cases_real = DataProvider.Real.Select(TransformCase).ToArray();

            this.m_regexPatternTokenizer = new LoopBasedRegexPatternTokenizer(new StringInterner());

            string TransformCase((string Regex, string[] Phrases) @case)
            {
                return @case.Regex;
            }
        }

        [Benchmark, BenchmarkCategory("RegexPatternTokenizer_ClassicCases")]
        public void Tokenizer_Classic()
        {
            foreach (string regex in m_cases_classic)
            {
                IPatternToken token = this.m_regexPatternTokenizer.Tokenize(regex, null, false);

                this.m_consumer.Consume(token);
            }
        }

        [Benchmark, BenchmarkCategory("RegexPatternTokenizer_RealCases")]
        public void Tokenizer_Real()
        {
            foreach (string regex in m_cases_real)
            {
                IPatternToken token = this.m_regexPatternTokenizer.Tokenize(regex, null, false);

                this.m_consumer.Consume(token);
            }
        }
    }
}