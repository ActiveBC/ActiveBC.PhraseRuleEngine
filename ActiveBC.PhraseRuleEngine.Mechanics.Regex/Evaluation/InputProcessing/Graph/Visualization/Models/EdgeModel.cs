namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Models
{
    internal sealed class EdgeModel
    {
        public string Label { get; }
        public NodeModel SourceNode { get; }
        public NodeModel DestinationNode { get; }

        public EdgeModel(string label, NodeModel sourceNode, NodeModel destinationNode)
        {
            Label = label;
            SourceNode = sourceNode;
            DestinationNode = destinationNode;
        }
    }
}