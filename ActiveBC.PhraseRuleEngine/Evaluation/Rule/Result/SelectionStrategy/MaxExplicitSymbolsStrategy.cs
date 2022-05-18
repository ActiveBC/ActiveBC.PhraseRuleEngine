namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result.SelectionStrategy
{
    public sealed class MaxExplicitSymbolsStrategy : IResultSelectionStrategy
    {
        public int Compare(RuleMatchResult a, RuleMatchResult b)
        {
            if (a.ExplicitlyMatchedSymbolsCount == b.ExplicitlyMatchedSymbolsCount)
            {
                return 0;
            }

            return a.ExplicitlyMatchedSymbolsCount > b.ExplicitlyMatchedSymbolsCount ? 1 : -1;
        }
    }
}