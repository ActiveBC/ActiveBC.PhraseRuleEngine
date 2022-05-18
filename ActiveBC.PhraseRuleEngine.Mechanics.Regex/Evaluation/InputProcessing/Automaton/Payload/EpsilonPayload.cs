using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.States;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Payload
{
    internal sealed class EpsilonPayload : ITransitionPayload
    {
        public static readonly EpsilonPayload Instance = new EpsilonPayload();

        public bool IsTransient => true;

        private EpsilonPayload()
        {
        }

        public void Consume(
            RuleInput input,
            RegexAutomatonState targetState,
            AutomatonProgress currentProgress,
            IRuleSpaceCache cache,
            in Stack<AutomatonProgress> progresses
        )
        {
            progresses.Push(currentProgress.Clone(targetState));
        }

        public IEnumerable<string> GetUsedWords()
        {
            yield break;
        }
    }
}