using System;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Tests.Helpers;

namespace ActiveBC.PhraseRuleEngine.Tests.Fixtures
{
    internal static class BranchesWithOptionalLiteralValueTypeNer
    {
        public static readonly RuleSetContainer Instance = new RuleSetContainer(
            @"
using System;
using ActiveBC.PhraseRuleEngine.Tests.Helpers;

int Number_1 = peg#(один)# { return 1; }
int Number_2 = peg#(два)# { return 2; }
Nullable<int> ValueType_BranchesWithOptionalLiteral = peg#(старт $Number_1?:n_1 финиш|старт $Number_2?:n_2 финиш)# { return n_1 ?? n_2; }",
            new RuleSetToken(
                new []
                {
                    new UsingToken("System"),
                    new UsingToken("ActiveBC.PhraseRuleEngine.Tests.Helpers"),
                },
                new []
                {
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(
                            new CSharpTypeNameWithNamespaceToken("int"),
                            Array.Empty<ICSharpTypeToken>()
                        ),
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
                                        )
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 1; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(
                            new CSharpTypeNameWithNamespaceToken("int"),
                            Array.Empty<ICSharpTypeToken>()
                        ),
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
                                        )
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return 2; }")
                    ),
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(
                            new CSharpTypeNameWithNamespaceToken("Nullable"),
                            new ICSharpTypeToken[]
                            {
                                new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("int"), Array.Empty<ICSharpTypeToken>())
                            }
                        ),
                        "ValueType_BranchesWithOptionalLiteral",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("старт"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number_1", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(0, 1),
                                            "n_1",
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("финиш"),
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
                                            new LiteralToken("старт"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number_2", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(0, 1),
                                            "n_2",
                                            null
                                        ),
                                        new BranchItemToken(
                                            new LiteralToken("финиш"),
                                            new QuantifierToken(1, 1),
                                            null,
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return n_1 ?? n_2; }")
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