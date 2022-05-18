using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.States;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Payload;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.Transitions
{
    // todo [code quality] separate "build models" (where setters are allowed) and "built models" (which are considered final)
    internal sealed class RegexAutomatonTransition : IDigraphEdge<RegexAutomatonState, RegexAutomatonTransition>
    {
        public int Id { get; }
        public RegexAutomatonState SourceState { get; private set; }
        public RegexAutomatonState TargetState { get; private set; }
        public ITransitionPayload Payload { get; }
        public IDigraphVertex<RegexAutomatonState, RegexAutomatonTransition> TargetVertex => TargetState;

        public RegexAutomatonTransition(
            int id,
            RegexAutomatonState sourceState,
            RegexAutomatonState targetState,
            ITransitionPayload payload
        )
        {
            this.Id = id;
            this.SourceState = sourceState;
            this.TargetState = targetState;
            this.Payload = payload;
        }

        public void Reverse()
        {
            RegexAutomatonState originalSourceState = this.SourceState;
            RegexAutomatonState originalTargetState = this.TargetState;

            ReplaceSourceState(originalTargetState);
            ReplaceTargetState(originalSourceState);
        }

        public void ReplaceSourceState(RegexAutomatonState newSourceState)
        {
            RegexAutomatonState originalSourceState = this.SourceState;

            this.SourceState = newSourceState;

            originalSourceState.OutgoingTransitions.Remove(this);
            this.SourceState.OutgoingTransitions.Add(this);
        }

        public void ReplaceTargetState(RegexAutomatonState newTargetState)
        {
            RegexAutomatonState originalTargetState = this.TargetState;

            this.TargetState = newTargetState;

            originalTargetState.IncomingTransitions.Remove(this);
            this.TargetState.IncomingTransitions.Add(this);
        }

        public void Remove()
        {
            this.SourceState.OutgoingTransitions.Remove(this);
            this.TargetState.IncomingTransitions.Remove(this);
        }
    }
}