using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Models;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Composers
{
    internal sealed class LookaheadComposer : IComposer
    {
        private readonly LookaheadToken m_lookaheadToken;
        private readonly IComposer m_child;

        public LookaheadComposer(LookaheadToken lookaheadToken, IComposer child)
        {
            this.m_lookaheadToken = lookaheadToken;
            this.m_child = child;
        }

        public bool Match(
            RuleInput input,
            ref int index,
            in PegInputProcessorDataCollector dataCollector,
            IRuleSpaceCache cache
        )
        {
            int boundIndex = index;

            bool result = this.m_child.Match(input, ref boundIndex, dataCollector, cache);

            return result != this.m_lookaheadToken.IsNegative;
        }

        public IEnumerable<string> GetUsedWords()
        {
            return this.m_child.GetUsedWords();
        }
    }
}