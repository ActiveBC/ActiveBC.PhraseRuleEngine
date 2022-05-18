using ActiveBC.PhraseRuleEngine.Reflection;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.TerminalDetectors
{
    internal interface ITerminalDetector : IUsedWordsProvider
    {
        bool WordMatches(string word, out int explicitlyMatchedSymbolsCount);
    }
}