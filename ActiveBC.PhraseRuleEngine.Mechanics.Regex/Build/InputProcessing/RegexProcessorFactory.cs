using System;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build;
using ActiveBC.PhraseRuleEngine.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Build.InputProcessing.Models;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.InputProcessing;
using ActiveBC.PhraseRuleEngine.Exceptions;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing.Automaton;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing.Automaton.Optimization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Models;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Evaluation.InputProcessing.Automaton.Walker;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.InputProcessing
{
    public sealed class RegexProcessorFactory : IInputProcessorFactory
    {
        private readonly OptimizationLevel m_optimizationLevel;

        public RegexProcessorFactory(OptimizationLevel optimizationLevel)
        {
            this.m_optimizationLevel = optimizationLevel;
        }

        public IInputProcessor Create(IPatternToken patternToken, IRuleSpace ruleSpace)
        {
            RegexGroupToken group = (RegexGroupToken) patternToken;

            try
            {
                RegexAutomaton automaton = new RegexAutomatonBuilder(group, ruleSpace).Build();

                RegexAutomatonPostprocessor.Instance.ValidateAndOptimize(automaton, this.m_optimizationLevel);

                return new RegexProcessor(automaton, RegexAutomatonWalker.Instance);
            }
            catch (Exception exception) when (exception is not RegexProcessorBuildException)
            {
                throw new RegexProcessorBuildException("Failed to create rule matcher.", exception);
            }
        }

        public RuleCapturedVariables ExtractOwnCapturedVariables(
            IPatternToken patternToken,
            IRuleDescriptionProvider ruleDescriptionProvider
        )
        {
            RegexGroupToken group = (RegexGroupToken) patternToken;

            Dictionary<string, Type> variables = new Dictionary<string, Type>();
            List<string> references = new List<string>();

            RecursivelySearchForVariables(group, ruleDescriptionProvider, variables, references);

            return new RuleCapturedVariables(variables, references);
        }

        private static void RecursivelySearchForVariables(
            RegexGroupToken group,
            IRuleDescriptionProvider ruleDescriptionProvider,
            in Dictionary<string, Type> collectedVariables,
            in List<string> collectedReferences
        )
        {
            IEnumerable<QuantifiableBranchItemToken> suitableBranchItems = group
                .Branches
                .SelectMany(branch => branch.Items)
                .OfType<QuantifiableBranchItemToken>();

            foreach (QuantifiableBranchItemToken branchItem in suitableBranchItems)
            {
                if (branchItem.VariableName is not null && branchItem.Quantifiable is RegexGroupToken)
                {
                    throw new RuleBuildException("Group capturing is not allowed.");
                }

                if (branchItem.Quantifiable is RegexGroupToken regexGroupToken)
                {
                    RecursivelySearchForVariables(
                        regexGroupToken,
                        ruleDescriptionProvider,
                        collectedVariables,
                        collectedReferences
                    );
                }
                else if (branchItem.VariableName is null && branchItem.Quantifiable is RuleReferenceToken ruleReference)
                {
                    collectedReferences.Add(ruleReference.GetRuleSpaceKey());
                }
                else if (branchItem.VariableName is not null)
                {
                    Type type = GetVariableType(branchItem.Quantifiable);

                    if (collectedVariables.TryGetValue(branchItem.VariableName, out Type? existing))
                    {
                        if (existing != type)
                        {
                            throw new RegexProcessorBuildException(
                                $"Variable '{branchItem.VariableName}' is declared with different types in different branches: " +
                                $"'{existing.FullName}' and '{type.FullName}'."
                            );
                        }
                    }
                    else
                    {
                        collectedVariables.Add(branchItem.VariableName, type);
                    }
                }
            }

            Type GetVariableType(IQuantifiableToken quantifiable)
            {
                return quantifiable switch
                {
                    AnyLiteralToken => typeof(string),
                    LiteralSetToken => typeof(string),
                    LiteralToken => typeof(string),
                    PrefixToken => typeof(string),
                    InfixToken => typeof(string),
                    SuffixToken => typeof(string),
                    RuleReferenceToken ruleReference => GetRuleResultTypeDescription(ruleReference),
                    _ => throw new RegexProcessorBuildException($"Unknown quantifiable type {quantifiable.GetType().FullName}."),
                };

                Type GetRuleResultTypeDescription(RuleReferenceToken ruleReference)
                {
                    string ruleKey = ruleReference.GetRuleSpaceKey();

                    if (ruleDescriptionProvider.TryGet(ruleKey, out Type? resultType))
                    {
                        return resultType;
                    }

                    throw new RegexProcessorBuildException(
                        $"Rule with name '{ruleReference.RuleName}' and key '{ruleKey}' does not exist."
                    );
                }
            }
        }
    }
}