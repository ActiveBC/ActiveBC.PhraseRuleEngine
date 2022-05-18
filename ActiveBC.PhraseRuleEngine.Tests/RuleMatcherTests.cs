using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result.SelectionStrategy;
using ActiveBC.PhraseRuleEngine.Exceptions;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Tests.Fixtures;
using ActiveBC.PhraseRuleEngine.Tests.Helpers;
using NUnit.Framework;

namespace ActiveBC.PhraseRuleEngine.Tests
{
    internal sealed class RuleMatcherTests
    {
        [Test]
        [TestCaseSource(nameof(Extracts_TupleType))]
        [TestCaseSource(nameof(Extracts_AnyLiteral))]
        [TestCaseSource(nameof(Extracts_Literal))]
        [TestCaseSource(nameof(Extracts_Prefix))]
        [TestCaseSource(nameof(Extracts_Infix))]
        [TestCaseSource(nameof(Extracts_Suffix))]
        [TestCaseSource(nameof(Extracts_OptionalLiteral_ReferenceType))]
        [TestCaseSource(nameof(Extracts_OptionalLiteral_ValueType))]
        [TestCaseSource(nameof(Extracts_RepetitionOfLiteral_ReferenceType))]
        [TestCaseSource(nameof(Extracts_RepetitionOfLiteral_ValueType))]
        [TestCaseSource(nameof(Extracts_BranchesWithRepetitionOfLiteral_ReferenceType))]
        [TestCaseSource(nameof(Extracts_BranchesWithRepetitionOfLiteral_ValueType))]
        [TestCaseSource(nameof(Extracts_BranchesWithOptionalLiteral_ReferenceType))]
        [TestCaseSource(nameof(Extracts_BranchesWithOptionalLiteral_ValueType))]
        [TestCaseSource(nameof(Extracts_Numbers0To9))]
        [TestCaseSource(nameof(Extracts_Digits0To9))]
        [TestCaseSource(nameof(Extracts_RelativeTimeSpan))]
        [TestCaseSource(nameof(Extracts_Doctors))]
        public void Extracts(
            RuleSetContainer ruleSet,
            string ruleName,
            string phrase,
            object? expectedResult,
            int? expectedLastUsedSymbolIndex
        )
        {
            RuleInput ruleInput = new RuleInput(
                phrase.Split(' ', StringSplitOptions.RemoveEmptyEntries),
                new RuleSpaceArguments(ImmutableDictionary<string, object?>.Empty)
            );

            RuleMatchResultCollection matchResults = ruleSet.RuleSpace[ruleName].MatchAndProject(
                ruleInput,
                0,
                new RuleArguments(ImmutableDictionary<string, object?>.Empty),
                new RuleSpaceCache()
            );

            RuleMatchResult? matchResult = matchResults.Best(new MaxExplicitSymbolsStrategy());

            Assert.IsNotNull(matchResult);

            Assert.AreEqual(expectedLastUsedSymbolIndex ?? ruleInput.Sequence.Length - 1, matchResult!.LastUsedSymbolIndex);

            if (expectedResult is IEnumerable expectedEnumerable)
            {
                Assert.IsInstanceOf<IEnumerable>(matchResult.Result.Value);
                CollectionAssert.AreEqual(expectedEnumerable, (IEnumerable) matchResult.Result.Value!);
            }
            else
            {
                if (expectedResult is not null)
                {
                    Assert.IsNotNull(matchResult.Result.Value);
                    Assert.AreEqual(expectedResult.GetType(), matchResult.Result.Value!.GetType());
                }

                Assert.AreEqual(expectedResult, matchResult.Result.Value);
            }
        }

