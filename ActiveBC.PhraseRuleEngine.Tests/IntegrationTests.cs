using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result.SelectionStrategy;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.Common;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing.Automaton.Optimization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Tests.Fixtures;
using NUnit.Framework;

namespace ActiveBC.PhraseRuleEngine.Tests
{
    [TestFixture]
    internal sealed class IntegrationTests
    {
        private IResultSelectionStrategy? m_bestReferenceSelectionStrategy;
        private RuleSpaceArguments? m_ruleSpaceArguments;

        private IRuleSpace? m_ruleSpace;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.m_bestReferenceSelectionStrategy = new CombinedStrategy(
                new IResultSelectionStrategy[]
                {
                    new MaxExplicitSymbolsStrategy(),
                    new MaxProgressStrategy(),
                }
            );

            this.m_ruleSpaceArguments = new RuleSpaceArguments(
                new Dictionary<string, object?>()
                {
                    {
                        "parameters",
                        new DummyParameters(
                            25,
                            new DummySaltParameters("eto_sol", 3),
                            new DummySaltNullParameters()
                        )
                    },
                    {
                        "parametersDigitsSalt",
                        new DummySaltParameters("eto_sol", 3)
                    },
                    {
                        "rule_space_test_string_argument",
                        "some fancy string"
                    },
                    {
                        "rule_space_test_int_argument",
                        42
                    },
                    {
                        "rule_space_test_enumerable_argument",
                        Enumerable.Empty<object>().Append(420).ToList()
                    },
                }
            );

            StringInterner stringInterner = new StringInterner();
            RuleSpaceFactory factory = new RuleSpaceFactory(
                new []
                {
                    new MechanicsBundle("peg", new LoopBasedPegPatternTokenizer(stringInterner), new PegProcessorFactory(this.m_bestReferenceSelectionStrategy), typeof(PegGroupToken)),
                    new MechanicsBundle("regex", new LoopBasedRegexPatternTokenizer(stringInterner), new RegexProcessorFactory(OptimizationLevel.Max), typeof(RegexGroupToken))
                }
            );

            Dictionary<string, string[]> phrases = new Dictionary<string, string[]>()
            {
                {
                    "Test2",
                    new []
                    {
                        ".* тест .*"
                    }
                },
                {
                    "Hello",
                    new []
                    {
                        "привет",
                        "добрый день",
                        "здравствуй~",
                        "доброго времени суток",
                        "я? приветствую [вас тебя]?",
                    }
                },
                {
                    "GoodBye",
                    new []
                    {
                        "пока",
                        "до свидания",
                        "прощай~",
                        "аривидерчи",
                    }
                },
                {
                    "GoodMorning",
                    new []
                    {
                        "доброе [утро утречко]",
                        "с .{0,3} утром",
                    }
                },
                {
                    "GoodMorning2",
                    new []
                    {
                        "гутен морген",
                    }
                },
                {
                    "GoodEvening",
                    new []
                    {
                        "добрый вечер~",
                        "с добрым [вечером вечерком вечерочком]",
                    }
                },
                {
                    "NumberComparison",
                    new []
                    {
                        "число <a = ner.digit()> больше чем число <b = ner.digit()>",
                    }
                },
                {
                    "NumberComparisonWithGreeting",
                    new []
                    {
                        "<greeting = patterns.Hello()> число <digit = ner.digit()>",
                    }
                },
                {
                    "Preferable",
                    new []
                    {
                        "желательно",
                        "хотелось бы",
                    }
                },
                {
                    "IfPossible",
                    new []
                    {
                        "по возможности",
                    }
                },
                {
                    "Hours",
                    new []
                    {
                        "[час часа часов]",
                    }
                },
                {
                    "Time",
                    new []
                    {
                        "это [было будет] <when = sdn.times()>",
                    }
                },
                {
                    "Maybe",
                    new []
                    {
                        "возможно",
                    }
                },
            };

