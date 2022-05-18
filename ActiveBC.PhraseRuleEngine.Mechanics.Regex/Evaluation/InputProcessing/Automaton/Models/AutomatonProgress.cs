using System;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.States;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models
{
    /// <remarks>
    /// Performance remarks: library performance depends on the way the fields in this class are declared.
    /// Please make sure you know what you are doing, when changing any of the fields declaration.
    /// </remarks>
    internal sealed class AutomatonProgress
    {
        public readonly int LastUsedSymbolIndex;
        public Dictionary<string, object?>? CapturedVariables;
        public readonly int ExplicitlyMatchedSymbolsCount;
        public readonly string? Marker;
        public readonly Func<object?>? CapturedValueFactory;
        public readonly RegexAutomatonState State;

        public AutomatonProgress(
            int lastUsedSymbolIndex,
            Dictionary<string, object?>? capturedVariables,
            int explicitlyMatchedSymbolsCount,
            string? marker,
            Func<object?>? capturedValueFactory,
            RegexAutomatonState state
        )
        {
            this.LastUsedSymbolIndex = lastUsedSymbolIndex;
            this.CapturedVariables = capturedVariables;
            this.ExplicitlyMatchedSymbolsCount = explicitlyMatchedSymbolsCount;
            this.Marker = marker;
            this.CapturedValueFactory = capturedValueFactory;
            this.State = state;
        }

        // as we can't now define nullable of nullable,
        // we're forced to use additional parameters to mark if nullable value should be replaces
        public AutomatonProgress Clone(
            RegexAutomatonState state,
            int? lastUsedSymbolIndex = null,
            int? explicitlyMatchedSymbolsCount = null,
            Dictionary<string, object?>? capturedVariables = null,
            bool replaceCapturedVariables = false,
            string? marker = null,
            bool replaceMarker = false,
            Func<object?>? capturedValueFactory = null,
            bool replaceCapturedValueFactory = false
        )
        {
            return new AutomatonProgress(
                lastUsedSymbolIndex ?? this.LastUsedSymbolIndex,
                replaceCapturedVariables ? capturedVariables : this.CapturedVariables?.ToDictionary(),
                explicitlyMatchedSymbolsCount ?? this.ExplicitlyMatchedSymbolsCount,
                replaceMarker ? marker : this.Marker,
                replaceCapturedValueFactory ? capturedValueFactory : this.CapturedValueFactory,
                state
            );
        }

        public RuleMatchResult Flush(RuleInput ruleInput, int firstSymbolIndex)
        {
            return new RuleMatchResult(
                ruleInput.Sequence,
                firstSymbolIndex,
                this.LastUsedSymbolIndex,
                this.CapturedVariables,
                this.ExplicitlyMatchedSymbolsCount,
                this.Marker,
                RuleMatchResult.LazyNull
            );
        }

        public void AddCapturedVariable(string variableName, object? variableValue)
        {
            this.CapturedVariables ??= new Dictionary<string, object?>();

            this.CapturedVariables[variableName] = variableValue;
        }
    }
}