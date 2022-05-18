using System;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Tests.Helpers;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Tests.Fixtures
{
    internal static class SumOrDifferenceBetweenNumbers0To9Grammar
    {
        public static readonly RuleSpaceSource Instance = new RuleSpaceSource(
            new Dictionary<string, (string Definition, PegGroupToken Token)>()
            {
                {
                    "Expression",
                    (
                        "($Sum|$Difference)",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Sum", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Difference", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        )
                    )
                },
                {
                    "Sum",
                    (
                        "($Number плюс $Number)",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("плюс"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        )
                    )
                },
                {
                    "Difference",
                    (
                        "($Number минус $Number)",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("минус"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        )
                    )
                },
                {
                    "Number",
                    (
                        "([ноль один два три четыре пять шесть семь восемь девять])",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralSetToken(
                                                false,
                                                new ILiteralSetMemberToken[]
                                                {
                                                    new LiteralToken("ноль"),
                                                    new LiteralToken("один"),
                                                    new LiteralToken("два"),
                                                    new LiteralToken("три"),
                                                    new LiteralToken("четыре"),
                                                    new LiteralToken("пять"),
                                                    new LiteralToken("шесть"),
                                                    new LiteralToken("семь"),
                                                    new LiteralToken("восемь"),
                                                    new LiteralToken("девять"),
                                                }
                                            ),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        )
                    )
                },
            }
        );
    }
}