using System;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Tests.Helpers;
using Peg = ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using Regex = ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Tests.Fixtures
{
    internal static class VoidPatternsNer
    {
        public static readonly RuleSetContainer Instance = new RuleSetContainer(
            @"
void Rule1_3 = peg#(один:one два три)# {}
void Rule4_6 = peg#(четыре пять:two шесть)# {}
void Rule7_9 = peg#(семь восемь девять:three)# {}
void Rule_Foo = peg#(число $Number:n1 больше чем число $Number:n2)# {}
void Rule_Bar = regex#(число <n1 = self.Number()# больше чем число <n2 = self.Number()#)# {}
void Rule_Baz = regex#(число <n1 = self.Number()# больше чем число <n2 = self.Number()# | число <n3 = self.Number()# меньше чем число <n4 = self.Number()#)# {}

int Number = peg#($Number_0:n_0|$Number_1:n_1|$Number_2:n_2|$Number_3:n_3|$Number_4:n_4|$Number_5:n_5|$Number_6:n_6|$Number_7:n_7|$Number_8:n_8|$Number_9:n_9)# { return ActiveBC.PhraseRuleEngine.Tests.Helpers.Pick.OneOf(n_0, n_1, n_2, n_3, n_4, n_5, n_6, n_7, n_8, n_9); }
int Number_0 = peg#(ноль)# { return 0; }
int Number_1 = peg#(один)# { return 1; }
int Number_2 = peg#(два)# { return 2; }
int Number_3 = peg#(три)# { return 3; }
int Number_4 = peg#(четыре)# { return 4; }
int Number_5 = peg#(пять)# { return 5; }
int Number_6 = peg#(шесть)# { return 6; }
int Number_7 = peg#(семь)# { return 7; }
int Number_8 = peg#(восемь)# { return 8; }
int Number_9 = peg#(девять)# { return 9; }
",
            new RuleSetToken(
                Array.Empty<UsingToken>(),
                new []
                {
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                        "Rule1_3",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new Peg.PegGroupToken(
                            new []
                            {
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("один"),
                                            new Peg.QuantifierToken(1, 1),
                                            "one",
                                            null
                                        ),
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("два"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("три"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        )
                                    }
                                )
                            }
                        ),
                        VoidProjectionToken.Instance
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                        "Rule4_6",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new Peg.PegGroupToken(
                            new []
                            {
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("четыре"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("пять"),
                                            new Peg.QuantifierToken(1, 1),
                                            "two",
                                            null
                                        ),
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("шесть"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        )
                                    }
                                )
                            }
                        ),
                        VoidProjectionToken.Instance
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                        "Rule7_9",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new Peg.PegGroupToken(
                            new []
                            {
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("семь"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("восемь"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("девять"),
                                            new Peg.QuantifierToken(1, 1),
                                            "three",
                                            null
                                        )
                                    }
                                )
                            }
                        ),
                        VoidProjectionToken.Instance
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                        "Rule_Foo",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new Peg.PegGroupToken(
                            new []
                            {
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("число"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new Peg.BranchItemToken(
                                            new Peg.RuleReferenceToken(null, "Number", Array.Empty<IRuleArgumentToken>()),
                                            new Peg.QuantifierToken(1, 1),
                                            "n1",
                                            null
                                        ),
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("больше"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("чем"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("число"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new Peg.BranchItemToken(
                                            new Peg.RuleReferenceToken(null, "Number", Array.Empty<IRuleArgumentToken>()),
                                            new Peg.QuantifierToken(1, 1),
                                            "n2",
                                            null
                                        )
                                    }
                                )
                            }
                        ),
                        VoidProjectionToken.Instance
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                        "Rule_Bar",
                        Array.Empty<CSharpParameterToken>(),
                        "regex",
                        new Regex.RegexGroupToken(
                            new []
                            {
                                new Regex.BranchToken(
                                    new Regex.IBranchItemToken[]
                                    {
                                        new Regex.QuantifiableBranchItemToken(new Regex.LiteralToken("число"), new Regex.QuantifierToken(1, 1), null),
                                        new Regex.QuantifiableBranchItemToken(new Regex.RuleReferenceToken(null, "Number", Array.Empty<IRuleArgumentToken>()), new Regex.QuantifierToken(1, 1), "n1"),
                                        new Regex.QuantifiableBranchItemToken(new Regex.LiteralToken("больше"), new Regex.QuantifierToken(1, 1), null),
                                        new Regex.QuantifiableBranchItemToken(new Regex.LiteralToken("чем"), new Regex.QuantifierToken(1, 1), null),
                                        new Regex.QuantifiableBranchItemToken(new Regex.LiteralToken("число"), new Regex.QuantifierToken(1, 1), null),
                                        new Regex.QuantifiableBranchItemToken(new Regex.RuleReferenceToken(null, "Number", Array.Empty<IRuleArgumentToken>()), new Regex.QuantifierToken(1, 1), "n2")
                                    }
                                )
                            }
                        ),
                        VoidProjectionToken.Instance
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                        "Rule_Baz",
                        Array.Empty<CSharpParameterToken>(),
                        "regex",
                        new Regex.RegexGroupToken(
                            new []
                            {
                                new Regex.BranchToken(
                                    new Regex.IBranchItemToken[]
                                    {
                                        new Regex.QuantifiableBranchItemToken(new Regex.LiteralToken("число"), new Regex.QuantifierToken(1, 1), null),
                                        new Regex.QuantifiableBranchItemToken(new Regex.RuleReferenceToken(null, "Number", Array.Empty<IRuleArgumentToken>()), new Regex.QuantifierToken(1, 1), "n1"),
                                        new Regex.QuantifiableBranchItemToken(new Regex.LiteralToken("больше"), new Regex.QuantifierToken(1, 1), null),
                                        new Regex.QuantifiableBranchItemToken(new Regex.LiteralToken("чем"), new Regex.QuantifierToken(1, 1), null),
                                        new Regex.QuantifiableBranchItemToken(new Regex.LiteralToken("число"), new Regex.QuantifierToken(1, 1), null),
                                        new Regex.QuantifiableBranchItemToken(new Regex.RuleReferenceToken(null, "Number", Array.Empty<IRuleArgumentToken>()), new Regex.QuantifierToken(1, 1), "n2")
                                    }
                                ),
                                new Regex.BranchToken(
                                    new Regex.IBranchItemToken[]
                                    {
                                        new Regex.QuantifiableBranchItemToken(new Regex.LiteralToken("число"), new Regex.QuantifierToken(1, 1), null),
                                        new Regex.QuantifiableBranchItemToken(new Regex.RuleReferenceToken(null, "Number", Array.Empty<IRuleArgumentToken>()), new Regex.QuantifierToken(1, 1), "n3"),
                                        new Regex.QuantifiableBranchItemToken(new Regex.LiteralToken("меньше"), new Regex.QuantifierToken(1, 1), null),
                                        new Regex.QuantifiableBranchItemToken(new Regex.LiteralToken("чем"), new Regex.QuantifierToken(1, 1), null),
                                        new Regex.QuantifiableBranchItemToken(new Regex.LiteralToken("число"), new Regex.QuantifierToken(1, 1), null),
                                        new Regex.QuantifiableBranchItemToken(new Regex.RuleReferenceToken(null, "Number", Array.Empty<IRuleArgumentToken>()), new Regex.QuantifierToken(1, 1), "n4")
                                    }
                                )
                            }
                        ),
                        VoidProjectionToken.Instance
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Number",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new Peg.PegGroupToken(
                            new []
                            {
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.RuleReferenceToken(null, "Number_0", Array.Empty<IRuleArgumentToken>()),
                                            new Peg.QuantifierToken(1, 1),
                                            "n_0",
                                            null
                                        )
                                    }
                                ),
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.RuleReferenceToken(null, "Number_1", Array.Empty<IRuleArgumentToken>()),
                                            new Peg.QuantifierToken(1, 1),
                                            "n_1",
                                            null
                                        )
                                    }
                                ),
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.RuleReferenceToken(null, "Number_2", Array.Empty<IRuleArgumentToken>()),
                                            new Peg.QuantifierToken(1, 1),
                                            "n_2",
                                            null
                                        )
                                    }
                                ),
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.RuleReferenceToken(null, "Number_3", Array.Empty<IRuleArgumentToken>()),
                                            new Peg.QuantifierToken(1, 1),
                                            "n_3",
                                            null
                                        )
                                    }
                                ),
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.RuleReferenceToken(null, "Number_4", Array.Empty<IRuleArgumentToken>()),
                                            new Peg.QuantifierToken(1, 1),
                                            "n_4",
                                            null
                                        )
                                    }
                                ),
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.RuleReferenceToken(null, "Number_5", Array.Empty<IRuleArgumentToken>()),
                                            new Peg.QuantifierToken(1, 1),
                                            "n_5",
                                            null
                                        )
                                    }
                                ),
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.RuleReferenceToken(null, "Number_6", Array.Empty<IRuleArgumentToken>()),
                                            new Peg.QuantifierToken(1, 1),
                                            "n_6",
                                            null
                                        )
                                    }
                                ),
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.RuleReferenceToken(null, "Number_7", Array.Empty<IRuleArgumentToken>()),
                                            new Peg.QuantifierToken(1, 1),
                                            "n_7",
                                            null
                                        )
                                    }
                                ),
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.RuleReferenceToken(null, "Number_8", Array.Empty<IRuleArgumentToken>()),
                                            new Peg.QuantifierToken(1, 1),
                                            "n_8",
                                            null
                                        )
                                    }
                                ),
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.RuleReferenceToken(null, "Number_9", Array.Empty<IRuleArgumentToken>()),
                                            new Peg.QuantifierToken(1, 1),
                                            "n_9",
                                            null
                                        )
                                    }
                                )
                            }
                        ),
                        new BodyBasedProjectionToken(
                            "{ return ActiveBC.PhraseRuleEngine.Tests.Helpers.Pick.OneOf(n_0, n_1, n_2, n_3, n_4, n_5, n_6, n_7, n_8, n_9); }"
                        )
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Number_0",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new Peg.PegGroupToken(
                            new []
                            {
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("ноль"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        )
                                    }
                                )
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 0; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Number_1",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new Peg.PegGroupToken(
                            new []
                            {
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("один"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        )
                                    }
                                )
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 1; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Number_2",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new Peg.PegGroupToken(
                            new []
                            {
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("два"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        )
                                    }
                                )
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 2; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Number_3",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new Peg.PegGroupToken(
                            new []
                            {
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("три"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        )
                                    }
                                )
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 3; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Number_4",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new Peg.PegGroupToken(
                            new []
                            {
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("четыре"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        )
                                    }
                                )
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 4; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Number_5",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new Peg.PegGroupToken(
                            new []
                            {
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("пять"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        )
                                    }
                                )
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 5; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Number_6",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new Peg.PegGroupToken(
                            new []
                            {
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("шесть"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        )
                                    }
                                )
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 6; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Number_7",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new Peg.PegGroupToken(
                            new []
                            {
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("семь"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        )
                                    }
                                )
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 7; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Number_8",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new Peg.PegGroupToken(
                            new []
                            {
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("восемь"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        )
                                    }
                                )
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 8; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>()),
                        "Number_9",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new Peg.PegGroupToken(
                            new []
                            {
                                new Peg.BranchToken(
                                    new []
                                    {
                                        new Peg.BranchItemToken(
                                            new Peg.LiteralToken("девять"),
                                            new Peg.QuantifierToken(1, 1),
                                            null,
                                            null
                                        )
                                    }
                                )
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 9; }")
                    )
                }
            ),
            new []
            {
                NerEnvironment.Mechanics.Peg,
                NerEnvironment.Mechanics.Regex,
            }
        );
    }
}