using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Reflection;

namespace ActiveBC.PhraseRuleEngine.Evaluation.InputProcessing
{
    public interface IInputProcessor : IUsedWordsProvider
    {
        RuleMatchResultCollection Match(RuleInput ruleInput, int firstSymbolIndex, IRuleSpaceCache cache);
    }
}