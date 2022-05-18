using System;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Types.Resolving;
using NUnit.Framework;

namespace ActiveBC.PhraseRuleEngine.Tests
{
    [TestFixture(TestOf = typeof(TypeResolver))]
    internal sealed class TypeResolverTests
    {
        [Test]
        [TestCaseSource(nameof(Resolves_TupleTypes))]
        [TestCaseSource(nameof(Resolves_KeywordTypes))]
        [TestCaseSource(nameof(Resolves_WithNamespaceRestrictions))]
        public void Resolves(ICSharpTypeToken typeToken, IReadOnlySet<string> usingNamespaces, Type expectedResult)
        {
            TypeResolver typeResolver = new TypeResolver();

            Type result = typeResolver.Resolve(typeToken, usingNamespaces, new LoadedAssembliesProvider());

            Assert.AreEqual(expectedResult, result);
        }

        #region Sources

        public static object?[][] Resolves_TupleTypes =
        {
            new object?[]
            {
                new TupleCSharpTypeToken(
                    new []
                    {
                        new CSharpTupleItemToken(
                            new ClassicCSharpTypeToken(
                                new CSharpTypeNameWithNamespaceToken("string"),
                                Array.Empty<ICSharpTypeToken>()
                            ),
                            null
                        ),
                        new CSharpTupleItemToken(
                            new TupleCSharpTypeToken(
                                new []
                                {
                                    new CSharpTupleItemToken(
                                        new ClassicCSharpTypeToken(
                                            new CSharpTypeNameWithNamespaceToken("System.Collections.Generic.IList"),
                                            new ICSharpTypeToken[]
                                            {
                                                new ClassicCSharpTypeToken(
                                                    new CSharpTypeNameWithNamespaceToken("System.TimeSpan"),
                                                    Array.Empty<ICSharpTypeToken>()
                                                ),
                                            }
                                        ),
                                        null
                                    ),
                                    new CSharpTupleItemToken(
                                        new ClassicCSharpTypeToken(
                                            new CSharpTypeNameWithNamespaceToken("TimeSpan"),
                                            Array.Empty<ICSharpTypeToken>()
                                        ),
                                        null
                                    ),
                                }
                            ),
                            null
                        )
                    }
                ),
                new HashSet<string>()
                {
                    "System",
                },
                typeof(ValueTuple<string, ValueTuple<IList<TimeSpan>, TimeSpan>>),
            },
            new object?[]
            {
                new ClassicCSharpTypeToken(
                    new CSharpTypeNameWithNamespaceToken("string"),
                    Array.Empty<ICSharpTypeToken>()
                ),
                new HashSet<string>(),
                typeof(string),
            },
            new object?[]
            {
                new ClassicCSharpTypeToken(
                    new CSharpTypeNameWithNamespaceToken("void"),
                    Array.Empty<ICSharpTypeToken>()
                ),
                new HashSet<string>(),
                typeof(void),
            },
        };

        public static object?[][] Resolves_KeywordTypes =
        {
            new object?[]
            {
                new ClassicCSharpTypeToken(
                    new CSharpTypeNameWithNamespaceToken("int"),
                    Array.Empty<ICSharpTypeToken>()
                ),
                new HashSet<string>(),
                typeof(int),
            },
            new object?[]
            {
                new ClassicCSharpTypeToken(
                    new CSharpTypeNameWithNamespaceToken("string"),
                    Array.Empty<ICSharpTypeToken>()
                ),
                new HashSet<string>(),
                typeof(string),
            },
            new object?[]
            {
                new ClassicCSharpTypeToken(
                    new CSharpTypeNameWithNamespaceToken("void"),
                    Array.Empty<ICSharpTypeToken>()
                ),
                new HashSet<string>(),
                typeof(void),
            },
        };

