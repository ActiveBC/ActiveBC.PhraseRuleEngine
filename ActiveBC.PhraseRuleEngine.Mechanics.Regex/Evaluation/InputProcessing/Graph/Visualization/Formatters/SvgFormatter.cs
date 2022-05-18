using System;
using System.IO;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Helpers;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Models;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Formatters
{
    internal sealed class SvgFormatter : IGraphFormatter<string>
    {
        public static readonly SvgFormatter Instance = new SvgFormatter();

        private const string FileExtension = "svg";

        public string Format(GraphModel graph)
        {
            string dotGraph = DotGraphFormatter.Instance.Format(graph);

            Uri pathToGvFile = new Uri(PathHelper.GetTempFilePath("gv"));

            File.WriteAllText(pathToGvFile.AbsolutePath, dotGraph);

            Uri pathToResultFile = new Uri(PathHelper.GetTempFilePath(FileExtension));

            DotHelper.RunDot(FileExtension, pathToGvFile, pathToResultFile);

            return File.ReadAllText(pathToResultFile.AbsolutePath);
        }
    }
}