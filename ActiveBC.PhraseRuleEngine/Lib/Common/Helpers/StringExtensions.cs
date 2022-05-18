using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ActiveBC.PhraseRuleEngine.Lib.Common.Helpers
{
    public static class StringExtensions
    {
        public static string JoinCharsToString(this IEnumerable<char> input)
        {
            return String.Join("", input);
        }

        public static string JoinToString(this IEnumerable<string> input, string separator = "")
        {
            return String.Join(separator, input);
        }

        public static string Capitalize(this string input)
        {
            if (input.Length == 0)
            {
                return input;
            }

            char[] charArray = input.ToCharArray();

            charArray[0] = char.ToUpperInvariant(charArray[0]);

            return new string(charArray);
        }

        public static string RemoveFromStart(this string input, string substring)
        {
            input.RemoveFromStart(substring, out string result);

            return result;
        }

        public static bool RemoveFromStart(this string input, string substring, out string result)
        {
            if (input.StartsWith(substring))
            {
                result = input.Substring(substring.Length);

                return true;
            }

            result = input;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToLowerFastRusEng(this string src)
        {
            char[] charArray = new char[src.Length];

            for (int index = 0; index < src.Length; ++index)
            {
                charArray[index] = src[index].ToLowerFastRusEng();
            }

            return new string(charArray);
        }
    }
}