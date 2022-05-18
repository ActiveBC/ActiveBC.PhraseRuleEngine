using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph
{
    internal sealed class Digraph<TVertex, TEdge>
        where TVertex : IDigraphVertex<TVertex, TEdge>
        where TEdge : IDigraphEdge<TVertex, TEdge>
    {
        public IReadOnlyDictionary<int, TVertex> Vertices { get; }
        public IReadOnlyDictionary<int, TEdge> Edges { get; }

        public Digraph(
            IReadOnlyDictionary<int, TVertex> vertices,
            IReadOnlyDictionary<int, TEdge> edges
        )
        {
            this.Vertices = vertices;
            this.Edges = edges;
        }
    }
}