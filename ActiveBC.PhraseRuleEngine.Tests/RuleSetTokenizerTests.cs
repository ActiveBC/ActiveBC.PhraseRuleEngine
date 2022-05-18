using System;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Equality;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Exceptions;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Equality;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Equality;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Tests.Fixtures;
using ActiveBC.PhraseRuleEngine.Tests.Helpers;
using ActiveBC.PhraseRuleEngine.Tests.Helpers.Dummy;
using NUnit.Framework;

namespace ActiveBC.PhraseRuleEngine.Tests
{
    [TestFixture(TestOf = typeof(LoopBasedRuleSetTokenizer))]
    internal sealed class RuleSetTokenizerTests
    {
        private readonly LoopBasedRuleSetTokenizer m_tokenizer = new LoopBasedRuleSetTokenizer(
            new Dictionary<string, IPatternTokenizer>()
            {
                {NerEnvironment.Mechanics.Dummy.Key, NerEnvironment.Mechanics.Dummy.Tokenizer},
            }
        );

        private readonly LoopBasedRuleSetTokenizer m_realTokenizer = new LoopBasedRuleSetTokenizer(
            new Dictionary<string, IPatternTokenizer>()
            {
                {NerEnvironment.Mechanics.Peg.Key, NerEnvironment.Mechanics.Peg.Tokenizer},
                {NerEnvironment.Mechanics.Regex.Key, NerEnvironment.Mechanics.Regex.Tokenizer},
            }
        );

        private static readonly IReadOnlyDictionary<string, RuleSetContainer> s_ruleSetsByKey = new Dictionary<string, RuleSetContainer>()
        {
            {nameof(AnyLiteralNer), AnyLiteralNer.Instance},
            {nameof(BranchesWithOptionalLiteralReferenceTypeNer), BranchesWithOptionalLiteralReferenceTypeNer.Instance},
            {nameof(BranchesWithOptionalLiteralValueTypeNer), BranchesWithOptionalLiteralValueTypeNer.Instance},
            {nameof(BranchesWithRepetitionOfLiteralReferenceTypeNer), BranchesWithRepetitionOfLiteralReferenceTypeNer.Instance},
            {nameof(BranchesWithRepetitionOfLiteralValueTypeNer), BranchesWithRepetitionOfLiteralValueTypeNer.Instance},
            {nameof(Digits0To9Ner), Digits0To9Ner.Instance},
            {nameof(DoctorsNer), DoctorsNer.Instance},
            {nameof(InfixNer), InfixNer.Instance},
            {nameof(LiteralNer), LiteralNer.Instance},
            {nameof(Numbers0To9Ner), Numbers0To9Ner.Instance},
            {nameof(OptionalLiteralReferenceTypeNer), OptionalLiteralReferenceTypeNer.Instance},
            {nameof(OptionalLiteralValueTypeNer), OptionalLiteralValueTypeNer.Instance},
            {nameof(PrefixNer), PrefixNer.Instance},
            {nameof(RelativeTimeSpanNer), RelativeTimeSpanNer.Instance},
            {nameof(RepetitionOfLiteralReferenceTypeNer), RepetitionOfLiteralReferenceTypeNer.Instance},
            {nameof(RepetitionOfLiteralValueTypeNer), RepetitionOfLiteralValueTypeNer.Instance},
            {nameof(RuleArgumentsExampleNer), RuleArgumentsExampleNer.Instance},
            {nameof(SuffixNer), SuffixNer.Instance},
            {nameof(TupleTypeNer), TupleTypeNer.Instance},
        };

        [Test]
        [TestCaseSource(nameof(Tokenizes_Tuples))]
        [TestCaseSource(nameof(Tokenizes_RulesWithGenericReturnTypes))]
        [TestCaseSource(nameof(Tokenizes_Usings))]
        [TestCaseSource(nameof(Tokenizes_Multiline))]
        public void Tokenizes(string ruleSet, RuleSetToken expectedRuleSetToken)
        {
            RuleSetToken ruleSetToken = this.m_tokenizer.Tokenize(ruleSet, null, true);

            Assert.IsNotNull(ruleSetToken);

            Assert.That(ruleSetToken, Is.EqualTo(expectedRuleSetToken).Using(new RuleSetTokenEqualityComparer(new PatternTokenEqualityComparer())));
        }

