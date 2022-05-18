using System;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.States;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.Transitions;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Visualization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Formatters;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Walker;
using ActiveBC.PhraseRuleEngine.Reflection;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models
{
    internal sealed class RegexAutomaton : IUsedWordsProvider
    {
        public RegexAutomatonState StartState { get; private set; }
        public RegexAutomatonState EndState { get; private set; }

        public RegexAutomaton(RegexAutomatonState startState, RegexAutomatonState endState)
        {
            this.StartState = startState;
            this.EndState = endState;
        }

        public IEnumerable<string> GetUsedWords()
        {
            HashSet<int> visitedStateIds = new HashSet<int>();

            return GetUsedWordsFromNewTransitions(this.StartState);

            IEnumerable<string> GetUsedWordsFromNewTransitions(RegexAutomatonState state)
            {
                IEnumerable<string> usedWords = Enumerable.Empty<string>();

                if (visitedStateIds.Add(state.Id))
                {
                    foreach (RegexAutomatonTransition transition in state.OutgoingTransitions)
                    {
                        usedWords = usedWords.Concat(transition.Payload.GetUsedWords());
                        usedWords = usedWords.Concat(GetUsedWordsFromNewTransitions(transition.TargetState));
                    }
                }

                return usedWords;
            }
        }

        public Uri VisualizeAs(VisualizationFormat format)
        {
            (IGraphFormatter<string> Formatter, string FileExtension) formatterData = format switch
            {
                VisualizationFormat.Gv => (DotGraphFormatter.Instance, "gv"),
                VisualizationFormat.Svg => (SvgFormatter.Instance, "svg"),
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, "Unsupported format")
            };

            return DigraphVisualizer.Instance.Format(
                "Regex automaton",
                RecursiveDfsDigraphWalker
                    .Instance
                    .DiscoverGraph<RegexAutomatonState, RegexAutomatonTransition>(this.StartState),
                RegexAutomatonLabelProvider.Instance,
                new SavingFormatter(formatterData.Formatter, formatterData.FileExtension)
            );
        }

        public static void AddTransition(RegexAutomatonTransition transition)
        {
            transition.SourceState.OutgoingTransitions.Add(transition);
            transition.TargetState.IncomingTransitions.Add(transition);
        }

        public void Reverse(IDigraphWalker digraphWalker)
        {
            Digraph<RegexAutomatonState, RegexAutomatonTransition> digraph = digraphWalker
                .DiscoverGraph<RegexAutomatonState, RegexAutomatonTransition>(this.StartState);

            foreach (RegexAutomatonTransition transition in digraph.Edges.Values)
            {
                transition.Reverse();
            }

            (this.StartState, this.EndState) = (this.EndState, this.StartState);
        }
    }
}