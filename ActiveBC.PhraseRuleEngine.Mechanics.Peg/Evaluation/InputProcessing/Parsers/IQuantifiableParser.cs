using System;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Reflection;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Parsers
{
    internal interface IQuantifiableParser : IUsedWordsProvider
    {
        Type ResultType { get; }
        bool TryParse(
            RuleInput input,
            IRuleSpaceCache cache,
            ref int index,
            out int explicitlyMatchedSymbolsCount,
            out object? result
        );
    }
}