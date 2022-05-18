using System;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Tests.Helpers;

namespace ActiveBC.PhraseRuleEngine.Tests.Fixtures
{
    internal static class RepetitionOfLiteralReferenceTypeNer
    {
        public static readonly RuleSetContainer Instance = new RuleSetContainer(
            "System.Collections.Generic.IReadOnlyCollection<System.String> ReferenceType_RepetitionOfLiteral = peg#(привет*:words)# { return words; }",
            new RuleSetToken(
                Array.Empty<UsingToken>(),
                new []
                {
                    new RuleToken(
                        null,
                        new ClassicCSharpTypeToken(
                            new CSharpTypeNameWithNamespaceToken("System.Collections.Generic.IReadOnlyCollection"),
                            new ICSharpTypeToken[]
                            {
                                new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("System.String"), Array.Empty<ICSharpTypeToken>()),
                            }
                        ),
                        "ReferenceType_RepetitionOfLiteral",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("привет"),
                                            new QuantifierToken(0, null),
                                            "words",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return words; }")
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