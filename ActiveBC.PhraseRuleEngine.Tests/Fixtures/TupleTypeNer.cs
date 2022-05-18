using System;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Tests.Helpers;

namespace ActiveBC.PhraseRuleEngine.Tests.Fixtures
{
    internal static class TupleTypeNer
    {
        public static readonly RuleSetContainer Instance = new RuleSetContainer(
            @"
using System;

((System.Int32, TimeSpan baz), string Bar) TupleType = peg#(.:word)# { return ((word.Length, TimeSpan.FromSeconds(word.Length)), $""{word}""); }
",
            new RuleSetToken(
                new []
                {
                    new UsingToken("System"),
                },
                new []
                {
                    new RuleToken(
                        null,
                        new TupleCSharpTypeToken(
                            new []
                            {
                                new CSharpTupleItemToken(
                                    new TupleCSharpTypeToken(
                                        new []
                                        {
                                            new CSharpTupleItemToken(
                                                new ClassicCSharpTypeToken(
                                                    new CSharpTypeNameWithNamespaceToken("System.Int32"),
                                                    Array.Empty<ICSharpTypeToken>()
                                                ),
                                                null
                                            ),
                                            new CSharpTupleItemToken(
                                                new ClassicCSharpTypeToken(
                                                    new CSharpTypeNameWithNamespaceToken("TimeSpan"),
                                                    Array.Empty<ICSharpTypeToken>()
                                                ),
                                                new CSharpIdentifierToken("baz")
                                            ),
                                        }
                                    ),
                                    null
                                ),
                                new CSharpTupleItemToken(
                                    new ClassicCSharpTypeToken(
                                        new CSharpTypeNameWithNamespaceToken("string"),
                                        Array.Empty<ICSharpTypeToken>()
                                    ),
                                    new CSharpIdentifierToken("Bar")
                                ),
                            }
                        ),
                        "TupleType",
                        Array.Empty<CSharpParameterToken>(),
                        "peg",
                        new PegGroupToken(
                            new []
                            {
                                new BranchToken(
                                    new []
                                    {
                                        new BranchItemToken(
                                            AnyLiteralToken.Instance,
                                            new QuantifierToken(1, 1),
                                            "word",
                                            null
                                        ),
                                    }
                                ),
                            }
                        ),
                        new BodyBasedProjectionToken("{ return ((word.Length, TimeSpan.FromSeconds(word.Length)), $\"{word}\"); }")
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