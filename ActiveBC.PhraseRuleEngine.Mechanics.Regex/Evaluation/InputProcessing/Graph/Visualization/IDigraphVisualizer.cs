using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Formatters;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Label;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization
{
    internal interface IDigraphVisualizer
    {
        /// <summary>
        /// Formats the given automaton as a graph in DOT syntax.
        /// </summary>
        /// <param name="name">Graph name.</param>
        /// <param name="digraph">Digraph to visualize.</param>
        /// <param name="labelProvider">Label provider.</param>
        /// <param name="formatter">Formatter instance.</param>
        /// <typeparam name="TVertex">Digraph vertex type.</typeparam>
        /// <typeparam name="TEdge">Digraph edge type.</typeparam>
        /// <typeparam name="TResult">Formatting result type.</typeparam>
        /// <returns>Formatting result .</returns>
        TResult Format<TVertex, TEdge, TResult>(
            string name,
            Digraph<TVertex, TEdge> digraph,
            ILabelProvider<TVertex, TEdge> labelProvider,
            IGraphFormatter<TResult> formatter
        )
            where TVertex : IDigraphVertex<TVertex, TEdge>
            where TEdge : IDigraphEdge<TVertex, TEdge>;
    }
}