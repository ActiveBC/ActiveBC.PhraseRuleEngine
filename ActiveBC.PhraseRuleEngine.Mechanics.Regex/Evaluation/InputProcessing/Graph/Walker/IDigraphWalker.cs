namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Walker
{
    /// <summary>
    /// This interface contains a set of method related to the Directed Graph traversing.
    /// </summary>
    internal interface IDigraphWalker
    {
        /// <summary>
        /// Discovers full graph model,
        /// consisting of vertices and edges, which are accessible from <paramref name="startVertex"/>.
        /// </summary>
        /// <param name="startVertex">Start vertex.</param>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <returns>Graph model.</returns>
        Digraph<TVertex, TEdge> DiscoverGraph<TVertex, TEdge>(TVertex startVertex)
            where TVertex : IDigraphVertex<TVertex, TEdge>
            where TEdge : IDigraphEdge<TVertex, TEdge>;
    }
}