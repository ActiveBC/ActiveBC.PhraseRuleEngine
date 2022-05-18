using System;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Composers;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Models;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Parsers
{
    internal sealed class GroupParser : IQuantifiableParser
    {
        public Type ResultType { get; } = typeof(void);

        private readonly OrderedChoiceComposer m_composer;

        public GroupParser(OrderedChoiceComposer composer)
        {
            this.m_composer = composer;
        }

        public bool TryParse(
            RuleInput input,
            IRuleSpaceCache cache,
            ref int index,
            out int explicitlyMatchedSymbolsCount,
            out object? result
        )
        {
            result = null;

            PegInputProcessorDataCollector dataCollector = new PegInputProcessorDataCollector();

            bool isMatched = this.m_composer.Match(input, ref index, dataCollector, cache);

            explicitlyMatchedSymbolsCount = dataCollector.ExplicitlyMatchedSymbolsCount;

            return isMatched;
        }

        public IEnumerable<string> GetUsedWords()
        {
            return this.m_composer.GetUsedWords();
        }
    }
}