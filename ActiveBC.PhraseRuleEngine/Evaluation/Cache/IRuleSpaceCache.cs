using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Cache
{
    public interface IRuleSpaceCache
    {
        RuleMatchResultCollection? GetResult(
            int ruleId,
            string[] inputSequence,
            int nextSymbolIndex,
            IReadOnlyDictionary<string, object?>? ruleArguments
        );

        void SetResult(
            int ruleId,
            string[] inputSequence,
            int nextSymbolIndex,
            IReadOnlyDictionary<string, object?>? ruleArguments,
            RuleMatchResultCollection result
        );
    }
}