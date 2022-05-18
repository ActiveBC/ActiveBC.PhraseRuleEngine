using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing.Automaton;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing.Automaton.Optimization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.States;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.Transitions;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Walker;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Exceptions;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Tests.Helpers;
using NUnit.Framework;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Tests
{
    internal sealed class RegexAutomatonPostprocessorTests
    {
        [Test]
        [TestCase("(один?)*")]
        [TestCase("(один?)+")]
        [TestCase("(один?){2,}")]
        [TestCase("(один*)*")]
        [TestCase("(один*)+")]
        [TestCase("(один*){2,}")]
        [TestCase("(один? два?)*")]
        [TestCase("(один? два?)+")]
        [TestCase("(один? два?){2,}")]
        [TestCase("(один* два*)*")]
        [TestCase("(один* два*)+")]
        [TestCase("(один* два*){2,}")]
        [TestCase("(один? два*)*")]
        [TestCase("(один? два*)+")]
        [TestCase("(один? два*){2,}")]
        [TestCase("(один* два?)*")]
        [TestCase("(один* два?)+")]
        [TestCase("(один* два?){2,}")]
        [TestCase("(один?|два|три)*")]
        [TestCase("(один|два?|три)*")]
        [TestCase("(один|два|три?)*")]
        [TestCase("один* (два?)+ три*")]
        [TestCase("(「маркер」)*")]
        [TestCase("(「маркер」)+")]
        [TestCase("(「маркер」){2,}")]
        [TestCase("(один? 「маркер」)*")]
        [TestCase("(один? 「маркер」)+")]
        [TestCase("(один? 「маркер」){2,}")]
        [TestCase("(один*「маркер」)*")]
        [TestCase("(один*「маркер」)+")]
        [TestCase("(один*「маркер」){2,}")]
        [TestCase("(「маркер」 один?)*")]
        [TestCase("(「маркер」 один?)+")]
        [TestCase("(「маркер」 один?){2,}")]
        [TestCase("(「маркер」 один*)*")]
        [TestCase("(「маркер」 один*)+")]
        [TestCase("(「маркер」 один*){2,}")]
        public void FailsWithLoopError(string regex)
        {
            RegexAutomaton automaton = new RegexAutomatonBuilder(
                (RegexGroupToken) StaticResources.Tokenizer.Tokenize(regex, null, false),
                DummyRuleSpace.Instance
            ).Build();

            RegexProcessorBuildException? exception = Assert.Throws<RegexProcessorBuildException>(
                () => RegexAutomatonPostprocessor.Instance.ValidateAndOptimize(automaton, OptimizationLevel.Min)
            );

            Assert.That(exception!.Message, Does.Match("Found undeterministic loop on state '\\d+'."));
        }

        [Test]
        [TestCase("(.)", 2, 1)]
        [TestCase("(один)", 2, 1)]
        [TestCase("([один])", 2, 1)]
        [TestCase("([^один~])", 2, 1)]
        [TestCase("(один~)", 2, 1)]
        [TestCase("(~один~)", 2, 1)]
        [TestCase("(~один)", 2, 1)]
        [TestCase("(.*)", 4, 5)]
        [TestCase("(.+)", 5, 6)]
        [TestCase("(.?)", 2, 2)]
        [TestCase("(.{3})", 4, 3)]
        [TestCase("(.{3,5})", 6, 7)]
        [TestCase("(.{3,})", 7, 8)]
        [TestCase("(.* один+ [один] [^один~] один~ ~один~ ~один)", 13, 16)]
        [TestCase("(привет|пока)", 2, 2)]
        [TestCase("(привет пока|хеллоу аривидерчи)", 4, 4)]
        [TestCase("((один))", 2, 1)]
        [TestCase("(((один)))", 2, 1)]
        public void DoesNothing(string regex, int statesCount, int transitionsCount)
        {
            TestStatesCount(regex, statesCount, transitionsCount, statesCount, transitionsCount, OptimizationLevel.Min);
            TestStatesCount(regex, statesCount, transitionsCount, statesCount, transitionsCount, OptimizationLevel.Max);
        }

        [TestCase("((a|a)*)", 4, 6, 4, 5, OptimizationLevel.Min)]
        [TestCase("((a|a)+)", 5, 8, 5, 6, OptimizationLevel.Min)]
        [TestCase("(a b|a c)", 4, 4, 3, 3, OptimizationLevel.Min)]
        [TestCase("(a b|a c|a d)", 5, 6, 3, 4, OptimizationLevel.Min)]
        [TestCase("(a b c|a b d)", 6, 6, 4, 4, OptimizationLevel.Min)]
        [TestCase("(a b c|a b d)", 6, 6, 4, 4, OptimizationLevel.Max)]
        [TestCase("(c b a|d b a)", 6, 6, 6, 6, OptimizationLevel.Min)]
        [TestCase("(c b a|d b a)", 6, 6, 4, 4, OptimizationLevel.Max)]
        [TestCase("(a|a)", 2, 2, 2, 1, OptimizationLevel.Min)]
        [TestCase("(a|a|a)", 2, 3, 2, 1, OptimizationLevel.Min)]
        [TestCase("(a b|a b|a b)", 5, 6, 3, 2, OptimizationLevel.Min)]
        [TestCase("(a?|a?)", 2, 4, 2, 2, OptimizationLevel.Min)]
        [TestCase("(a{3}|a{3})", 6, 6, 4, 3, OptimizationLevel.Min)]
        [TestCase("(a{3,5}|a{3,5})", 10, 14, 6, 7, OptimizationLevel.Min)]
        [TestCase("((a a a) x|(a a a) y)", 8, 8, 5, 5, OptimizationLevel.Min)]
        [TestCase("(((a a a) x)|((a a a) y))", 8, 8, 5, 5, OptimizationLevel.Min)]
        [TestCase("((a a a x)|(a a a y))", 8, 8, 5, 5, OptimizationLevel.Min)]
        [TestCase("((a|b c)|(b c|a))", 4, 6, 3, 3, OptimizationLevel.Min)]
        [TestCase("((a b|c d)|(c d|a b))", 6, 8, 4, 4, OptimizationLevel.Min)]
        [TestCase("(a b|a b|c d)", 5, 6, 4, 4, OptimizationLevel.Min)]
        [TestCase("(a b|a b|a c)", 5, 6, 3, 3, OptimizationLevel.Min)]
        [TestCase("(a b|a b|a b c)", 6, 7, 4, 4, OptimizationLevel.Min)]
        [TestCase("((a b|c d|e f)|(c d|a b))", 7, 10, 5, 6, OptimizationLevel.Min)]
        [TestCase("(a b с|a b d|a b c d|a b|a d|a f)", 12, 16, 5, 9, OptimizationLevel.Min)]
        [TestCase("(a b?|a b?) 「foo」", 5, 7, 4, 4, OptimizationLevel.Min)]
        [TestCase("(b?|b?) 「foo」", 3, 5, 3, 3, OptimizationLevel.Min)]
        // todo [realtime performance] fix those tests
        // [TestCase("(a+|a+)", 8, 12, 5, 6, OptimizationLevel.Min)]
        // [TestCase("(a*|a*)", 6, 10, 4, 5, OptimizationLevel.Min)]
        // [TestCase("(a{3,}|a{3,})", 12, 16, 7, 8, OptimizationLevel.Min)]
        // [TestCase("(a b*|a b*) 「foo」", 9, 13, 6, 7, OptimizationLevel.Min)]
        // [TestCase("(b*|b*) 「foo」", 7, 11, 5, 6, OptimizationLevel.Min)]
        public void Optimizes(
            string regex,
            int startStatesCount,
            int startTransitionsCount,
            int endStatesCount,
            int endTransitionsCount,
            OptimizationLevel optimizationLevel
        )
        {
            TestStatesCount(regex, startStatesCount, startTransitionsCount, endStatesCount, endTransitionsCount, optimizationLevel);
        }

        private static void TestStatesCount(
            string regex,
            int startStatesCount,
            int startTransitionsCount,
            int endStatesCount,
            int endTransitionsCount,
            OptimizationLevel optimizationLevel
        )
        {
            RegexAutomaton automaton = new RegexAutomatonBuilder(
                (RegexGroupToken) StaticResources.Tokenizer.Tokenize(regex, null, false),
                DummyRuleSpace.Instance
            ).Build();

            Digraph<RegexAutomatonState, RegexAutomatonTransition> startDigraph = RecursiveDfsDigraphWalker
                .Instance
                .DiscoverGraph<RegexAutomatonState, RegexAutomatonTransition>(automaton.StartState);

            Assert.AreEqual(startStatesCount, startDigraph.Vertices.Count);
            Assert.AreEqual(startTransitionsCount, startDigraph.Edges.Count);

            RegexAutomatonPostprocessor.Instance.ValidateAndOptimize(automaton, optimizationLevel);

            Digraph<RegexAutomatonState, RegexAutomatonTransition> endDigraph = RecursiveDfsDigraphWalker
                .Instance
                .DiscoverGraph<RegexAutomatonState, RegexAutomatonTransition>(automaton.StartState);

            Assert.AreEqual(endStatesCount, endDigraph.Vertices.Count);
            Assert.AreEqual(endTransitionsCount, endDigraph.Edges.Count);
        }
    }
}
