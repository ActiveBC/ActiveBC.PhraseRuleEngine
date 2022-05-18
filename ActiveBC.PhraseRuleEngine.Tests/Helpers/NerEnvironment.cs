using System;
using ActiveBC.PhraseRuleEngine.Build;
using ActiveBC.PhraseRuleEngine.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Build.InputProcessing.Models;
using ActiveBC.PhraseRuleEngine.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.InputProcessing;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result.SelectionStrategy;
using ActiveBC.PhraseRuleEngine.Lib.Common;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing.Automaton.Optimization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Tests.Helpers.Dummy;

namespace ActiveBC.PhraseRuleEngine.Tests.Helpers
{
    public static class NerEnvironment
    {
        public static class Mechanics
        {
            private static readonly StringInterner StringInterner = new StringInterner();

            public static readonly MechanicsBundle Peg = new MechanicsBundle(
                "peg",
                new LoopBasedPegPatternTokenizer(StringInterner),
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
            );

            public static readonly MechanicsBundle Regex = new MechanicsBundle(
                "regex",
                new LoopBasedRegexPatternTokenizer(StringInterner),
                new RegexProcessorFactory(OptimizationLevel.Max),
                typeof(RegexGroupToken)
            );

            public static MechanicsBundle Dummy { get; } = new MechanicsBundle(
                "dummy",
                new DummyTokenizer(),
                new DummyProcessorFactory(),
                typeof(DummyPatternToken)
            );
        }

        private sealed class DummyTokenizer : IPatternTokenizer
        {
            public IPatternToken Tokenize(string pattern, string? @namespace, bool caseSensitive)
            {
                return new DummyPatternToken(pattern);
            }
        }

        private sealed class DummyProcessorFactory : IInputProcessorFactory
        {
            public IInputProcessor Create(IPatternToken patternToken, IRuleSpace ruleSpace)
            {
                throw new Exception("Wrong usage");
            }

            public RuleCapturedVariables ExtractOwnCapturedVariables(IPatternToken patternToken, IRuleDescriptionProvider ruleDescriptionProvider)
            {
                throw new Exception("Wrong usage");
            }
        }
    }
}