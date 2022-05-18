using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.InputProcessing;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Composers;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Models;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing
{
    internal sealed class PegProcessor : IInputProcessor
    {
        private readonly OrderedChoiceComposer m_root;

        public PegProcessor(OrderedChoiceComposer root)
        {
            this.m_root = root;
        }

        public RuleMatchResultCollection Match(RuleInput ruleInput, int firstSymbolIndex, IRuleSpaceCache cache)
        {
            PegInputProcessorDataCollector dataCollector = new PegInputProcessorDataCollector();

            int nextSymbolIndex = firstSymbolIndex;

            bool isMatched = this.m_root.Match(ruleInput, ref nextSymbolIndex, dataCollector, cache);

            if (isMatched)
            {
                return new RuleMatchResultCollection(
                    new []
                    {
                        new RuleMatchResult(
                            ruleInput.Sequence,
                            firstSymbolIndex,
                            nextSymbolIndex - 1,
                            dataCollector.CapturedVariables,
                            dataCollector.ExplicitlyMatchedSymbolsCount,
                            null,
                            RuleMatchResult.LazyNull
                        ),
                    }
                );
            }

            return new RuleMatchResultCollection(0);
        }

        public IEnumerable<string> GetUsedWords()
        {
            return this.m_root.GetUsedWords();
        }
    }
}