        [Test]
        [TestCaseSource(nameof(AcceptsRuleArguments_Mixed))]
        public void AcceptsRuleArguments(
            RuleSetContainer ruleSet,
            string ruleName,
            string phrase,
            IReadOnlyDictionary<string, object?> ruleArguments,
            object expectedResult
        )
        {
            RuleInput ruleInput = new RuleInput(
                phrase.Split(' ', StringSplitOptions.RemoveEmptyEntries),
                new RuleSpaceArguments(
                    new Dictionary<string, object?>()
                    {
                        {"parameters", new AcceptsRuleArgumentsParameters("фу", 24)}
                    }
                )
            );

            RuleMatchResultCollection matchResults = ruleSet.RuleSpace[ruleName].MatchAndProject(
                ruleInput,
                0,
                new RuleArguments(ruleArguments),
                new RuleSpaceCache()
            );

            RuleMatchResult? matchResult = matchResults.Best(new MaxExplicitSymbolsStrategy());

            Assert.True(matchResult is not null);

            Assert.AreEqual(ruleInput.Sequence.Length - 1, matchResult!.LastUsedSymbolIndex);

            Assert.AreSame(expectedResult.GetType(), matchResult.Result.Value?.GetType() ?? typeof(object));
            Assert.AreEqual(expectedResult, matchResult.Result.Value);
        }

        [Test]
        [TestCaseSource(nameof(CapturesVariables_Mixed))]
        public void CapturesVariables(
            RuleSetContainer ruleSet,
            string ruleName,
            string phrase,
            IReadOnlyDictionary<string, object?> expectedCapturedVariables
        )
        {
            RuleInput ruleInput = new RuleInput(
                phrase.Split(' '),
                new RuleSpaceArguments(ImmutableDictionary<string, object?>.Empty)
            );

            RuleMatchResultCollection matchResults = ruleSet.RuleSpace[ruleName].MatchAndProject(
                ruleInput,
                0,
                new RuleArguments(ImmutableDictionary<string, object?>.Empty),
                new RuleSpaceCache()
            );

            RuleMatchResult? matchResult = matchResults.Best(new MaxExplicitSymbolsStrategy());

            Assert.True(matchResult is not null);

            Assert.AreEqual(ruleInput.Sequence.Length - 1, matchResult!.LastUsedSymbolIndex);

            CollectionAssert.AreEquivalent(expectedCapturedVariables, matchResult.CapturedVariables);
        }

        [Test]
        [TestCaseSource(nameof(Fails_DuplicateRule))]
        public void Fails(RuleSetToken ruleSetToken, string expectedExceptionMessage)
        {
            RuleBuildException? exception = Assert.Throws<RuleBuildException>(
                () => new RuleSpaceFactory(
                    new []
                    {
                        NerEnvironment.Mechanics.Peg,
                    }
                ).CreateWithAliases(
                    new[] { ruleSetToken },
                    Array.Empty<RuleToken>(),
                    ImmutableDictionary<string, IRuleMatcher>.Empty,
                    ImmutableDictionary<string, IRuleSpace>.Empty,
                    ImmutableDictionary<string, Type>.Empty,
                    new LoadedAssembliesProvider()
                )
            );

            Assert.AreEqual(expectedExceptionMessage, exception!.Message);
        }

        #region Sources

        #region Sources_Extracts

        public static object?[][] Extracts_TupleType()
        {
            return new []
            {
                new object?[]
                {
                    TupleTypeNer.Instance,
                    "TupleType",
                    "привет",
                    ((6, TimeSpan.FromSeconds(6)), "привет"),
                    null,
                },
                new object?[]
                {
                    TupleTypeNer.Instance,
                    "TupleType",
                    "пока",
                    ((4, TimeSpan.FromSeconds(4)), "пока"),
                    null,
                },
            };
        }

        public static object?[][] Extracts_AnyLiteral()
        {
            return new []
            {
                new object?[]
                {
                    AnyLiteralNer.Instance,
                    "ReferenceType_AnyLiteral",
                    "привет",
                    "привет",
                    null,
                },
                new object?[]
                {
                    AnyLiteralNer.Instance,
                    "ReferenceType_AnyLiteral",
                    "пока",
                    "пока",
                    null,
                },
            };
        }

        public static object?[][] Extracts_Literal()
        {
            return new []
            {
                new object?[]
                {
                    LiteralNer.Instance,
                    "ReferenceType_Literal",
                    "привет",
                    "привет",
                    null,
                },
            };
        }

