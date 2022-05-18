using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Models;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Formatters
{
    internal interface IGraphFormatter<out TResult>
    {
        TResult Format(GraphModel graph);
    }
}