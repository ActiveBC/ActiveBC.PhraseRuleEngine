using ActiveBC.PhraseRuleEngine.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result.SelectionStrategy;
using ActiveBC.PhraseRuleEngine.Lib.Common;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing.Automaton.Optimization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Tests.Helpers
{
    internal static class StaticResources
    {
        public static readonly StringInterner StringInterner = new StringInterner();
        public static readonly IPatternTokenizer Tokenizer = new LoopBasedRegexPatternTokenizer(StringInterner);
        public static readonly IResultSelectionStrategy ResultSelectionStrategy = new CombinedStrategy(
            new IResultSelectionStrategy[]
            {
                new MaxExplicitSymbolsStrategy(),
                new MaxProgressStrategy(),
            }
        );

        public static MechanicsBundle RegexMechanics(OptimizationLevel optimizationLevel = OptimizationLevel.Min)
        {
            return new MechanicsBundle(
                "regex",
                Tokenizer,
                new RegexProcessorFactory(optimizationLevel),
                typeof(RegexGroupToken)
            );
        }
    }
}