        public static object?[][] Extracts_Prefix()
        {
            return new []
            {
                new object?[]
                {
                    PrefixNer.Instance,
                    "ReferenceType_Prefix",
                    "привет",
                    "привет",
                    null,
                },
                new object?[]
                {
                    PrefixNer.Instance,
                    "ReferenceType_Prefix",
                    "приветики",
                    "приветики",
                    null,
                },
                new object?[]
                {
                    PrefixNer.Instance,
                    "ReferenceType_Prefix",
                    "приветули",
                    "приветули",
                    null,
                },
            };
        }

        public static object?[][] Extracts_Infix()
        {
            return new []
            {
                new object?[]
                {
                    InfixNer.Instance,
                    "ReferenceType_Infix",
                    "ству",
                    "ству",
                    null,
                },
                new object?[]
                {
                    InfixNer.Instance,
                    "ReferenceType_Infix",
                    "ствую",
                    "ствую",
                    null,
                },
                new object?[]
                {
                    InfixNer.Instance,
                    "ReferenceType_Infix",
                    "приветству",
                    "приветству",
                    null,
                },
                new object?[]
                {
                    InfixNer.Instance,
                    "ReferenceType_Infix",
                    "приветствую",
                    "приветствую",
                    null,
                },
            };
        }

        public static object?[][] Extracts_Suffix()
        {
            return new []
            {
                new object?[]
                {
                    SuffixNer.Instance,
                    "ReferenceType_Suffix",
                    "ствую",
                    "ствую",
                    null,
                },
                new object?[]
                {
                    SuffixNer.Instance,
                    "ReferenceType_Suffix",
                    "приветствую",
                    "приветствую",
                    null,
                },
                new object?[]
                {
                    SuffixNer.Instance,
                    "ReferenceType_Suffix",
                    "здравствую",
                    "здравствую",
                    null,
                },
            };
        }

        public static object?[][] Extracts_OptionalLiteral_ReferenceType()
        {
            return new []
            {
                new object?[]
                {
                    OptionalLiteralReferenceTypeNer.Instance,
                    "ReferenceType_OptionalLiteral",
                    "",
                    null,
                    null,
                },
                new object?[]
                {
                    OptionalLiteralReferenceTypeNer.Instance,
                    "ReferenceType_OptionalLiteral",
                    "привет",
                    "привет",
                    null,
                },
            };
        }

        public static object?[][] Extracts_OptionalLiteral_ValueType()
        {
            return new []
            {
                new object?[]
                {
                    OptionalLiteralValueTypeNer.Instance,
                    "ValueType_OptionalLiteral",
                    "",
                    -100,
                    null,
                },
                new object?[]
                {
                    OptionalLiteralValueTypeNer.Instance,
                    "ValueType_OptionalLiteral",
                    "один",
                    100,
                    null,
                },
            };
        }

        public static object?[][] Extracts_RepetitionOfLiteral_ReferenceType()
        {
            return new []
            {
                new object?[]
                {
                    RepetitionOfLiteralReferenceTypeNer.Instance,
                    "ReferenceType_RepetitionOfLiteral",
                    "",
                    Array.Empty<string>(),
                    null,
                },
                new object?[]
                {
                    RepetitionOfLiteralReferenceTypeNer.Instance,
                    "ReferenceType_RepetitionOfLiteral",
                    "привет",
                    new []{"привет"},
                    null,
                },
                new object?[]
                {
                    RepetitionOfLiteralReferenceTypeNer.Instance,
                    "ReferenceType_RepetitionOfLiteral",
                    "привет привет привет",
                    new []{"привет", "привет", "привет"},
                    null,
                },
            };
        }

        public static object?[][] Extracts_RepetitionOfLiteral_ValueType()
        {
            return new []
            {
                new object?[]
                {
                    RepetitionOfLiteralValueTypeNer.Instance,
                    "ValueType_RepetitionOfLiteral",
                    "",
                    Array.Empty<string>(),
                    null,
                },
                new object?[]
                {
                    RepetitionOfLiteralValueTypeNer.Instance,
                    "ValueType_RepetitionOfLiteral",
                    "один",
                    new []{1},
                    null,
                },
                new object?[]
                {
                    RepetitionOfLiteralValueTypeNer.Instance,
                    "ValueType_RepetitionOfLiteral",
                    "один один один",
                    new []{1, 1, 1},
                    null,
                },
            };
        }

