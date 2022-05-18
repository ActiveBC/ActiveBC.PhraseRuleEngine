using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Evaluation.InputProcessing;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Tests.Fixtures;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Tests.Helpers;
using NUnit.Framework;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Tests
{
    // todo [tests] add all the peg-patterns and use these test-cases for tokenization tests as well
    // todo [tests] reorganize
    [TestFixture(TestOf = typeof(PegProcessor))]
    internal sealed class PegProcessorTests
    {
        [Test]
        [TestCaseSource(nameof(MatchesAndProjects_NegativeLookaheadWithinQuantifiedGroup))]
        [TestCaseSource(nameof(MatchesAndProjects_NegativeLookaheadBetweenTerminals))]
        [TestCaseSource(nameof(MatchesAndProjects_NegativeQuantifiedLookahead))]
        [TestCaseSource(nameof(MatchesAndProjects_NegativeLookahead))]
        [TestCaseSource(nameof(MatchesAndProjects_QuantifiedLookahead))]
        [TestCaseSource(nameof(MatchesAndProjects_Lookahead))]
        [TestCaseSource(nameof(MatchesAndProjects_Literal))]
        [TestCaseSource(nameof(MatchesAndProjects_AnyLiteral))]
        [TestCaseSource(nameof(MatchesAndProjects_Prefix))]
        [TestCaseSource(nameof(MatchesAndProjects_Infix))]
        [TestCaseSource(nameof(MatchesAndProjects_Suffix))]
        [TestCaseSource(nameof(MatchesAndProjects_PositiveLiteralSet))]
        [TestCaseSource(nameof(MatchesAndProjects_NegativeLiteralSet))]
        [TestCaseSource(nameof(MatchesAndProjects_NestedGroup))]
        [TestCaseSource(nameof(MatchesAndProjects_LiteralSequence))]
        [TestCaseSource(nameof(MatchesAndProjects_OptionalLiteral))]
        [TestCaseSource(nameof(MatchesAndProjects_OptionalPositiveLiteralSet))]
        [TestCaseSource(nameof(MatchesAndProjects_OptionalNegativeLiteralSet))]
        [TestCaseSource(nameof(MatchesAndProjects_OptionalAnyLiteral))]
        [TestCaseSource(nameof(MatchesAndProjects_RepetitionOfLiteral))]
        [TestCaseSource(nameof(MatchesAndProjects_RepetitionOfPositiveLiteralSet))]
        [TestCaseSource(nameof(MatchesAndProjects_RepetitionOfNegativeLiteralSet))]
        [TestCaseSource(nameof(MatchesAndProjects_RepetitionOfAnyLiteral))]
        [TestCaseSource(nameof(MatchesAndProjects_GroupWithLiterals))]
        [TestCaseSource(nameof(MatchesAndProjects_GroupWithSequences))]
        [TestCaseSource(nameof(MatchesAndProjects_RecursionWithSimpleSumGrammar))]
        [TestCaseSource(nameof(MatchesAndProjects_RecursionWithSimpleArithmeticsGrammar))]
        [TestCaseSource(nameof(MatchesAndProjects_RecursionWithRuleRepeatedInDifferentBranches))]
        public void MatchesAndProjects(RuleSpaceSource grammar, string ruleName, string phrase, bool expectedIsMatched, int? expectedEndIndex)
        {
            const int firstSymbolIndex = 0;
            RuleInput ruleInput = new RuleInput(
                phrase.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                new RuleSpaceArguments(ImmutableDictionary<string, object?>.Empty)
            );

            RuleMatchResultCollection results = grammar.RuleSpace[ruleName].MatchAndProject(
                ruleInput,
                firstSymbolIndex,
                new RuleArguments(ImmutableDictionary<string, object?>.Empty),
                new RuleSpaceCache()
            );

            Assert.AreEqual(expectedIsMatched, results.Count == 1);
            if (results.Count == 1)
            {
                Assert.AreEqual(expectedEndIndex ?? ruleInput.Sequence.Length, results.Single().LastUsedSymbolIndex + 1);
            }
        }

        #region Sources

        #region Sources_MatchesAndProjects

        public static object?[][] MatchesAndProjects_NegativeLookaheadWithinQuantifiedGroup()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "NegativeLookaheadWithinQuantifiedGroup",
                        (
                            "открыть (!закрыть .){0,3} закрыть",
                            new PegGroupToken(
                                new []
                                {
                                    new BranchToken(
                                        new []
                                        {
                                            new BranchItemToken(
                                                new LiteralToken("открыть"),
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
                                                                    new LiteralToken("закрыть"),
                                                                    new QuantifierToken(1, 1),
                                                                    null,
                                                                    new LookaheadToken(true)
                                                                ),
                                                                new BranchItemToken(
                                                                    AnyLiteralToken.Instance,
                                                                    new QuantifierToken(1, 1),
                                                                    null,
                                                                    null
                                                                ),
                                                            }
                                                        )
                                                    }
                                                ),
                                                new QuantifierToken(0, 3),
                                                null,
                                                null
                                            ),
                                            new BranchItemToken(
                                                new LiteralToken("закрыть"),
                                                new QuantifierToken(1, 1),
                                                null,
                                                null
                                            )
                                        }
                                    ),
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadWithinQuantifiedGroup",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadWithinQuantifiedGroup",
                    "открыть закрыть",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadWithinQuantifiedGroup",
                    "открыть привет закрыть",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadWithinQuantifiedGroup",
                    "открыть привет прикол закрыть",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadWithinQuantifiedGroup",
                    "открыть привет прикол пока закрыть",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadWithinQuantifiedGroup",
                    "открыть привет прикол пока куку закрыть",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadWithinQuantifiedGroup",
                    "открыть закрыть прикол закрыть",
                    true,
                    2,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadWithinQuantifiedGroup",
                    "открыть прикол закрыть привет закрыть",
                    true,
                    3,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadWithinQuantifiedGroup",
                    "открыть прикол привет закрыть куку закрыть",
                    true,
                    4,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadWithinQuantifiedGroup",
                    "открыть прикол привет куку закрыть хаха закрыть",
                    true,
                    5,
                },
            };
        }

        public static object?[][] MatchesAndProjects_NegativeLookaheadBetweenTerminals()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "NegativeLookaheadBetweenTerminals",
                        (
                            "ноль !один{0,3} один",
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
                                                new QuantifierToken(0, 3),
                                                null,
                                                new LookaheadToken(true)
                                            ),
                                            new BranchItemToken(
                                                new LiteralToken("один"),
                                                new QuantifierToken(1, 1),
                                                null,
                                                null
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadBetweenTerminals",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadBetweenTerminals",
                    "ноль один",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadBetweenTerminals",
                    "ноль привет один",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadBetweenTerminals",
                    "ноль привет прикол один",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadBetweenTerminals",
                    "ноль привет прикол пока один",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadBetweenTerminals",
                    "ноль привет прикол пока куку один",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadBetweenTerminals",
                    "ноль один прикол один",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadBetweenTerminals",
                    "ноль прикол один привет один",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadBetweenTerminals",
                    "ноль прикол привет один куку один",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookaheadBetweenTerminals",
                    "ноль прикол привет куку один хаха один",
                    false,
                    null,
                },
            };
        }

        public static object?[][] MatchesAndProjects_NegativeQuantifiedLookahead()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "NegativeQuantifiedLookahead",
                        (
                            String.Empty,
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
                                                new QuantifierToken(2, 3),
                                                null,
                                                new LookaheadToken(true)
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeQuantifiedLookahead",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeQuantifiedLookahead",
                    "ноль",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeQuantifiedLookahead",
                    "ноль один",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeQuantifiedLookahead",
                    "ноль один один",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeQuantifiedLookahead",
                    "ноль один один один",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeQuantifiedLookahead",
                    "один один один один",
                    false,
                    null,
                },
            };
        }

        public static object?[][] MatchesAndProjects_NegativeLookahead()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "NegativeLookahead",
                        (
                            String.Empty,
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
                                                new LookaheadToken(true)
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookahead",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookahead",
                    "ноль",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookahead",
                    "ноль один",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLookahead",
                    "ноль один один",
                    false,
                    null,
                },
            };
        }

        public static object?[][] MatchesAndProjects_QuantifiedLookahead()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "QuantifiedLookahead",
                        (
                            String.Empty,
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
                                                new QuantifierToken(2, 3),
                                                null,
                                                new LookaheadToken(false)
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "QuantifiedLookahead",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "QuantifiedLookahead",
                    "ноль",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "QuantifiedLookahead",
                    "ноль один",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "QuantifiedLookahead",
                    "ноль один один",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "QuantifiedLookahead",
                    "ноль один один один",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "QuantifiedLookahead",
                    "ноль один один один один",
                    true,
                    1,
                },
            };
        }

        public static object?[][] MatchesAndProjects_Lookahead()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "Lookahead",
                        (
                            String.Empty,
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
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "Lookahead",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Lookahead",
                    "ноль",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Lookahead",
                    "ноль один",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Lookahead",
                    "ноль один один",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Lookahead",
                    "один",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Lookahead",
                    "один один",
                    false,
                    null,
                },
            };
        }

        public static object?[][] MatchesAndProjects_Literal()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "Literal",
                        (
                            String.Empty,
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
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "Literal",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Literal",
                    "привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Literal",
                    "приветт",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Literal",
                    "приве",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Literal",
                    "ппривет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Literal",
                    "ривет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Literal",
                    "привет привет",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Literal",
                    "привет пока",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Literal",
                    "пока привет",
                    false,
                    null,
                },
            };
        }

        public static object?[][] MatchesAndProjects_Prefix()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "Prefix",
                        (
                            String.Empty,
                            new PegGroupToken(
                                new []
                                {
                                    new BranchToken(
                                        new []
                                        {
                                            new BranchItemToken(
                                                new PrefixToken("привет"),
                                                new QuantifierToken(1, 1),
                                                null,
                                                null
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "Prefix",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Prefix",
                    "привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Prefix",
                    "приветт",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Prefix",
                    "приве",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Prefix",
                    "ппривет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Prefix",
                    "ривет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Prefix",
                    "привет привет",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Prefix",
                    "привет пока",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Prefix",
                    "пока привет",
                    false,
                    null,
                },
            };
        }

        public static object?[][] MatchesAndProjects_Infix()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "Infix",
                        (
                            String.Empty,
                            new PegGroupToken(
                                new []
                                {
                                    new BranchToken(
                                        new []
                                        {
                                            new BranchItemToken(
                                                new InfixToken("привет"),
                                                new QuantifierToken(1, 1),
                                                null,
                                                null
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "Infix",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Infix",
                    "привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Infix",
                    "приветт",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Infix",
                    "приве",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Infix",
                    "ппривет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Infix",
                    "ривет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Infix",
                    "привет привет",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Infix",
                    "привет пока",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Infix",
                    "пока привет",
                    false,
                    null,
                },
            };
        }

        public static object?[][] MatchesAndProjects_Suffix()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "Suffix",
                        (
                            String.Empty,
                            new PegGroupToken(
                                new []
                                {
                                    new BranchToken(
                                        new []
                                        {
                                            new BranchItemToken(
                                                new SuffixToken("привет"),
                                                new QuantifierToken(1, 1),
                                                null,
                                                null
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "Suffix",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Suffix",
                    "привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Suffix",
                    "приветт",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Suffix",
                    "приве",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Suffix",
                    "ппривет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Suffix",
                    "ривет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Suffix",
                    "привет привет",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Suffix",
                    "привет пока",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "Suffix",
                    "пока привет",
                    false,
                    null,
                },
            };
        }

        public static object?[][] MatchesAndProjects_AnyLiteral()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "AnyLiteral",
                        (
                            String.Empty,
                            new PegGroupToken(
                                new []
                                {
                                    new BranchToken(
                                        new []
                                        {
                                            new BranchItemToken(
                                                AnyLiteralToken.Instance,
                                                new QuantifierToken(1, 1),
                                                null,
                                                null
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "AnyLiteral",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "AnyLiteral",
                    "привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "AnyLiteral",
                    "пока",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "AnyLiteral",
                    "привет привет",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "AnyLiteral",
                    "привет пока",
                    true,
                    1,
                },
            };
        }

        public static object?[][] MatchesAndProjects_PositiveLiteralSet()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "PositiveLiteralSet",
                        (
                            String.Empty,
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
                                                        new LiteralToken("привет"),
                                                        new LiteralToken("пока"),
                                                    }
                                                ),
                                                new QuantifierToken(1, 1),
                                                null,
                                                null
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "PositiveLiteralSet",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "PositiveLiteralSet",
                    "привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "PositiveLiteralSet",
                    "пока",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "PositiveLiteralSet",
                    "приве",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "PositiveLiteralSet",
                    "ока",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "PositiveLiteralSet",
                    "привет пока",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "PositiveLiteralSet",
                    "пока привет",
                    true,
                    1,
                },
            };
        }

        public static object?[][] MatchesAndProjects_NegativeLiteralSet()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "NegativeLiteralSet",
                        (
                            String.Empty,
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
                                                        new LiteralToken("привет"),
                                                        new LiteralToken("пока"),
                                                    }
                                                ),
                                                new QuantifierToken(1, 1),
                                                null,
                                                null
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLiteralSet",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLiteralSet",
                    "привет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLiteralSet",
                    "пока",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLiteralSet",
                    "приве",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLiteralSet",
                    "ока",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLiteralSet",
                    "привет пока",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLiteralSet",
                    "пока привет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLiteralSet",
                    "ривет ока",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NegativeLiteralSet",
                    "ока ривет",
                    true,
                    1,
                },
            };
        }

        public static object?[][] MatchesAndProjects_LiteralSequence()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "LiteralSequence",
                        (
                            String.Empty,
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
                                                new LiteralToken("пока"),
                                                new QuantifierToken(1, 1),
                                                null,
                                                null
                                            ),
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "LiteralSequence",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "LiteralSequence",
                    "привет пока",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "LiteralSequence",
                    "привет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "LiteralSequence",
                    "пока",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "LiteralSequence",
                    "пока привет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "LiteralSequence",
                    "ппривет ппока",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "LiteralSequence",
                    "ривет ока",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "LiteralSequence",
                    "привет привет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "LiteralSequence",
                    "пока пока",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "LiteralSequence",
                    "привет пока куку",
                    true,
                    2,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "LiteralSequence",
                    "куку привет пока",
                    false,
                    null,
                },
            };
        }

        public static object?[][] MatchesAndProjects_NestedGroup()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "NestedGroup",
                        (
                            String.Empty,
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
                                                                ),
                                                                new BranchItemToken(
                                                                    new LiteralToken("пока"),
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
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "NestedGroup",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NestedGroup",
                    "привет пока",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NestedGroup",
                    "привет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NestedGroup",
                    "пока",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NestedGroup",
                    "пока привет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NestedGroup",
                    "ппривет ппока",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NestedGroup",
                    "ривет ока",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NestedGroup",
                    "привет привет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NestedGroup",
                    "пока пока",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NestedGroup",
                    "привет пока куку",
                    true,
                    2,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "NestedGroup",
                    "куку привет пока",
                    false,
                    null,
                },
            };
        }

        public static object?[][] MatchesAndProjects_OptionalLiteral()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "OptionalLiteral",
                        (
                            String.Empty,
                            new PegGroupToken(
                                new []
                                {
                                    new BranchToken(
                                        new []
                                        {
                                            new BranchItemToken(
                                                new LiteralToken("привет"),
                                                new QuantifierToken(0, 1),
                                                null,
                                                null
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalLiteral",
                    "",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalLiteral",
                    "привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalLiteral",
                    "пока",
                    true,
                    0,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalLiteral",
                    "привет привет",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalLiteral",
                    "привет пока",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalLiteral",
                    "пока привет",
                    true,
                    0,
                },
            };
        }

        public static object?[][] MatchesAndProjects_OptionalPositiveLiteralSet()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "OptionalPositiveLiteralSet",
                        (
                            String.Empty,
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
                                                        new LiteralToken("привет"),
                                                        new LiteralToken("пока"),
                                                    }
                                                ),
                                                new QuantifierToken(0, 1),
                                                null,
                                                null
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalPositiveLiteralSet",
                    "",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalPositiveLiteralSet",
                    "привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalPositiveLiteralSet",
                    "пока",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalPositiveLiteralSet",
                    "куку",
                    true,
                    0,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalPositiveLiteralSet",
                    "привет пока",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalPositiveLiteralSet",
                    "пока привет",
                    true,
                    1,
                },
            };
        }

        public static object?[][] MatchesAndProjects_OptionalNegativeLiteralSet()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "OptionalNegativeLiteralSet",
                        (
                            String.Empty,
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
                                                        new LiteralToken("привет"),
                                                        new LiteralToken("пока"),
                                                    }
                                                ),
                                                new QuantifierToken(0, 1),
                                                null,
                                                null
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalNegativeLiteralSet",
                    "",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalNegativeLiteralSet",
                    "привет",
                    true,
                    0,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalNegativeLiteralSet",
                    "пока",
                    true,
                    0,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalNegativeLiteralSet",
                    "куку",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalNegativeLiteralSet",
                    "ляля",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalNegativeLiteralSet",
                    "куку куку",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalNegativeLiteralSet",
                    "куку ляля",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalNegativeLiteralSet",
                    "привет куку",
                    true,
                    0,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalNegativeLiteralSet",
                    "куку привет",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalNegativeLiteralSet",
                    "привет пока",
                    true,
                    0,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalNegativeLiteralSet",
                    "пока привет",
                    true,
                    0,
                },
            };
        }

        public static object?[][] MatchesAndProjects_OptionalAnyLiteral()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "OptionalAnyLiteral",
                        (
                            String.Empty,
                            new PegGroupToken(
                                new []
                                {
                                    new BranchToken(
                                        new []
                                        {
                                            new BranchItemToken(
                                                AnyLiteralToken.Instance,
                                                new QuantifierToken(0, 1),
                                                null,
                                                null
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalAnyLiteral",
                    "",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalAnyLiteral",
                    "привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalAnyLiteral",
                    "пока",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalAnyLiteral",
                    "привет привет",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "OptionalAnyLiteral",
                    "привет пока",
                    true,
                    1,
                },
            };
        }

        public static object?[][] MatchesAndProjects_RepetitionOfLiteral()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "RepetitionOfLiteral",
                        (
                            String.Empty,
                            new PegGroupToken(
                                new []
                                {
                                    new BranchToken(
                                        new []
                                        {
                                            new BranchItemToken(
                                                new LiteralToken("привет"),
                                                new QuantifierToken(0, null),
                                                null,
                                                null
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfLiteral",
                    "",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfLiteral",
                    "привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfLiteral",
                    "пока",
                    true,
                    0,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfLiteral",
                    "привет привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfLiteral",
                    "привет привет привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfLiteral",
                    "привет пока",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfLiteral",
                    "пока привет",
                    true,
                    0,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfLiteral",
                    "привет пока привет",
                    true,
                    1,
                },
            };
        }

        public static object?[][] MatchesAndProjects_RepetitionOfPositiveLiteralSet()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "RepetitionOfPositiveLiteralSet",
                        (
                            String.Empty,
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
                                                        new LiteralToken("привет"),
                                                        new LiteralToken("пока"),
                                                    }
                                                ),
                                                new QuantifierToken(0, null),
                                                null,
                                                null
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfPositiveLiteralSet",
                    "",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfPositiveLiteralSet",
                    "привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfPositiveLiteralSet",
                    "пока",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfPositiveLiteralSet",
                    "куку",
                    true,
                    0,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfPositiveLiteralSet",
                    "привет пока",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfPositiveLiteralSet",
                    "пока привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfPositiveLiteralSet",
                    "привет привет пока",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfPositiveLiteralSet",
                    "пока привет привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfPositiveLiteralSet",
                    "привет пока привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfPositiveLiteralSet",
                    "привет пока куку",
                    true,
                    2,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfPositiveLiteralSet",
                    "привет куку пока",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfPositiveLiteralSet",
                    "куку привет пока",
                    true,
                    0,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfPositiveLiteralSet",
                    "куку куку",
                    true,
                    0,
                },
            };
        }

        public static object?[][] MatchesAndProjects_RepetitionOfNegativeLiteralSet()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "RepetitionOfNegativeLiteralSet",
                        (
                            String.Empty,
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
                                                        new LiteralToken("привет"),
                                                        new LiteralToken("пока"),
                                                    }
                                                ),
                                                new QuantifierToken(0, null),
                                                null,
                                                null
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfNegativeLiteralSet",
                    "",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfNegativeLiteralSet",
                    "привет",
                    true,
                    0,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfNegativeLiteralSet",
                    "пока",
                    true,
                    0,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfNegativeLiteralSet",
                    "куку",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfNegativeLiteralSet",
                    "куку куку",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfNegativeLiteralSet",
                    "куку ляля",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfNegativeLiteralSet",
                    "привет куку",
                    true,
                    0,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfNegativeLiteralSet",
                    "куку привет",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfNegativeLiteralSet",
                    "куку ляля привет",
                    true,
                    2,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfNegativeLiteralSet",
                    "привет пока",
                    true,
                    0,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfNegativeLiteralSet",
                    "пока привет",
                    true,
                    0,
                },
            };
        }

        public static object?[][] MatchesAndProjects_RepetitionOfAnyLiteral()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "RepetitionOfAnyLiteral",
                        (
                            String.Empty,
                            new PegGroupToken(
                                new []
                                {
                                    new BranchToken(
                                        new []
                                        {
                                            new BranchItemToken(
                                                AnyLiteralToken.Instance,
                                                new QuantifierToken(0, null),
                                                null,
                                                null
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfAnyLiteral",
                    "",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfAnyLiteral",
                    "привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfAnyLiteral",
                    "пока",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfAnyLiteral",
                    "привет привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "RepetitionOfAnyLiteral",
                    "привет пока",
                    true,
                    null,
                },
            };
        }

        public static object?[][] MatchesAndProjects_GroupWithLiterals()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "GroupWithLiterals",
                        (
                            String.Empty,
                            new PegGroupToken(
                                new []
                                {
                                    new BranchToken(
                                        new []
                                        {
                                            new BranchItemToken(new LiteralToken("привет"), new QuantifierToken(1, 1), null, null),
                                        }
                                    ),
                                    new BranchToken(
                                        new []
                                        {
                                            new BranchItemToken(new LiteralToken("пока"), new QuantifierToken(1, 1), null, null),
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithLiterals",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithLiterals",
                    "привет пока",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithLiterals",
                    "привет",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithLiterals",
                    "пока",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithLiterals",
                    "пока привет",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithLiterals",
                    "куку",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithLiterals",
                    "привет привет",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithLiterals",
                    "пока пока",
                    true,
                    1,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithLiterals",
                    "куку привет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithLiterals",
                    "куку пока",
                    false,
                    null,
                },
            };
        }

        public static object?[][] MatchesAndProjects_GroupWithSequences()
        {
            RuleSpaceSource ruleSpaceSource = new RuleSpaceSource(
                new Dictionary<string, (string Definition, PegGroupToken Token)>()
                {
                    {
                        "GroupWithSequences",
                        (
                            String.Empty,
                            new PegGroupToken(
                                new []
                                {
                                    new BranchToken(
                                        new []
                                        {
                                            new BranchItemToken(new LiteralToken("привет"), new QuantifierToken(1, 1), null, null),
                                            new BranchItemToken(new LiteralToken("меня"), new QuantifierToken(1, 1), null, null),
                                            new BranchItemToken(new LiteralToken("зовут"), new QuantifierToken(1, 1), null, null),
                                            new BranchItemToken(new LiteralToken("андрей"), new QuantifierToken(1, 1), null, null),
                                        }
                                    ),
                                    new BranchToken(
                                        new []
                                        {
                                            new BranchItemToken(new LiteralToken("привет"), new QuantifierToken(1, 1), null, null),
                                            new BranchItemToken(new LiteralToken("меня"), new QuantifierToken(1, 1), null, null),
                                            new BranchItemToken(new LiteralToken("зовут"), new QuantifierToken(1, 1), null, null),
                                            new BranchItemToken(new LiteralToken("антон"), new QuantifierToken(1, 1), null, null),
                                        }
                                    ),
                                    new BranchToken(
                                        new []
                                        {
                                            new BranchItemToken(new LiteralToken("привет"), new QuantifierToken(1, 1), null, null),
                                            new BranchItemToken(new LiteralToken("меня"), new QuantifierToken(1, 1), null, null),
                                            new BranchItemToken(new LiteralToken("никуда"), new QuantifierToken(1, 1), null, null),
                                            new BranchItemToken(new LiteralToken("не"), new QuantifierToken(1, 1), null, null),
                                            new BranchItemToken(new LiteralToken("зовут"), new QuantifierToken(1, 1), null, null),
                                        }
                                    )
                                }
                            )
                        )
                    }
                }
            );

            return new []
            {
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithSequences",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithSequences",
                    "привет меня зовут андрей",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithSequences",
                    "привет меня зовут антон",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithSequences",
                    "привет меня никуда не зовут",
                    true,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithSequences",
                    "привет меня никуда не",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithSequences",
                    "привет меня никуда андрей",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithSequences",
                    "привет меня зовут олег",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithSequences",
                    "привет меня зовут",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithSequences",
                    "привет",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithSequences",
                    "привет привет меня зовут андрей",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithSequences",
                    "куку",
                    false,
                    null,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithSequences",
                    "привет меня зовут андрей андрей",
                    true,
                    4,
                },
                new object?[]
                {
                    ruleSpaceSource,
                    "GroupWithSequences",
                    "привет меня зовут андрей антон",
                    true,
                    4,
                },
            };
        }

        public static object?[][] MatchesAndProjects_RecursionWithSimpleSumGrammar()
        {
            return new []
            {
                new object?[]
                {
                    SumOfNumbers0To9Grammar.Instance,
                    "Sum",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    SumOfNumbers0To9Grammar.Instance,
                    "Sum",
                    "ноль",
                    true,
                    null,
                },
                new object?[]
                {
                    SumOfNumbers0To9Grammar.Instance,
                    "Sum",
                    "пять",
                    true,
                    null,
                },
                new object?[]
                {
                    SumOfNumbers0To9Grammar.Instance,
                    "Sum",
                    "один два",
                    true,
                    1,
                },
                new object?[]
                {
                    SumOfNumbers0To9Grammar.Instance,
                    "Sum",
                    "ноль плюс ноль",
                    true,
                    null,
                },
                new object?[]
                {
                    SumOfNumbers0To9Grammar.Instance,
                    "Sum",
                    "ноль плюс ноль плюс ноль",
                    true,
                    null,
                },
                new object?[]
                {
                    SumOfNumbers0To9Grammar.Instance,
                    "Sum",
                    "ноль плюс один плюс ноль",
                    true,
                    null,
                },
                new object?[]
                {
                    SumOfNumbers0To9Grammar.Instance,
                    "Sum",
                    "два плюс один плюс ноль",
                    true,
                    null,
                },
                new object?[]
                {
                    SumOfNumbers0To9Grammar.Instance,
                    "Sum",
                    "ноль плюс один плюс два",
                    true,
                    null,
                },
                new object?[]
                {
                    SumOfNumbers0To9Grammar.Instance,
                    "Sum",
                    "плюс",
                    false,
                    null,
                },
                new object?[]
                {
                    SumOfNumbers0To9Grammar.Instance,
                    "Sum",
                    "плюс один",
                    false,
                    null,
                },
                new object?[]
                {
                    SumOfNumbers0To9Grammar.Instance,
                    "Sum",
                    "один плюс",
                    true,
                    1,
                },
                new object?[]
                {
                    SumOfNumbers0To9Grammar.Instance,
                    "Sum",
                    "один плюс два плюс",
                    true,
                    3,
                },
                new object?[]
                {
                    SumOfNumbers0To9Grammar.Instance,
                    "Sum",
                    "ох плюс ах",
                    false,
                    null,
                },
                new object?[]
                {
                    SumOfNumbers0To9Grammar.Instance,
                    "Sum",
                    "один минус два",
                    true,
                    1,
                },
            };
        }

        public static object?[][] MatchesAndProjects_RecursionWithSimpleArithmeticsGrammar()
        {
            return new []
            {
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "один",
                    true,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "один плюс два",
                    true,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "ноль минус девять",
                    true,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "два умножить на девять",
                    true,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "три поделить на ноль",
                    true,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "два умножить на девять поделить на ноль",
                    true,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "два плюс три умножить на девять минус четыре поделить на ноль умножить на два",
                    true,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "открывающаяся скобка два плюс три закрывающаяся скобка умножить на девять минус четыре",
                    true,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "открывающаяся скобка два плюс три умножить на девять минус четыре закрывающаяся скобка",
                    true,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "плюс",
                    false,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "минус",
                    false,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "умножить на",
                    false,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "поделить на",
                    false,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "один плюс",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "плюс один",
                    false,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "один умножить на",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "четыре поделить на",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "умножить на один",
                    false,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "поделить на пять",
                    false,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "один на два",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "один умножить два",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "один поделить два",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "один плюс минус два",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "один умножить на поделить на два",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "один плюс умножить на два",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "один умножить на плюс два",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "один один умножить на два",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "один один два",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "один умножить на один два",
                    true,
                    4,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "яблоко умножить на кокос",
                    false,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "слива плюс груша",
                    false,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "орхидея минус ананас",
                    false,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "четыре поделить на поезд",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "облако умножить на пять",
                    false,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "два плюс открывающаяся скобка",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "два плюс открывающаяся скобка три",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "открывающаяся скобка два плюс три",
                    false,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "два открывающаяся скобка плюс три закрывающаяся скобка",
                    true,
                    1,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "закрывающаяся скобка два плюс три открывающаяся скобка",
                    false,
                    null,
                },
                new object?[]
                {
                    ArithmeticOperationsOnNumbers0To9Grammar.Instance,
                    "Expression",
                    "закрывающаяся скобка два плюс три открывающаяся скобка умножить на шесть",
                    false,
                    null,
                },
            };
        }

        public static object?[][] MatchesAndProjects_RecursionWithRuleRepeatedInDifferentBranches()
        {
            return new []
            {
                new object?[]
                {
                    SumOrDifferenceBetweenNumbers0To9Grammar.Instance,
                    "Expression",
                    "",
                    false,
                    null,
                },
                new object?[]
                {
                    SumOrDifferenceBetweenNumbers0To9Grammar.Instance,
                    "Expression",
                    "один",
                    false,
                    null,
                },
                new object?[]
                {
                    SumOrDifferenceBetweenNumbers0To9Grammar.Instance,
                    "Expression",
                    "плюс",
                    false,
                    null,
                },
                new object?[]
                {
                    SumOrDifferenceBetweenNumbers0To9Grammar.Instance,
                    "Expression",
                    "минус",
                    false,
                    null,
                },
                new object?[]
                {
                    SumOrDifferenceBetweenNumbers0To9Grammar.Instance,
                    "Expression",
                    "один плюс",
                    false,
                    null,
                },
                new object?[]
                {
                    SumOrDifferenceBetweenNumbers0To9Grammar.Instance,
                    "Expression",
                    "минус один",
                    false,
                    null,
                },
                new object?[]
                {
                    SumOrDifferenceBetweenNumbers0To9Grammar.Instance,
                    "Expression",
                    "минус один плюс",
                    false,
                    null,
                },
                new object?[]
                {
                    SumOrDifferenceBetweenNumbers0To9Grammar.Instance,
                    "Expression",
                    "один плюс два",
                    true,
                    null,
                },
                new object?[]
                {
                    SumOrDifferenceBetweenNumbers0To9Grammar.Instance,
                    "Expression",
                    "ноль минус три",
                    true,
                    null,
                },
            };
        }

        #endregion

        #endregion
    }
}