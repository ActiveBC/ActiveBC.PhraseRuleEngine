using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.TerminalDetectors;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Benchmarking.Benchmarks
{
    public class TerminalDetectorBenchmarks
    {
        private const int RunsNumber = 10000000;

        private readonly AnyLiteralDetector m_anyLiteralDetector;
        private readonly LiteralDetector m_literalDetector;
        private readonly LiteralSetDetector m_literalSetDetector;
        private readonly LiteralSetDetector m_negatedLiteralSetDetector;

        private readonly Consumer m_consumer = new Consumer();

        public TerminalDetectorBenchmarks()
        {
            this.m_anyLiteralDetector = AnyLiteralDetector.Instance;
            this.m_literalDetector = new LiteralDetector(new LiteralToken("привет"));
            this.m_literalSetDetector = new LiteralSetDetector(new LiteralSetToken(false, new ILiteralSetMemberToken[]{new LiteralToken("привет")}));
            this.m_negatedLiteralSetDetector = new LiteralSetDetector(new LiteralSetToken(true, new ILiteralSetMemberToken[]{new LiteralToken("привет")}));
        }

        [Benchmark, BenchmarkCategory("TerminalDetectorBenchmarks_Positive")]
        public void AnyLiteralDetector_Positive()
        {
            for (int i = 0; i < RunsNumber; i++)
            {
                bool isMatched = this.m_anyLiteralDetector.WordMatches("привет");

                this.m_consumer.Consume(isMatched);
            }
        }

        [Benchmark(Baseline = true), BenchmarkCategory("TerminalDetectorBenchmarks_Positive")]
        public void LiteralDetector_Positive()
        {
            for (int i = 0; i < RunsNumber; i++)
            {
                bool isMatched = this.m_literalDetector.WordMatches("привет");

                this.m_consumer.Consume(isMatched);
            }
        }

        [Benchmark(Baseline = true), BenchmarkCategory("TerminalDetectorBenchmarks_Negative")]
        public void LiteralDetector_Negative()
        {
            for (int i = 0; i < RunsNumber; i++)
            {
                bool isMatched = this.m_literalDetector.WordMatches("пока");

                this.m_consumer.Consume(isMatched);
            }
        }

        [Benchmark, BenchmarkCategory("TerminalDetectorBenchmarks_Positive")]
        public void LiteralSetDetector_Positive()
        {
            for (int i = 0; i < RunsNumber; i++)
            {
                bool isMatched = this.m_literalSetDetector.WordMatches("привет");

                this.m_consumer.Consume(isMatched);
            }
        }

        [Benchmark, BenchmarkCategory("TerminalDetectorBenchmarks_Negative")]
        public void LiteralSetDetector_Negative()
        {
            for (int i = 0; i < RunsNumber; i++)
            {
                bool isMatched = this.m_literalSetDetector.WordMatches("пока");

                this.m_consumer.Consume(isMatched);
            }
        }

        [Benchmark, BenchmarkCategory("TerminalDetectorBenchmarks_Positive")]
        public void NegatedLiteralSetDetector_Positive()
        {
            for (int i = 0; i < RunsNumber; i++)
            {
                bool isMatched = this.m_negatedLiteralSetDetector.WordMatches("пока");

                this.m_consumer.Consume(isMatched);
            }
        }

        [Benchmark, BenchmarkCategory("TerminalDetectorBenchmarks_Negative")]
        public void NegatedLiteralSetDetector_Negative()
        {
            for (int i = 0; i < RunsNumber; i++)
            {
                bool isMatched = this.m_negatedLiteralSetDetector.WordMatches("привет");

                this.m_consumer.Consume(isMatched);
            }
        }
    }
}