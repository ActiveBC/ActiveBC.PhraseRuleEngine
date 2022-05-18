using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Models;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Composers
{
    internal sealed class SequenceComposer : IComposer
    {
        private readonly IReadOnlyCollection<IComposer> m_pieces;

        public SequenceComposer(IReadOnlyCollection<IComposer> pieces)
        {
            this.m_pieces = pieces;
        }

        public bool Match(
            RuleInput input,
            ref int index,
            in PegInputProcessorDataCollector dataCollector,
            IRuleSpaceCache cache
        )
        {
            foreach (IComposer piece in this.m_pieces)
            {
                if (!piece.Match(input, ref index, dataCollector, cache))
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<string> GetUsedWords()
        {
            return this.m_pieces.SelectMany(choice => choice.GetUsedWords());
        }
    }
}