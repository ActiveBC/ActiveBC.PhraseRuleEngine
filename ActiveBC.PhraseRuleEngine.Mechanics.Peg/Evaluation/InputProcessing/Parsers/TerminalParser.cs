using System;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.TerminalDetectors;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Parsers
{
    internal sealed class TerminalParser : IQuantifiableParser
    {
        public Type ResultType { get; } = typeof(string);

        private readonly ITerminalDetector m_terminalDetector;

        public TerminalParser(ITerminalDetector mTerminalDetector)
        {
            this.m_terminalDetector = mTerminalDetector;
        }

        public bool TryParse(
            RuleInput input,
            IRuleSpaceCache cache,
            ref int index,
            out int explicitlyMatchedSymbolsCount,
            out object? result
        )
        {
            if (index >= input.Sequence.Length)
            {
                explicitlyMatchedSymbolsCount = 0;
                result = null;

                return false;
            }

            string word = input.Sequence[index];

            bool isTerminalDetected = this.m_terminalDetector.WordMatches(word);

            if (isTerminalDetected)
            {
                explicitlyMatchedSymbolsCount = 1;
                result = word;

                ++index;
            }
            else
            {
                explicitlyMatchedSymbolsCount = 0;
                result = null;
            }

            return isTerminalDetected;
        }

        public IEnumerable<string> GetUsedWords()
        {
            return this.m_terminalDetector.GetUsedWords();
        }
    }
}