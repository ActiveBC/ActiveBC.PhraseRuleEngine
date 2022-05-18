using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Rule.Static.Attributes;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Tests.Fixtures
{
    [StaticRuleContainer("ner")]
    public static class DummyStaticNerContainer
    {
        [StaticRule("digit", nameof(GetUsedWords))]
        public static (bool success, int result, int lastUserSymbolIndex) ParseDigit(string[] sequence, int startIndex)
        {
            int? digit = TryParseDigit(sequence[startIndex]);

            if (digit is not null)
            {
                return (true, digit.Value, startIndex);
            }

            return (false, default, default);
        }

        [StaticRule("digit_with_salt", nameof(GetUsedWords))]
        public static (bool success, string? result, int lastUsedSymbolIndex) ParseDigitWithSalt(
            string[] sequence,
            int startIndex,
            string? salt = null,
            int? saltRepeatTimes = null
        )
        {
            int? digit = TryParseDigit(sequence[startIndex]);

            if (digit is not null)
            {
                return (true, $"digit_{digit.Value}_salt_{Enumerable.Repeat(salt ?? "no salt", saltRepeatTimes ?? 1).JoinToString()}", startIndex);
            }

            return (false, default, default);
        }

        [StaticRule("number_line", nameof(GetUsedWords))]
        public static IEnumerable<(int result, int lastUserSymbolIndex)> ParseNumberLine(string[] sequence, int startIndex)
        {
            for (int index = startIndex; index < sequence.Length; index++)
            {
                int? digit = TryParseDigit(sequence[index]);

                if (digit is null)
                {
                    yield break;
                }

                yield return (digit.Value, index);
            }
        }

        private static int? TryParseDigit(string symbol)
        {
            return symbol switch
            {
                "ноль" => 0,
                "один" => 1,
                "два" => 2,
                "три" => 3,
                "четыре" => 4,
                "пять" => 5,
                "шесть" => 6,
                "семь" => 7,
                "восемь" => 8,
                "девять" => 9,
                _ => null,
            };
        }

        private static IEnumerable<string> GetUsedWords()
        {
            return Enumerable.Empty<string>();
        }
    }
}