using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;

namespace ActiveBC.PhraseRuleEngine.Build.Rule.Source
{
    internal interface IRuleSource
    {
        IRuleMatcher GetRuleMatcher(in IRuleSpace ruleSpace);
    }
}