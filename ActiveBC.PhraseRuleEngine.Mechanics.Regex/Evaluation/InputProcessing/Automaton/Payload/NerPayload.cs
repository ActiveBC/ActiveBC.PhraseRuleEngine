using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.ArgumentsBinding;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models.States;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Payload
{
    internal sealed class NerPayload : ITransitionPayload
    {
        public bool IsTransient => false;

        public readonly string RuleKey;
        public readonly IRuleArgumentToken[] RuleArguments;
        private readonly IRuleSpace m_ruleSpace;

        private IRuleMatcher? m_matcher;
        private IRuleMatcher Matcher => this.m_matcher ??= this.m_ruleSpace[this.RuleKey];

        public NerPayload(RuleReferenceToken ruleReference, IRuleSpace ruleSpace)
        {
            this.RuleKey = ruleReference.GetRuleSpaceKey();
            this.RuleArguments = ruleReference.Arguments;
            this.m_ruleSpace = ruleSpace;
        }

        public void Consume(
            RuleInput input,
            RegexAutomatonState targetState,
            AutomatonProgress currentProgress,
            IRuleSpaceCache cache,
            in Stack<AutomatonProgress> progresses
        )
        {
            RuleMatchResultCollection resultCollection = this
                .Matcher
                .MatchAndProject(
                    input,
                    currentProgress.LastUsedSymbolIndex + 1,
                    ArgumentsBinder.BindRuleArguments(
                        this.Matcher.Parameters,
                        input.RuleSpaceArguments,
                        this.RuleArguments
                    ),
                    cache
                );

            if (resultCollection.Count == 0)
            {
                return;
            }

            foreach (RuleMatchResult result in resultCollection)
            {
                progresses.Push(
                    currentProgress.Clone(
                        targetState,
                        lastUsedSymbolIndex: result.LastUsedSymbolIndex,
                        explicitlyMatchedSymbolsCount: currentProgress.ExplicitlyMatchedSymbolsCount + result.ExplicitlyMatchedSymbolsCount,
                        replaceCapturedValueFactory: true,
                        capturedValueFactory: () => result.Result.Value
                    )
                );
            }
        }

        public IEnumerable<string> GetUsedWords()
        {
            return this.Matcher.GetUsedWords();
        }
    }
}