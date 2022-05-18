using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.TerminalDetectors
{
    internal sealed class InfixDetector : ITerminalDetector
    {
        private readonly InfixToken m_infix;

        public InfixDetector(InfixToken infix)
        {
            this.m_infix = infix;
        }

        public bool WordMatches(string word)
        {
            return WordMatches(this.m_infix, word);
        }

        public IEnumerable<string> GetUsedWords()
        {
            yield break;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WordMatches(InfixToken infix, string word)
        {
            return word.Contains(infix.Infix, System.StringComparison.Ordinal);
        }
    }
}