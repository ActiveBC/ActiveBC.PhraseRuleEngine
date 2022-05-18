using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.TerminalDetectors
{
    internal sealed class LiteralDetector : ITerminalDetector
    {
        private readonly LiteralToken m_literal;

        public LiteralDetector(LiteralToken literal)
        {
            this.m_literal = literal;
        }

        public bool WordMatches(string word)
        {
            return WordMatches(this.m_literal, word);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WordMatches(LiteralToken literal, string word)
        {
            return word.Equals(literal.Literal, StringComparison.Ordinal);
        }

        public IEnumerable<string> GetUsedWords()
        {
            yield return this.m_literal.Literal;
        }
    }
}