        public static object?[][] Resolves_WithNamespaceRestrictions =
        {
            new object?[]
            {
                new ClassicCSharpTypeToken(
                    new CSharpTypeNameWithNamespaceToken("TimeSpan"),
                    Array.Empty<ICSharpTypeToken>()
                ),
                new HashSet<string>()
                {
                    "System",
                },
                typeof(TimeSpan),
            },
            new object?[]
            {
                new ClassicCSharpTypeToken(
                    new CSharpTypeNameWithNamespaceToken("List"),
                    new ICSharpTypeToken[]
                    {
                        new ClassicCSharpTypeToken(
                            new CSharpTypeNameWithNamespaceToken("string"),
                            Array.Empty<ICSharpTypeToken>()
                        ),
                    }
                ),
                new HashSet<string>()
                {
                    "System.Collections.Generic",
                },
                typeof(List<string>),
            },
            new object?[]
            {
                new ClassicCSharpTypeToken(
                    new CSharpTypeNameWithNamespaceToken("IDictionary"),
                    new ICSharpTypeToken[]
                    {
                        new ClassicCSharpTypeToken(
                            new CSharpTypeNameWithNamespaceToken("string"),
                            Array.Empty<ICSharpTypeToken>()
                        ),
                        new ClassicCSharpTypeToken(
                            new CSharpTypeNameWithNamespaceToken("System.String"),
                            Array.Empty<ICSharpTypeToken>()
                        ),
                    }
                ),
                new HashSet<string>()
                {
                    "System.Collections.Generic",
                },
                typeof(IDictionary<string, string>),
            },
            new object?[]
            {
                new ClassicCSharpTypeToken(
                    new CSharpTypeNameWithNamespaceToken("IReadOnlyCollection"),
                    new ICSharpTypeToken[]
                    {
                        new ClassicCSharpTypeToken(
                            new CSharpTypeNameWithNamespaceToken("List"),
                            new ICSharpTypeToken[]
                            {
                                new ClassicCSharpTypeToken(
                                    new CSharpTypeNameWithNamespaceToken("Int32"),
                                    Array.Empty<ICSharpTypeToken>()
                                ),
                            }
                        ),
                    }
                ),
                new HashSet<string>()
                {
                    "System",
                    "System.Collections.Generic",
                },
                typeof(IReadOnlyCollection<List<int>>),
            },
            new object?[]
            {
                new ClassicCSharpTypeToken(
                    new CSharpTypeNameWithNamespaceToken("IReadOnlyCollection"),
                    new ICSharpTypeToken[]
                    {
                        new ClassicCSharpTypeToken(
                            new CSharpTypeNameWithNamespaceToken("List"),
                            new ICSharpTypeToken[]
                            {
                                new ClassicCSharpTypeToken(
                                    new CSharpTypeNameWithNamespaceToken("System.Int32"),
                                    Array.Empty<ICSharpTypeToken>()
                                ),
                            }
                        ),
                    }
                ),
                new HashSet<string>()
                {
                    "System",
                    "System.Collections.Generic",
                },
                typeof(IReadOnlyCollection<List<int>>),
            },
            new object?[]
            {
                new ClassicCSharpTypeToken(
                    new CSharpTypeNameWithNamespaceToken("IReadOnlyCollection"),
                    new ICSharpTypeToken[]
                    {
                        new ClassicCSharpTypeToken(
                            new CSharpTypeNameWithNamespaceToken("System.Collections.Generic.List"),
                            new ICSharpTypeToken[]
                            {
                                new ClassicCSharpTypeToken(
                                    new CSharpTypeNameWithNamespaceToken("System.Int32"),
                                    Array.Empty<ICSharpTypeToken>()
                                ),
                            }
                        ),
                    }
                ),
                new HashSet<string>()
                {
                    "System.Collections.Generic",
                },
                typeof(IReadOnlyCollection<List<int>>),
            },
            new object?[]
            {
                new ClassicCSharpTypeToken(
                    new CSharpTypeNameWithNamespaceToken("System.Collections.Generic.IReadOnlyCollection"),
                    new ICSharpTypeToken[]
                    {
                        new ClassicCSharpTypeToken(
                            new CSharpTypeNameWithNamespaceToken("List"),
                            new ICSharpTypeToken[]
                            {
                                new ClassicCSharpTypeToken(
                                    new CSharpTypeNameWithNamespaceToken("System.Int32"),
                                    Array.Empty<ICSharpTypeToken>()
                                ),
                            }
                        ),
                    }
                ),
                new HashSet<string>()
                {
                    "System.Collections.Generic",
                },
                typeof(IReadOnlyCollection<List<int>>),
            },
        };

        #endregion
    }
}