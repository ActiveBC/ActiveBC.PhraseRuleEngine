using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.States;
using ActiveBC.PhraseRuleEngine.Reflection;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Payload
{
    internal interface ITransitionPayload : IUsedWordsProvider
    {
        bool IsTransient { get; }
        void Consume(
            RuleInput input,
            RegexAutomatonState targetState,
            AutomatonProgress currentProgress,
            IRuleSpaceCache cache,
            in Stack<AutomatonProgress> progresses
        );
    }
}