            Dictionary<string, string> sdn = new Dictionary<string, string>()
            {
                {
                    "times",
                    @"
using ActiveBC.PhraseRuleEngine.Tests.Helpers;

string Root = peg#($Timing:timing $patterns.Maybe?)#
{
    return timing;
}

string Timing = peg#($after:after|$before:before)#
{
    return Pick.OneOf(after, before);
}

string after = peg#(через $ner.digit:hours $patterns.Hours)# { return hours > 2 ? ""не скоро"" : ""скоро""; }

string before = peg#($ner.digit:hours $patterns.Hours назад)# { return hours > 2 ? ""давно"" : ""недавно""; }
"
                },
                {
                    "digit",
                    @"
using ActiveBC.PhraseRuleEngine.Tests.Helpers;

int Root = peg#($Number:n)# { return n; }

int Number = peg#($Number_0:n_0|$Number_1:n_1|$Number_2:n_2|$Number_3:n_3|$Number_4:n_4|$Number_5:n_5|$Number_6:n_6|$Number_7:n_7|$Number_8:n_8|$Number_9:n_9)# { return Pick.OneOf(n_0, n_1, n_2, n_3, n_4, n_5, n_6, n_7, n_8, n_9); }
int Number_0 = peg#(ноль)# { return 0; }
int Number_1 = peg#(один)# { return 1; }
int Number_2 = peg#(два)# { return 2; }
int Number_3 = peg#(три)# { return 3; }
int Number_4 = peg#(четыре)# { return 4; }
int Number_5 = peg#(пять)# { return 5; }
int Number_6 = peg#(шесть)# { return 6; }
int Number_7 = peg#(семь)# { return 7; }
int Number_8 = peg#(восемь)# { return 8; }
int Number_9 = peg#(девять)# { return 9; }"
                },
                {
                    "digits_salted_sum",
                    @"
int Root = peg#($SaltedSum:s)# { return s; }

int SaltedSum = peg#($sdn.digit:a плюс $sdn.digit:b)# { return a + b + parameters.Salt; }
"
                },
                {
                    "rule_space_args_test",
                    @"
string StringValue = peg#(.:word)#
{
    return $""{word}_{rule_space_test_string_argument}_{rule_space_test_int_argument}_{string.Join("" "", rule_space_test_enumerable_argument)}"";
}"
                },
                {
                    "rule_args_test",
                    @"
string PegDefaultArgs = peg#($ner.digit_with_salt:word)#
{
    return word;
}

string PegDefaultRepeat = peg#($ner.digit_with_salt(parameters.DigitsSalt.Salt, default):word)#
{
    return word;
}

string PegDefaultSalt = peg#($ner.digit_with_salt(default, parameters.DigitsSalt.RepeatTimes):word)#
{
    return word;
}

string PegDefinedArgs = peg#($ner.digit_with_salt(parameters.DigitsSalt.Salt, parameters.DigitsSalt.RepeatTimes):word)#
{
    return word;
}

string RegexDefaultArgs = regex#(<word = ner.digit_with_salt(default, default)>)#
{
    return word;
}

string RegexDefaultRepeat = regex#(<word = ner.digit_with_salt(parametersDigitsSalt.Salt, default)>)#
{
    return word;
}

string RegexDefaultSalt = regex#(<word = ner.digit_with_salt(default, parametersDigitsSalt.RepeatTimes)>)#
{
    return word;
}

string RegexDefinedArgs = regex#(<word = ner.digit_with_salt(parametersDigitsSalt.Salt, parametersDigitsSalt.RepeatTimes)>)#
{
    return word;
}"
                },
                {
                    "multivalued_ners",
                    @"
int Foo = peg#($ner.number_line:number)#
{
    return number;
}

"
                },
            };

            IReadOnlyCollection<RuleToken> phraseRulesByName = phrases
                .MapValue(patterns => $"({patterns.Select(pattern => $"({pattern})").JoinToString(" | ")})")
                .MapValue(pattern => factory.PatternTokenizers["regex"].Tokenize(pattern, "patterns", false))
                .MapValue(
                    (ruleName, patternToken) => new RuleToken(
                        "patterns",
                        new ResolvedCSharpTypeToken("string", typeof(string)),
                        ruleName,
                        Array.Empty<CSharpParameterToken>(),
                        "regex",
                        patternToken,
                        MatchedInputBasedProjectionToken.Instance
                    )
                )
                .SelectValues()
                .ToArray();

            IReadOnlyCollection<RuleSetToken> sdnRuleSetsByName = sdn
                .MapValue((ruleSetName, ruleSet) => factory.RuleSetTokenizer.Tokenize(ruleSet, $"sdn.{ruleSetName}", true))
                .SelectValues()
                .ToArray();