        public static object?[][] Extracts_BranchesWithRepetitionOfLiteral_ReferenceType()
        {
            return new []
            {
                new object?[]
                {
                    BranchesWithRepetitionOfLiteralReferenceTypeNer.Instance,
                    "ReferenceType_BranchesWithRepetitionOfLiteral",
                    "привет",
                    new []{"привет"},
                    null,
                },
                new object?[]
                {
                    BranchesWithRepetitionOfLiteralReferenceTypeNer.Instance,
                    "ReferenceType_BranchesWithRepetitionOfLiteral",
                    "привет привет привет",
                    new []{"привет", "привет", "привет"},
                    null,
                },
                new object?[]
                {
                    BranchesWithRepetitionOfLiteralReferenceTypeNer.Instance,
                    "ReferenceType_BranchesWithRepetitionOfLiteral",
                    "пока",
                    new []{"пока"},
                    null,
                },
                new object?[]
                {
                    BranchesWithRepetitionOfLiteralReferenceTypeNer.Instance,
                    "ReferenceType_BranchesWithRepetitionOfLiteral",
                    "пока пока пока",
                    new []{"пока", "пока", "пока"},
                    null,
                },
            };
        }

        public static object?[][] Extracts_BranchesWithRepetitionOfLiteral_ValueType()
        {
            return new []
            {
                new object?[]
                {
                    BranchesWithRepetitionOfLiteralValueTypeNer.Instance,
                    "ValueType_BranchesWithRepetitionOfLiteral",
                    "один",
                    new []{1},
                    null,
                },
                new object?[]
                {
                    BranchesWithRepetitionOfLiteralValueTypeNer.Instance,
                    "ValueType_BranchesWithRepetitionOfLiteral",
                    "один один один",
                    new []{1, 1, 1},
                    null,
                },
                new object?[]
                {
                    BranchesWithRepetitionOfLiteralValueTypeNer.Instance,
                    "ValueType_BranchesWithRepetitionOfLiteral",
                    "два",
                    new []{2},
                    null,
                },
                new object?[]
                {
                    BranchesWithRepetitionOfLiteralValueTypeNer.Instance,
                    "ValueType_BranchesWithRepetitionOfLiteral",
                    "два два два",
                    new []{2, 2, 2},
                    null,
                },
            };
        }

        public static object?[][] Extracts_BranchesWithOptionalLiteral_ReferenceType()
        {
            return new []
            {
                new object?[]
                {
                    BranchesWithOptionalLiteralReferenceTypeNer.Instance,
                    "ReferenceType_BranchesWithOptionalLiteral",
                    "старт финиш",
                    null,
                    null,
                },
                new object?[]
                {
                    BranchesWithOptionalLiteralReferenceTypeNer.Instance,
                    "ReferenceType_BranchesWithOptionalLiteral",
                    "старт привет финиш",
                    "привет",
                    null,
                },
                new object?[]
                {
                    BranchesWithOptionalLiteralReferenceTypeNer.Instance,
                    "ReferenceType_BranchesWithOptionalLiteral",
                    "старт пока финиш",
                    "пока",
                    null,
                },
            };
        }

        public static object?[][] Extracts_BranchesWithOptionalLiteral_ValueType()
        {
            return new []
            {
                new object?[]
                {
                    BranchesWithOptionalLiteralValueTypeNer.Instance,
                    "ValueType_BranchesWithOptionalLiteral",
                    "старт финиш",
                    null,
                    null,
                },
                new object?[]
                {
                    BranchesWithOptionalLiteralValueTypeNer.Instance,
                    "ValueType_BranchesWithOptionalLiteral",
                    "старт один финиш",
                    1,
                    null,
                },
                new object?[]
                {
                    BranchesWithOptionalLiteralValueTypeNer.Instance,
                    "ValueType_BranchesWithOptionalLiteral",
                    "старт два финиш",
                    2,
                    null,
                },
            };
        }

