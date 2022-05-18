namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result.SelectionStrategy
{
    public sealed class MaxProgressStrategy : IResultSelectionStrategy
    {
        public int Compare(RuleMatchResult a, RuleMatchResult b)
        {
            if (a.LastUsedSymbolIndex == b.LastUsedSymbolIndex)
            {
                return 0;
            }

            return a.LastUsedSymbolIndex > b.LastUsedSymbolIndex ? 1 : -1;
        }
    }
}