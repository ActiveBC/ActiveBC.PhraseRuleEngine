using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.TerminalDetectors
{
    internal sealed class SuffixDetector : ITerminalDetector
    {
        private readonly SuffixToken m_suffix;

        public SuffixDetector(SuffixToken suffix)
        {
            this.m_suffix = suffix;
        }

        public bool WordMatches(string word)
        {
            return WordMatches(this.m_suffix, word);
        }

        public IEnumerable<string> GetUsedWords()
        {
            yield break;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WordMatches(SuffixToken suffix, string word)
        {
            return word.EndsWith(suffix.Suffix, StringComparison.Ordinal);
        }
    }
}