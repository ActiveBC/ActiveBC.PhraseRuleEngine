namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph
{
    public interface IDigraphEdge<TVertex, TEdge>
        where TEdge : IDigraphEdge<TVertex, TEdge>
        where TVertex : IDigraphVertex<TVertex, TEdge>
    {
        int Id { get; }
        IDigraphVertex<TVertex, TEdge> TargetVertex { get; }
    }
}