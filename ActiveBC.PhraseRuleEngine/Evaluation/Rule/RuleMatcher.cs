using System;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.InputProcessing;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Rule
{
    internal sealed class RuleMatcher : IRuleMatcher
    {
        private readonly IInputProcessor m_inputProcessor;
        private readonly IRuleProjection m_projection;
        private readonly CapturedVariablesParameters m_capturedVariablesParameters;

        public RuleParameters Parameters { get; }
        public RuleMatchResultDescription ResultDescription { get; }

        public RuleMatcher(
            IInputProcessor inputProcessor,
            RuleParameters ruleParameters,
            CapturedVariablesParameters capturedVariablesParameters,
            RuleMatchResultDescription resultDescription,
            IRuleProjection projection
        )
        {
            this.m_inputProcessor = inputProcessor;
            this.Parameters = ruleParameters;
            this.m_capturedVariablesParameters = capturedVariablesParameters;
            this.ResultDescription = resultDescription;
            this.m_projection = projection;
        }

        public RuleMatchResultCollection Match(RuleInput input, int firstSymbolIndex, IRuleSpaceCache cache)
        {
            return this.m_inputProcessor.Match(input, firstSymbolIndex, cache);
        }

        public RuleMatchResultCollection MatchAndProject(
            RuleInput input,
            int firstSymbolIndex,
            RuleArguments ruleArguments,
            IRuleSpaceCache cache
        )
        {
            RuleMatchResultCollection inputProcessorResult = Match(input, firstSymbolIndex, cache);

            return new RuleMatchResultCollection(
                inputProcessorResult
                    .Select(
                        result => new RuleMatchResult(
                            result.Source,
                            result.FirstUsedSymbolIndex,
                            result.LastUsedSymbolIndex,
                            result.CapturedVariables,
                            result.ExplicitlyMatchedSymbolsCount,
                            result.Marker,
                            new Lazy<object?>(
                                () => ProjectionFactory.GetProjectionResult(
                                    result,
                                    this.m_capturedVariablesParameters,
                                    input,
                                    firstSymbolIndex,
                                    ruleArguments,
                                    this.m_projection
                                )
                            )
                        )
                    ),
                inputProcessorResult.Count
            );
        }

        public IEnumerable<string> GetUsedWords()
        {
            return this.m_inputProcessor.GetUsedWords();
        }
    }
}