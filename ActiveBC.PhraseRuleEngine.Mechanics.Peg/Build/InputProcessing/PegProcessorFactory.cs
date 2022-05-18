using System;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build;
using ActiveBC.PhraseRuleEngine.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Build.InputProcessing.Models;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.InputProcessing;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result.SelectionStrategy;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Composers;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.Parsers;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing.TerminalDetectors;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.InputProcessing
{
    public sealed class PegProcessorFactory : IInputProcessorFactory
    {
        private readonly IResultSelectionStrategy m_bestReferenceSelectionStrategy;

        public PegProcessorFactory(IResultSelectionStrategy bestReferenceSelectionStrategy)
        {
            this.m_bestReferenceSelectionStrategy = bestReferenceSelectionStrategy;
        }

        public IInputProcessor Create(IPatternToken patternToken, IRuleSpace ruleSpace)
        {
            PegGroupToken pegGroupToken = (PegGroupToken) patternToken;

            return new PegProcessor(CreateGroupComposer(pegGroupToken, ruleSpace, false));
        }

        public RuleCapturedVariables ExtractOwnCapturedVariables(
            IPatternToken patternToken,
            IRuleDescriptionProvider ruleDescriptionProvider
        )
        {
            PegGroupToken group = (PegGroupToken) patternToken;

            SortedDictionary<string, Type> variableTypeByVariableName = new SortedDictionary<string, Type>();

            bool hasManyBranches = group.Branches.Length > 1;

            foreach (BranchToken branch in group.Branches)
            {
                foreach (BranchItemToken branchItem in branch.Items.Where(item => item.VariableName is not null))
                {
                    Type originalVariableType = GetVariableType(branchItem.Quantifiable);

                    Type actualVariableType = GetBranchesCountModifiedType(
                        GetQuantifiedType(originalVariableType, branchItem.Quantifier)
                    );

                    variableTypeByVariableName.Add(branchItem.VariableName!, actualVariableType);
                }
            }

            return new RuleCapturedVariables(variableTypeByVariableName, Array.Empty<string>());

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
                    RuleReferenceToken ruleReference => ruleDescriptionProvider[ruleReference.GetRuleSpaceKey()],
                    _ => throw new PegProcessorBuildException($"Unknown quantifiable type {quantifiable.GetType().FullName}."),
                };
            }

            Type GetQuantifiedType(Type type, QuantifierToken quantifier)
            {
                if (quantifier.Max is null or > 1)
                {
                    return typeof(IReadOnlyCollection<>).MakeGenericType(type);
                }

                if (quantifier.Min == 0 && quantifier.Max == 1)
                {
                    return MakeNullableIfPossible(type);
                }

                return type;
            }

            Type GetBranchesCountModifiedType(Type type)
            {
                if (hasManyBranches)
                {
                    return MakeNullableIfPossible(type);
                }

                return type;
            }

            Type MakeNullableIfPossible(Type type)
            {
                if (type.IsValueType && !IsNullable())
                {
                    return typeof(Nullable<>).MakeGenericType(type);
                }

                return type;

                bool IsNullable()
                {
                    return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
                }
            }
        }

        private OrderedChoiceComposer CreateGroupComposer(PegGroupToken group, IRuleSpace ruleSpace, bool isPartOfNestedGroup)
        {
            return new OrderedChoiceComposer(
                group
                    .Branches
                    .Select(
                        branch => new SequenceComposer(
                            branch
                                .Items
                                .Select<BranchItemToken, IComposer>(
                                    branchItem =>
                                    {
                                        if (branchItem.VariableName is not null)
                                        {
                                            if (isPartOfNestedGroup)
                                            {
                                                throw new PegProcessorBuildException(
                                                    $"Variable capturing is not allowed in nested groups " +
                                                    $"(variable name '{branchItem.VariableName}')."
                                                );
                                            }

                                            if (branchItem.Quantifiable is PegGroupToken)
                                            {
                                                throw new PegProcessorBuildException(
                                                    $"Group is not capturable " +
                                                    $"(variable name '{branchItem.VariableName}')."
                                                );
                                            }

                                            if (branchItem.Lookahead is not null)
                                            {
                                                throw new PegProcessorBuildException(
                                                    $"Capturing lookahead items is not allowed " +
                                                    $"(variable name '{branchItem.VariableName}')."
                                                );
                                            }
                                        }

                                        if (branchItem.Quantifier.Min < 0)
                                        {
                                            throw new PegProcessorBuildException(
                                                $"Min value of quantifier must be greater or equal to zero, " +
                                                $"'{branchItem.Quantifier}' given."
                                            );
                                        }

                                        if (branchItem.Quantifier.Max < 1)
                                        {
                                            throw new PegProcessorBuildException(
                                                $"Max value of quantifier must be greater or equal to one, " +
                                                $"'{branchItem.Quantifier}' given."
                                            );
                                        }

                                        if (branchItem.Quantifier.Min > branchItem.Quantifier.Max)
                                        {
                                            throw new PegProcessorBuildException(
                                                $"Max value of quantifier must be greater or equal to min value, " +
                                                $"'{branchItem.Quantifier}' given."
                                            );
                                        }

                                        QuantifiedPieceComposer quantifiedPieceComposer = new QuantifiedPieceComposer(
                                            branchItem.Quantifiable switch
                                            {
                                                ITerminalToken terminalToken => new TerminalParser(
                                                    terminalToken switch
                                                    {
                                                        AnyLiteralToken => AnyLiteralDetector.Instance,
                                                        LiteralSetToken literalSet => new LiteralSetDetector(literalSet),
                                                        LiteralToken literal => new LiteralDetector(literal),
                                                        PrefixToken prefix => new PrefixDetector(prefix),
                                                        InfixToken infix => new InfixDetector(infix),
                                                        SuffixToken suffix => new SuffixDetector(suffix),
                                                        _ => throw new PegProcessorBuildException(
                                                            $"Unknown terminal type {terminalToken.GetType().FullName}."
                                                        ),
                                                    }
                                                ),
                                                RuleReferenceToken ruleReferenceToken => new RuleReferenceParser(
                                                    ruleReferenceToken,
                                                    this.m_bestReferenceSelectionStrategy,
                                                    ruleSpace
                                                ),
                                                PegGroupToken groupToken => new GroupParser(
                                                    CreateGroupComposer(groupToken, ruleSpace, true)
                                                ),
                                                _ => throw new PegProcessorBuildException(
                                                    $"Unknown quantifiable type " +
                                                    $"{branchItem.Quantifiable.GetType().FullName}."
                                                ),
                                            },
                                            branchItem.Quantifier,
                                            branchItem.VariableName
                                        );

                                        if (branchItem.Lookahead is not null)
                                        {
                                            return new LookaheadComposer(branchItem.Lookahead, quantifiedPieceComposer);
                                        }

                                        return quantifiedPieceComposer;
                                    }
                                )
                                .ToArray()
                        )
                    )
                    .ToArray()
            );
        }
    }
}