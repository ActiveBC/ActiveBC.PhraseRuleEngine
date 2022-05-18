using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result.SelectionStrategy;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Exceptions;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Tests.Helpers;
using NUnit.Framework;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Tests
{
    [TestFixture(TestOf = typeof(PegProcessorFactory))]
    internal sealed class PegProcessorFactoryTests
    {
        private readonly PegProcessorFactory m_factory = new PegProcessorFactory(
            new CombinedStrategy(
                new IResultSelectionStrategy[]
                {
                    new MaxExplicitSymbolsStrategy(),
                    new MaxProgressStrategy(),
                }
            )
        );

        [Test]
        [TestCaseSource(nameof(Fails_LookaheadCapture))]
        [TestCaseSource(nameof(Fails_VariablesInNestedGroup))]
        [TestCaseSource(nameof(Fails_GroupCapture))]
        [TestCaseSource(nameof(Fails_InvalidQuantifier))]
        public void Fails(PegGroupToken group, string expectedExceptionMessage)
        {
            PegProcessorBuildException? exception = Assert.Throws<PegProcessorBuildException>(
                () => this.m_factory.Create(group, DummyRuleSpace.Instance)
            );

            Assert.AreEqual(expectedExceptionMessage, exception!.Message);
        }

        #region Sources

        #region Sources_Fails

        public static object?[][] Fails_LookaheadCapture()
        {
            return new []
            {
                new object?[]
                {
                    new PegGroupToken(
                        new []
                        {
                            new BranchToken(
                                new []
                                {
                                    new BranchItemToken(
                                        new LiteralToken("привет"),
                                        new QuantifierToken(1, 1),
                                        "foo",
                                        new LookaheadToken(true)
                                    ),
                                }
                            ),
                        }
                    ),
                    "Capturing lookahead items is not allowed (variable name 'foo').",
                },
                new object?[]
                {
                    new PegGroupToken(
                        new []
                        {
                            new BranchToken(
                                new []
                                {
                                    new BranchItemToken(
                                        new LiteralToken("пока"),
                                        new QuantifierToken(2, 10),
                                        "bar",
                                        new LookaheadToken(false)
                                    ),
                                }
                            ),
                        }
                    ),
                    "Capturing lookahead items is not allowed (variable name 'bar').",
                },
            };
        }

        public static object?[][] Fails_VariablesInNestedGroup()
        {
            return new []
            {
                new object?[]
                {
                    new PegGroupToken(
                        new []
                        {
                            new BranchToken(
                                new []
                                {
                                    new BranchItemToken(
                                        new PegGroupToken(
                                            new []
                                            {
                                                new BranchToken(
                                                    new []
                                                    {
                                                        new BranchItemToken(
                                                            new LiteralToken("привет"),
                                                            new QuantifierToken(1, 1),
                                                            "bar",
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
                    "Variable capturing is not allowed in nested groups (variable name 'bar').",
                },
            };
        }

        public static object?[][] Fails_GroupCapture()
        {
            return new []
            {
                new object?[]
                {
                    new PegGroupToken(
                        new []
                        {
                            new BranchToken(
                                new []
                                {
                                    new BranchItemToken(
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
                                                        )
                                                    }
                                                ),
                                            }
                                        ),
                                        new QuantifierToken(1, 1),
                                        "foo",
                                        null
                                    )
                                }
                            ),
                        }
                    ),
                    "Group is not capturable (variable name 'foo').",
                },
                new object?[]
                {
                    new PegGroupToken(
                        new []
                        {
                            new BranchToken(
                                new []
                                {
                                    new BranchItemToken(
                                        new PegGroupToken(
                                            new []
                                            {
                                                new BranchToken(
                                                    new []
                                                    {
                                                        new BranchItemToken(
                                                            new LiteralToken("привет"),
                                                            new QuantifierToken(1, 1),
                                                            "bar",
                                                            null
                                                        ),
                                                    }
                                                ),
                                            }
                                        ),
                                        new QuantifierToken(1, 1),
                                        "foo",
                                        null
                                    ),
                                }
                            ),
                        }
                    ),
                    "Group is not capturable (variable name 'foo').",
                }
            };
        }

        public static object?[][] Fails_InvalidQuantifier()
        {
            return new []
            {
                new object?[]
                {
                    new PegGroupToken(
                        new []
                        {
                            new BranchToken(
                                new []
                                {
                                    new BranchItemToken(
                                        new LiteralToken("привет"),
                                        new QuantifierToken(-1, -1),
                                        null,
                                        null
                                    ),
                                }
                            ),
                        }
                    ),
                    "Min value of quantifier must be greater or equal to zero, '{-1,-1}' given.",
                },
                new object?[]
                {
                    new PegGroupToken(
                        new []
                        {
                            new BranchToken(
                                new []
                                {
                                    new BranchItemToken(
                                        new LiteralToken("привет"),
                                        new QuantifierToken(-1, 0),
                                        null,
                                        null
                                    ),
                                }
                            ),
                        }
                    ),
                    "Min value of quantifier must be greater or equal to zero, '{-1,0}' given.",
                },
                new object?[]
                {
                    new PegGroupToken(
                        new []
                        {
                            new BranchToken(
                                new []
                                {
                                    new BranchItemToken(
                                        new LiteralToken("привет"),
                                        new QuantifierToken(-1, 1),
                                        null,
                                        null
                                    ),
                                }
                            ),
                        }
                    ),
                    "Min value of quantifier must be greater or equal to zero, '{-1,1}' given.",
                },
                new object?[]
                {
                    new PegGroupToken(
                        new []
                        {
                            new BranchToken(
                                new []
                                {
                                    new BranchItemToken(
                                        new LiteralToken("привет"),
                                        new QuantifierToken(-1, 3),
                                        null,
                                        null
                                    )
                                }
                            ),
                        }
                    ),
                    "Min value of quantifier must be greater or equal to zero, '{-1,3}' given.",
                },
                new object?[]
                {
                    new PegGroupToken(
                        new []
                        {
                            new BranchToken(
                                new []
                                {
                                    new BranchItemToken(
                                        new LiteralToken("привет"),
                                        new QuantifierToken(-1, null),
                                        null,
                                        null
                                    )
                                }
                            ),
                        }
                    ),
                    "Min value of quantifier must be greater or equal to zero, '{-1,}' given.",
                },
                new object?[]
                {
                    new PegGroupToken(
                        new []
                        {
                            new BranchToken(
                                new []
                                {
                                    new BranchItemToken(
                                        new LiteralToken("привет"),
                                        new QuantifierToken(0, -1),
                                        null,
                                        null
                                    ),
                                }
                            ),
                        }
                    ),
                    "Max value of quantifier must be greater or equal to one, '{0,-1}' given.",
                },
                new object?[]
                {
                    new PegGroupToken(
                        new []
                        {
                            new BranchToken(
                                new []
                                {
                                    new BranchItemToken(
                                        new LiteralToken("привет"),
                                        new QuantifierToken(10, -2),
                                        null,
                                        null
                                    ),
                                }
                            ),
                        }
                    ),
                    "Max value of quantifier must be greater or equal to one, '{10,-2}' given.",
                },
                new object?[]
                {
                    new PegGroupToken(
                        new []
                        {
                            new BranchToken(
                                new []
                                {
                                    new BranchItemToken(
                                        new LiteralToken("привет"),
                                        new QuantifierToken(0, 0),
                                        null,
                                        null
                                    ),
                                }
                            ),
                        }
                    ),
                    "Max value of quantifier must be greater or equal to one, '{0,0}' given.",
                },
                new object?[]
                {
                    new PegGroupToken(
                        new []
                        {
                            new BranchToken(
                                new []
                                {
                                    new BranchItemToken(
                                        new LiteralToken("привет"),
                                        new QuantifierToken(2, 1),
                                        null,
                                        null
                                    )
                                }
                            ),
                        }
                    ),
                    "Max value of quantifier must be greater or equal to min value, '{2,1}' given.",
                },
                new object?[]
                {
                    new PegGroupToken(
                        new []
                        {
                            new BranchToken(
                                new []
                                {
                                    new BranchItemToken(
                                        new LiteralToken("привет"),
                                        new QuantifierToken(10, 5),
                                        null,
                                        null
                                    )
                                }
                            ),
                        }
                    ),
                    "Max value of quantifier must be greater or equal to min value, '{10,5}' given.",
                },
            };
        }

        #endregion

        #endregion
    }
}