            this.m_ruleSpace = factory.CreateWithAliases(
                sdnRuleSetsByName,
                phraseRulesByName,
                new [] { typeof(DummyStaticNerContainer) }
                    .Select(container => factory.StaticRuleFactory.ConvertStaticRuleContainerToRuleMatchers(container))
                    .Merge(),
                new Dictionary<string, IRuleSpace>(),
                this.m_ruleSpaceArguments.Values.MapValue(argument => argument!.GetType()).ToDictionary(),
                new LoadedAssembliesProvider()
            );
        }

        [Test]
        [TestCase("ner.digit", "четыре", true, 4)]
        [TestCase("ner.digit", "ноль", true, 0)]
        [TestCase("ner.digit", "сто", false)]
        [TestCase("patterns.Hello", "здравствуйте", true, "здравствуйте")]
        [TestCase("patterns.Hello", "пока", false)]
        [TestCase("patterns.GoodBye", "до свидания", true, "до свидания")]
        [TestCase("patterns.GoodBye", "привет", false)]
        [TestCase("patterns.GoodMorning", "с невероятно добрым утром", true, "с невероятно добрым утром")]
        [TestCase("patterns.GoodMorning", "с невероятно добрым вечером", false)]
        [TestCase("patterns.GoodMorning2", "гутен морген", true, "гутен морген")]
        [TestCase("patterns.GoodMorning2", "гутен таг", false)]
        [TestCase("patterns.GoodEvening", "с добрым вечерочком", true, "с добрым вечерочком")]
        [TestCase("patterns.GoodEvening", "с добрым утречком", false)]
        [TestCase("patterns.NumberComparison", "число пять больше чем число три", true, "число пять больше чем число три")]
        [TestCase("patterns.NumberComparison", "число пять меньше чем число три", false)]
        [TestCase("patterns.NumberComparisonWithGreeting", "привет число два", true, "привет число два")]
        [TestCase("patterns.NumberComparisonWithGreeting", "пока число два", false)]
        [TestCase("patterns.Preferable", "хотелось бы", true, "хотелось бы")]
        [TestCase("patterns.Preferable", "хотелось бык", false)]
        [TestCase("patterns.IfPossible", "по возможности", true, "по возможности")]
        [TestCase("patterns.IfPossible", "по возможностям", false)]
        [TestCase("patterns.Hours", "час", true, "час")]
        [TestCase("patterns.Hours", "чак", false)]
        [TestCase("patterns.Time", "это будет через два часа", true, "это будет через два часа")]
        [TestCase("patterns.Time", "это будет через день", false)]
        [TestCase("patterns.Time", "это было пять часов назад", true, "это было пять часов назад")]
        [TestCase("patterns.Time", "это было пять дней назад", false)]
        [TestCase("patterns.Maybe", "возможно", true, "возможно")]
        [TestCase("patterns.Maybe", "невозможно", false)]
        [TestCase("patterns.Test2", "тест тест тест", true, "тест тест тест")]
        [TestCase("patterns.Test2", "ох ах тест ах ох", true, "ох ах тест ах ох")]
        [TestCase("patterns.Test2", "ох тест ах", true, "ох тест ах")]
        [TestCase("patterns.Test2", "ох тест", true, "ох тест")]
        [TestCase("patterns.Test2", "тест ах", true, "тест ах")]
        [TestCase("patterns.Test2", "тест", true, "тест")]
        [TestCase("patterns.Test2", "бест", false)]
        [TestCase("sdn.times", "через пять часов", true, "не скоро")]
        [TestCase("sdn.times", "через один час", true, "скоро")]
        [TestCase("sdn.times", "через пять дней", false)]
        [TestCase("sdn.times", "три часа назад", true, "давно")]
        [TestCase("sdn.times", "один час назад", true, "недавно")]
        [TestCase("sdn.times", "через пять дней", false)]
        [TestCase("sdn.times.Root", "через пять часов", true, "не скоро")]
        [TestCase("sdn.times.Root", "через один час", true, "скоро")]
        [TestCase("sdn.times.Root", "через пять дней", false)]
        [TestCase("sdn.times.Root", "три часа назад", true, "давно")]
        [TestCase("sdn.times.Root", "один час назад", true, "недавно")]
        [TestCase("sdn.times.Root", "через пять дней", false)]
        [TestCase("sdn.times.Timing", "через пять часов", true, "не скоро")]
        [TestCase("sdn.times.Timing", "через один час", true, "скоро")]
        [TestCase("sdn.times.Timing", "через пять дней", false)]
        [TestCase("sdn.times.Timing", "три часа назад", true, "давно")]
        [TestCase("sdn.times.Timing", "один час назад", true, "недавно")]
        [TestCase("sdn.times.Timing", "через пять дней", false)]
        [TestCase("sdn.times.after", "через пять часов", true, "не скоро")]
        [TestCase("sdn.times.after", "через один час", true, "скоро")]
        [TestCase("sdn.times.after", "через пять дней", false)]
        [TestCase("sdn.times.before", "три часа назад", true, "давно")]
        [TestCase("sdn.times.before", "один час назад", true, "недавно")]
        [TestCase("sdn.times.before", "три дня назад", false)]
        [TestCase("sdn.digit", "пять", true, 5)]
        [TestCase("sdn.digit", "шесть", true, 6)]
        [TestCase("sdn.digit", "десять", false)]
        [TestCase("sdn.digit.Root", "пять", true, 5)]
        [TestCase("sdn.digit.Root", "шесть", true, 6)]
        [TestCase("sdn.digit.Root", "десять", false)]
        [TestCase("sdn.digit.Number", "пять", true, 5)]
        [TestCase("sdn.digit.Number", "шесть", true, 6)]
        [TestCase("sdn.digit.Number", "десять", false)]
        [TestCase("sdn.digit.Number_5", "пять", true, 5)]
        [TestCase("sdn.digit.Number_5", "шесть", false)]
        [TestCase("sdn.digits_salted_sum", "ноль плюс ноль", true, 25)]
        [TestCase("sdn.digits_salted_sum", "два плюс пять", true, 32)]
        [TestCase("sdn.digits_salted_sum", "два минус пять", false)]
        [TestCase("sdn.digits_salted_sum.Root", "ноль плюс ноль", true, 25)]
        [TestCase("sdn.digits_salted_sum.Root", "два плюс пять", true, 32)]
        [TestCase("sdn.digits_salted_sum.Root", "два минус пять", false)]
        [TestCase("sdn.digits_salted_sum.SaltedSum", "ноль плюс ноль", true, 25)]
        [TestCase("sdn.digits_salted_sum.SaltedSum", "два плюс пять", true, 32)]
        [TestCase("sdn.digits_salted_sum.SaltedSum", "два минус пять", false)]
        [TestCase("sdn.rule_space_args_test.StringValue", "ноль", true, "ноль_some fancy string_42_420")]
        [TestCase("sdn.rule_args_test.PegDefaultArgs", "пять", true, "digit_5_salt_no salt")]
        [TestCase("sdn.rule_args_test.PegDefaultRepeat", "пять", true, "digit_5_salt_eto_sol")]
        [TestCase("sdn.rule_args_test.PegDefaultSalt", "пять", true, "digit_5_salt_no saltno saltno salt")]
        [TestCase("sdn.rule_args_test.PegDefinedArgs", "пять", true, "digit_5_salt_eto_soleto_soleto_sol")]
        [TestCase("sdn.rule_args_test.RegexDefaultArgs", "пять", true, "digit_5_salt_no salt")]
        [TestCase("sdn.rule_args_test.RegexDefaultRepeat", "пять", true, "digit_5_salt_eto_sol")]
        [TestCase("sdn.rule_args_test.RegexDefaultSalt", "пять", true, "digit_5_salt_no saltno saltno salt")]
        [TestCase("sdn.rule_args_test.RegexDefinedArgs", "пять", true, "digit_5_salt_eto_soleto_soleto_sol")]
        [TestCase("sdn.multivalued_ners.Foo", "один два три", true, 3)]
        [TestCase("sdn.multivalued_ners.Foo", "один куку три", true, 1, 0)]
        [TestCase("sdn.multivalued_ners.Foo", "один два куку", true, 2, 1)]
        [TestCase("sdn.multivalued_ners.Foo", "куку два три", false)]
        [TestCase("ner.number_line", "один два три", true, 3)]
        [TestCase("ner.number_line", "один куку три", true, 1, 0)]
        [TestCase("ner.number_line", "один два куку", true, 2, 1)]
        [TestCase("ner.number_line", "куку два три", false)]
        public void MatchesAndProjects(
            string ruleKey,
            string phrase,
            bool expectedIsMatched,
            object? expectedExtractedEntity = null,
            int? expectedLastUsedSymbolIndex = null
        )
        {
            RuleInput input = new RuleInput(
                phrase.Split(' ', StringSplitOptions.RemoveEmptyEntries),
                this.m_ruleSpaceArguments!
            );

            RuleMatchResultCollection matchResults = this.m_ruleSpace![ruleKey].MatchAndProject(
                input,
                0,
                new RuleArguments(ImmutableDictionary<string, object?>.Empty),
                new RuleSpaceCache()
            );

            RuleMatchResult? matchResult = matchResults.Best(this.m_bestReferenceSelectionStrategy!);

            Assert.AreEqual(expectedIsMatched, matchResult is not null);

            if (matchResult is not null)
            {
                Assert.AreEqual(expectedLastUsedSymbolIndex ?? (expectedIsMatched ? input.Sequence.Length - 1 : -1), matchResult.LastUsedSymbolIndex);
                Assert.AreEqual(expectedExtractedEntity, matchResult.Result.Value);
            }
        }

