using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Walker
{
    internal interface IRegexAutomatonWalker<in TAutomaton>
    {
        RuleMatchResultCollection Walk(
            TAutomaton automaton,
            RuleInput ruleInput,
            int firstSymbolIndex,
            IRuleSpaceCache cache
        );
    }
}