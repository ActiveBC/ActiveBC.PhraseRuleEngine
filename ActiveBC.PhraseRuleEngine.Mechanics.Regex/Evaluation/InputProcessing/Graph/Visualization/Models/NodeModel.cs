namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Models
{
    internal sealed class NodeModel
    {
        public int Id { get; }
        public string Label { get; }

        public NodeModel(int id, string label)
        {
            this.Id = id;
            this.Label = label;
        }
    }
}