        [TestCaseSource(nameof(CapturesVariables_Cases))]
        public void CapturesVariables(
            string ruleKey,
            string phrase,
            IReadOnlyDictionary<string, object?> expectedCapturedVariables
        )
        {
            RuleInput input = new RuleInput(
                phrase.Split(' ', StringSplitOptions.RemoveEmptyEntries),
                this.m_ruleSpaceArguments!
            );

            RuleMatchResultCollection matchResults = this.m_ruleSpace![ruleKey].MatchAndProject(
                input,
                0,
                new RuleArguments(ImmutableDictionary<string, object?>.Empty),
                new RuleSpaceCache()
            );

            RuleMatchResult? matchResult = matchResults.Best(this.m_bestReferenceSelectionStrategy!);

            Assert.IsNotNull(matchResult);

            CollectionAssert.AreEquivalent(expectedCapturedVariables, matchResult!.CapturedVariables);
        }

        #region Sources

        #region Sources_CapturesVariables

        public static object?[][] CapturesVariables_Cases()
        {
            return new[]
            {
                new object?[]
                {
                    "patterns.NumberComparisonWithGreeting",
                    "привет число пять",
                    new Dictionary<string, object?>()
                    {
                        {"greeting", "привет"},
                        {"digit", 5},
                    }
                },
                new object?[]
                {
                    "patterns.NumberComparisonWithGreeting",
                    "добрый день число семь",
                    new Dictionary<string, object?>()
                    {
                        {"greeting", "добрый день"},
                        {"digit", 7},
                    }
                },
                new object?[]
                {
                    "patterns.NumberComparisonWithGreeting",
                    "я приветствую тебя число один",
                    new Dictionary<string, object?>()
                    {
                        {"greeting", "я приветствую тебя"},
                        {"digit", 1},
                    }
                },
            };
        }

        #endregion

        #endregion
    }

    public sealed class DummyParameters
    {
        public int Salt { get; }
        public DummySaltParameters DigitsSalt { get; }
        public DummySaltNullParameters NullDigitsSalt { get; }

        public DummyParameters(int salt, DummySaltParameters digitsSalt, DummySaltNullParameters nullDigitsSalt)
        {
            this.Salt = salt;
            this.DigitsSalt = digitsSalt;
            this.NullDigitsSalt = nullDigitsSalt;
        }
    }

    public sealed class DummySaltParameters
    {
        public string Salt { get; }
        public int RepeatTimes { get; }

        public DummySaltParameters(string salt, int repeatTimes)
        {
            this.Salt = salt;
            this.RepeatTimes = repeatTimes;
        }
    }

    public sealed class DummySaltNullParameters
    {
        public string? Salt { get; }
        public int? RepeatTimes { get; }

        public DummySaltNullParameters()
        {
            this.Salt = null;
            this.RepeatTimes = null;
        }
    }
}