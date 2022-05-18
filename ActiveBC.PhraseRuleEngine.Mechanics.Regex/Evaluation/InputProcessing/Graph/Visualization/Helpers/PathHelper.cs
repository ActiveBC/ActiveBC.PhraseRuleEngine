using System;
using System.IO;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Helpers
{
    internal static class PathHelper
    {
        public static string GetTempFilePath(string extension)
        {
            return Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.{extension.Trim('.')}");
        }
    }
}