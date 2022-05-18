using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.Transitions;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.States
{
    // todo [code quality] separate "build models" (where setters are allowed) and "built models" (which are considered final)
    // todo [realtime performance] make OutgoingTransitions declared as RegexAutomatonTransition[] (list slows things down)
    internal sealed class RegexAutomatonState : IDigraphVertex<RegexAutomatonState, RegexAutomatonTransition>
    {
        public int Id { get; }
        public List<RegexAutomatonTransition> IncomingTransitions { get; }
        public List<RegexAutomatonTransition> OutgoingTransitions { get; }

        public IReadOnlyCollection<IDigraphEdge<RegexAutomatonState, RegexAutomatonTransition>> Edges => this.OutgoingTransitions;

        public RegexAutomatonState(int id, int outgoingTransitionsCount, int incomingTransitionsCount)
        {
            this.Id = id;
            // todo [realtime performance] we can try to initialize this collections in a lazy way
            this.OutgoingTransitions = new List<RegexAutomatonTransition>(outgoingTransitionsCount);
            this.IncomingTransitions = new List<RegexAutomatonTransition>(incomingTransitionsCount);
        }

        public void MoveIncomingTransitionsToOtherState(RegexAutomatonState other)
        {
            // todo [realtime performance] we can try perform replacement in some other way to remove this ToArray call
            foreach (RegexAutomatonTransition incomingTransition in this.IncomingTransitions.ToArray())
            {
                incomingTransition.ReplaceTargetState(other);
            }

            if (this.IncomingTransitions.Count > 0)
            {
                throw new RegexProcessorBuildException(
                    $"Cannot move incoming transitions from state {this.Id} to state {other.Id}: " +
                    $"incoming transitions list is not empty."
                );
            }
        }

        public void MoveOutgoingTransitionsToOtherState(RegexAutomatonState other)
        {
            // todo [realtime performance] we can try perform replacement in some other way to remove this ToArray call
            foreach (RegexAutomatonTransition outgoingTransition in this.OutgoingTransitions.ToArray())
            {
                outgoingTransition.ReplaceSourceState(other);
            }

            if (this.OutgoingTransitions.Count > 0)
            {
                throw new RegexProcessorBuildException(
                    $"Cannot move outgoing transitions from state {this.Id} to state {other.Id}: " +
                    $"outgoing transitions list is not empty."
                );
            }
        }
    }
}