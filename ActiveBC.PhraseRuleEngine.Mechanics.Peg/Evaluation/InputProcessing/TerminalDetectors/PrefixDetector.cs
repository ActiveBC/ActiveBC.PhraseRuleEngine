using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.TerminalDetectors
{
    internal sealed class PrefixDetector : ITerminalDetector
    {
        private readonly PrefixToken m_prefix;

        public PrefixDetector(PrefixToken prefix)
        {
            this.m_prefix = prefix;
        }

        public bool WordMatches(string word)
        {
            return WordMatches(this.m_prefix, word);
        }

        public IEnumerable<string> GetUsedWords()
        {
            yield break;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WordMatches(PrefixToken prefix, string word)
        {
            return word.StartsWith(prefix.Prefix, StringComparison.Ordinal);
        }
    }
}