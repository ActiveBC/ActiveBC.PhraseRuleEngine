using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.TerminalDetectors
{
    internal sealed class LiteralSetDetector : ITerminalDetector
    {
        private readonly LiteralSetToken m_token;

        public LiteralSetDetector(LiteralSetToken token)
        {
            this.m_token = token;
        }

        public bool WordMatches(string word)
        {
            bool hasPositiveMatch = false;

            foreach (ILiteralSetMemberToken member in this.m_token.Members)
            {
                bool memberMatched = member switch
                {
                    LiteralToken literalToken => LiteralDetector.WordMatches(literalToken, word),
                    PrefixToken prefixToken => PrefixDetector.WordMatches(prefixToken, word),
                    InfixToken infixToken => InfixDetector.WordMatches(infixToken, word),
                    SuffixToken suffixToken => SuffixDetector.WordMatches(suffixToken, word),
                    _ => throw new PegProcessorMatchException($"Unknown literal set member type {member.GetType().FullName}."),
                };

                if (memberMatched)
                {
                    hasPositiveMatch = true;
                    break;
                }
            }

            return this.m_token.IsNegative != hasPositiveMatch;
        }

        public IEnumerable<string> GetUsedWords()
        {
            foreach (ILiteralSetMemberToken member in this.m_token.Members)
            {
                if (member is LiteralToken literalToken)
                {
                    yield return literalToken.Literal;
                }
            }
        }
    }
}