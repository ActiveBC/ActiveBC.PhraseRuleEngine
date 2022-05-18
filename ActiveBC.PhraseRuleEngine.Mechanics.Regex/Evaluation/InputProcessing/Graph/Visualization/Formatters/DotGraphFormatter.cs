using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Models;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Formatters
{
    internal sealed class DotGraphFormatter : IGraphFormatter<string>
    {
        public static readonly DotGraphFormatter Instance = new DotGraphFormatter();

        private DotGraphFormatter()
        {
        }

        /// <summary>
        /// Formats <paramref name="graph"/> in DOT syntax.
        /// </summary>
        /// <param name="graph">Graph object to format.</param>
        /// <returns>DOT graph representation.</returns>
        public string Format(GraphModel graph)
        {
            return $"digraph {graph.Name} {{\r\n{FormatAllPieces().ToArray().JoinToString("\r\n")}\r\n}}";

            IEnumerable<string> FormatAllPieces()
            {
                foreach (NodeModel node in graph.Nodes.Values)
                {
                    yield return $"  {FormatNode(node)}";
                }

                foreach (EdgeModel edge in graph.Edges)
                {
                    yield return $"  {FormatEdge(edge)}";
                }
            }
        }

        private string FormatNode(NodeModel node)
        {
            return $@"{GetNodeId(node)} [label=""{node.Label}""];";
        }

        private string FormatEdge(EdgeModel edge)
        {
            return $@"{GetNodeId(edge.SourceNode)} -> {GetNodeId(edge.DestinationNode)} [style=""solid"" label=""{edge.Label}""];";
        }

        private static string GetNodeId(NodeModel node)
        {
            return $"node_{node.Id}";
        }
    }
}