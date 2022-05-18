namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result.SelectionStrategy
{
    public interface IResultSelectionStrategy
    {
        int Compare(RuleMatchResult x, RuleMatchResult y);
    }
}