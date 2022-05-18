using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Models
{
    internal sealed class GraphModel
    {
        public string Name { get; }
        public IReadOnlyDictionary<int, NodeModel> Nodes { get; }
        public IReadOnlyCollection<EdgeModel> Edges { get; }

        public GraphModel(string name, IReadOnlyDictionary<int, NodeModel> nodes, IReadOnlyCollection<EdgeModel> edges)
        {
            this.Name = name;
            this.Nodes = nodes;
            this.Edges = edges;
        }
    }
}