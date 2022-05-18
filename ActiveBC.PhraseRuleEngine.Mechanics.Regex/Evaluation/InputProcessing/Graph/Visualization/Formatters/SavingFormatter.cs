using System;
using System.IO;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Helpers;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Models;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Formatters
{
    internal sealed class SavingFormatter : IGraphFormatter<Uri>
    {
        private readonly IGraphFormatter<string> m_underlyingFormatter;
        private readonly string m_fileExtension;

        public SavingFormatter(
            IGraphFormatter<string> underlyingFormatter,
            string fileExtension
        )
        {
            this.m_underlyingFormatter = underlyingFormatter;
            this.m_fileExtension = fileExtension;
        }

        public Uri Format(GraphModel graph)
        {
            string formattedGraph = this.m_underlyingFormatter.Format(graph);

            string pathToFile = PathHelper.GetTempFilePath(this.m_fileExtension);

            File.WriteAllText(pathToFile, formattedGraph);

            return new Uri(pathToFile);
        }
    }
}