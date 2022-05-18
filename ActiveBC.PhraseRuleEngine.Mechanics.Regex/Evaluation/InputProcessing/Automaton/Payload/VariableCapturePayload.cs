using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.States;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Payload
{
    internal sealed class VariableCapturePayload : ITransitionPayload
    {
        public bool IsTransient => true;

        /// <remarks>
        /// Performance remarks: library performance depends on the way this field is declared.
        /// Please make sure you know what you are doing, when changing this field's declaration.
        /// </remarks>
        public readonly string VariableName;

        public VariableCapturePayload(string variableName)
        {
            this.VariableName = variableName;
        }

        public void Consume(
            RuleInput input,
            RegexAutomatonState targetState,
            AutomatonProgress currentProgress,
            IRuleSpaceCache cache,
            in Stack<AutomatonProgress> progresses
        )
        {
            if (currentProgress.CapturedValueFactory is not null)
            {
                AutomatonProgress newProgress = currentProgress.Clone(
                    targetState,
                    replaceCapturedValueFactory: true,
                    capturedValueFactory: null
                );

                newProgress.AddCapturedVariable(
                    this.VariableName,
                    currentProgress.CapturedValueFactory()
                );

                progresses.Push(newProgress);
            }
            else
            {
                progresses.Push(currentProgress.Clone(targetState));
            }
        }

        public IEnumerable<string> GetUsedWords()
        {
            yield break;
        }
    }
}