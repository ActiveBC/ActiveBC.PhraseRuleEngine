using System;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.Common;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Equality;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Exceptions;
using NUnit.Framework;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Tests
{
    [TestFixture(TestOf = typeof(LoopBasedRegexPatternTokenizer))]
    internal sealed class RegexPatternTokenizerTests
    {
        private readonly LoopBasedRegexPatternTokenizer m_tokenizer = new LoopBasedRegexPatternTokenizer(new StringInterner());

        [Test]
        [TestCaseSource(nameof(Marker))]
        [TestCaseSource(nameof(Group))]
        [TestCaseSource(nameof(Literal))]
        [TestCaseSource(nameof(Prefix))]
        [TestCaseSource(nameof(Infix))]
        [TestCaseSource(nameof(Suffix))]
        [TestCaseSource(nameof(ManyLiteralLikePieces))]
        [TestCaseSource(nameof(ManyBranches))]
        [TestCaseSource(nameof(LiteralSet))]
        [TestCaseSource(nameof(Ner))]
        [TestCaseSource(nameof(QuantifierPlus))]
        [TestCaseSource(nameof(QuantifierQuestion))]
        [TestCaseSource(nameof(QuantifierStar))]
        [TestCaseSource(nameof(QuantifierExplicit))]
        [TestCaseSource(nameof(ComplexCases))]
        [TestCaseSource(nameof(EmptyLiteralSet))]
        public void Tokenizes(string pattern, RegexGroupToken expectedPatternToken)
        {
            RegexGroupToken patternToken = (RegexGroupToken) this.m_tokenizer.Tokenize(pattern, null, false);

            Assert.IsNotNull(patternToken);

            Assert.That(patternToken, Is.EqualTo(expectedPatternToken).Using(RegexGroupTokenEqualityComparer.Instance));
        }

        [Test]
        [TestCase("()", "Empty branches are not allowed. Absolute position: 1. Line: 0; position in line: 1. Near character: ')'. Context: '()'.")]
        [TestCase("(", "Not closed open parentheses. Absolute position: 0. Line: 0; position in line: 0. Near character: '('. Context: '('.")]
        [TestCase("(один два 「」)", "Empty marker. Absolute position: 10. Line: 0; position in line: 10. Near character: '「'. Context: '(один два 「」)'.")]
        [TestCase("(~)", "Empty word. Absolute position: 2. Line: 0; position in line: 2. Near character: ')'. Context: '(~)'.")]
        [TestCase("(~~)", "Empty word. Absolute position: 2. Line: 0; position in line: 2. Near character: '~'. Context: '(~~)'.")]
        [TestCase("(~а~~)", "Empty word. Absolute position: 5. Line: 0; position in line: 5. Near character: ')'. Context: '(~а~~)'.")]
        [TestCase("(~~а)", "Empty word. Absolute position: 2. Line: 0; position in line: 2. Near character: '~'. Context: '(~~а)'.")]
        [TestCase("(а~а)", "Found word start in invalid position. Absolute position: 3. Line: 0; position in line: 3. Near character: 'а'. Context: '(а~а)'.")]
        [TestCase("([~])", "Empty word. Absolute position: 3. Line: 0; position in line: 3. Near character: ']'. Context: '([~])'.")]
        [TestCase("(~|~)", "Empty word. Absolute position: 2. Line: 0; position in line: 2. Near character: '|'. Context: '(~|~)'.")]
        [TestCase("(а+а)", "Found word start in invalid position. Absolute position: 3. Line: 0; position in line: 3. Near character: 'а'. Context: '(а+а)'.")]
        [TestCase("(а?а)", "Found word start in invalid position. Absolute position: 3. Line: 0; position in line: 3. Near character: 'а'. Context: '(а?а)'.")]
        [TestCase("(а*а)", "Found word start in invalid position. Absolute position: 3. Line: 0; position in line: 3. Near character: 'а'. Context: '(а*а)'.")]
        [TestCase("(а{1,2}а)", "Found word start in invalid position. Absolute position: 7. Line: 0; position in line: 7. Near character: 'а'. Context: '(а{1,2}а)'.")]
        [TestCase("(а??)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '?'. Context: '(а??)'.")]
        [TestCase("(а?+)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '+'. Context: '(а?+)'.")]
        [TestCase("(а?*)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '*'. Context: '(а?*)'.")]
        [TestCase("(а?{1})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(а?{1})'.")]
        [TestCase("(а?{1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(а?{1,2})'.")]
        [TestCase("(а?{1,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(а?{1,})'.")]
        [TestCase("(а+?)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '?'. Context: '(а+?)'.")]
        [TestCase("(а++)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '+'. Context: '(а++)'.")]
        [TestCase("(а+*)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '*'. Context: '(а+*)'.")]
        [TestCase("(а+{1})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(а+{1})'.")]
        [TestCase("(а+{1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(а+{1,2})'.")]
        [TestCase("(а+{1,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(а+{1,})'.")]
        [TestCase("(а*?)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '?'. Context: '(а*?)'.")]
        [TestCase("(а*+)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '+'. Context: '(а*+)'.")]
        [TestCase("(а**)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '*'. Context: '(а**)'.")]
        [TestCase("(а*{1})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(а*{1})'.")]
        [TestCase("(а*{1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(а*{1,2})'.")]
        [TestCase("(а*{1,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(а*{1,})'.")]
        [TestCase("(а{1}?)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 5. Line: 0; position in line: 5. Near character: '?'. Context: '(а{1}?)'.")]
        [TestCase("(а{1}+)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 5. Line: 0; position in line: 5. Near character: '+'. Context: '(а{1}+)'.")]
        [TestCase("(а{1}*)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 5. Line: 0; position in line: 5. Near character: '*'. Context: '(а{1}*)'.")]
        [TestCase("(а{1}{1})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 5. Line: 0; position in line: 5. Near character: '{'. Context: '(а{1}{1})'.")]
        [TestCase("(а{1}{1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 5. Line: 0; position in line: 5. Near character: '{'. Context: '(а{1}{1,2})'.")]
        [TestCase("(а{1}{1,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 5. Line: 0; position in line: 5. Near character: '{'. Context: '(а{1}{1,})'.")]
        [TestCase("(а{1,2}?)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 7. Line: 0; position in line: 7. Near character: '?'. Context: '(а{1,2}?)'.")]
        [TestCase("(а{1,2}+)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 7. Line: 0; position in line: 7. Near character: '+'. Context: '(а{1,2}+)'.")]
        [TestCase("(а{1,2}*)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 7. Line: 0; position in line: 7. Near character: '*'. Context: '(а{1,2}*)'.")]
        [TestCase("(а{1,2}{1})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 7. Line: 0; position in line: 7. Near character: '{'. Context: '(а{1,2}{1})'.")]
        [TestCase("(а{1,2}{1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 7. Line: 0; position in line: 7. Near character: '{'. Context: '(а{1,2}{1,2})'.")]
        [TestCase("(а{1,2}{1,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 7. Line: 0; position in line: 7. Near character: '{'. Context: '(а{1,2}{1,})'.")]
        [TestCase("(а{1,}?)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 6. Line: 0; position in line: 6. Near character: '?'. Context: '(а{1,}?)'.")]
        [TestCase("(а{1,}+)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 6. Line: 0; position in line: 6. Near character: '+'. Context: '(а{1,}+)'.")]
        [TestCase("(а{1,}*)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 6. Line: 0; position in line: 6. Near character: '*'. Context: '(а{1,}*)'.")]
        [TestCase("(а{1,}{1})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '(а{1,}{1})'.")]
        [TestCase("(а{1,}{1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '(а{1,}{1,2})'.")]
        [TestCase("(а{1,}{1,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '(а{1,}{1,})'.")]
        [TestCase("(「маркер」*)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 9. Line: 0; position in line: 9. Near character: '*'. Context: '(「маркер」*)'.")]
        [TestCase("(「маркер」+)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 9. Line: 0; position in line: 9. Near character: '+'. Context: '(「маркер」+)'.")]
        [TestCase("(「маркер」{1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 9. Line: 0; position in line: 9. Near character: '{'. Context: '(「маркер」{1,2})'.")]
        [TestCase("(「маркер」{2,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 9. Line: 0; position in line: 9. Near character: '{'. Context: '(「маркер」{2,})'.")]
        [TestCase("(「маркер」{3})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 9. Line: 0; position in line: 9. Near character: '{'. Context: '(「маркер」{3})'.")]
        [TestCase("(|привет|пока)", "Empty branches are not allowed. Absolute position: 1. Line: 0; position in line: 1. Near character: '|'. Context: '(|привет|пок'.")]
        [TestCase("(один два три четыре пять (|привет|пока) шесть семь восемь)", "Empty branches are not allowed. Absolute position: 27. Line: 0; position in line: 27. Near character: '|'. Context: 'ыре пять (|привет|пок'.")]
        [TestCase("(один два три четыре пять (привет||пока) шесть семь восемь)", "Empty branches are not allowed. Absolute position: 34. Line: 0; position in line: 34. Near character: '|'. Context: 'ь (привет||пока) шест'.")]
        [TestCase("(один два три четыре пять (привет|пока|) шесть семь восемь)", "Empty branches are not allowed. Absolute position: 39. Line: 0; position in line: 39. Near character: ')'. Context: 'ивет|пока|) шесть сем'.")]
        [TestCase("(один два три четыре пять (||привет|пока) шесть семь восемь)", "Empty branches are not allowed. Absolute position: 27. Line: 0; position in line: 27. Near character: '|'. Context: 'ыре пять (||привет|по'.")]
        [TestCase("(один два три четыре пять (привет|пока||) шесть семь восемь)", "Empty branches are not allowed. Absolute position: 39. Line: 0; position in line: 39. Near character: '|'. Context: 'ивет|пока||) шесть се'.")]
        [TestCase("(один два три четыре пять () шесть семь восемь)", "Empty branches are not allowed. Absolute position: 27. Line: 0; position in line: 27. Near character: ')'. Context: 'ыре пять () шесть сем'.")]
        [TestCase("({1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 1. Line: 0; position in line: 1. Near character: '{'. Context: '({1,2})'.")]
        [TestCase("({2,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 1. Line: 0; position in line: 1. Near character: '{'. Context: '({2,})'.")]
        [TestCase("({3})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 1. Line: 0; position in line: 1. Near character: '{'. Context: '({3})'.")]
        [TestCase("(+)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 1. Line: 0; position in line: 1. Near character: '+'. Context: '(+)'.")]
        [TestCase("(?)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 1. Line: 0; position in line: 1. Near character: '?'. Context: '(?)'.")]
        [TestCase("(*)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 1. Line: 0; position in line: 1. Near character: '*'. Context: '(*)'.")]
        [TestCase("(один {1,2})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '(один {1,2})'.")]
        [TestCase("(один {2,})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '(один {2,})'.")]
        [TestCase("(один {3})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '(один {3})'.")]
        [TestCase("(один +)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 6. Line: 0; position in line: 6. Near character: '+'. Context: '(один +)'.")]
        [TestCase("(один ?)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 6. Line: 0; position in line: 6. Near character: '?'. Context: '(один ?)'.")]
        [TestCase("(один *)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 6. Line: 0; position in line: 6. Near character: '*'. Context: '(один *)'.")]
        [TestCase("(~one~ {1,2})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 7. Line: 0; position in line: 7. Near character: '{'. Context: '(~one~ {1,2})'.")]
        [TestCase("(~one~ {2,})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 7. Line: 0; position in line: 7. Near character: '{'. Context: '(~one~ {2,})'.")]
        [TestCase("(~one~ {3})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 7. Line: 0; position in line: 7. Near character: '{'. Context: '(~one~ {3})'.")]
        [TestCase("(~one~ +)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 7. Line: 0; position in line: 7. Near character: '+'. Context: '(~one~ +)'.")]
        [TestCase("(~one~ ?)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 7. Line: 0; position in line: 7. Near character: '?'. Context: '(~one~ ?)'.")]
        [TestCase("(~one~ *)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 7. Line: 0; position in line: 7. Near character: '*'. Context: '(~one~ *)'.")]
        [TestCase("(.\r\n{1,2})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 4. Line: 1; position in line: 0. Near character: '{'. Context: '{1,2})'.")]
        [TestCase("(.\r\n{2,})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 4. Line: 1; position in line: 0. Near character: '{'. Context: '{2,})'.")]
        [TestCase("(.\r\n{3})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 4. Line: 1; position in line: 0. Near character: '{'. Context: '{3})'.")]
        [TestCase("(.\r\n+)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 4. Line: 1; position in line: 0. Near character: '+'. Context: '+)'.")]
        [TestCase("(.\r\n?)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 4. Line: 1; position in line: 0. Near character: '?'. Context: '?)'.")]
        [TestCase("(.\r\n*)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 4. Line: 1; position in line: 0. Near character: '*'. Context: '*)'.")]
        [TestCase("([один one]\t \t{1,2})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 14. Line: 0; position in line: 14. Near character: '{'. Context: 'ин one]\t \t{1,2})'.")]
        [TestCase("([один one]\t \t{2,})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 14. Line: 0; position in line: 14. Near character: '{'. Context: 'ин one]\t \t{2,})'.")]
        [TestCase("([один one]\t \t{3})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 14. Line: 0; position in line: 14. Near character: '{'. Context: 'ин one]\t \t{3})'.")]
        [TestCase("([один one]\t \t+)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 14. Line: 0; position in line: 14. Near character: '+'. Context: 'ин one]\t \t+)'.")]
        [TestCase("([один one]\t \t?)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 14. Line: 0; position in line: 14. Near character: '?'. Context: 'ин one]\t \t?)'.")]
        [TestCase("([один one]\t \t*)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 14. Line: 0; position in line: 14. Near character: '*'. Context: 'ин one]\t \t*)'.")]
        [TestCase("привет{0}", "Cannot create a quantifier {0} with n < 1. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: 'привет{0}'.")]
        [TestCase("привет{-1}", "Cannot create a quantifier {-1} with n < 1. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: 'привет{-1}'.")]
        [TestCase("привет{-53}", "Cannot create a quantifier {-53} with n < 1. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: 'привет{-53}'.")]
        [TestCase("привет{-1,}", "Cannot create a quantifier {-1,} with n < 0. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: 'привет{-1,}'.")]
        [TestCase("привет{-53,}", "Cannot create a quantifier {-53,} with n < 0. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: 'привет{-53,}'.")]
        [TestCase("привет{-1,15}", "Cannot create a quantifier {-1,15} with n < 0. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: 'привет{-1,15}'.")]
        [TestCase("привет{15,-1}", "Cannot create a quantifier {15,-1} with n > m. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: 'привет{15,-1}'.")]
        [TestCase("привет{-67,36}", "Cannot create a quantifier {-67,36} with n < 0. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: 'привет{-67,36}'.")]
        [TestCase("привет{36,-67}", "Cannot create a quantifier {36,-67} with n > m. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: 'привет{36,-67}'.")]
        [TestCase("привет{1,0}", "Cannot create a quantifier {1,0} with n > m. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: 'привет{1,0}'.")]
        [TestCase("привет{2,1}", "Cannot create a quantifier {2,1} with n > m. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: 'привет{2,1}'.")]
        [TestCase("привет{38,24}", "Cannot create a quantifier {38,24} with n > m. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: 'привет{38,24}'.")]
        [TestCase("привет{96,53}", "Cannot create a quantifier {96,53} with n > m. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: 'привет{96,53}'.")]
        public void Fails(string pattern, string expectedErrorMessage)
        {
            RegexPatternTokenizationException? exception = Assert.Throws<RegexPatternTokenizationException>(
                () => this.m_tokenizer.Tokenize(pattern, null, false)
            );

            Assert.AreEqual(expectedErrorMessage, exception!.Message);
        }

        #region Sources

        #region Sources_Tokenizes

        public static object?[][] Literal =
        {
            new object?[]
            {
                "(а)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(аб)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("аб"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
        };

        public static object?[][] Prefix =
        {
            new object?[]
            {
                "(а~)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new PrefixToken("а"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(аб~)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new PrefixToken("аб"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
        };

        public static object?[][] Infix =
        {
            new object?[]
            {
                "(~а~)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new InfixToken("а"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~аб~)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new InfixToken("аб"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
        };

        public static object?[][] Suffix =
        {
            new object?[]
            {
                "(~а)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new SuffixToken("а"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~аб)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new SuffixToken("аб"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
        };

        public static object?[][] ManyLiteralLikePieces =
        {
            new object?[]
            {
                "(а б)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new LiteralToken("б"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(аб вг)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("аб"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new LiteralToken("вг"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(а а~ ~а~ ~а)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new PrefixToken("а"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new InfixToken("а"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new SuffixToken("а"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(аб ~вг~ вг~ ~де)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("аб"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new InfixToken("вг"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new PrefixToken("вг"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new SuffixToken("де"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
        };

        public static object?[][] ManyBranches =
        {
            new object?[]
            {
                "(а|б)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(1, 1), null),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("б"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(аб|вг|де)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("аб"), new QuantifierToken(1, 1), null),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("вг"), new QuantifierToken(1, 1), null),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("де"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
        };

        public static object?[][] Marker =
        {
            new object?[]
            {
                "((это тест грамматики <someVar = sdn.RelativeTimeSpan()>) 「__id_0」)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new RegexGroupToken(
                                        new []
                                        {
                                            new BranchToken(
                                                new IBranchItemToken[]
                                                {
                                                    new QuantifiableBranchItemToken(
                                                        new LiteralToken("это"),
                                                        new QuantifierToken(1, 1),
                                                        null
                                                    ),
                                                    new QuantifiableBranchItemToken(
                                                        new LiteralToken("тест"),
                                                        new QuantifierToken(1, 1),
                                                        null
                                                    ),
                                                    new QuantifiableBranchItemToken(
                                                        new LiteralToken("грамматики"),
                                                        new QuantifierToken(1, 1),
                                                        null
                                                    ),
                                                    new QuantifiableBranchItemToken(
                                                        new RuleReferenceToken(
                                                            null,
                                                            "sdn.RelativeTimeSpan",
                                                            Array.Empty<IRuleArgumentToken>()
                                                        ),
                                                        new QuantifierToken(1, 1),
                                                        "someVar"
                                                    ),
                                                }
                                            ),
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    null
                                ),
                                new MarkerToken("__id_0"),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(「foo」 а?|~бв~+|[^гд])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new MarkerToken("foo"),
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(0, 1), null),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new InfixToken("бв"), new QuantifierToken(1, null), null),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(true, new ILiteralSetMemberToken[]{new LiteralToken("гд")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(「foo1」 (「фуу2」 а+|(「```」 а)*))",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new MarkerToken("foo1"),
                                new QuantifiableBranchItemToken(
                                    new RegexGroupToken(
                                        new []
                                        {
                                            new BranchToken(
                                                new IBranchItemToken[]
                                                {
                                                    new MarkerToken("фуу2"),
                                                    new QuantifiableBranchItemToken(
                                                        new LiteralToken("а"),
                                                        new QuantifierToken(1, null),
                                                        null
                                                    ),
                                                }
                                            ),
                                            new BranchToken(
                                                new IBranchItemToken[]
                                                {
                                                    new QuantifiableBranchItemToken(
                                                        new RegexGroupToken(
                                                            new []
                                                            {
                                                                new BranchToken(
                                                                    new IBranchItemToken[]
                                                                    {
                                                                        new MarkerToken("```"),
                                                                        new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(1, 1), null),
                                                                    }
                                                                ),
                                                            }
                                                        ),
                                                        new QuantifierToken(0, null),
                                                        null
                                                    ),
                                                }
                                            ),
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    null
                                )
                            }
                        )
                    }
                )
            },
            new object?[]
            {
                "(「foo」 а)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new MarkerToken("foo"),
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(「123」 а)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new MarkerToken("123"),
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(「эюя」 а)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new MarkerToken("эюя"),
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(「.*'`\"\uD800」 а)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new MarkerToken(".*'`\"\uD800"),
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(「ц.d*ё'」 а)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new MarkerToken("ц.d*ё'"),
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
        };

        public static object?[][] Group =
        {
            new object?[]
            {
                "((а|б в)|(б в|г))",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new RegexGroupToken(
                                        new []
                                        {
                                            new BranchToken(
                                                new IBranchItemToken[]
                                                {
                                                    new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(1, 1), null),
                                                }
                                            ),
                                            new BranchToken(
                                                new IBranchItemToken[]
                                                {
                                                    new QuantifiableBranchItemToken(new LiteralToken("б"), new QuantifierToken(1, 1), null),
                                                    new QuantifiableBranchItemToken(new LiteralToken("в"), new QuantifierToken(1, 1), null),
                                                }
                                            ),
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    null
                                )
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new RegexGroupToken(
                                        new []
                                        {
                                            new BranchToken(
                                                new IBranchItemToken[]
                                                {
                                                    new QuantifiableBranchItemToken(new LiteralToken("б"), new QuantifierToken(1, 1), null),
                                                    new QuantifiableBranchItemToken(new LiteralToken("в"), new QuantifierToken(1, 1), null),
                                                }
                                            ),
                                            new BranchToken(
                                                new IBranchItemToken[]
                                                {
                                                    new QuantifiableBranchItemToken(new LiteralToken("г"), new QuantifierToken(1, 1), null),
                                                }
                                            ),
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    null
                                )
                            }
                        )
                    }
                ),
            }
        };

        public static object?[][] ComplexCases =
        {
            new object?[]
            {
                "(「foo1」 а|а+|а <foo = bar.baz(a.b)>?|[чижик пыжик~ ~где ты]?|(на ~жз~ [к ~п~ т] (「фуу2」 <baz = foo.bar(default)>{2,5} ~ж+ з{1,20}|~п~*|[^~фыва пролд~]))*|[^к клмо лмо] опрстуф~ (.?|.+){11} .* .{2,23}|эюя~)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new MarkerToken("foo1"),
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(1, 1), null),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(1, null), null),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(
                                    new RuleReferenceToken(
                                        null,
                                        "bar.baz",
                                        new IRuleArgumentToken[]{new RuleChainedMemberAccessArgumentToken(new [] {"a", "b"})}
                                    ),
                                    new QuantifierToken(0, 1),
                                    "foo"
                                ),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new LiteralSetToken(
                                        false,
                                        new ILiteralSetMemberToken[]
                                        {
                                            new LiteralToken("чижик"),
                                            new PrefixToken("пыжик"),
                                            new SuffixToken("где"),
                                            new LiteralToken("ты"),
                                        }
                                    ),
                                    new QuantifierToken(0, 1),
                                    null
                                ),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new RegexGroupToken(
                                        new []
                                        {
                                            new BranchToken(
                                                new IBranchItemToken[]
                                                {
                                                    new QuantifiableBranchItemToken(new LiteralToken("на"), new QuantifierToken(1, 1), null),
                                                    new QuantifiableBranchItemToken(new InfixToken("жз"), new QuantifierToken(1, 1), null),
                                                    new QuantifiableBranchItemToken(
                                                        new LiteralSetToken(
                                                            false,
                                                            new ILiteralSetMemberToken[]
                                                            {
                                                                new LiteralToken("к"),
                                                                new InfixToken("п"),
                                                                new LiteralToken("т"),
                                                            }
                                                        ),
                                                        new QuantifierToken(1, 1),
                                                        null
                                                    ),
                                                    new QuantifiableBranchItemToken(
                                                        new RegexGroupToken(
                                                            new []
                                                            {
                                                                new BranchToken(
                                                                    new IBranchItemToken[]
                                                                    {
                                                                        new MarkerToken("фуу2"),
                                                                        new QuantifiableBranchItemToken(new RuleReferenceToken(null, "foo.bar", new IRuleArgumentToken[]{RuleDefaultArgumentToken.Instance}), new QuantifierToken(2, 5), "baz"),
                                                                        new QuantifiableBranchItemToken(new SuffixToken("ж"), new QuantifierToken(1, null), null),
                                                                        new QuantifiableBranchItemToken(new LiteralToken("з"), new QuantifierToken(1, 20), null),
                                                                    }
                                                                ),
                                                                new BranchToken(
                                                                    new IBranchItemToken[]
                                                                    {
                                                                        new QuantifiableBranchItemToken(new InfixToken("п"), new QuantifierToken(0, null), null),
                                                                    }
                                                                ),
                                                                new BranchToken(
                                                                    new IBranchItemToken[]
                                                                    {
                                                                        new QuantifiableBranchItemToken(
                                                                            new LiteralSetToken(
                                                                                true,
                                                                                new ILiteralSetMemberToken[]
                                                                                {
                                                                                    new SuffixToken("фыва"),
                                                                                    new PrefixToken("пролд"),
                                                                                }
                                                                            ),
                                                                            new QuantifierToken(1, 1),
                                                                            null
                                                                        ),
                                                                    }
                                                                ),
                                                            }
                                                        ),
                                                        new QuantifierToken(1, 1),
                                                        null
                                                    ),
                                                }
                                            ),
                                        }
                                    ),
                                    new QuantifierToken(0, null),
                                    null
                                ),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new LiteralSetToken(
                                        true,
                                        new ILiteralSetMemberToken[]
                                        {
                                            new LiteralToken("к"),
                                            new LiteralToken("клмо"),
                                            new LiteralToken("лмо"),
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    null
                                ),
                                new QuantifiableBranchItemToken(new PrefixToken("опрстуф"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(
                                    new RegexGroupToken(
                                        new []
                                        {
                                            new BranchToken(
                                                new IBranchItemToken[]
                                                {
                                                    new QuantifiableBranchItemToken(AnyLiteralToken.Instance, new QuantifierToken(0, 1), null),
                                                }
                                            ),
                                            new BranchToken(
                                                new IBranchItemToken[]
                                                {
                                                    new QuantifiableBranchItemToken(AnyLiteralToken.Instance, new QuantifierToken(1, null), null),
                                                }
                                            ),
                                        }
                                    ),
                                    new QuantifierToken(11, 11),
                                    null
                                ),
                                new QuantifiableBranchItemToken(AnyLiteralToken.Instance, new QuantifierToken(0, null), null),
                                new QuantifiableBranchItemToken(AnyLiteralToken.Instance, new QuantifierToken(2, 23), null),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new PrefixToken("эюя"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
        };

        public static object?[][] LiteralSet =
        {
            new object?[]
            {
                "([а])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(false, new ILiteralSetMemberToken[]{new LiteralToken("а")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([аб])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(false, new ILiteralSetMemberToken[]{new LiteralToken("аб")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^а])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(true, new ILiteralSetMemberToken[]{new LiteralToken("а")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^аб])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(true, new ILiteralSetMemberToken[]{new LiteralToken("аб")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([а б])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(false, new ILiteralSetMemberToken[]{new LiteralToken("а"), new LiteralToken("б")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([аб вг])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(false, new ILiteralSetMemberToken[]{new LiteralToken("аб"), new LiteralToken("вг")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^а б])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(true, new ILiteralSetMemberToken[]{new LiteralToken("а"), new LiteralToken("б")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^аб вг])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(true, new ILiteralSetMemberToken[]{new LiteralToken("аб"), new LiteralToken("вг")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
        };

        public static object?[][] Ner =
        {
            new object?[]
            {
                "(<SomeVar = Ner.SomeNer(SomeArg.FooBar)>)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new RuleReferenceToken(
                                        null,
                                        "Ner.SomeNer",
                                        new IRuleArgumentToken[]
                                        {
                                            new RuleChainedMemberAccessArgumentToken(new [] {"SomeArg", "FooBar"}),
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    "SomeVar"
                                )
                            }
                        )
                    }
                ),
            },
            new object?[]
            {
                "(<some_var = ner_obj.some_ner(some_arg.foo_bar)>)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new RuleReferenceToken(
                                        null,
                                        "ner_obj.some_ner",
                                        new IRuleArgumentToken[]
                                        {
                                            new RuleChainedMemberAccessArgumentToken(new [] {"some_arg", "foo_bar"}),
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    "some_var"
                                )
                            }
                        )
                    }
                ),
            },
            new object?[]
            {
                "(<someVar = ner.someNer(someArg.Foo, anotherArg.Bar)>)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new RuleReferenceToken(
                                        null,
                                        "ner.someNer",
                                        new IRuleArgumentToken[]
                                        {
                                            new RuleChainedMemberAccessArgumentToken(new [] {"someArg", "Foo"}),
                                            new RuleChainedMemberAccessArgumentToken(new [] {"anotherArg", "Bar"}),
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    "someVar"
                                )
                            }
                        )
                    }
                ),
            },
            new object?[]
            {
                "(<someVar = ner.some_ner(default)>)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new RuleReferenceToken(
                                        null,
                                        "ner.some_ner",
                                        new IRuleArgumentToken[]
                                        {
                                            RuleDefaultArgumentToken.Instance,
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    "someVar"
                                )
                            }
                        )
                    }
                ),
            },
            new object?[]
            {
                "(<someVar = ner.some_ner(foo.Foo, default, foo.Foo)>)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new RuleReferenceToken(
                                        null,
                                        "ner.some_ner",
                                        new IRuleArgumentToken[]
                                        {
                                            new RuleChainedMemberAccessArgumentToken(new [] {"foo", "Foo"}),
                                            RuleDefaultArgumentToken.Instance,
                                            new RuleChainedMemberAccessArgumentToken(new [] {"foo", "Foo"}),
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    "someVar"
                                )
                            }
                        )
                    }
                ),
            },
        };

        public static object?[][] QuantifierPlus =
        {
            new object?[]
            {
                "(а+)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(1, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(аб+)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("аб"), new QuantifierToken(1, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^а ~б ~в~ г~]+)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new LiteralSetToken(
                                        true,
                                        new ILiteralSetMemberToken[]
                                        {
                                            new LiteralToken("а"),
                                            new SuffixToken("б"),
                                            new InfixToken("в"),
                                            new PrefixToken("г"),
                                        }
                                    ),
                                    new QuantifierToken(1, null),
                                    null
                                ),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(а~+)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new PrefixToken("а"), new QuantifierToken(1, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~а~+)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new InfixToken("а"), new QuantifierToken(1, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~а+)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new SuffixToken("а"), new QuantifierToken(1, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(.+)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(AnyLiteralToken.Instance, new QuantifierToken(1, null), null),
                            }
                        ),
                    }
                ),
            },
        };

        public static object?[][] QuantifierQuestion =
        {
            new object?[]
            {
                "(а?)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(0, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(аб?)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("аб"), new QuantifierToken(0, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^а ~б ~в~ г~]?)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new LiteralSetToken(
                                        true,
                                        new ILiteralSetMemberToken[]
                                        {
                                            new LiteralToken("а"),
                                            new SuffixToken("б"),
                                            new InfixToken("в"),
                                            new PrefixToken("г"),
                                        }
                                    ),
                                    new QuantifierToken(0, 1),
                                    null
                                ),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(а~?)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new PrefixToken("а"), new QuantifierToken(0, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~а~?)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new InfixToken("а"), new QuantifierToken(0, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~а?)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new SuffixToken("а"), new QuantifierToken(0, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(.?)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(AnyLiteralToken.Instance, new QuantifierToken(0, 1), null),
                            }
                        ),
                    }
                ),
            },
        };

        public static object?[][] QuantifierStar =
        {
            new object?[]
            {
                "(а*)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(0, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(аб*)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("аб"), new QuantifierToken(0, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^а ~б ~в~ г~]*)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new LiteralSetToken(
                                        true,
                                        new ILiteralSetMemberToken[]
                                        {
                                            new LiteralToken("а"),
                                            new SuffixToken("б"),
                                            new InfixToken("в"),
                                            new PrefixToken("г"),
                                        }
                                    ),
                                    new QuantifierToken(0, null),
                                    null
                                ),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(а~*)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new PrefixToken("а"), new QuantifierToken(0, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~а~*)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new InfixToken("а"), new QuantifierToken(0, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~а*)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new SuffixToken("а"), new QuantifierToken(0, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(.*)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(AnyLiteralToken.Instance, new QuantifierToken(0, null), null),
                            }
                        ),
                    }
                ),
            },
        };

        public static object?[][] QuantifierExplicit =
        {
            new object?[]
            {
                "(а{2,33})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(2, 33), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(аб{2,33})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("аб"), new QuantifierToken(2, 33), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(а{22,})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(22, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(а{33})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(33, 33), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(а{44,44})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("а"), new QuantifierToken(44, 44), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^а ~б ~в~ г~]{23,24})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new LiteralSetToken(
                                        true,
                                        new ILiteralSetMemberToken[]
                                        {
                                            new LiteralToken("а"),
                                            new SuffixToken("б"),
                                            new InfixToken("в"),
                                            new PrefixToken("г"),
                                        }
                                    ),
                                    new QuantifierToken(23, 24),
                                    null
                                ),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(а~{23,})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new PrefixToken("а"), new QuantifierToken(23, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~а~{20})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new InfixToken("а"), new QuantifierToken(20, 20), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~а{17,18})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new SuffixToken("а"), new QuantifierToken(17, 18), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(.{20,})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(AnyLiteralToken.Instance, new QuantifierToken(20, null), null),
                            }
                        ),
                    }
                ),
            },
        };

        public static object?[][] EmptyLiteralSet =
        {
            new object?[]
            {
                "([])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new LiteralSetToken(false, Array.Empty<ILiteralSetMemberToken>()),
                                    new QuantifierToken(1, 1),
                                    null
                                )
                            }
                        )
                    }
                ),
            },
            new object?[]
            {
                "([^])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(
                                    new LiteralSetToken(true, Array.Empty<ILiteralSetMemberToken>()),
                                    new QuantifierToken(1, 1),
                                    null
                                ),
                            }
                        )
                    }
                ),
            },
        };

        #endregion

        #endregion
    }
}