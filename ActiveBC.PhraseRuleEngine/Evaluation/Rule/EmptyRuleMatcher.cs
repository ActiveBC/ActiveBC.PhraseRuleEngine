using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule
{
    internal sealed class EmptyRuleMatcher : IRuleMatcher
    {
        public RuleParameters Parameters { get; }
        public RuleMatchResultDescription ResultDescription { get; }

        public EmptyRuleMatcher(RuleParameters parameters, RuleMatchResultDescription resultDescription)
        {
            this.Parameters = parameters;
            this.ResultDescription = resultDescription;
        }

        public RuleMatchResultCollection Match(RuleInput input, int firstSymbolIndex, IRuleSpaceCache cache)
        {
            return new RuleMatchResultCollection(0);
        }

        public RuleMatchResultCollection MatchAndProject(
            RuleInput input,
            int firstSymbolIndex,
            RuleArguments ruleArguments,
            IRuleSpaceCache cache
        )
        {
            return Match(input, firstSymbolIndex, cache);
        }

        public IEnumerable<string> GetUsedWords()
        {
            yield break;
        }
    }
}