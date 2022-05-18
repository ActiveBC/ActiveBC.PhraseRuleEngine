using System;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Rule.Static.Attributes;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Tests.Fixtures
{
    [StaticRuleContainer("dummy_ner")]
    public static class DummyNerSource
    {
        [StaticRule("hi", nameof(GetUsedWords))]
        public static (bool success, string? result, int lastUserSymbolIndex) ParseHi(string[] sequence, int startIndex)
        {
            string[] hi = new[] { "привет", "здравствуй" };

            if (hi.Contains(sequence[startIndex]))
            {
                return (true, "приветствие", startIndex);
            }

            return (false, null, 0);
        }

        [StaticRule("bye", nameof(GetUsedWords))]
        public static (bool success, string? result, int lastUserSymbolIndex) ParseBye(string[] sequence, int startIndex)
        {
            string[] bye = new[] { "пока", "прощай" };

            if (bye.Contains(sequence[startIndex]))
            {
                return (true, "прощание", startIndex);
            }

            return (false, null, 0);
        }

        [StaticRule("two", nameof(GetUsedWords))]
        public static (bool success, int result, int lastUserSymbolIndex) ParseTwo(string[] sequence, int startIndex)
        {
            if (sequence[startIndex] == "два")
            {
                return (true, 2, startIndex);
            }

            return (false, 0, 0);
        }

        [StaticRule("three_with_optional_four", nameof(GetUsedWords))]
        public static (bool success, int[] result, int lastUserSymbolIndex) ParseThreeWithOptionalFour(string[] sequence, int startIndex)
        {
            string[] expectedSequence = new[] { "три", "четыре" };
            int[] expectedSequenceNer = new[] { 3, 4 };

            bool success = false;
            int[] result = new int[2];
            int lastUserSymbolIndex = 0;

            int expectedSequenceIndex = 0;
            for (int sequenceIndex = startIndex; sequenceIndex < sequence.Length; sequenceIndex++, expectedSequenceIndex++)
            {
                if (expectedSequenceIndex >= expectedSequence.Length)
                {
                    break;
                }

                if (expectedSequence[expectedSequenceIndex] != sequence[sequenceIndex])
                {
                    break;
                }

                success = true;
                result[expectedSequenceIndex] = expectedSequenceNer[expectedSequenceIndex];
                lastUserSymbolIndex = sequenceIndex;
            }

            return (success, result, lastUserSymbolIndex);
        }

        [StaticRule("any_next_two_words", nameof(GetUsedWords))]
        public static (bool success, string? result, int lastUserSymbolIndex) ParseAnyTwoWords(string[] sequence, int startIndex)
        {
            return (true, "два слова", Math.Min(startIndex + 1, sequence.Length - 1));
        }

        [StaticRule("any_next_three_words", nameof(GetUsedWords))]
        public static (bool success, string? result, int lastUserSymbolIndex) ParseAnyThreeWords(string[] sequence, int startIndex)
        {
            return (true, "три слова", Math.Min(startIndex + 2, sequence.Length - 1));
        }

        [StaticRule("hi_star", nameof(GetUsedWords))]
        public static (bool success, string? result, int lastUserSymbolIndex) ParseHiStar(string[] sequence, int startIndex)
        {
            bool isMatched = false;
            int lastUserSymbolIndex = 0;
            for (int sequenceIndex = startIndex; sequenceIndex < sequence.Length; sequenceIndex++)
            {
                if ("привет" == sequence[sequenceIndex])
                {
                    isMatched = true;
                    lastUserSymbolIndex = sequenceIndex;
                }
                else
                {
                    break;
                }
            }

            return (isMatched, isMatched ? "приветствие" : null, lastUserSymbolIndex);
        }

        private static IEnumerable<string> GetUsedWords()
        {
            return Enumerable.Empty<string>();
        }
    }
}