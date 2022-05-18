using ActiveBC.Tools.Benchmarking;
using BenchmarkDotNet.Running;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Benchmarking
{
    internal sealed class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "profile")
            {
                Profile();
            }
            else
            {
                Benchmark(args);
            }
        }

        private static void Benchmark(string[] args)
        {
            // this will run all the benchmarks
            BenchmarkRunner.Run(typeof(Program).Assembly, Configs.Default, args);

            // this will run single benchmark
            // BenchmarkRunner.Run<TerminalDetectorBenchmarks>(Configs.Default, args);
        }

        private static void Profile()
        {
            // new TerminalDetectorBenchmarks().LiteralSetDetector_Positive();
        }
    }
}