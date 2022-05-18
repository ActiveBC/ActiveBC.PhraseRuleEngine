using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing.Automaton.Optimization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Tests.Fixtures;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Tests.Helpers;
using NUnit.Framework;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Tests
{
    [TestFixture(TestOf = typeof(RegexProcessor))]
    internal sealed class RegexInputProcessorTests
    {
        [Test]
        [TestCase("(ноль один)", "ноль один", null)]
        [TestCase("(ноль 「маркер0」|один 「маркер1」)", "ноль", "маркер0")]
        [TestCase("(ноль 「маркер0」|один 「маркер1」)", "один", "маркер1")]
        [TestCase("(ноль один 「маркер」)", "ноль один", "маркер")]
        [TestCase("((привет) 「маркер-привет」|(пока) 「маркер-пока」)", "привет", "маркер-привет")]
        [TestCase("((привет) 「маркер-привет」|(пока) 「маркер-пока」)", "пока", "маркер-пока")]
        [TestCase("(((привет) 「маркер-привет」|(пока) 「маркер-пока」))", "привет", "маркер-привет")]
        [TestCase("(((привет) 「маркер-привет」|(пока) 「маркер-пока」))", "пока", "маркер-пока")]
        [TestCase("(hello)", "hello", null)]
        [TestCase("((hello))", "hello", null)]
        [TestCase("(привет)", "привет", null)]
        [TestCase("((привет))", "привет", null)]
        [TestCase("(「marker_1」hello)", "hello", "marker_1")]
        [TestCase("((「marker_1」hello))", "hello", "marker_1")]
        [TestCase("(「marker_1」привет)", "привет", "marker_1")]
        [TestCase("((「marker_1」привет))", "привет", "marker_1")]
        [TestCase("(「marker_1」(hello|bye))", "hello", "marker_1")]
        [TestCase("(「marker_1」(hello|bye))", "bye", "marker_1")]
        [TestCase("((「marker_1」(hello|bye)))", "hello", "marker_1")]
        [TestCase("((「marker_1」(hello|bye)))", "bye", "marker_1")]
        [TestCase("(「marker_1」(привет|пока))", "привет", "marker_1")]
        [TestCase("(「marker_1」(привет|пока))", "пока", "marker_1")]
        [TestCase("((「marker_1」(привет|пока)))", "привет", "marker_1")]
        [TestCase("((「marker_1」(привет|пока)))", "пока", "marker_1")]
        [TestCase("(「marker_1」hello|bye)", "hello", "marker_1")]
        [TestCase("(「marker_1」hello|bye)", "bye", null)]
        [TestCase("((「marker_1」hello|bye))", "hello", "marker_1")]
        [TestCase("((「marker_1」hello|bye))", "bye", null)]
        [TestCase("(「marker_1」привет|пока)", "привет", "marker_1")]
        [TestCase("(「marker_1」привет|пока)", "пока", null)]
        [TestCase("((「marker_1」привет|пока))", "привет", "marker_1")]
        [TestCase("((「marker_1」привет|пока))", "пока", null)]
        [TestCase("(hello「marker_1」)", "hello", "marker_1")]
        [TestCase("((hello「marker_1」))", "hello", "marker_1")]
        [TestCase("(привет「marker_1」)", "привет", "marker_1")]
        [TestCase("((привет「marker_1」))", "привет", "marker_1")]
        [TestCase("((hello|bye)「marker_1」)", "hello", "marker_1")]
        [TestCase("((hello|bye)「marker_1」)", "bye", "marker_1")]
        [TestCase("(((hello|bye)「marker_1」))", "hello", "marker_1")]
        [TestCase("(((hello|bye)「marker_1」))", "bye", "marker_1")]
        [TestCase("((привет|пока)「marker_1」)", "привет", "marker_1")]
        [TestCase("((привет|пока)「marker_1」)", "пока", "marker_1")]
        [TestCase("(((привет|пока)「marker_1」))", "привет", "marker_1")]
        [TestCase("(((привет|пока)「marker_1」))", "пока", "marker_1")]
        [TestCase("(hello|bye「marker_1」)", "hello", null)]
        [TestCase("(hello|bye「marker_1」)", "bye", "marker_1")]
        [TestCase("((hello|bye「marker_1」))", "hello", null)]
        [TestCase("((hello|bye「marker_1」))", "bye", "marker_1")]
        [TestCase("(привет|пока「marker_1」)", "привет", null)]
        [TestCase("(привет|пока「marker_1」)", "пока", "marker_1")]
        [TestCase("((привет|пока「marker_1」))", "привет", null)]
        [TestCase("((привет|пока「marker_1」))", "пока", "marker_1")]
        public void TokenizesAndMatchesAndProjectsAndExtractsMarker(
            string regex,
            string phrase,
            string? expectedMarker
        )
        {
            const bool caseSensitive = false;

            RuleInput ruleInput = CreateRuleInput(phrase, caseSensitive);

            RuleMatchResultCollection results = CreateSingleMatcher(regex, caseSensitive).MatchAndProject(
                ruleInput,
                0,
                new RuleArguments(ImmutableDictionary<string, object?>.Empty),
                new RuleSpaceCache()
            );

            Assert.Greater(results.Count, 0);

            RuleMatchResultCollection fullMatches = results.ExcludeEmptyMatches().GetFullMatches();

            foreach (RuleMatchResult result in fullMatches)
            {
                Assert.AreEqual(ruleInput.Sequence.Length, result.LastUsedSymbolIndex + 1);

                Assert.AreEqual(expectedMarker, result.Marker);
            }
        }

        [Test]
        [TestCaseSource(nameof(TokenizesAndMatchesAndProjects_Literal))]
        [TestCaseSource(nameof(TokenizesAndMatchesAndProjects_LiteralSet))]
        [TestCaseSource(nameof(TokenizesAndMatchesAndProjects_RepetitionOfLiteral))]
        [TestCaseSource(nameof(TokenizesAndMatchesAndProjects_OptionalLiteral))]
        [TestCaseSource(nameof(TokenizesAndMatchesAndProjects_OptionalRepetitionLiteral))]
        [TestCaseSource(nameof(TokenizesAndMatchesAndProjects_QuantifiedLiteral))]
        [TestCaseSource(nameof(TokenizesAndMatchesAndProjects_Branches))]
        [TestCaseSource(nameof(TokenizesAndMatchesAndProjects_Ners))]
        [TestCaseSource(nameof(TokenizesAndMatchesAndProjects_NestedGroups))]
        [TestCaseSource(nameof(TokenizesAndMatchesAndProjects_OptionalNestedGroups))]
        [TestCaseSource(nameof(TokenizesAndMatchesAndProjects_ComplexCases))]
        [TestCaseSource(nameof(TokenizesAndMatchesAndProjects_WeirdCases))]
        public void TokenizesAndMatchesAndProjects(
            string regex,
            string phrase,
            bool expectedIsMatched,
            int? expectedEndIndex = null,
            int? expectedExplicitlyMatchedSymbolsCount = null
        )
        {
            TokenizesAndMatchesAndProjectsWithStartIndex(
                regex,
                phrase,
                0,
                expectedIsMatched,
                expectedEndIndex,
                expectedExplicitlyMatchedSymbolsCount
            );
        }

        [Test]
        [TestCaseSource(nameof(TokenizesAndMatchesAndProjectsWithStartIndex_Literal))]
        [TestCaseSource(nameof(TokenizesAndMatchesAndProjectsWithStartIndex_RepetitionOfLiteral))]
        public void TokenizesAndMatchesAndProjectsWithStartIndex(
            string regex,
            string phrase,
            int firstSymbolIndex,
            bool expectedIsMatched,
            int? expectedEndIndex = null,
            int? expectedExplicitlyMatchedSymbolsCount = null
        )
        {
            const bool caseSensitive = false;

            RuleInput ruleInput = CreateRuleInput(phrase, caseSensitive);

            RuleMatchResultCollection results = CreateSingleMatcher(regex, caseSensitive).MatchAndProject(
                ruleInput,
                firstSymbolIndex,
                new RuleArguments(ImmutableDictionary<string, object?>.Empty),
                new RuleSpaceCache()
            );

            bool isMatched = results.Count > 0;

            Assert.AreEqual(expectedIsMatched, isMatched);

            if (isMatched)
            {
                RuleMatchResult result = results.Best(StaticResources.ResultSelectionStrategy)!;

                Assert.AreEqual(expectedEndIndex ?? ruleInput.Sequence.Length, result.LastUsedSymbolIndex + 1);

                if (expectedExplicitlyMatchedSymbolsCount is not null)
                {
                    Assert.AreEqual(expectedExplicitlyMatchedSymbolsCount, result.ExplicitlyMatchedSymbolsCount);
                }
            }
        }

        [Test]
        [TestCaseSource(nameof(ExtractsVariables_Ners))]
        public void ExtractsVariables(string regex, string phrase, IDictionary<string, object?> expectedVariables)
        {
            const bool caseSensitive = false;

            RuleInput ruleInput = CreateRuleInput(phrase, caseSensitive);

            RuleMatchResultCollection results = CreateSingleMatcher(regex, caseSensitive).MatchAndProject(
                ruleInput,
                0,
                new RuleArguments(ImmutableDictionary<string, object?>.Empty),
                new RuleSpaceCache()
            );

            Assert.IsTrue(results.Count == 1);

            RuleMatchResult result = results.Single();

            Assert.AreEqual(ruleInput.Sequence.Length, result.LastUsedSymbolIndex + 1);

            CollectionAssert.AreEquivalent(expectedVariables, result.CapturedVariables);
        }

        [Test]
        [TestCaseSource(nameof(ExtractsMultipleResults_WithNers))]
        public void ExtractsMultipleResults(
            string regex,
            string phrase,
            IDictionary<string, object?>[] expectedVariables
        )
        {
            const bool caseSensitive = false;

            RuleInput ruleInput = CreateRuleInput(phrase, caseSensitive);

            RuleMatchResultCollection results = CreateSingleMatcher(regex, caseSensitive).MatchAndProject(
                ruleInput,
                0,
                new RuleArguments(ImmutableDictionary<string, object?>.Empty),
                new RuleSpaceCache()
            );

            Assert.IsTrue(results.Count > 1);

            foreach (RuleMatchResult result in results)
            {
                Assert.AreEqual(ruleInput.Sequence.Length - 1, result.LastUsedSymbolIndex);
            }

            CollectionAssert.AreEquivalent(
                expectedVariables.Select(capturedVariables => capturedVariables.OrderBy(pair => pair.Key)),
                results.Select(result => (result.CapturedVariables ?? ImmutableDictionary<string, object?>.Empty).OrderBy(pair => pair.Key)).ToArray()
            );
        }

        [Test]
        [TestCaseSource(nameof(HandlesRuleReferences_Mixed))]
        public void HandlesRuleReferences(
            Dictionary<string, string> rules,
            string ruleKey,
            string phrase,
            IDictionary<string, object?>? expectedVariables = null
        )
        {
            const bool caseSensitive = false;

            RuleInput ruleInput = CreateRuleInput(phrase, caseSensitive);

            IRuleSpace ruleSpace = CreateRuleSpace(rules, caseSensitive, OptimizationLevel.Min);

            RuleMatchResultCollection results = ruleSpace[ruleKey].MatchAndProject(
                ruleInput,
                0,
                new RuleArguments(ImmutableDictionary<string, object?>.Empty),
                new RuleSpaceCache()
            );

            Assert.AreEqual(1, results.Count);

            RuleMatchResult result = results.Single();

            Assert.AreEqual(ruleInput.Sequence.Length, result.LastUsedSymbolIndex + 1);

            CollectionAssert.AreEquivalent(expectedVariables, result.CapturedVariables);
        }

        private IRuleSpace CreateRuleSpace(
            Dictionary<string, string> rules,
            bool caseSensitive,
            OptimizationLevel optimizationLevel
        )
        {
            MechanicsBundle regexpMechanics = StaticResources.RegexMechanics(optimizationLevel);

            RuleSpaceSource<RegexGroupToken> ruleSpaceSource = new RuleSpaceSource<RegexGroupToken>(
                new RuleSpaceFactory(
                    new []
                    {
                        regexpMechanics,
                    }
                ),
                rules
                    .MapValue(regex => (regex, (RegexGroupToken) regexpMechanics.Tokenizer.Tokenize(regex, null, caseSensitive)))
                    .ToDictionaryWithKnownCapacity(rules.Count),
            new []
                {
                    typeof(DummyNerSource),
                }
            );

            return ruleSpaceSource.RuleSpace;
        }

        private static IRuleMatcher CreateSingleMatcher(
            string regex,
            bool caseSensitive,
            OptimizationLevel optimizationLevel = OptimizationLevel.Min
        )
        {
            const string ruleName = "foo";

            MechanicsBundle regexpMechanics = StaticResources.RegexMechanics(optimizationLevel);

            RuleSpaceSource<RegexGroupToken> ruleSpaceSource = new RuleSpaceSource<RegexGroupToken>(
                new RuleSpaceFactory(
                    new []
                    {
                        regexpMechanics,
                    }
                ),
                new Dictionary<string, (string Definition, RegexGroupToken Token)>()
                {
                    {
                        ruleName,
                        (
                            regex,
                            (RegexGroupToken) regexpMechanics.Tokenizer.Tokenize(regex, null, caseSensitive)
                        )
                    },
                },
                new []
                {
                    typeof(DummyNerSource),
                }
            );

            return ruleSpaceSource.RuleSpace[ruleName];
        }

        private static RuleInput CreateRuleInput(string phrase, bool caseSensitive)
        {
            if (!caseSensitive)
            {
                phrase = phrase.ToLowerFastRusEng();
            }

            return new RuleInput(
                phrase.Split(' ', StringSplitOptions.RemoveEmptyEntries),
                new RuleSpaceArguments(ImmutableDictionary<string, object?>.Empty)
            );
        }

        #region Sources

        #region Sources_TokenizesAndMatchesAndProjects

        public static object?[][] TokenizesAndMatchesAndProjects_Literal()
        {
            return new []
            {
                new object?[]
                {
                    "(привет)",
                    "",
                    false,
                    null,
                    1,
                },
                new object?[]
                {
                    "(привет)",
                    "пока",
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "(привет)",
                    "пока привет",
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "(привет)",
                    "привет",
                    true,
                    null,
                    1,
                },
                new object?[]
                {
                    "(привет)",
                    "привет пока",
                    true,
                    1,
                    1,
                },
                new object?[]
                {
                    "(привет)",
                    "привет пока привет",
                    true,
                    1,
                    1,
                },
            };
        }

        public static object?[][] TokenizesAndMatchesAndProjects_LiteralSet()
        {
            return new[]
            {
                new object?[]
                {
                    "я [^ не]{0,3} знаю",
                    "я вас знаю",
                    true,
                    null,
                    2,
                },
                new object?[]
                {
                    "я [^ не]{0,3} знаю",
                    "я вас не знаю",
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "я .{0,3} [^ не]* знаю",
                    "я вас знаю",
                    true,
                    null,
                    2,
                },
                new object?[]
                {
                    "я .{0,3} [^ не]* знаю",
                    "я вас не знаю",
                    true,
                    null,
                    2,
                },
                new object?[]
                {
                    "я .{0,3} [не] знаю",
                    "я не знаю",
                    true,
                    null,
                    3,
                },
                new object?[]
                {
                    "я .{0,3} [не] знаю",
                    "я вас не знаю",
                    true,
                    null,
                    3,
                },
            };
        }

        public static object?[][] TokenizesAndMatchesAndProjects_RepetitionOfLiteral()
        {
            return new []
            {
                new object?[]
                {
                    "(привет+)",
                    "пока",
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "(привет+)",
                    "привет",
                    true,
                    null,
                    1,
                },
                new object?[]
                {
                    "(привет+)",
                    "привет пока",
                    true,
                    1,
                    1,
                },
                new object?[]
                {
                    "(привет+)",
                    "привет привет",
                    true,
                    null,
                    2,
                },
                new object?[]
                {
                    "(привет+)",
                    "привет привет привет пока",
                    true,
                    3,
                    3,
                },
            };
        }

        public static object?[][] TokenizesAndMatchesAndProjects_OptionalLiteral()
        {
            return new []
            {
                new object?[]
                {
                    "(привет?)",
                    "пока",
                    true,
                    0,
                    0,
                },
                new object?[]
                {
                    "(привет?)",
                    "привет",
                    true,
                    null,
                    1,
                },
                new object?[]
                {
                    "(привет?)",
                    "привет пока",
                    true,
                    1,
                    1,
                },
                new object?[]
                {
                    "(привет?)",
                    "привет привет",
                    true,
                    1,
                    1,
                },
                new object?[]
                {
                    "(один привет? два)",
                    "один привет два",
                    true,
                    null,
                    3,
                },
                new object?[]
                {
                    "(один привет? два)",
                    "один два",
                    true,
                    null,
                    2,
                },
            };
        }

        public static object?[][] TokenizesAndMatchesAndProjects_OptionalRepetitionLiteral()
        {
            return new []
            {
                new object?[]
                {
                    "(привет*)",
                    "пока",
                    true,
                    0,
                    0,
                },
                new object?[]
                {
                    "(привет*)",
                    "привет",
                    true,
                    null,
                    1,
                },
                new object?[]
                {
                    "(привет*)",
                    "привет пока",
                    true,
                    1,
                    1,
                },
                new object?[]
                {
                    "(привет*)",
                    "привет привет",
                    true,
                    null,
                    2,
                },
                new object?[]
                {
                    "(привет*)",
                    "привет привет привет пока",
                    true,
                    3,
                    3,
                },
            };
        }

        public static object?[][] TokenizesAndMatchesAndProjects_QuantifiedLiteral()
        {
            return new []
            {
                new object?[]
                {
                    "(привет{1})",
                    "пока",
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "(привет{1})",
                    "привет",
                    true,
                    null,
                    1,
                },
                new object?[]
                {
                    "(привет{1})",
                    "привет привет",
                    true,
                    1,
                    1,
                },
                new object?[]
                {
                    "(привет{3})",
                    "пока",
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "(привет{3})",
                    "привет",
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "(привет{3})",
                    "привет привет",
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "(привет{3})",
                    "привет привет привет",
                    true,
                    null,
                    3,
                },
                new object?[]
                {
                    "(привет{3})",
                    "привет привет привет привет",
                    true,
                    3,
                    3,
                },
                new object?[]
                {
                    "(привет{3,})",
                    "пока",
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "(привет{3,})",
                    "привет",
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "(привет{3,})",
                    "привет привет",
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "(привет{3,})",
                    "привет привет привет",
                    true,
                    null,
                    3,
                },
                new object?[]
                {
                    "(привет{3,})",
                    "привет привет привет привет",
                    true,
                    null,
                    4,
                },
                new object?[]
                {
                    "(привет{3,})",
                    "привет привет привет привет",
                    true,
                    null,
                    4,
                },
                new object?[]
                {
                    "(привет{3,})",
                    "привет привет привет пока",
                    true,
                    3,
                    3,
                },
                new object?[]
                {
                    "(привет{3,})",
                    "привет привет привет привет пока",
                    true,
                    4,
                    4,
                },
                new object?[]
                {
                    "(привет{3,5})",
                    "пока",
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "(привет{3,5})",
                    "привет",
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "(привет{3,5})",
                    "привет привет",
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "(привет{3,5})",
                    "привет привет привет",
                    true,
                    null,
                    3,
                },
                new object?[]
                {
                    "(привет{3,5})",
                    "привет привет привет привет",
                    true,
                    null,
                    4,
                },
                new object?[]
                {
                    "(привет{3,5})",
                    "привет привет привет привет привет",
                    true,
                    null,
                    5,
                },
                new object?[]
                {
                    "(привет{3,5})",
                    "привет привет привет привет привет привет",
                    true,
                    5,
                    5,
                },
                new object?[]
                {
                    "(привет{3,5})",
                    "привет привет пока",
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "(привет{3,5})",
                    "привет привет привет пока",
                    true,
                    3,
                    3,
                },
                new object?[]
                {
                    "(привет{3,5})",
                    "привет привет привет привет привет пока",
                    true,
                    5,
                    5,
                },
                new object?[]
                {
                    "(привет{0,1000})",
                    "",
                    true,
                    null,
                    0
                },
                new object?[]
                {
                    "(привет{0,1000})",
                    "привет",
                    true,
                    null,
                    1
                },
                new object?[]
                {
                    "(привет{0,1000})",
                    Enumerable.Repeat("привет", 1000).JoinToString(" "),
                    true,
                    null,
                    1000
                },
            };
        }

        public static object?[][] TokenizesAndMatchesAndProjects_Branches()
        {
            return new[]
            {
                new object?[] { "(один два)", "один два", true, null, 2, },
                new object?[] { "(привет|пока)", "пока", true, null, 1, },
                new object?[] { "(привет|пока)", "привет", true, null, 1, },
                new object?[] { "(привет|пока)", "ало", false, null, null, },
                new object?[] { "(один два|один три)", "один два", true, null, 2, },
                new object?[] { "(один два|один три)", "один три", true, null, 2, },
                new object?[] { "(раз два|один два)", "раз два", true, null, 2, },
                new object?[] { "(раз два|один два)", "один два", true, null, 2, },
                new object?[] { "(один два хватит|один три хватит)", "один два хватит", true, null, 3, },
                new object?[] { "(один два хватит|один три хватит)", "один три хватит", true, null, 3, },
                new object?[] { "(один один хватит|один два хватит|один три хватит|один четыре хватит)", "один один хватит", true, null, 3, },
                new object?[] { "(один один хватит|один два хватит|один три хватит|один четыре хватит)", "один два хватит", true, null, 3, },
                new object?[] { "(один один хватит|один два хватит|один три хватит|один четыре хватит)", "один три хватит", true, null, 3, },
                new object?[] { "(один один хватит|один два хватит|один три хватит|один четыре хватит)", "один четыре хватит", true, null, 3, },
                new object?[] { "(привет{2,4}|пока?|ало*|хай+)", "", true, null, 0, },
                new object?[] { "(привет{2,4}|пока?|ало*|хай+)", "привет", true, 0, 0, },
                new object?[] { "(привет{2,4}|пока?|ало*|хай+)", "привет привет", true, null, 2, },
                new object?[] { "(привет{2,4}|пока?|ало*|хай+)", "привет привет привет", true, null, 3, },
                new object?[] { "(привет{2,4}|пока?|ало*|хай+)", "привет привет привет привет", true, null, 4, },
                new object?[] { "(привет{2,4}|пока?|ало*|хай+)", "привет привет привет привет привет", true, 4, 4, },
                new object?[] { "(привет{2,4}|пока?|ало*|хай+)", "пока", true, null, 1, },
                new object?[] { "(привет{2,4}|пока?|ало*|хай+)", "пока пока", true, 1, 1, },
                new object?[] { "(привет{2,4}|пока?|ало*|хай+)", "ало", true, null, 1, },
                new object?[] { "(привет{2,4}|пока?|ало*|хай+)", "ало ало", true, null, 2, },
                new object?[] { "(привет{2,4}|пока?|ало*|хай+)", "ало ало ало", true, null, 3, },
                new object?[] { "(привет{2,4}|пока?|ало*|хай+)", "хай", true, null, 1, },
                new object?[] { "(привет{2,4}|пока?|ало*|хай+)", "хай хай", true, null, 2, },
                new object?[] { "(привет{2,4}|пока?|ало*|хай+)", "хай хай хай", true, null, 3, },
                new object?[] { "(три четыре|<var1 = dummy_ner.three_with_optional_four()>)", "три четыре", true, null, 2, },
                new object?[] { "(<var1 = dummy_ner.three_with_optional_four()>|три четыре)", "три четыре", true, null, 2, },
                new object?[] { "(привет привет привет привет|<var1 = dummy_ner.hi_star()>)", "привет привет привет привет", true, null, 4, },
                new object?[] { "(<var1 = dummy_ner.hi_star()>|привет привет привет привет)", "привет привет привет привет", true, null, 4, },
                new object?[] { "(привет привет привет|<var1 = dummy_ner.any_next_three_words()>)", "привет привет привет", true, null, 3, },
                new object?[] { "(<var1 = dummy_ner.any_next_three_words()>|привет привет привет)", "привет привет привет", true, null, 3, },
                new object?[] { "((один два три четыре пять|один <var1 = dummy_ner.any_next_three_words()>) шесть семь)", "один два три четыре пять шесть семь", true, null, 7, },
                new object?[] { "((один <var1 = dummy_ner.any_next_three_words()>|один два три четыре пять) шесть семь)", "один два три четыре пять шесть семь", true, null, 7, },
                new object?[] { "((один два три четыре пять|один <var1 = dummy_ner.any_next_three_words()>) шесть семь)", "один два три четыре шесть семь", true, null, 6, },
                new object?[] { "((один <var1 = dummy_ner.any_next_three_words()>|один два три четыре пять) шесть семь)", "один два три четыре шесть семь", true, null, 6, },
                new object?[] { "(привет (<var1 = dummy_ner.any_next_three_words()>|<var1 = dummy_ner.any_next_two_words()>) пока?)", "привет один два пока", true, null, 4, },
                new object?[] { "(привет (<var1 = dummy_ner.any_next_three_words()>|<var1 = dummy_ner.any_next_two_words()>) пока?)", "привет один два три пока", true, null, 5, },
                new object?[] { "(привет (<var1 = dummy_ner.any_next_two_words()>|<var1 = dummy_ner.any_next_three_words()>) пока?)", "привет один два пока", true, null, 4, },
                new object?[] { "(привет (<var1 = dummy_ner.any_next_two_words()>|<var1 = dummy_ner.any_next_three_words()>) пока?)", "привет один два три пока", true, null, 5, },
            };
        }

        public static object?[][] TokenizesAndMatchesAndProjects_Ners()
        {
            return new[]
            {
                new object?[] { "(<var1 = dummy_ner.hi()>|<var2 = dummy_ner.bye()>)", "пока", true, null, 1, },
                new object?[] { "(<var1 = dummy_ner.hi()>|<var2 = dummy_ner.bye()>)", "привет", true, null, 1, },
                new object?[] { "(<var1 = dummy_ner.hi()>|<var2 = dummy_ner.bye()>)", "ало", false, null, 1, },
                new object?[] { "(два <dummy_ner.three_with_optional_four> четыре)", "два три четыре", false, null, 3, },
                new object?[] { "(два <dummy_ner.three_with_optional_four> четыре?)", "два три четыре", true, null, 3, },
                new object?[] { "(два три <dummy_ner.three_with_optional_four> четыре?)", "два три четыре", false, null, 3, },
                new object?[] { "(два три? <dummy_ner.three_with_optional_four> четыре?)", "два три четыре", true, null, 3, },
                new object?[] { "(два три? четыре? <dummy_ner.three_with_optional_four>)", "два три четыре", true, null, 3, },
                new object?[] { "(два <dummy_ner.three_with_optional_four> три? четыре?)", "два три четыре", true, null, 3, },
                new object?[] { "((<var1 = dummy_ner.hi()>))", "привет", true, null, 1, },
                new object?[] { "((<var1 = dummy_ner.hi()>)+)", "привет", true, null, 1, },
                new object?[] { "((<var1 = dummy_ner.hi()>)*)", "", true, null, 0, },
                new object?[] { "((<var1 = dummy_ner.hi()>)*)", "привет", true, null, 1, },
                new object?[] { "((<var1 = dummy_ner.hi()>)*)", "привет привет", true, null, 2, },
            };
        }

        public static object?[][] TokenizesAndMatchesAndProjects_NestedGroups()
        {
            return new[]
            {
                new object?[] { "((один))", "один", true, null, 1, },
                new object?[] { "((привет)|(пока))", "пока", true, null, 1, },
                new object?[] { "((привет)|(пока))", "привет", true, null, 1, },
                new object?[] { "((привет)|(пока))", "ало", false, null, 1, },
                new object?[] { "((привет{2,4})|(пока?)|(ало*)|(хай+))", "", true, null, 0, },
                new object?[] { "((привет{2,4})|(пока?)|(ало*)|(хай+))", "привет", true, 0, 0, },
                new object?[] { "((привет{2,4})|(пока?)|(ало*)|(хай+))", "привет привет", true, null, 2, },
                new object?[] { "((привет{2,4})|(пока?)|(ало*)|(хай+))", "привет привет привет", true, null, 3, },
                new object?[] { "((привет{2,4})|(пока?)|(ало*)|(хай+))", "привет привет привет привет", true, null, 4, },
                new object?[] { "((привет{2,4})|(пока?)|(ало*)|(хай+))", "привет привет привет привет привет", true, 4, 4, },
                new object?[] { "((привет{2,4})|(пока?)|(ало*)|(хай+))", "пока", true, null, 1, },
                new object?[] { "((привет{2,4})|(пока?)|(ало*)|(хай+))", "пока пока", true, 1, 1, },
                new object?[] { "((привет{2,4})|(пока?)|(ало*)|(хай+))", "ало", true, null, 1, },
                new object?[] { "((привет{2,4})|(пока?)|(ало*)|(хай+))", "ало ало", true, null, 2, },
                new object?[] { "((привет{2,4})|(пока?)|(ало*)|(хай+))", "ало ало ало", true, null, 3, },
                new object?[] { "((привет{2,4})|(пока?)|(ало*)|(хай+))", "хай", true, null, 1, },
                new object?[] { "((привет{2,4})|(пока?)|(ало*)|(хай+))", "хай хай", true, null, 2, },
                new object?[] { "((привет{2,4})|(пока?)|(ало*)|(хай+))", "хай хай хай", true, null, 3, },
                new object?[] { "(.* ((один)「1」|(два)「2」) .*)", "один", true, null, 1, },
                new object?[] { "(.* ((один)「1」|(два)「2」) .*)", "два", true, null, 1, },
            };
        }

        public static object?[][] TokenizesAndMatchesAndProjects_OptionalNestedGroups()
        {
            return new[]
            {
                new object?[] { "((один|два|три)?)", "", true, null, 0, },
                new object?[] { "((один|два|три)?)", "один", true, null, 1, },
                new object?[] { "((один|два|три)?)", "два", true, null, 1, },
                new object?[] { "((один|два|три)?)", "три", true, null, 1, },
                new object?[] { "(((один|два|три)?))", "", true, null, 0, },
                new object?[] { "(((один|два|три)?))", "один", true, null, 1, },
                new object?[] { "(((один|два|три)?))", "два", true, null, 1, },
                new object?[] { "(((один|два|три)?))", "три", true, null, 1, },
                new object?[] { "(((один|два|три))?)", "", true, null, 0, },
                new object?[] { "(((один|два|три))?)", "один", true, null, 1, },
                new object?[] { "(((один|два|три))?)", "два", true, null, 1, },
                new object?[] { "(((один|два|три))?)", "три", true, null, 1, },
                new object?[] { "((один|два|три)*)", "", true, null, 0, },
                new object?[] { "((один|два|три)*)", "один", true, null, 1, },
                new object?[] { "((один|два|три)*)", "два", true, null, 1, },
                new object?[] { "((один|два|три)*)", "три", true, null, 1, },
                new object?[] { "((один|два|три)*)", "четыре", true, 0, 0, },
                new object?[] { "((один|два|три)*)", "два один два три один два", true, null, 6, },
                new object?[] { "(((один|два|три))*)", "", true, null, 0, },
                new object?[] { "(((один|два|три))*)", "один", true, null, 1, },
                new object?[] { "(((один|два|три))*)", "два", true, null, 1, },
                new object?[] { "(((один|два|три))*)", "три", true, null, 1, },
                new object?[] { "(((один|два|три))*)", "четыре", true, 0, 0, },
                new object?[] { "(((один|два|три))*)", "два один два три один два", true, null, 6, },
                new object?[] { "((один|два|три){2,})", "", false, null, null, },
                new object?[] { "((один|два|три){2,})", "один", false, null, null, },
                new object?[] { "((один|два|три){2,})", "два", false, null, null, },
                new object?[] { "((один|два|три){2,})", "три", false, null, null, },
                new object?[] { "((один|два|три){2,})", "четыре", false, null, null, },
                new object?[] { "((один|два|три){2,})", "один два", true, null, 2, },
                new object?[] { "((один|два|три){2,})", "два три", true, null, 2, },
                new object?[] { "((один|два|три){2,})", "три один", true, null, 2, },
                new object?[] { "((один|два|три){2,})", "один два четыре", true, 2, 2, },
                new object?[] { "((один|два|три){2,})", "один два три четыре", true, 3, 3, },
                new object?[] { "((один|два|три){2,})", "три четыре", false, null, null, },
                new object?[] { "((один|два|три){2,})", "два один два три один два", true, null, 6, },
            };
        }

        public static object?[][] TokenizesAndMatchesAndProjects_ComplexCases()
        {
            return new[]
            {
                new object?[] { "мясо", "мясо", true, null, 1, },
                new object?[] { "еда (мясо|хлеб)*", "еда хлеб", true, null, 2, },
                new object?[] { "мясо рыба", "мясо рыба", true, null, 2, },
                new object?[] { "мясо рыба сало", " мясо рыба сало ", true, null, 3, },
                new object?[] { "(мясо|рыба)* сало", "мясо рыба мясо рыба", false, null, 0, },
                new object?[] { "(мясо|рыба)* мясо", "мясо мясо рыба мясо", true, null, 4, },
                new object?[] { "( мясо | рыба )* мясо", "мясо рыба мясо сало", true, 3, 3, },
                new object?[] { "мясо (рыба|сало)* хлеб", "мясо рыба рыба сало хлеб", true, null, 5, },
                new object?[] { ".* хлеб", "мясо рыба рыба сало хлеб", true, null, 1, },
                new object?[] { "[^хлеб]* хлеб", "мясо рыба рыба сало хлеб", true, null, 1, },
                new object?[] { "сыр [^хлеб сыр]* хлеб", "сыр мясо рыба рыба сало хлеб", true, null, 2, },
                new object?[] { "сыр [мясо рыба]* хлеб", "сыр мясо рыба рыба хлеб", true, null, 5, },
                new object?[] { "[какое-то какой-то]* хлеб", "какой-то хлеб", true, null, 2, },
                new object?[] { "(какое-то|какой-то)* хлеб", "какой-то хлеб", true, null, 2, },
                new object?[] { "a", "a", true, null, 1, },
                new object?[] { "a b", "a b", true, null, 2, },
                new object?[] { "a b c d e f g", "a b c d e f g", true, null, 7, },
                new object?[] { "(a|b)* a", "a b a b a b a b a b", true, 9, 9, },
                new object?[] { "(a|b)* a", "a a a a a a a a b a", true, null, 10, },
                new object?[] { "(a|b)* a", "a a a a a a b a c", true, 8, 8, },
                new object?[] { "a(b|c)* d", "a b c c b c c c d", true, null, 9, },
                new object?[] { "a(b|c)* d", "a b c c b c c c d e", true, 9, 9, },
                new object?[] { "a z x y (b|c)+ d e f", "a z x y b c c b c c c d e f", true, null, 14, },
                new object?[] { "a...(b|c)+ d e f", "a z x y b c c b c c c d e f", true, null, 11, },
                new object?[] { "a[b c]+ z", "a b c c b c c c z", true, null, 9, },
                new object?[] { "a[b c]+ z", "a b x x x x c z ", false, null, 0, },
                new object?[] { "a[^b c]+ z", "a b x x x x c z", false, null, 0, },
                new object?[] { "a[^b c]+ z", "a x x x x z", true, null, 2, },
                new object?[] { "(a мясо?)+", "a", true, null, 1, },
                new object?[] { "(a мясо?)*", "a", true, null, 1, },
                new object?[] { "(a мясо*)?", "a", true, null, 1, },
                new object?[] { "(a мясо+)*", "a мясо", true, null, 2, },
                new object?[] { "(a мясо+)+", "a мясо", true, null, 2, },
                new object?[] { "(a мясо*)+", "a", true, null, 1, },
                new object?[] { ".{0,3}", "a", true, null, 0, },
                new object?[] { "(  (в роде | было)+ | ищу )+", "было было ", true, null, 2, },
                new object?[] { "мЯсО", "МяСо", true, null, 1, },
                new object?[] { "[мЯсО]", "МяСо", true, null, 1, },
                new object?[] { "не~", "не", true, null, 1, },
                new object?[] { "не~", "нельзя", true, null, 1, },
                new object?[] { "не~", "свиней", false, null, 0, },
                new object?[] { "не~", "вовне", false, null, 0, },
                new object?[] { "~не", "не", true, null, 1, },
                new object?[] { "~не", "вовне", true, null, 1, },
                new object?[] { "~не", "свиней", false, null, 0, },
                new object?[] { "~не~", "не", true, null, 1, },
                new object?[] { "~не~", "вовне", true, null, 1, },
                new object?[] { "~не~", "нельзя", true, null, 1, },
                new object?[] { "~не~", "свиней", true, null, 1, },
                new object?[] { "~не~", "ненене", true, null, 1, },
                new object?[] { "~не~", "вася", false, null, 0, },
                new object?[] { "(~не|не~)", "свиней", false, null, 0, },
                new object?[] { "(~не|не~)", "нелепо", true, null, 1, },
                new object?[] { "(~не|не~)", "не", true, null, 1, },
                new object?[] { "~не~+", "не нелепо свиней вовне ненавидеть ненадо", true, null, 6, },
                new object?[] { "~не~+", "не вася", true, 1, 1, },
                new object?[] { "[~не~]+", "не вася", true, 1, 1, },
                new object?[] { "[~не~]+", "не", true, null, 1, },
                new object?[] { "[~не~ вася]+", "не вася", true, null, 2, },
                new object?[] { "[~не~]+", "не нелепо свиней вовне ненавидеть ненадо", true, null, 6, },
                new object?[] { "[~не не~]+", "не нелепо свиней вовне ненавидеть ненадо", true, 2, 2, },
                new object?[] { "[~не не~]+", "не нелепо вовне ненавидеть ненадо", true, null, 5, },
                new object?[] { "[не~ ~да]+", "не нелепо свиней вовне ненавидеть ненадо", true, 2, 2, },
                new object?[] { "[не~ ~да]+", "не нелепо ненавидеть ненадо еда да кизлодда", true, null, 7, },
                new object?[] { "[не~ ~дай]+", "не нелепо ненавидеть ненадо еда да кизлодда", true, 4, 4, },
                new object?[] { "[не~ ~да ыть]+", "не еда ыть", true, null, 3, },
                new object?[] { "[не~ ~да ыть]+", "не еда пыщь", true, 2, 2, },
                new object?[] { "еда (((рыба|мясо){2}))", "еда рыба мясо", true, null, 3, },
                new object?[] { "еда (((рыба|мясо){2} (рыба|мясо){0,2}))", "еда рыба мясо мясо рыба", true, null, 5, },
                new object?[] { "еда (((рыба|мясо){2}){2}){2}", "еда рыба мясо мясо рыба мясо мясо мясо мясо", true, null, 9, },
                new object?[] { "Привет (тебя|меня) зовут ((черная|белая) (пантера|акула))", "Привет меня зовут черная пантера", true, null, 5, },
                new object?[] { "Песня такая пам{3} (пам пам){2} (пам пам пам) (пам){2}", "Песня такая пам пам пам пам пам пам пам пам пам пам пам пам", true, null, 14, },
                new object?[] { "Песня такая пам{3} (пам пам){2} (пам пам пам) (пам){2}", "Песня такая пам пам пам пам пам пам пам пам пам пам пам", false, null, null, },
                new object?[] { "Песня такая пам{3} (пам пам){2} (пам пам пам) (пам){2}", "Песня такая пам пам пам пам пам пам пам пам пам пам пам пам пам", true, 14, 14, },
                new object?[] { "(тут группа (и еще [группа набор]){0,3}){1,2}", "тут группа и еще набор тут группа и еще группа", true, null, 10, },
                new object?[] { "a b (c d [e f g]{1,} (h|k){2} ((g|b){2,3} (c d){0,1}){0,1}){1}", "a b c d e g k h g b b", true, null, 11, },
                new object?[] { "a b (c d [e f g]{1,} (h|k){2} ((g|b){2,3} (c d){0,1}){0,1}){1}", "a b c d e g k h g b b g", true, 11, 11, },
                new object?[] { "a b (c d [e f g]{1,} (h|k){2} ((g|b){2,3} (c d){0,1}){0,1}){1}", "a b c d e g k k h g b b", true, 8, 8, },
            };
        }

        public static object?[][] TokenizesAndMatchesAndProjects_WeirdCases()
        {
            return new []
            {
                new object?[] { "(один+)?", "", true, null, 0, },
                new object?[] { "(один+)?", "один", true, null, 1, },
                new object?[] { "(один+)?", "один один", true, null, 2, },
                new object?[] { "(один+)?", "один один два", true, 2, 2, },
                new object?[] { "(один+)+", "", false, null, null, },
                new object?[] { "(один+)+", "один", true, null, 1, },
                new object?[] { "(один+)+", "один один", true, null, 2, },
                new object?[] { "(один+)+", "один один два", true, 2, 2, },
                new object?[] { "(один+)*", "", true, null, 0, },
                new object?[] { "(один+)*", "один", true, null, 1, },
                new object?[] { "(один+)*", "один один", true, null, 2, },
                new object?[] { "(один+)*", "один один два", true, 2, 2, },
                new object?[] { "(один+){2,}", "", false, null, null, },
                new object?[] { "(один+){2,}", "один", false, null, null, },
                new object?[] { "(один+){2,}", "один один", true, null, 2, },
                new object?[] { "(один+){2,}", "один один один", true, null, 3, },
                new object?[] { "(один+){2,}", "один один два", true, 2, 2, },
                new object?[] { "(один*)?", "", true, null, 0, },
                new object?[] { "(один*)?", "один", true, null, 1, },
                new object?[] { "(один*)?", "один один", true, null, 2, },
                new object?[] { "(один*)?", "один один один два", true, 3, 3, },
                new object?[] { "(один?)?", "", true, null, 0, },
                new object?[] { "(один?)?", "один", true, null, 1, },
                new object?[] { "(один?)?", "один один", true, 1, 1, },
                new object?[] { "(один?)?", "один один два", true, 1, 1, },
                new object?[] { "(один{3,})?", "", true, null, 0, },
                new object?[] { "(один{3,})?", "один", true, 0, 0, },
                new object?[] { "(один{3,})?", "один один", true, 0, 0, },
                new object?[] { "(один{3,})?", "один один один", true, null, 3, },
                new object?[] { "(один{3,})?", "один один один один", true, null, 4, },
                new object?[] { "(один{3,})?", "один один один один два", true, 4, 4, },
                new object?[] { "(один{3,})+", "", false, null, 0, },
                new object?[] { "(один{3,})+", "один", false, 0, 0, },
                new object?[] { "(один{3,})+", "один один", false, 0, 0, },
                new object?[] { "(один{3,})+", "один один один", true, null, 3, },
                new object?[] { "(один{3,})+", "один один один один", true, null, 4, },
                new object?[] { "(один{3,})+", "один один один два", true, 3, 3, },
                new object?[] { "(один{3,})*", "", true, null, 0, },
                new object?[] { "(один{3,})*", "один", true, 0, 0, },
                new object?[] { "(один{3,})*", "один один", true, 0, 0, },
                new object?[] { "(один{3,})*", "один один один", true, null, 3, },
                new object?[] { "(один{3,})*", "один один один один", true, null, 4, },
                new object?[] { "(один{3,})*", "один один один два", true, 3, 3, },
                new object?[] { "(один{3,}){2,}", "", false, null, null, },
                new object?[] { "(один{3,}){2,}", "один", false, null, null, },
                new object?[] { "(один{3,}){2,}", "один один", false, null, null, },
                new object?[] { "(один{3,}){2,}", "один один один", false, null, null, },
                new object?[] { "(один{3,}){2,}", "один один один один", false, null, null, },
                new object?[] { "(один{3,}){2,}", "один один один один один", false, null, null, },
                new object?[] { "(один{3,}){2,}", "один один один один один один", true, null, 6, },
                new object?[] { "(один{3,}){2,}", "один один один один один один один", true, null, 7, },
                new object?[] { "(один{3,}){2,}", "один один один один один один один один", true, null, 8, },
                new object?[] { "(один{3,}){2,}", "один один один один один один один один два", true, 8, 8, },
            };
        }

        #endregion

        #region Sources_TokenizesAndMatchesAndProjectsWithStartIndex

        public static object?[][] TokenizesAndMatchesAndProjectsWithStartIndex_Literal()
        {
            return new []
            {
                new object?[]
                {
                    "(привет)",
                    "пока привет",
                    1,
                    true,
                    null,
                    1,
                },
                new object?[]
                {
                    "(привет)",
                    "пока привет пока",
                    1,
                    true,
                    2,
                    1,
                },
                new object?[]
                {
                    "(привет)",
                    "пока привет привет пока",
                    1,
                    true,
                    2,
                    1,
                },
                new object?[]
                {
                    "(привет)",
                    "пока привет привет пока",
                    2,
                    true,
                    3,
                    1,
                },
                new object?[]
                {
                    "(привет)",
                    "пока ало привет пока",
                    1,
                    false,
                    null,
                    1,
                },
            };
        }

        public static object?[][] TokenizesAndMatchesAndProjectsWithStartIndex_RepetitionOfLiteral()
        {
            return new []
            {
                new object?[]
                {
                    "(привет+)",
                    "пока пока",
                    1,
                    false,
                    null,
                    null,
                },
                new object?[]
                {
                    "(привет+)",
                    "пока привет",
                    1,
                    true,
                    null,
                    1,
                },
                new object?[]
                {
                    "(привет+)",
                    "пока привет пока",
                    1,
                    true,
                    2,
                    1,
                },
                new object?[]
                {
                    "(привет+)",
                    "пока привет привет",
                    1,
                    true,
                    null,
                    2,
                },
                new object?[]
                {
                    "(привет+)",
                    "пока привет привет привет пока",
                    1,
                    true,
                    4,
                    3,
                },
            };
        }

        #endregion

        #region ExtractsVariables

        public static object?[][] ExtractsVariables_Ners()
        {
            return new[]
            {
                new object?[]
                {
                    "(<var1 = dummy_ner.hi()>|<var2 = dummy_ner.bye()>)",
                    "пока",
                    new Dictionary<string, object?>()
                    {
                        {"var2", "прощание"},
                    }
                },
                new object?[]
                {
                    "(<var1 = dummy_ner.hi()>|<var2 = dummy_ner.bye()>)",
                    "привет",
                    new Dictionary<string, object?>()
                    {
                        {"var1", "приветствие"},
                    }
                },
                new object?[]
                {
                    "(два три? <var1 = dummy_ner.three_with_optional_four()> четыре?)",
                    "два три четыре",
                    new Dictionary<string, object?>()
                    {
                        {"var1", new []{3, 4}},
                    },
                },
                new object?[]
                {
                    "(два три? четыре? <var1 = dummy_ner.three_with_optional_four()>)",
                    "два три четыре",
                    new Dictionary<string, object?>()
                    {
                        {"var1", new []{3, 4}},
                    },
                },
                new object?[]
                {
                    "(два <var1 = dummy_ner.three_with_optional_four()> три? четыре?)",
                    "два три четыре",
                    new Dictionary<string, object?>()
                    {
                        {"var1", new []{3, 4}},
                    },
                },
                new object?[]
                {
                    "(один два? <var1 = dummy_ner.two()> три)",
                    "один два три",
                    new Dictionary<string, object?>()
                    {
                        {"var1", 2},
                    },
                },
                new object?[]
                {
                    "(один <var1 = dummy_ner.two()> два? три)",
                    "один два три",
                    new Dictionary<string, object?>()
                    {
                        {"var1", 2},
                    },
                },
            };
        }

        #endregion

        #region ExtractsMultipleResults

        public static object?[][] ExtractsMultipleResults_WithNers()
        {
            return new[]
            {
                new object?[]
                {
                    "(один (два|<var1 = dummy_ner.two()>) три)",
                    "один два три",
                    new []
                    {
                        new Dictionary<string, object?>()
                        {
                            { "var1", 2 },
                        },
                        new Dictionary<string, object?>(),
                    }
                },
                new object?[]
                {
                    "(один (<var1 = dummy_ner.two()>|два) три)",
                    "один два три",
                    new []
                    {
                        new Dictionary<string, object?>()
                        {
                            { "var1", 2 },
                        },
                        new Dictionary<string, object?>(),
                    }
                },
            };
        }

        #endregion

        #region HandlesRuleReferences

        public static object?[][] HandlesRuleReferences_Mixed()
        {
            return new[]
            {
                new object?[]
                {
                    new Dictionary<string, string>()
                    {
                        {"main", "(<ref1> <ref2>)"},
                        {"ref1", "(<var1 = dummy_ner.hi()>)"},
                        {"ref2", "(<var1 = dummy_ner.hi()>)"},
                    },
                    "main",
                    "привет привет",
                    new Dictionary<string, object?>()
                    {
                        {"var1", "приветствие"},
                    }
                },
            };
        }

        #endregion

        #endregion
    }
}