        public static object?[][] Extracts_Numbers0To9()
        {
            return new []
            {
                new object?[]
                {
                    Numbers0To9Ner.Instance,
                    "Number_7",
                    "семь",
                    7,
                    null,
                },
                new object?[]
                {
                    Numbers0To9Ner.Instance,
                    "Number",
                    "ноль",
                    0,
                    null,
                },
                new object?[]
                {
                    Numbers0To9Ner.Instance,
                    "Number",
                    "один",
                    1,
                    null,
                },
                new object?[]
                {
                    Numbers0To9Ner.Instance,
                    "Number",
                    "два",
                    2,
                    null,
                },
                new object?[]
                {
                    Numbers0To9Ner.Instance,
                    "Number",
                    "три",
                    3,
                    null,
                },
                new object?[]
                {
                    Numbers0To9Ner.Instance,
                    "Number",
                    "четыре",
                    4,
                    null,
                },
                new object?[]
                {
                    Numbers0To9Ner.Instance,
                    "Number",
                    "пять",
                    5,
                    null,
                },
                new object?[]
                {
                    Numbers0To9Ner.Instance,
                    "Number",
                    "шесть",
                    6,
                    null,
                },
                new object?[]
                {
                    Numbers0To9Ner.Instance,
                    "Number",
                    "семь",
                    7,
                    null,
                },
                new object?[]
                {
                    Numbers0To9Ner.Instance,
                    "Number",
                    "восемь",
                    8,
                    null,
                },
                new object?[]
                {
                    Numbers0To9Ner.Instance,
                    "Number",
                    "девять",
                    9,
                    null,
                },
            };
        }

        public static object?[][] Extracts_Digits0To9()
        {
            return new [] {
                new object?[] {Digits0To9Ner.Instance, "Number_7", "семь", "7", null,},
                new object?[] {Digits0To9Ner.Instance, "Number", "ноль", "0", null,},
                new object?[] {Digits0To9Ner.Instance, "Number", "один", "1", null,},
                new object?[] {Digits0To9Ner.Instance, "Number", "два", "2", null,},
                new object?[] {Digits0To9Ner.Instance, "Number", "три", "3", null,},
                new object?[] {Digits0To9Ner.Instance, "Number", "четыре", "4", null,},
                new object?[] {Digits0To9Ner.Instance, "Number", "пять", "5", null,},
                new object?[] {Digits0To9Ner.Instance, "Number", "шесть", "6", null,},
                new object?[] {Digits0To9Ner.Instance, "Number", "семь", "7", null,},
                new object?[] {Digits0To9Ner.Instance, "Number", "восемь", "8", null,},
                new object?[] {Digits0To9Ner.Instance, "Number", "девять", "9", null,},
            };
        }

