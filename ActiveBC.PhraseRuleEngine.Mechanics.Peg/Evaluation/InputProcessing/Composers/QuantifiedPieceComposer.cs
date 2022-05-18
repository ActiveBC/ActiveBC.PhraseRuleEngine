using System;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Models;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Parsers;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Composers
{
    internal sealed class QuantifiedPieceComposer : IComposer
    {
        private readonly IQuantifiableParser m_quantifiable;
        private readonly QuantifierToken m_quantifier;
        private readonly string? m_variableName;

        public QuantifiedPieceComposer(
            IQuantifiableParser quantifiable,
            QuantifierToken quantifier,
            string? variableName
        )
        {
            this.m_quantifiable = quantifiable;
            this.m_quantifier = quantifier;
            this.m_variableName = variableName;
        }

        public bool Match(
            RuleInput input,
            ref int index,
            in PegInputProcessorDataCollector dataCollector,
            IRuleSpaceCache cache
        )
        {
            QuantifiableResultsCollector? resultsCollector = this.m_variableName is null
                ? null
                : new QuantifiableResultsCollector(this.m_quantifiable.ResultType, this.m_quantifier.Max);

            if (Quantify(input, ref index, dataCollector, resultsCollector, cache))
            {
                if (resultsCollector is not null)
                {
                    dataCollector
                        .CapturedVariables
                        .Add(this.m_variableName!, resultsCollector.GetResult());
                }

                return true;
            }

            return false;
        }

        public IEnumerable<string> GetUsedWords()
        {
            return this.m_quantifiable.GetUsedWords();
        }

        private bool Quantify(
            RuleInput input,
            ref int index,
            in PegInputProcessorDataCollector dataCollector,
            in QuantifiableResultsCollector? resultsCollector,
            IRuleSpaceCache cache
        )
        {
            for (int i = 0; i < this.m_quantifier.Min; i++)
            {
                bool isMatched = this.m_quantifiable.TryParse(
                    input,
                    cache,
                    ref index,
                    out int explicitlyMatchedSymbolsCount,
                    out object? result
                );

                if (isMatched)
                {
                    dataCollector.ExplicitlyMatchedSymbolsCount += explicitlyMatchedSymbolsCount;
                    resultsCollector?.LocalResults.Add(result);
                }
                else
                {
                    return false;
                }
            }

            if (this.m_quantifier.Max is not null)
            {
                for (int i = this.m_quantifier.Min; i < this.m_quantifier.Max.Value; i++)
                {
                    bool isMatched = this.m_quantifiable.TryParse(
                        input,
                        cache,
                        ref index,
                        out int explicitlyMatchedSymbolsCount,
                        out object? result
                    );

                    if (isMatched)
                    {
                        dataCollector.ExplicitlyMatchedSymbolsCount += explicitlyMatchedSymbolsCount;
                        resultsCollector?.LocalResults.Add(result);
                    }
                    else
                    {
                        break;
                    }
                }

                return true;
            }

            while (this.m_quantifiable.TryParse(input, cache, ref index, out int explicitlyMatchedSymbolsCount, out object? result))
            {
                dataCollector.ExplicitlyMatchedSymbolsCount += explicitlyMatchedSymbolsCount;
                resultsCollector?.LocalResults.Add(result);
            }

            return true;
        }

        private class QuantifiableResultsCollector
        {
            private readonly Type m_unaryResultType;
            private readonly bool m_resultTypeIsCollection;
            public List<object?> LocalResults { get; }

            public QuantifiableResultsCollector(Type unaryResultType, int? resultsCount)
            {
                this.m_unaryResultType = unaryResultType;
                this.m_resultTypeIsCollection = resultsCount != 1;
                this.LocalResults = resultsCount is null ? new List<object?>() : new List<object?>(resultsCount.Value);
            }

            public object? GetResult()
            {
                if (!this.m_resultTypeIsCollection)
                {
                    if (this.LocalResults.Count > 1)
                    {
                        throw new PegProcessorMatchException(
                            $"Invalid matching results count {this.LocalResults.Count} (expected 0 or 1)."
                        );
                    }

                    return this.LocalResults.SingleOrDefault();
                }

                (object list, Action<object?> addMethod) = CSharpCodeHelper.CreateGenericList(this.m_unaryResultType);

                foreach (object? unaryResult in this.LocalResults.Where(localResult => localResult is not null))
                {
                    addMethod(unaryResult);
                }

                return list;
            }
        }
    }
}