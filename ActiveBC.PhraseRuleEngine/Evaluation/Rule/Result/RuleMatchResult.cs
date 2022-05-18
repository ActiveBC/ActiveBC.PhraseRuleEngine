using System;
using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result
{
    public sealed class RuleMatchResult
    {
        public static readonly Lazy<object?> LazyNull = new Lazy<object?>(() => null);

        public IReadOnlyCollection<string> Source { get; }
        public int FirstUsedSymbolIndex { get; }
        public int LastUsedSymbolIndex { get; }
        public IReadOnlyDictionary<string, object?>? CapturedVariables { get; }
        public int ExplicitlyMatchedSymbolsCount { get; }
        public string? Marker { get; }
        public Lazy<object?> Result { get; }

        public RuleMatchResult(
            IReadOnlyCollection<string> source,
            int firstUsedSymbolIndex,
            int lastUsedSymbolIndex,
            IReadOnlyDictionary<string, object?>? capturedVariables,
            int explicitlyMatchedSymbolsCount,
            string? marker,
            Lazy<object?> result
        )
        {
            this.Source = source;
            this.FirstUsedSymbolIndex = firstUsedSymbolIndex;
            this.LastUsedSymbolIndex = lastUsedSymbolIndex;
            this.CapturedVariables = capturedVariables;
            this.ExplicitlyMatchedSymbolsCount = explicitlyMatchedSymbolsCount;
            this.Marker = marker;
            this.Result = result;
        }

        public bool TryGetCapturedVariable(string variableName, out object? variable)
        {
            if (this.CapturedVariables is not null && this.CapturedVariables.TryGetValue(variableName, out object? capturedVariable))
            {
                variable = capturedVariable;
                return true;
            }

            variable = null;
            return false;
        }
    }
}