using System;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Tests.Helpers;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Tests.Fixtures
{
    internal static class ArithmeticOperationsOnNumbers0To9Grammar
    {
        public static readonly RuleSpaceSource Instance = new RuleSpaceSource(
            new Dictionary<string, (string Definition, PegGroupToken Token)>()
            {
                {
                    "Expression",
                    (
                        "($Sum)",
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
                            }
                        )
                    )
                },
                {
                    "Sum",
                    (
                        "($Product $SumAfterProduct*)",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Product", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "SumAfterProduct", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(0, null),
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
                    "SumAfterProduct",
                    (
                        "([плюс минус] $Product)",
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
                                                    new LiteralToken("плюс"),
                                                    new LiteralToken("минус"),
                                                }
                                            ),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Product", Array.Empty<IRuleArgumentToken>()),
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
                    "Product",
                    (
                        "($Value $ProductAfterValue*)",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Value", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "ProductAfterValue", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(0, null),
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
                    "ProductAfterValue",
                    (
                        "([умножить поделить] на $Value)",
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
                                                    new LiteralToken("умножить"),
                                                    new LiteralToken("поделить"),
                                                }
                                            ),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("на"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Value", Array.Empty<IRuleArgumentToken>()),
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
                    "Value",
                    (
                        "($Number|открывающаяся скобка $Expression закрывающаяся скобка)",
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
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("открывающаяся"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("скобка"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Expression", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("закрывающаяся"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("скобка"),
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
                }
            }
        );
    }
}