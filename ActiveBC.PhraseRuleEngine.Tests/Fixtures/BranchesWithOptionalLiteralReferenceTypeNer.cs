using System;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Tests.Helpers;

namespace ActiveBC.PhraseRuleEngine.Tests.Fixtures
{
    internal static class BranchesWithOptionalLiteralReferenceTypeNer
    {
        public static readonly RuleSetContainer Instance = new RuleSetContainer(
            "string ReferenceType_BranchesWithOptionalLiteral = peg#(старт привет?:word_1 финиш|старт пока?:word_2 финиш)# { return word_1 ?? word_2; }",
            new RuleSetToken(
                Array.Empty<UsingToken>(),
                new []
                {
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(
                            new CSharpTypeNameWithNamespaceToken("string"),
                            Array.Empty<ICSharpTypeToken>()
                        ),
                        "ReferenceType_BranchesWithOptionalLiteral",
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
                                            new LiteralToken("привет"),
                                            new QuantifierToken(0, 1),
                                            "word_1",
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
                                            new LiteralToken("пока"),
                                            new QuantifierToken(0, 1),
                                            "word_2",
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
                        new BodyBasedProjectionToken("{ return word_1 ?? word_2; }")
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