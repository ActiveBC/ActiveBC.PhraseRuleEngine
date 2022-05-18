using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Formatters;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Label;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Models;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization
{
    internal sealed class DigraphVisualizer : IDigraphVisualizer
    {
        public static readonly DigraphVisualizer Instance = new DigraphVisualizer();

        private DigraphVisualizer()
        {
        }

        public TResult Format<TVertex, TEdge, TResult>(
            string name,
            Digraph<TVertex, TEdge> digraph,
            ILabelProvider<TVertex, TEdge> labelProvider,
            IGraphFormatter<TResult> formatter
        )
            where TVertex : IDigraphVertex<TVertex, TEdge>
            where TEdge : IDigraphEdge<TVertex, TEdge>
        {
            GraphModel graph = CreateModel(name.Replace(' ', '_'), digraph, labelProvider);

            return formatter.Format(graph);
        }

        private static GraphModel CreateModel<TVertex, TEdge>(
            string name,
            Digraph<TVertex, TEdge> digraph,
            ILabelProvider<TVertex, TEdge> labelProvider
        )
            where TVertex : IDigraphVertex<TVertex, TEdge>
            where TEdge : IDigraphEdge<TVertex, TEdge>
        {
            Dictionary<int, NodeModel> nodeModels = digraph
                .Vertices
                .MapValue(vertex => new NodeModel(vertex.Id, labelProvider.GetLabel(vertex)))
                .ToDictionary();

            Dictionary<int, NodeModel> edgeSourceNodesByEdgeId = digraph
                .Vertices
                .Values
                .SelectMany(
                    vertex => vertex
                        .Edges
                        .Select(edge => new KeyValuePair<int, NodeModel>(edge.Id, nodeModels[vertex.Id]))
                )
                .Distinct(new EntityEqualityComparer<KeyValuePair<int,NodeModel>, int>(pair => pair.Key))
                .ToDictionary();

            IReadOnlyCollection<EdgeModel> edgeModels = digraph
                .Edges
                .Values
                .Select(
                    edge => new EdgeModel(
                        labelProvider.GetLabel(edge),
                        edgeSourceNodesByEdgeId[edge.Id],
                        nodeModels[edge.TargetVertex.Id]
                    )
                )
                .ToArray();

            return new GraphModel(name, nodeModels, edgeModels);
        }
    }
}