        [Test]
        [TestCaseSource(nameof(TokenizesReal_MatcherCases))]
        public void TokenizesReal(string ruleSetKey)
        {
            RuleSetContainer ruleSetContainer = s_ruleSetsByKey[ruleSetKey];
            RuleSetToken ruleSetToken = this.m_realTokenizer.Tokenize(ruleSetContainer.Definition, null, true);

            Assert.IsNotNull(ruleSetToken);

            Assert.That(ruleSetToken, Is.EqualTo(ruleSetContainer.Token).Using(new RuleSetTokenEqualityComparer(new PatternTokenEqualityComparer())));
        }

        [Test]
        [TestCaseSource(nameof(Fails_MissingRuleType))]
        [TestCaseSource(nameof(Fails_InvalidRuleType))]
        [TestCaseSource(nameof(Fails_MissingRuleName))]
        [TestCaseSource(nameof(Fails_InvalidRuleName))]
        [TestCaseSource(nameof(Fails_InvalidPattern))]
        [TestCaseSource(nameof(Fails_InvalidJunction))]
        public void Fails(string ruleSet, string expectedErrorMessage)
        {
            PhraseRuleEngineTokenizationException? exception = Assert.Throws<PhraseRuleEngineTokenizationException>(
                () => this.m_tokenizer.Tokenize(ruleSet, null, true)
            );

            Assert.AreEqual(expectedErrorMessage, exception!.Message);
        }

        #region Sources

        #region Sources_Tokenizes

