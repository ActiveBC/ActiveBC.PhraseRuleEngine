using System;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Tests.Helpers;

namespace ActiveBC.PhraseRuleEngine.Tests.Fixtures
{
    /// <summary>
    /// Simple grammar which recognizes numbers 0-9 and returns their integer representation.
    /// </summary>
    internal static class Numbers0To9Ner
    {
        public static readonly RuleSetContainer Instance = new RuleSetContainer(
            @"
System.Int32 Number = peg#($Number_0:n_0|$Number_1:n_1|$Number_2:n_2|$Number_3:n_3|$Number_4:n_4|$Number_5:n_5|$Number_6:n_6|$Number_7:n_7|$Number_8:n_8|$Number_9:n_9)# { return ActiveBC.PhraseRuleEngine.Tests.Helpers.Pick.OneOf(n_0, n_1, n_2, n_3, n_4, n_5, n_6, n_7, n_8, n_9); }
System.Int32 Number_0 = peg#(ноль)# => 0
System.Int32 Number_1 = peg#(один)# => 1
System.Int32 Number_2 = peg#(два)# => 2
System.Int32 Number_3 = peg#(три)# => 3
System.Int32 Number_4 = peg#(четыре)# => 4
System.Int32 Number_5 = peg#(пять)# => 5
System.Int32 Number_6 = peg#(шесть)# => 6
System.Int32 Number_7 = peg#(семь)# => 7
System.Int32 Number_8 = peg#(восемь)# => 8
System.Int32 Number_9 = peg#(девять)# => 9",
            new RuleSetToken(
                Array.Empty<UsingToken>(),
                new []
                {
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("System.Int32"), Array.Empty<ICSharpTypeToken>()),
                        "Number",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number_0", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_0",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number_1", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_1",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number_2", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_2",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number_3", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_3",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number_4", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_4",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number_5", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_5",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number_6", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_6",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number_7", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_7",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number_8", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_8",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number_9", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(1, 1),
                                            "n_9",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return ActiveBC.PhraseRuleEngine.Tests.Helpers.Pick.OneOf(n_0, n_1, n_2, n_3, n_4, n_5, n_6, n_7, n_8, n_9); }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("System.Int32"), Array.Empty<ICSharpTypeToken>()),
                        "Number_0",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("ноль"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new ConstantProjectionToken(0)
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("System.Int32"), Array.Empty<ICSharpTypeToken>()),
                        "Number_1",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("один"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new ConstantProjectionToken(1)
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("System.Int32"), Array.Empty<ICSharpTypeToken>()),
                        "Number_2",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("два"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new ConstantProjectionToken(2)
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("System.Int32"), Array.Empty<ICSharpTypeToken>()),
                        "Number_3",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("три"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new ConstantProjectionToken(3)
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("System.Int32"), Array.Empty<ICSharpTypeToken>()),
                        "Number_4",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("четыре"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new ConstantProjectionToken(4)
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("System.Int32"), Array.Empty<ICSharpTypeToken>()),
                        "Number_5",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("пять"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new ConstantProjectionToken(5)
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("System.Int32"), Array.Empty<ICSharpTypeToken>()),
                        "Number_6",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("шесть"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new ConstantProjectionToken(6)
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("System.Int32"), Array.Empty<ICSharpTypeToken>()),
                        "Number_7",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("семь"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new ConstantProjectionToken(7)
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("System.Int32"), Array.Empty<ICSharpTypeToken>()),
                        "Number_8",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("восемь"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new ConstantProjectionToken(8)
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("System.Int32"), Array.Empty<ICSharpTypeToken>()),
                        "Number_9",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("девять"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new ConstantProjectionToken(9)
                    ),
                }
            ),
            new []
            {
                NerEnvironment.Mechanics.Peg,
            }
        );
    }
}