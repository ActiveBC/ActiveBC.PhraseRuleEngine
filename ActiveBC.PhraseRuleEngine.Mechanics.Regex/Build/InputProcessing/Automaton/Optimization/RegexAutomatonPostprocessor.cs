using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing.Automaton.Equality;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.States;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.Transitions;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Payload;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Walker;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing.Automaton.Optimization
{
    internal sealed class RegexAutomatonPostprocessor
    {
        public static readonly RegexAutomatonPostprocessor Instance = new RegexAutomatonPostprocessor(RecursiveDfsDigraphWalker.Instance);

        private readonly IDigraphWalker m_digraphWalker;

        private RegexAutomatonPostprocessor(IDigraphWalker digraphWalker)
        {
            this.m_digraphWalker = digraphWalker;
        }

        public void ValidateAndOptimize(RegexAutomaton automaton, OptimizationLevel optimizationLevel)
        {
            RunAllOptimizationsAndValidation(automaton, new HashSet<int>());

            if (optimizationLevel == OptimizationLevel.Min)
            {
                return;
            }

            // todo [non-realtime performance] we can avoid reversing automaton back and forward
            // and just replace Incoming/Outgoing, Source/Target, and Start/End values in the code that does the optimization
            // (i.e. we simulate reversing by using reversed properties)
            automaton.Reverse(this.m_digraphWalker);

            RunAllOptimizationsAndValidation(automaton, null);

            automaton.Reverse(this.m_digraphWalker);
        }

        private static void RunAllOptimizationsAndValidation(RegexAutomaton automaton, ISet<int>? localEpsilonClosure)
        {
            ISet<int> visitedStates = new HashSet<int>();

            Walk(automaton.StartState);

            void Walk(RegexAutomatonState state)
            {
                if (visitedStates.Add(state.Id))
                {
                    if (state.Id != automaton.EndState.Id)
                    {
                        if (TryEliminateState(state, out RegexAutomatonState? newState))
                        {
                            Walk(newState);
                        }
                        else
                        {
                            if (!TryMergeSiblings(state, out List<RegexAutomatonTransition>? transitionsToFollow))
                            {
                                transitionsToFollow = state.OutgoingTransitions;
                            }

                            if (localEpsilonClosure is not null)
                            {
                                DiscoverEpsilonClosure(transitionsToFollow, state.Id);
                                localEpsilonClosure.Clear();
                            }

                            foreach (RegexAutomatonTransition transitionToFollow in transitionsToFollow)
                            {
                                Walk(transitionToFollow.TargetState);
                            }
                        }
                    }
                }
            }

            void DiscoverEpsilonClosure(List<RegexAutomatonTransition> transitions, int referenceStateId)
            {
                foreach (RegexAutomatonTransition transition in transitions.Where(transition => transition.Payload.IsTransient))
                {
                    if (!localEpsilonClosure.Add(transition.Id))
                    {
                        throw new RegexProcessorBuildException($"Found undeterministic loop on state '{referenceStateId}'.");
                    }

                    DiscoverEpsilonClosure(transition.TargetState.OutgoingTransitions, transition.TargetState.Id);
                }
            }

            bool TryEliminateState(
                RegexAutomatonState state,
                [MaybeNullWhen(false)] out RegexAutomatonState replacement
            )
            {
                if (state.Id != automaton.StartState.Id && state.OutgoingTransitions.Count == 1)
                {
                    RegexAutomatonTransition outgoingTransition = state.OutgoingTransitions[0];

                    if (outgoingTransition.Payload is EpsilonPayload)
                    {
                        state.MoveIncomingTransitionsToOtherState(outgoingTransition.TargetState);

                        outgoingTransition.Remove();

                        replacement = outgoingTransition.TargetState;
                        return true;
                    }
                }

                replacement = null;

                return false;
            }

            bool TryMergeSiblings(
                RegexAutomatonState state,
                [MaybeNullWhen(false)] out List<RegexAutomatonTransition> transitionsToFollow
            )
            {
                if (state.OutgoingTransitions.Count > 1)
                {
                    List<RegexAutomatonTransition> newTransitions = new List<RegexAutomatonTransition>(
                        state.OutgoingTransitions.Count
                    );

                    IEnumerable<IGrouping<ITransitionPayload, RegexAutomatonTransition>> groupingsByPayload = state
                        .OutgoingTransitions
                        .GroupBy(transition => transition.Payload, TransitionPayloadEqualityComparer.Instance);

                    foreach (IGrouping<ITransitionPayload,RegexAutomatonTransition> payloadGrouping in groupingsByPayload)
                    {
                        IGrouping<int, RegexAutomatonTransition>[] groupingsByTargetState = payloadGrouping
                            .GroupBy(transition => transition.TargetState.Id)
                            .ToArray();

                        IGrouping<int, RegexAutomatonTransition>[] mergeableGroupings = groupingsByTargetState
                            .Where(targetStateGrouping => targetStateGrouping.Count() > 1)
                            .ToArray();

                        IEnumerable<RegexAutomatonTransition> orphans = groupingsByTargetState
                            .Except(mergeableGroupings)
                            .SelectMany(targetStateGrouping => targetStateGrouping);

                        MergeTransitions(orphans, newTransitions, false);

                        foreach (IGrouping<int, RegexAutomatonTransition> mergeableGrouping in mergeableGroupings)
                        {
                            MergeTransitions(mergeableGrouping, newTransitions, true);
                        }
                    }

                    if (newTransitions.Count == 1 && TryEliminateState(newTransitions[0].SourceState, out RegexAutomatonState? newSourceState))
                    {
                        transitionsToFollow = newSourceState.OutgoingTransitions;
                    }
                    else
                    {
                        transitionsToFollow = newTransitions;
                    }

                    return true;
                }

                transitionsToFollow = null;

                return false;
            }
        }

        private static void MergeTransitions(
            IEnumerable<RegexAutomatonTransition> transitions,
            in List<RegexAutomatonTransition> newTransitions,
            bool hasSameTargetState
        )
        {
            RegexAutomatonTransition? mainOutgoingTransition = null;
            foreach (RegexAutomatonTransition transition in transitions)
            {
                if (!hasSameTargetState && transition.TargetState.IncomingTransitions.Count != 1)
                {
                    newTransitions.Add(transition);
                    continue;
                }

                if (mainOutgoingTransition is null)
                {
                    mainOutgoingTransition = transition;
                    newTransitions.Add(transition);
                    continue;
                }

                RegexAutomatonTransition duplicateOutgoingTransition = transition;

                if (!hasSameTargetState)
                {
                    duplicateOutgoingTransition.TargetState.MoveOutgoingTransitionsToOtherState(mainOutgoingTransition.TargetState);
                }

                duplicateOutgoingTransition.Remove();
            }
        }
    }
}