namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Label
{
    internal interface ILabelProvider<in TVertex, in TEdge>
        where TVertex : IDigraphVertex<TVertex, TEdge>
        where TEdge : IDigraphEdge<TVertex, TEdge>
    {
        string GetLabel(TVertex vertex);

        string GetLabel(TEdge edge);
    }
}