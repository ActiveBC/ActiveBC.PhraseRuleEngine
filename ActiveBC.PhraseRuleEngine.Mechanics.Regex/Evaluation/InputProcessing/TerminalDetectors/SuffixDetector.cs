using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.TerminalDetectors
{
    internal sealed class SuffixDetector : ITerminalDetector
    {
        public readonly string Suffix;

        public SuffixDetector(SuffixToken suffix)
        {
            this.Suffix = suffix.Suffix;
        }

        public bool WordMatches(string word, out int explicitlyMatchedSymbolsCount)
        {
            if (WordMatches(this.Suffix, word))
            {
                explicitlyMatchedSymbolsCount = 1;
                return true;
            }

            explicitlyMatchedSymbolsCount = 0;
            return false;
        }

        public IEnumerable<string> GetUsedWords()
        {
            yield break;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WordMatches(string suffix, string word)
        {
            return word.EndsWith(suffix, StringComparison.Ordinal);
        }
    }
}