using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.States;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.TerminalDetectors;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Payload
{
    internal sealed class TerminalPayload : ITransitionPayload
    {
        public bool IsTransient => false;

        /// <remarks>
        /// Performance remarks: library performance depends on the way this field is declared.
        /// Please make sure you know what you are doing, when changing this field's declaration.
        /// </remarks>
        public readonly ITerminalDetector TerminalDetector;

        public TerminalPayload(ITerminalDetector terminalDetector)
        {
            this.TerminalDetector = terminalDetector;
        }

        public void Consume(
            RuleInput input,
            RegexAutomatonState targetState,
            AutomatonProgress currentProgress,
            IRuleSpaceCache cache,
            in Stack<AutomatonProgress> progresses
        )
        {
            int nextSymbolIndex = currentProgress.LastUsedSymbolIndex + 1;

            if (nextSymbolIndex < input.Sequence.Length)
            {
                if (this.TerminalDetector.WordMatches(input.Sequence[nextSymbolIndex], out int explicitlyMatchedSymbolsCount))
                {
                    progresses.Push(
                        currentProgress.Clone(
                            targetState,
                            lastUsedSymbolIndex: nextSymbolIndex,
                            explicitlyMatchedSymbolsCount: currentProgress.ExplicitlyMatchedSymbolsCount + explicitlyMatchedSymbolsCount
                        )
                    );
                }
            }
        }

        public IEnumerable<string> GetUsedWords()
        {
            return this.TerminalDetector.GetUsedWords();
        }
    }
}