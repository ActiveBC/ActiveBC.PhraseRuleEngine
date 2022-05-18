using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.Transitions;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Walker
{
    internal sealed class RegexAutomatonWalker : IRegexAutomatonWalker<RegexAutomaton>
    {
        public static readonly RegexAutomatonWalker Instance = new RegexAutomatonWalker();

        public RuleMatchResultCollection Walk(
            RegexAutomaton automaton,
            RuleInput ruleInput,
            int firstSymbolIndex,
            IRuleSpaceCache cache
        )
        {
            // todo this is rough estimate, think if we can predict this number more precisely
            RuleMatchResultCollection results = new RuleMatchResultCollection(10);
            Stack<AutomatonProgress> progresses = new Stack<AutomatonProgress>(10);

            progresses.Push(
                new AutomatonProgress(
                    firstSymbolIndex - 1,
                    null,
                    0,
                    null,
                    null,
                    automaton.StartState
                )
            );

            while (progresses.TryPop(out AutomatonProgress? progress))
            {
                if (progress.State.Id == automaton.EndState.Id)
                {
                    results.Add(progress.Flush(ruleInput, firstSymbolIndex));
                    continue;
                }

                foreach (RegexAutomatonTransition transition in progress.State.OutgoingTransitions)
                {
                    transition.Payload.Consume(ruleInput, transition.TargetState, progress, cache, progresses);
                }
            }

            return results;
        }
    }
}