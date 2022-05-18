using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.Common;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Equality;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Exceptions;
using NUnit.Framework;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Tests
{
    [TestFixture(TestOf = typeof(LoopBasedPegPatternTokenizer))]
    internal sealed class PegPatternTokenizerTests
    {
        private readonly LoopBasedPegPatternTokenizer m_tokenizer = new LoopBasedPegPatternTokenizer(new StringInterner());

        [Test]
        [TestCaseSource(nameof(Tokenizes_RuleArguments))]
        [TestCaseSource(nameof(Tokenizes_Lookahead))]
        [TestCaseSource(nameof(Tokenizes_RussianAlphabet))]
        [TestCaseSource(nameof(Tokenizes_CapturedPieces))]
        [TestCaseSource(nameof(Tokenizes_NestedGroups))]
        [TestCaseSource(nameof(Tokenizes_NonRussian))]
        public void Tokenizes(string pattern, PegGroupToken expectedPatternToken)
        {
            PegGroupToken patternToken = (PegGroupToken) this.m_tokenizer.Tokenize(pattern, null, true);

            Assert.IsNotNull(patternToken);

            Assert.That(patternToken, Is.EqualTo(expectedPatternToken).Using(new PegGroupTokenEqualityComparer()));
        }

        [Test]
        [TestCaseSource(nameof(Fails_InvalidPattern))]
        public void Fails(string pattern, string expectedErrorMessage)
        {
            PegPatternTokenizationException? exception = Assert.Throws<PegPatternTokenizationException>(
                () => this.m_tokenizer.Tokenize(pattern, null, true)
            );

            Assert.AreEqual(expectedErrorMessage, exception!.Message);
        }

        #region Sources

        #region Sources_Tokenizes

        public static object?[][] Tokenizes_RuleArguments =
        {
            new object?[]
            {
                "($foo.bar(patterns.foo, default, patterns.bar))",
                new PegGroupToken(
                    new []
                    {
                        new BranchToken(
                            new []
                            {
                                new BranchItemToken(
                                    new RuleReferenceToken(
                                        null,
                                        "foo.bar",
                                        new IRuleArgumentToken[]
                                        {
                                            new RuleChainedMemberAccessArgumentToken(new []{"patterns", "foo"}),
                                            RuleDefaultArgumentToken.Instance,
                                            new RuleChainedMemberAccessArgumentToken(new []{"patterns", "bar"}),
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    null,
                                    null
                                ),
                            }
                        )
                    }
                )
            },
        };

        public static object?[][] Tokenizes_Lookahead =
        {
            new object?[]
            {
                "(ноль &один !два &три+ !четыре{0,2}:four)",
                new PegGroupToken(
                    new []
                    {
                        new BranchToken(
                            new []
                            {
                                new BranchItemToken(
                                    new LiteralToken("ноль"),
                                    new QuantifierToken(1, 1),
                                    null,
                                    null
                                ),
                                new BranchItemToken(
                                    new LiteralToken("один"),
                                    new QuantifierToken(1, 1),
                                    null,
                                    new LookaheadToken(false)
                                ),
                                new BranchItemToken(
                                    new LiteralToken("два"),
                                    new QuantifierToken(1, 1),
                                    null,
                                    new LookaheadToken(true)
                                ),
                                new BranchItemToken(
                                    new LiteralToken("три"),
                                    new QuantifierToken(1, null),
                                    null,
                                    new LookaheadToken(false)
                                ),
                                new BranchItemToken(
                                    new LiteralToken("четыре"),
                                    new QuantifierToken(0, 2),
                                    "four",
                                    new LookaheadToken(true)
                                ),
                            }
                        )
                    }
                )
            },
        };

        public static object?[][] Tokenizes_RussianAlphabet =
        {
            new object?[]
            {
                "(абвгдеёжзийклмнопрстуфхцчшщъыьэюя АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ)",
                new PegGroupToken(
                    new []
                    {
                        new BranchToken(
                            new []
                            {
                                new BranchItemToken(
                                    new LiteralToken("абвгдеёжзийклмнопрстуфхцчшщъыьэюя"),
                                    new QuantifierToken(1, 1),
                                    null,
                                    null
                                ),
                                new BranchItemToken(
                                    new LiteralToken("АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"),
                                    new QuantifierToken(1, 1),
                                    null,
                                    null
                                ),
                            }
                        ),
                    }
                )
            },
        };

        public static object?[][] Tokenizes_CapturedPieces =
        {
            new object?[]
            {
                "(привет:word)",
                new PegGroupToken(
                    new []
                    {
                        new BranchToken(
                            new []
                            {
                                new BranchItemToken(
                                    new LiteralToken("привет"),
                                    new QuantifierToken(1, 1),
                                    "word",
                                    null
                                ),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([салют пока]:word)",
                new PegGroupToken(
                    new []
                    {
                        new BranchToken(
                            new []
                            {
                                new BranchItemToken(
                                    new LiteralSetToken(
                                        false,
                                        new ILiteralSetMemberToken[]
                                        {
                                            new LiteralToken("салют"),
                                            new LiteralToken("пока"),
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    "word",
                                    null
                                ),
                            }
                        ),
                    }
                )
            },
        };

        public static object?[][] Tokenizes_NestedGroups =
        {
            new object?[]
            {
                "(привет (пока|до свидания))",
                new PegGroupToken(
                    new []
                    {
                        new BranchToken(
                            new []
                            {
                                new BranchItemToken(
                                    new LiteralToken("привет"),
                                    new QuantifierToken(1, 1),
                                    null,
                                    null
                                ),
                                new BranchItemToken(
                                    new PegGroupToken(
                                        new []
                                        {
                                            new BranchToken(
                                                new []
                                                {
                                                    new BranchItemToken(
                                                        new LiteralToken("пока"),
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
                                                        new LiteralToken("до"),
                                                        new QuantifierToken(1, 1),
                                                        null,
                                                        null
                                                    ),
                                                    new BranchItemToken(
                                                        new LiteralToken("свидания"),
                                                        new QuantifierToken(1, 1),
                                                        null,
                                                        null
                                                    ),
                                                }
                                            ),
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    null,
                                    null
                                ),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(привет (пока|до (свидания|встречи))+)",
                new PegGroupToken(
                    new []
                    {
                        new BranchToken(
                            new []
                            {
                                new BranchItemToken(
                                    new LiteralToken("привет"),
                                    new QuantifierToken(1, 1),
                                    null,
                                    null
                                ),
                                new BranchItemToken(
                                    new PegGroupToken(
                                        new []
                                        {
                                            new BranchToken(
                                                new []
                                                {
                                                    new BranchItemToken(
                                                        new LiteralToken("пока"),
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
                                                        new LiteralToken("до"),
                                                        new QuantifierToken(1, 1),
                                                        null,
                                                        null
                                                    ),
                                                    new BranchItemToken(
                                                        new PegGroupToken(
                                                            new []
                                                            {
                                                                new BranchToken(
                                                                    new []
                                                                    {
                                                                        new BranchItemToken(
                                                                            new LiteralToken("свидания"),
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
                                                                            new LiteralToken("встречи"),
                                                                            new QuantifierToken(1, 1),
                                                                            null,
                                                                            null
                                                                        ),
                                                                    }
                                                                ),
                                                            }
                                                        ),
                                                        new QuantifierToken(1, 1),
                                                        null,
                                                        null
                                                    ),
                                                }
                                            ),
                                        }
                                    ),
                                    new QuantifierToken(1, null),
                                    null,
                                    null
                                ),
                            }
                        ),
                    }
                )
            },
        };

        public static object?[][] Tokenizes_NonRussian =
        {
            new object?[]
            {
                "(1)",
                new PegGroupToken(
                    new []
                    {
                        new BranchToken(
                            new []
                            {
                                new BranchItemToken(
                                    new LiteralToken("1"),
                                    new QuantifierToken(1, 1),
                                    null,
                                    null
                                ),
                            }
                        ),
                    }
                )
            },
            new object?[]
            {
                "(2~?)",
                new PegGroupToken(
                    new []
                    {
                        new BranchToken(
                            new []
                            {
                                new BranchItemToken(
                                    new PrefixToken("2"),
                                    new QuantifierToken(0, 1),
                                    null,
                                    null
                                ),
                            }
                        ),
                    }
                )
            },
            new object?[]
            {
                "(~3~+)",
                new PegGroupToken(
                    new []
                    {
                        new BranchToken(
                            new []
                            {
                                new BranchItemToken(
                                    new InfixToken("3"),
                                    new QuantifierToken(1, null),
                                    null,
                                    null
                                ),
                            }
                        ),
                    }
                )
            },
            new object?[]
            {
                "(~4*)",
                new PegGroupToken(
                    new []
                    {
                        new BranchToken(
                            new []
                            {
                                new BranchItemToken(
                                    new SuffixToken("4"),
                                    new QuantifierToken(0, null),
                                    null,
                                    null
                                ),
                            }
                        ),
                    }
                )
            },
            new object?[]
            {
                "(1foo{1,2})",
                new PegGroupToken(
                    new []
                    {
                        new BranchToken(
                            new []
                            {
                                new BranchItemToken(
                                    new LiteralToken("1foo"),
                                    new QuantifierToken(1, 2),
                                    null,
                                    null
                                ),
                            }
                        ),
                    }
                )
            },
            new object?[]
            {
                "([1 23~ ~4~ ~56 foo])",
                new PegGroupToken(
                    new []
                    {
                        new BranchToken(
                            new []
                            {
                                new BranchItemToken(
                                    new LiteralSetToken(
                                        false,
                                        new ILiteralSetMemberToken[]
                                        {
                                            new LiteralToken("1"),
                                            new PrefixToken("23"),
                                            new InfixToken("4"),
                                            new SuffixToken("56"),
                                            new LiteralToken("foo"),
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    null,
                                    null
                                ),
                            }
                        ),
                    }
                )
            },
            new object?[]
            {
                "([^1 23~ ~4~ ~56 foo])",
                new PegGroupToken(
                    new []
                    {
                        new BranchToken(
                            new []
                            {
                                new BranchItemToken(
                                    new LiteralSetToken(
                                        true,
                                        new ILiteralSetMemberToken[]
                                        {
                                            new LiteralToken("1"),
                                            new PrefixToken("23"),
                                            new InfixToken("4"),
                                            new SuffixToken("56"),
                                            new LiteralToken("foo"),
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    null,
                                    null
                                ),
                            }
                        ),
                    }
                )
            },
        };

        #endregion

        #region Sources_Fails

        public static object?[][] Fails_InvalidPattern =
        {
            new object?[]
            {
                "()",
                "Empty branches are not allowed. Absolute position: 1. Line: 0; position in line: 1. Near character: ')'. Context: '()'.",
            },
            new object?[]
            {
                "($Правило)",
                "Failed to parse rule reference at all. Absolute position: 1. Line: 0; position in line: 1. Near character: '$'. Context: '($Правило)'.",
            },
            new object?[]
            {
                "(привет::word)",
                "Empty variable name. Absolute position: 8. Line: 0; position in line: 8. Near character: ':'. Context: '(привет::word)'.",
            },
            new object?[]
            {
                "(привет:вord)",
                "Invalid variable name. Absolute position: 8. Line: 0; position in line: 8. Near character: 'в'. Context: '(привет:вord)'.",
            },
            new object?[]
            {
                "(приветword)",
                "Unhandled char. Check word for different language usage. Absolute position: 7. Line: 0; position in line: 7. Near character: 'w'. Context: '(приветword)'.",
            },
            new object?[]
            {
                "(привет%)",
                "Unexpected char. Absolute position: 7. Line: 0; position in line: 7. Near character: '%'. Context: '(привет%)'.",
            },
            new object?[]
            {
                "([])",
                "Empty literal set. Absolute position: 2. Line: 0; position in line: 2. Near character: ']'. Context: '([])'.",
            },
            new object?[]
            {
                "([привет)",
                "Unhandled char in literal set. Absolute position: 8. Line: 0; position in line: 8. Near character: ')'. Context: '([привет)'.",
            },
            new object?[]
            {
                "([^])",
                "Empty literal set. Absolute position: 3. Line: 0; position in line: 3. Near character: ']'. Context: '([^])'.",
            },
            new object?[]
            {
                "([[]])",
                "Unhandled char in literal set. Absolute position: 2. Line: 0; position in line: 2. Near character: '['. Context: '([[]])'.",
            },
            new object?[]
            {
                "([.])",
                "Unhandled char in literal set. Absolute position: 2. Line: 0; position in line: 2. Near character: '.'. Context: '([.])'.",
            },
            new object?[]
            {
                "([привет .])",
                "Unhandled char in literal set. Absolute position: 9. Line: 0; position in line: 9. Near character: '.'. Context: '([привет .])'.",
            },
            new object?[]
            {
                "([привет+])",
                "Unhandled char in literal set. Absolute position: 8. Line: 0; position in line: 8. Near character: '+'. Context: '([привет+])'.",
            },
            new object?[]
            {
                "([привет?])",
                "Unhandled char in literal set. Absolute position: 8. Line: 0; position in line: 8. Near character: '?'. Context: '([привет?])'.",
            },
            new object?[]
            {
                "([привет*])",
                "Unhandled char in literal set. Absolute position: 8. Line: 0; position in line: 8. Near character: '*'. Context: '([привет*])'.",
            },
            new object?[]
            {
                "([привет{1,2}])",
                "Unhandled char in literal set. Absolute position: 8. Line: 0; position in line: 8. Near character: '{'. Context: '([привет{1,2}])'.",
            },
        };

        #endregion

        #endregion
    }
}