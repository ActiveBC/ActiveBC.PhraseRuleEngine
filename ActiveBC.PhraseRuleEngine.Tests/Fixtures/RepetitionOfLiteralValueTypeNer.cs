using System;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Tests.Helpers;

namespace ActiveBC.PhraseRuleEngine.Tests.Fixtures
{
    internal static class RepetitionOfLiteralValueTypeNer
    {
        public static readonly RuleSetContainer Instance = new RuleSetContainer(
            @"
using System.Collections.Generic;

int Number_1 = peg#(один)# { return 1; }
System.Collections.Generic.IReadOnlyCollection<System.Int32> ValueType_RepetitionOfLiteral = peg#($Number_1*:numbers)# { return numbers; }
",
            new RuleSetToken(
                new []
                {
                    new UsingToken("System.Collections.Generic"),
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
                            new CSharpTypeNameWithNamespaceToken("System.Collections.Generic.IReadOnlyCollection"),
                            new ICSharpTypeToken[]
                            {
                                new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("System.Int32"), Array.Empty<ICSharpTypeToken>()),
                            }
                        ),
                        "ValueType_RepetitionOfLiteral",
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
                                            new QuantifierToken(0, null),
                                            "numbers",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return numbers; }")
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