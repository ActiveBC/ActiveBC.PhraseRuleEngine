using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Models;
using ActiveBC.PhraseRuleEngine.Reflection;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Composers
{
    internal interface IComposer : IUsedWordsProvider
    {
        bool Match(
            RuleInput input,
            ref int index,
            in PegInputProcessorDataCollector dataCollector,
            IRuleSpaceCache cache
        );
    }
}