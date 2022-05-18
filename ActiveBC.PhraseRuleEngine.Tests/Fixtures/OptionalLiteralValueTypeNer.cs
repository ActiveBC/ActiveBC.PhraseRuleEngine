using System;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Tests.Helpers;

namespace ActiveBC.PhraseRuleEngine.Tests.Fixtures
{
    internal static class OptionalLiteralValueTypeNer
    {
        public static readonly RuleSetContainer Instance = new RuleSetContainer(
            @"
int Number_1 = peg#(один)# { return 1; }
System.Int32 ValueType_OptionalLiteral = peg#($Number_1?:match)# { return match == 1 ? 100 : -100; }",
            new RuleSetToken(
                Array.Empty<UsingToken>(),
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
                            new CSharpTypeNameWithNamespaceToken("System.Int32"),
                            Array.Empty<ICSharpTypeToken>()
                        ),
                        "ValueType_OptionalLiteral",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new RuleReferenceToken(null, "Number_1", Array.Empty<IRuleArgumentToken>()),
                                            new QuantifierToken(0, 1),
                                            "match",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return match == 1 ? 100 : -100; }")
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