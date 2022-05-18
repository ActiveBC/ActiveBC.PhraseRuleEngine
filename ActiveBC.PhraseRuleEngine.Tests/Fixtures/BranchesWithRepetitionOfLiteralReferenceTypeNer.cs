using System;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Tests.Helpers;

namespace ActiveBC.PhraseRuleEngine.Tests.Fixtures
{
    internal static class BranchesWithRepetitionOfLiteralReferenceTypeNer
    {
        public static readonly RuleSetContainer Instance = new RuleSetContainer(
            "System.Collections.Generic.IReadOnlyCollection<string> ReferenceType_BranchesWithRepetitionOfLiteral = peg#(привет+:words_1|пока+:words_2)# { return words_1 ?? words_2; }",
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
                                new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("string"), Array.Empty<ICSharpTypeToken>()),
                            }
                        ),
                        "ReferenceType_BranchesWithRepetitionOfLiteral",
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
                                            new QuantifierToken(1, null),
                                            "words_1",
                                            null
                                        ),
                                    }
                                ),
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            new LiteralToken("пока"),
                                            new QuantifierToken(1, null),
                                            "words_2",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return words_1 ?? words_2; }")
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