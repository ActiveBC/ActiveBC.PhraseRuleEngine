using ActiveBC.PhraseRuleEngine.Reflection;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.TerminalDetectors
{
    internal interface ITerminalDetector : IUsedWordsProvider
    {
        bool WordMatches(string word);
    }
}