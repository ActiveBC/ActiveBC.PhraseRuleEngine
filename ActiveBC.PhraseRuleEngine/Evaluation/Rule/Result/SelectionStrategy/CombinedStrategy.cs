using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result.SelectionStrategy
{
    public sealed class CombinedStrategy : IResultSelectionStrategy
    {
        private readonly IReadOnlyCollection<IResultSelectionStrategy> m_strategies;

        public CombinedStrategy(IReadOnlyCollection<IResultSelectionStrategy> strategies)
        {
            this.m_strategies = strategies;
        }

        public int Compare(RuleMatchResult a, RuleMatchResult b)
        {
            foreach (IResultSelectionStrategy strategy in this.m_strategies)
            {
                int result = strategy.Compare(a, b);

                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }
    }
}