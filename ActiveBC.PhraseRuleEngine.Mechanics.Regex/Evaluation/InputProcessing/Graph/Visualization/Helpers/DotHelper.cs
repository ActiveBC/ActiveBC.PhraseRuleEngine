using System;
using System.Diagnostics;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Graph.Visualization.Helpers
{
    internal static class DotHelper
    {
        public static void RunDot(string outputFormat, Uri pathToGvFile, Uri pathToOutputFile)
        {
            Process? process = Process.Start(
                new ProcessStartInfo
                {
                    FileName = "dot",
                    Arguments = $"-T{outputFormat} {pathToGvFile.AbsolutePath} -o {pathToOutputFile.AbsolutePath}"
                }
            );

            if (process is null)
            {
                throw new ApplicationException($"Cannot convert 'gv' to '{outputFormat}' using dot.exe.");
            }

            process.WaitForExit();
        }
    }
}