        public static object?[][] Extracts_RelativeTimeSpan()
        {
            return new []
            {
                // HoursExact
                // HoursNumber_HalfAnHour
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "за полчаса",
                    -TimeSpan.FromMinutes(30),
                    null,
                },
                // HoursExact_WithHalf
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "через пол часа",
                    TimeSpan.FromMinutes(30),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "за полтора часа",
                    -TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(30)),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "через один с половиной час",
                    TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(30)),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "за два с половиной часа",
                    -TimeSpan.FromHours(2).Add(TimeSpan.FromMinutes(30)),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "через пять с половиной часов",
                    TimeSpan.FromHours(5).Add(TimeSpan.FromMinutes(30)),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "за девять с половиной часов",
                    -TimeSpan.FromHours(9).Add(TimeSpan.FromMinutes(30)),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "через двенадцать с половиной часов",
                    TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(30)),
                    null,
                },
                // HoursAndMinutes_WithWords
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "за час ноль две минуты",
                    -TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(2)),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "через один час ноль девять минут",
                    TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(9)),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "за два часа пятнадцать минут",
                    -TimeSpan.FromHours(2).Add(TimeSpan.FromMinutes(15)),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "через три часа двадцать минут",
                    TimeSpan.FromHours(3).Add(TimeSpan.FromMinutes(20)),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "за пять часов сорок одну минута",
                    -TimeSpan.FromHours(5).Add(TimeSpan.FromMinutes(41)),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "через двенадцать часов пятьдесят девять минут",
                    TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(59)),
                    null,
                },
                // HoursAndMinutes_WithoutWords
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "за час ноль пять",
                    -TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(5)),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "через два ноль две",
                    TimeSpan.FromHours(2).Add(TimeSpan.FromMinutes(2)),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "за пять пятнадцать",
                    -TimeSpan.FromHours(5).Add(TimeSpan.FromMinutes(15)),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "через семь тридцать пять",
                    TimeSpan.FromHours(7).Add(TimeSpan.FromMinutes(35)),
                    null,
                },
                // HoursOnly_WithWord
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "за час",
                    -TimeSpan.FromHours(1),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "через два часа",
                    TimeSpan.FromHours(2),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "за пять часов",
                    -TimeSpan.FromHours(5),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "через двенадцать часов",
                    TimeSpan.FromHours(12),
                    null,
                },
                // MinutesOnly_WithWord
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "через две минуты",
                    TimeSpan.FromMinutes(2),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "за пять минут",
                    -TimeSpan.FromMinutes(5),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "через тридцать минут",
                    TimeSpan.FromMinutes(30),
                    null,
                },
                new object?[]
                {
                    RelativeTimeSpanNer.Instance,
                    "Expression",
                    "за пятьдесят пять минут",
                    -TimeSpan.FromMinutes(55),
                    null,
                },
            };
        }

        public static object?[][] Extracts_Doctors()
        {
            return new []
            {
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "терапевт",
                    "терапевту",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "терапевту",
                    "терапевту",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "хирург",
                    "хирургу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "хирургу",
                    "хирургу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "окулист",
                    "офтальмологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "окулисту",
                    "офтальмологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "глазной",
                    "офтальмологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "глазному",
                    "офтальмологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "офтальмолог",
                    "офтальмологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "офтальмологу",
                    "офтальмологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "отоларинголог",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "отоларингологу",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "лор",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "лору",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "ухогорлонос",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "ухогорлоносу",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "горлонос",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "горлоносу",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "горлунос",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "горлуносу",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "ухо горло нос",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "ухо горло носу",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "горлу носу",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "горлу нос",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "горло носу",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "горло нос",
                    "отоларингологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "уролог",
                    "урологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "урологу",
                    "урологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гинеколог",
                    "гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гинекологу",
                    "гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "женский",
                    "гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "женскому",
                    "гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "женщина гинеколог",
                    "женщине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "женщине гинекологу",
                    "женщине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гинеколог женщина",
                    "женщине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гинекологу женщине",
                    "женщине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "женщина-гинеколог",
                    "женщине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "женщине-гинекологу",
                    "женщине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гинеколог-женщина",
                    "женщине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гинекологу-женщине",
                    "женщине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "мужчина гинеколог",
                    "мужчине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "мужчине гинекологу",
                    "мужчине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гинеколог мужчина",
                    "мужчине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гинекологу мужчине",
                    "мужчине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "мужчина-гинеколог",
                    "мужчине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "мужчине-гинекологу",
                    "мужчине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гинеколог-мужчина",
                    "мужчине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гинекологу-мужчине",
                    "мужчине гинекологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "невролог",
                    "неврологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "неврологу",
                    "неврологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "травматолог",
                    "травматологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "травматологу",
                    "травматологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "кожный",
                    "дерматологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "кожному",
                    "дерматологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "кожник",
                    "дерматологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "кожнику",
                    "дерматологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гастроэнтеролог",
                    "гастроэнтерологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гастроэнтерологу",
                    "гастроэнтерологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "кардиолог",
                    "кардиологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "кардиологу",
                    "кардиологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "эндокринолог",
                    "эндокринологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "эндокринологу",
                    "эндокринологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматолог",
                    "стоматологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматологу",
                    "стоматологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "дантист",
                    "стоматологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "дантисту",
                    "стоматологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "зубной",
                    "стоматологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "зубному",
                    "стоматологу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "на гигиеническую чистку зубов",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "на гигиеническую очистку зубов",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гигиеническую чистку зубов",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гигиеническую очистку зубов",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "на чистку зубов",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "на очистку зубов",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "чистку зубов",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "очистку зубов",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "на гигиеническую чистку",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "на гигиеническую очистку",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гигиеническая чистка зубов",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гигиеническая очистка зубов",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "чистка зубов",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "очистка зубов",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гигиеническая чистка",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "гигиеническая очистка",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "почистить зубы",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "очистить зубы",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "почистили зубы",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "очистили зубы",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "зубы почистить",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "зубы очистить",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "зубы почистили",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "зубы очистили",
                    "специалисту по гигиенической чистке зубов",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматолог терапевт",
                    "стоматологу-терапевту",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматолог терапевту",
                    "стоматологу-терапевту",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматологу терапевту",
                    "стоматологу-терапевту",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматолог-терапевт",
                    "стоматологу-терапевту",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматолог-терапевту",
                    "стоматологу-терапевту",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматологу-терапевту",
                    "стоматологу-терапевту",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматолог хирург",
                    "стоматологу-хирургу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматолог хирургу",
                    "стоматологу-хирургу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматологу хирургу",
                    "стоматологу-хирургу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматолог-хирург",
                    "стоматологу-хирургу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматолог-хирургу",
                    "стоматологу-хирургу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматологу-хирургу",
                    "стоматологу-хирургу",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "ортодонт",
                    "стоматологу-ортодонту",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматолог ортодонт",
                    "стоматологу-ортодонту",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "ортодонту",
                    "стоматологу-ортодонту",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматолог ортодонту",
                    "стоматологу-ортодонту",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматологу ортодонту",
                    "стоматологу-ортодонту",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматолог-ортодонт",
                    "стоматологу-ортодонту",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматолог-ортодонту",
                    "стоматологу-ортодонту",
                    null,
                },
                new object?[]
                {
                    DoctorsNer.Instance,
                    "Doctor",
                    "стоматологу-ортодонту",
                    "стоматологу-ортодонту",
                    null,
                },
            };
        }

        #endregion

        #region Sources_CapturesVariables

        public static object?[][] CapturesVariables_Mixed()
        {
            return new []
            {
                new object?[]
                {
                    VoidPatternsNer.Instance,
                    "Rule1_3",
                    "один два три",
                    new Dictionary<string, object?>()
                    {
                        {"one", "один"},
                    },
                },
                new object?[]
                {
                    VoidPatternsNer.Instance,
                    "Rule4_6",
                    "четыре пять шесть",
                    new Dictionary<string, object?>()
                    {
                        {"two", "пять"},
                    },
                },
                new object?[]
                {
                    VoidPatternsNer.Instance,
                    "Rule7_9",
                    "семь восемь девять",
                    new Dictionary<string, object?>()
                    {
                        {"three", "девять"},
                    },
                },
                new object?[]
                {
                    VoidPatternsNer.Instance,
                    "Rule_Foo",
                    "число два больше чем число четыре",
                    new Dictionary<string, object?>()
                    {
                        {"n1", 2},
                        {"n2", 4},
                    },
                },
                new object?[]
                {
                    VoidPatternsNer.Instance,
                    "Rule_Foo",
                    "число три больше чем число три",
                    new Dictionary<string, object?>()
                    {
                        {"n1", 3},
                        {"n2", 3},
                    },
                },
                new object?[]
                {
                    VoidPatternsNer.Instance,
                    "Rule_Bar",
                    "число два больше чем число четыре",
                    new Dictionary<string, object?>()
                    {
                        {"n1", 2},
                        {"n2", 4},
                    },
                },
                new object?[]
                {
                    VoidPatternsNer.Instance,
                    "Rule_Bar",
                    "число три больше чем число три",
                    new Dictionary<string, object?>()
                    {
                        {"n1", 3},
                        {"n2", 3},
                    },
                },
                new object?[]
                {
                    VoidPatternsNer.Instance,
                    "Rule_Baz",
                    "число два больше чем число четыре",
                    new Dictionary<string, object?>()
                    {
                        {"n1", 2},
                        {"n2", 4},
                    },
                },
                new object?[]
                {
                    VoidPatternsNer.Instance,
                    "Rule_Baz",
                    "число два меньше чем число четыре",
                    new Dictionary<string, object?>()
                    {
                        {"n3", 2},
                        {"n4", 4},
                    },
                },
                new object?[]
                {
                    VoidPatternsNer.Instance,
                    "Number",
                    "два",
                    new Dictionary<string, object?>()
                    {
                        {"n_2", 2},
                    },
                },
                new object?[]
                {
                    VoidPatternsNer.Instance,
                    "Number",
                    "три",
                    new Dictionary<string, object?>()
                    {
                        {"n_3", 3},
                    },
                },
                new object?[]
                {
                    VoidPatternsNer.Instance,
                    "Number_0",
                    "ноль",
                    new Dictionary<string, object?>(),
                },
            };
        }

        #endregion

        #region Sources_AcceptsRuleArguments

        public static object?[][] AcceptsRuleArguments_Mixed()
        {
            return new []
            {
                new object?[]
                {
                    RuleArgumentsExampleNer.Instance,
                    "DummyRuleWithArguments",
                    "привет",
                    new Dictionary<string, object>()
                    {
                        {"arg1", "пока"},
                        {"arg2", 42},
                    },
                    "привет_arg1<пока>_arg2<42>",
                },
                new object?[]
                {
                    RuleArgumentsExampleNer.Instance,
                    "DummyRuleWithReferenceToRuleWithArguments1",
                    "куку",
                    new Dictionary<string, object>(),
                    "куку_arg1<фу>_arg2<24>",
                },
                new object?[]
                {
                    RuleArgumentsExampleNer.Instance,
                    "DummyRuleWithReferenceToRuleWithArguments2",
                    "куку",
                    new Dictionary<string, object>(),
                    "куку_arg1<>_arg2<24>",
                },
                new object?[]
                {
                    RuleArgumentsExampleNer.Instance,
                    "DummyRuleWithReferenceToRuleWithArguments3",
                    "куку",
                    new Dictionary<string, object>(),
                    "куку_arg1<фу>_arg2<0>",
                },
                new object?[]
                {
                    RuleArgumentsExampleNer.Instance,
                    "DummyRuleWithReferenceToRuleWithArguments4",
                    "куку",
                    new Dictionary<string, object>(),
                    "куку_arg1<>_arg2<0>",
                },
            };
        }

        #endregion

        #region Sources_Fails

        public static object?[][] Fails_DuplicateRule()
        {
            return new []
            {
                new object?[]
                {
                    new RuleSetToken(
                        Array.Empty<UsingToken>(),
                        new []
                        {
                            new RuleToken(
                                null,
                                new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                                "Foo",
                                Array.Empty<CSharpParameterToken>(),
                                "peg",
                                new PegGroupToken(
                                    new []
                                    {
                                        new BranchToken(
                                            new []
                                            {
                                                new BranchItemToken(
                                                    new LiteralToken("привет"),
                                                    new QuantifierToken(1, 1),
                                                    null,
                                                    null
                                                )
                                            }
                                        ),
                                    }
                                ),
                                VoidProjectionToken.Instance
                            ),
                            new RuleToken(
                                null,
                                new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                                "Foo",
                                Array.Empty<CSharpParameterToken>(),
                                "peg",
                                new PegGroupToken(
                                    new []
                                    {
                                        new BranchToken(
                                            new []
                                            {
                                                new BranchItemToken(
                                                    new LiteralToken("пока"),
                                                    new QuantifierToken(1, 1),
                                                    null,
                                                    null
                                                )
                                            }
                                        ),
                                    }
                                ),
                                VoidProjectionToken.Instance
                            ),
                        }
                    ),
                    "Duplicate rule 'Foo'.",
                },
            };
        }

        #endregion

        #endregion

        #region Models

        public sealed class AcceptsRuleArgumentsParameters
        {
            public string Foo { get; }
            public int Bar { get; }

            public AcceptsRuleArgumentsParameters(string foo, int bar)
            {
                this.Foo = foo;
                this.Bar = bar;
            }
        }

        #endregion
    }
}