        public static object?[][] Tokenizes_Tuples =
        {
            new object?[]
            {
                "(string, (System.Collections.Generic.IList<System.TimeSpan>, TimeSpan)) Foo = dummy## { return default; }",
                new RuleSetToken(
                    Array.Empty<UsingToken>(),
                    new []
                    {
                        new RuleToken(
                            null,
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
                            "Foo",
                            Array.Empty<CSharpParameterToken>(),
                            "dummy",
                            new DummyPatternToken(""),
                            new BodyBasedProjectionToken("{ return default; }")
                        ),
                    }
                ),
            },
            new object?[]
            {
                "((System.Collections.Generic.IList<System.TimeSpan>, TimeSpan baz), string Bar) Foo = dummy## { return default; }",
                new RuleSetToken(
                    Array.Empty<UsingToken>(),
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
                            "Foo",
                            Array.Empty<CSharpParameterToken>(),
                            "dummy",
                            new DummyPatternToken(""),
                            new BodyBasedProjectionToken("{ return default; }")
                        ),
                    }
                ),
            },
        };

        public static object?[][] Tokenizes_RulesWithGenericReturnTypes =
        {
            new object?[]
            {
                "IReadOnlyCollection<string> Foo = dummy## {}",
                new RuleSetToken(
                    Array.Empty<UsingToken>(),
                    new []
                    {
                        new RuleToken(
                            null,
                            new ClassicCSharpTypeToken(
                                new CSharpTypeNameWithNamespaceToken("IReadOnlyCollection"),
                                new ICSharpTypeToken[]
                                {
                                    new ClassicCSharpTypeToken(
                                        new CSharpTypeNameWithNamespaceToken("string"),
                                        Array.Empty<ICSharpTypeToken>()
                                    ),
                                }
                            ),
                            "Foo",
                            Array.Empty<CSharpParameterToken>(),
                            "dummy",
                            new DummyPatternToken(""),
                            VoidProjectionToken.Instance
                        ),
                    }
                ),
            },
            new object?[]
            {
                "System.Collections.Generic.IReadOnlyCollection<System.Int32> Foo = dummy## {}",
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
                                    new ClassicCSharpTypeToken(
                                        new CSharpTypeNameWithNamespaceToken("System.Int32"),
                                        Array.Empty<ICSharpTypeToken>()
                                    ),
                                }
                            ),
                            "Foo",
                            Array.Empty<CSharpParameterToken>(),
                            "dummy",
                            new DummyPatternToken(""),
                            VoidProjectionToken.Instance
                        ),
                    }
                ),
            },
            new object?[]
            {
                "System.Collections.Generic.IReadOnlyCollection<List<System.Collections.Generic.IReadOnlyCollection<System.Collections.Generic.IList<System.String>>>> Foo = dummy## {}",
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
                                    new ClassicCSharpTypeToken(
                                        new CSharpTypeNameWithNamespaceToken("List"),
                                        new ICSharpTypeToken[]
                                        {
                                            new ClassicCSharpTypeToken(
                                                new CSharpTypeNameWithNamespaceToken("System.Collections.Generic.IReadOnlyCollection"),
                                                new ICSharpTypeToken[]
                                                {
                                                    new ClassicCSharpTypeToken(
                                                        new CSharpTypeNameWithNamespaceToken("System.Collections.Generic.IList"),
                                                        new ICSharpTypeToken[]
                                                        {
                                                            new ClassicCSharpTypeToken(
                                                                new CSharpTypeNameWithNamespaceToken("System.String"),
                                                                Array.Empty<ICSharpTypeToken>()
                                                            ),
                                                        }
                                                    ),
                                                }
                                            ),
                                        }
                                    ),
                                }
                            ),
                            "Foo",
                            Array.Empty<CSharpParameterToken>(),
                            "dummy",
                            new DummyPatternToken(""),
                            VoidProjectionToken.Instance
                        ),
                    }
                ),
            },
        };

        public static object?[][] Tokenizes_Usings =
        {
            new object?[]
            {
                @"
using System;
using System.Foo;
using System.Foo.Bar;

void Bar = dummy## {}",
                new RuleSetToken(
                    new []
                    {
                        new UsingToken("System"),
                        new UsingToken("System.Foo"),
                        new UsingToken("System.Foo.Bar"),
                    },
                    new []
                    {
                        new RuleToken(
                            null,
                            new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                            "Bar",
                            Array.Empty<CSharpParameterToken>(),
                            "dummy",
                            new DummyPatternToken(""),
                            VoidProjectionToken.Instance
                        ),
                    }
                ),
            },
        };

        public static object?[][] Tokenizes_Multiline =
        {
            new object?[]
            {
                @"
void Foo = dummy #привет#
{}
void Bar = dummy
#

#
{
}


void Baz = dummy#при

вет#{}",
                new RuleSetToken(
                    Array.Empty<UsingToken>(),
                    new []
                    {
                        new RuleToken(
                            null,
                            new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                            "Foo",
                            Array.Empty<CSharpParameterToken>(),
                            "dummy",
                            new DummyPatternToken("привет"),
                            VoidProjectionToken.Instance
                        ),
                        new RuleToken(
                            null,
                            new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                            "Bar",
                            Array.Empty<CSharpParameterToken>(),
                            "dummy",
                            new DummyPatternToken("\r\n\r\n"),
                            VoidProjectionToken.Instance
                        ),
                        new RuleToken(
                            null,
                            new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                            "Baz",
                            Array.Empty<CSharpParameterToken>(),
                            "dummy",
                            new DummyPatternToken("при\r\n\r\nвет"),
                            VoidProjectionToken.Instance
                        ),
                    }
                ),
            },
            new object?[]
            {
                @"
void Foo = dummy##
{}
void Bar = dummy##{
}",
                new RuleSetToken(
                    Array.Empty<UsingToken>(),
                    new []
                    {
                        new RuleToken(
                            null,
                            new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                            "Foo",
                            Array.Empty<CSharpParameterToken>(),
                            "dummy",
                            new DummyPatternToken(""),
                            VoidProjectionToken.Instance
                        ),
                        new RuleToken(
                            null,
                            new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                            "Bar",
                            Array.Empty<CSharpParameterToken>(),
                            "dummy",
                            new DummyPatternToken(""),
                            VoidProjectionToken.Instance
                        ),
                    }
                ),
            },
            new object?[]
            {
                @"
void Foo =
dummy##
{
    return;
}

void Bar =
dummy##
{
    {};
    return;
}",
                new RuleSetToken(
                    Array.Empty<UsingToken>(),
                    new []
                    {
                        new RuleToken(
                            null,
                            new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                            "Foo",
                            Array.Empty<CSharpParameterToken>(),
                            "dummy",
                            new DummyPatternToken(""),
                            new BodyBasedProjectionToken("{\r\n    return;\r\n}")
                        ),
                        new RuleToken(
                            null,
                            new ClassicCSharpTypeToken(new CSharpTypeNameWithNamespaceToken("void"), Array.Empty<ICSharpTypeToken>()),
                            "Bar",
                            Array.Empty<CSharpParameterToken>(),
                            "dummy",
                            new DummyPatternToken(""),
                            new BodyBasedProjectionToken("{\r\n    {};\r\n    return;\r\n}")
                        ),
                    }
                ),
            },
        };

        public static string[] TokenizesReal_MatcherCases()
        {
            return s_ruleSetsByKey.Keys.ToArray();
        }

        #endregion

        #region Sources_Fails

        public static object?[][] Fails_MissingRuleType =
        {
            new object?[]
            {
                "Foo = dummy## {}",
                "Failed to parse c# identifier in rule name. Absolute position: 4. Line: 0; position in line: 4. Near character: '='. Context: 'Foo = dummy## {'."
            },
        };

        public static object?[][] Fails_InvalidRuleType =
        {
            new object?[]
            {
                "vo%id Foo = dummy## {}",
                "Failed to parse whitespace after after rule result type. Absolute position: 2. Line: 0; position in line: 2. Near character: '%'. Context: 'vo%id Foo = d'.",
            },
            new object?[]
            {
                "voi/d Foo = dummy## {}",
                "Failed to parse whitespace after rule result type. Absolute position: 3. Line: 0; position in line: 3. Near character: '/'. Context: 'voi/d Foo = du'."
            },
            new object?[]
            {
                "строка Foo = dummy## {}",
                "Failed to parse c# type. Absolute position: 0. Line: 0; position in line: 0. Near character: 'с'. Context: 'строка Foo '."
            },
            new object?[]
            {
                "32 Foo = dummy## {}",
                "Failed to parse c# type. Absolute position: 0. Line: 0; position in line: 0. Near character: '3'. Context: '32 Foo = du'."
            },
            new object?[]
            {
                "32.Int Foo = dummy## {}",
                "Failed to parse c# type. Absolute position: 0. Line: 0; position in line: 0. Near character: '3'. Context: '32.Int Foo '."
            },
            new object?[]
            {
                "32Int Foo = dummy## {}",
                "Failed to parse c# type. Absolute position: 0. Line: 0; position in line: 0. Near character: '3'. Context: '32Int Foo ='."
            },
        };

        public static object?[][] Fails_MissingRuleName =
        {
            new object?[]
            {
                "void = dummy## {}",
                "Failed to parse c# identifier in rule name. Absolute position: 5. Line: 0; position in line: 5. Near character: '='. Context: 'void = dummy## {'."
            },
        };

        public static object?[][] Fails_InvalidRuleName =
        {
            new object?[]
            {
                "void Фуу = dummy## {}",
                "Failed to parse c# identifier in rule name. Absolute position: 5. Line: 0; position in line: 5. Near character: 'Ф'. Context: 'void Фуу = dummy'."
            },
            new object?[]
            {
                "void Fуу = dummy## {}",
                "Unmatched character '='. Absolute position: 6. Line: 0; position in line: 6. Near character: 'у'. Context: 'void Fуу = dummy#'."
            },
            new object?[]
            {
                "void 1Foo = dummy## {}",
                "Failed to parse c# identifier in rule name. Absolute position: 5. Line: 0; position in line: 5. Near character: '1'. Context: 'void 1Foo = dumm'."
            },
            new object?[]
            {
                "void F/oo = dummy## {}",
                "Failed to parse whitespace after rule name. Absolute position: 6. Line: 0; position in line: 6. Near character: '/'. Context: 'void F/oo = dummy'."
            },
            new object?[]
            {
                "void F.oo = dummy## {}",
                "Unmatched character '='. Absolute position: 6. Line: 0; position in line: 6. Near character: '.'. Context: 'void F.oo = dummy'."
            },
        };

        public static object?[][] Fails_InvalidPattern =
        {
            new object?[]
            {
                "void Foo = dummy() {}",
                "Unmatched pattern start. Absolute position: 16. Line: 0; position in line: 16. Near character: '('. Context: 'oo = dummy() {}'."
            },
            new object?[]
            {
                "void Foo = dummy# {}",
                "Unmatched pattern end."
            },
            new object?[]
            {
                "void Foo = привет {}",
                "Failed to parse c# identifier in pattern key at all. Absolute position: 11. Line: 0; position in line: 11. Near character: 'п'. Context: 'oid Foo = привет {}'."
            },
            new object?[]
            {
                "void Foo = #привет# {}",
                "Failed to parse c# identifier in pattern key at all. Absolute position: 11. Line: 0; position in line: 11. Near character: '#'. Context: 'oid Foo = #привет# {}'."
            },
            new object?[]
            {
                @"void Bar = dummy#пока# {}
void Foo = привет {}",
                "Failed to parse c# identifier in pattern key at all. Absolute position: 38. Line: 1; position in line: 11. Near character: 'п'. Context: 'oid Foo = привет {}'."
            },
        };

        public static object?[][] Fails_InvalidJunction =
        {
            new object?[]
            {
                "void Foo dummy## {}",
                "Unmatched character '='. Absolute position: 9. Line: 0; position in line: 9. Near character: 'd'. Context: 'void Foo dummy## {}'."
            },
        };

        #endregion

        #endregion

        #region EqualityComparers

        private sealed class PatternTokenEqualityComparer : IEqualityComparer<IPatternToken>
        {
            public bool Equals(IPatternToken? x, IPatternToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x switch
                {
                    DummyPatternToken token => DummyPatternTokenEqualityComparer.Instance.Equals(token, y as DummyPatternToken),
                    PegGroupToken token => PegGroupTokenEqualityComparer.Instance.Equals(token, y as PegGroupToken),
                    RegexGroupToken token => RegexGroupTokenEqualityComparer.Instance.Equals(token, y as RegexGroupToken),
                    _ => throw new ArgumentOutOfRangeException(nameof(x)),
                };
            }

            public int GetHashCode(IPatternToken obj)
            {
                return obj switch
                {
                    DummyPatternToken token => DummyPatternTokenEqualityComparer.Instance.GetHashCode(token),
                    PegGroupToken token => PegGroupTokenEqualityComparer.Instance.GetHashCode(token),
                    RegexGroupToken token => RegexGroupTokenEqualityComparer.Instance.GetHashCode(token),
                    _ => throw new ArgumentOutOfRangeException(nameof(obj)),
                };
            }
        }

        private sealed class DummyPatternTokenEqualityComparer : IEqualityComparer<DummyPatternToken>
        {
            public static readonly DummyPatternTokenEqualityComparer Instance = new DummyPatternTokenEqualityComparer();

            private DummyPatternTokenEqualityComparer()
            {
            }

            public bool Equals(DummyPatternToken? x, DummyPatternToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.Pattern == y.Pattern;
            }

            public int GetHashCode(DummyPatternToken obj)
            {
                return obj.Pattern.GetHashCode();
            }
        }

        #endregion
    }
}