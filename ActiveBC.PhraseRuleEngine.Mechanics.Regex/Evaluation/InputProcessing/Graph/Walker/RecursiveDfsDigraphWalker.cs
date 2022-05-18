using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Walker
{
    /// <summary>
    /// This class implements recursive version of "Depth-First Search" algorithm of graph traversing.
    /// </summary>
    internal sealed class RecursiveDfsDigraphWalker : IDigraphWalker
    {
        public static readonly RecursiveDfsDigraphWalker Instance = new RecursiveDfsDigraphWalker();

        public Digraph<TVertex, TEdge> DiscoverGraph<TVertex, TEdge>(TVertex startVertex)
            where TVertex : IDigraphVertex<TVertex, TEdge>
            where TEdge : IDigraphEdge<TVertex, TEdge>
        {
            Dictionary<int, TVertex> vertices = new Dictionary<int, TVertex>();
            Dictionary<int, TEdge> edges = new Dictionary<int, TEdge>();

            DiscoverGraph(startVertex, vertices, edges);

            return new Digraph<TVertex, TEdge>(vertices, edges);
        }

        private static void DiscoverGraph<TVertex, TEdge>(
            TVertex vertex,
            in IDictionary<int, TVertex> vertices,
            in IDictionary<int, TEdge> edges
        )
            where TVertex : IDigraphVertex<TVertex, TEdge>
            where TEdge : IDigraphEdge<TVertex, TEdge>
        {
            if (vertices.TryAdd(vertex.Id, vertex))
            {
                foreach (TEdge edge in vertex.Edges.Cast<TEdge>())
                {
                    if (edges.TryAdd(edge.Id, edge))
                    {
                        DiscoverGraph((TVertex) edge.TargetVertex, vertices, edges);
                    }
                }
            }
        }
    }
}