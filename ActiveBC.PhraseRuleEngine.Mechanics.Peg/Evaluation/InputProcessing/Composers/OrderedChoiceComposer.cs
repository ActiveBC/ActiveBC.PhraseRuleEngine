using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Models;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Composers
{
    internal sealed class OrderedChoiceComposer : IComposer
    {
        private readonly IReadOnlyCollection<IComposer> m_choices;

        public OrderedChoiceComposer(IReadOnlyCollection<IComposer> choices)
        {
            this.m_choices = choices;
        }

        public bool Match(
            RuleInput input,
            ref int index,
            in PegInputProcessorDataCollector dataCollector,
            IRuleSpaceCache cache
        )
        {
            foreach (IComposer choice in this.m_choices)
            {
                int choiceBoundIndex = index;

                if (choice.Match(input, ref choiceBoundIndex, dataCollector, cache))
                {
                    index = choiceBoundIndex;
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<string> GetUsedWords()
        {
            return this.m_choices.SelectMany(choice => choice.GetUsedWords());
        }
    }
}