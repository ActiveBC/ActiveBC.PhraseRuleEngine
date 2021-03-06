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
        [TestCase("(???????? ?????? ??????)", "Empty marker. Absolute position: 10. Line: 0; position in line: 10. Near character: '???'. Context: '(???????? ?????? ??????)'.")]
        [TestCase("(~)", "Empty word. Absolute position: 2. Line: 0; position in line: 2. Near character: ')'. Context: '(~)'.")]
        [TestCase("(~~)", "Empty word. Absolute position: 2. Line: 0; position in line: 2. Near character: '~'. Context: '(~~)'.")]
        [TestCase("(~??~~)", "Empty word. Absolute position: 5. Line: 0; position in line: 5. Near character: ')'. Context: '(~??~~)'.")]
        [TestCase("(~~??)", "Empty word. Absolute position: 2. Line: 0; position in line: 2. Near character: '~'. Context: '(~~??)'.")]
        [TestCase("(??~??)", "Found word start in invalid position. Absolute position: 3. Line: 0; position in line: 3. Near character: '??'. Context: '(??~??)'.")]
        [TestCase("([~])", "Empty word. Absolute position: 3. Line: 0; position in line: 3. Near character: ']'. Context: '([~])'.")]
        [TestCase("(~|~)", "Empty word. Absolute position: 2. Line: 0; position in line: 2. Near character: '|'. Context: '(~|~)'.")]
        [TestCase("(??+??)", "Found word start in invalid position. Absolute position: 3. Line: 0; position in line: 3. Near character: '??'. Context: '(??+??)'.")]
        [TestCase("(?????)", "Found word start in invalid position. Absolute position: 3. Line: 0; position in line: 3. Near character: '??'. Context: '(?????)'.")]
        [TestCase("(??*??)", "Found word start in invalid position. Absolute position: 3. Line: 0; position in line: 3. Near character: '??'. Context: '(??*??)'.")]
        [TestCase("(??{1,2}??)", "Found word start in invalid position. Absolute position: 7. Line: 0; position in line: 7. Near character: '??'. Context: '(??{1,2}??)'.")]
        [TestCase("(????)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '?'. Context: '(????)'.")]
        [TestCase("(???+)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '+'. Context: '(???+)'.")]
        [TestCase("(???*)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '*'. Context: '(???*)'.")]
        [TestCase("(???{1})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(???{1})'.")]
        [TestCase("(???{1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(???{1,2})'.")]
        [TestCase("(???{1,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(???{1,})'.")]
        [TestCase("(??+?)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '?'. Context: '(??+?)'.")]
        [TestCase("(??++)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '+'. Context: '(??++)'.")]
        [TestCase("(??+*)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '*'. Context: '(??+*)'.")]
        [TestCase("(??+{1})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(??+{1})'.")]
        [TestCase("(??+{1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(??+{1,2})'.")]
        [TestCase("(??+{1,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(??+{1,})'.")]
        [TestCase("(??*?)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '?'. Context: '(??*?)'.")]
        [TestCase("(??*+)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '+'. Context: '(??*+)'.")]
        [TestCase("(??**)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '*'. Context: '(??**)'.")]
        [TestCase("(??*{1})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(??*{1})'.")]
        [TestCase("(??*{1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(??*{1,2})'.")]
        [TestCase("(??*{1,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 3. Line: 0; position in line: 3. Near character: '{'. Context: '(??*{1,})'.")]
        [TestCase("(??{1}?)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 5. Line: 0; position in line: 5. Near character: '?'. Context: '(??{1}?)'.")]
        [TestCase("(??{1}+)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 5. Line: 0; position in line: 5. Near character: '+'. Context: '(??{1}+)'.")]
        [TestCase("(??{1}*)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 5. Line: 0; position in line: 5. Near character: '*'. Context: '(??{1}*)'.")]
        [TestCase("(??{1}{1})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 5. Line: 0; position in line: 5. Near character: '{'. Context: '(??{1}{1})'.")]
        [TestCase("(??{1}{1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 5. Line: 0; position in line: 5. Near character: '{'. Context: '(??{1}{1,2})'.")]
        [TestCase("(??{1}{1,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 5. Line: 0; position in line: 5. Near character: '{'. Context: '(??{1}{1,})'.")]
        [TestCase("(??{1,2}?)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 7. Line: 0; position in line: 7. Near character: '?'. Context: '(??{1,2}?)'.")]
        [TestCase("(??{1,2}+)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 7. Line: 0; position in line: 7. Near character: '+'. Context: '(??{1,2}+)'.")]
        [TestCase("(??{1,2}*)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 7. Line: 0; position in line: 7. Near character: '*'. Context: '(??{1,2}*)'.")]
        [TestCase("(??{1,2}{1})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 7. Line: 0; position in line: 7. Near character: '{'. Context: '(??{1,2}{1})'.")]
        [TestCase("(??{1,2}{1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 7. Line: 0; position in line: 7. Near character: '{'. Context: '(??{1,2}{1,2})'.")]
        [TestCase("(??{1,2}{1,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 7. Line: 0; position in line: 7. Near character: '{'. Context: '(??{1,2}{1,})'.")]
        [TestCase("(??{1,}?)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 6. Line: 0; position in line: 6. Near character: '?'. Context: '(??{1,}?)'.")]
        [TestCase("(??{1,}+)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 6. Line: 0; position in line: 6. Near character: '+'. Context: '(??{1,}+)'.")]
        [TestCase("(??{1,}*)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 6. Line: 0; position in line: 6. Near character: '*'. Context: '(??{1,}*)'.")]
        [TestCase("(??{1,}{1})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '(??{1,}{1})'.")]
        [TestCase("(??{1,}{1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '(??{1,}{1,2})'.")]
        [TestCase("(??{1,}{1,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '(??{1,}{1,})'.")]
        [TestCase("(??????????????????*)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 9. Line: 0; position in line: 9. Near character: '*'. Context: '(??????????????????*)'.")]
        [TestCase("(??????????????????+)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 9. Line: 0; position in line: 9. Near character: '+'. Context: '(??????????????????+)'.")]
        [TestCase("(??????????????????{1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 9. Line: 0; position in line: 9. Near character: '{'. Context: '(??????????????????{1,2})'.")]
        [TestCase("(??????????????????{2,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 9. Line: 0; position in line: 9. Near character: '{'. Context: '(??????????????????{2,})'.")]
        [TestCase("(??????????????????{3})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 9. Line: 0; position in line: 9. Near character: '{'. Context: '(??????????????????{3})'.")]
        [TestCase("(|????????????|????????)", "Empty branches are not allowed. Absolute position: 1. Line: 0; position in line: 1. Near character: '|'. Context: '(|????????????|??????'.")]
        [TestCase("(???????? ?????? ?????? ???????????? ???????? (|????????????|????????) ?????????? ???????? ????????????)", "Empty branches are not allowed. Absolute position: 27. Line: 0; position in line: 27. Near character: '|'. Context: '?????? ???????? (|????????????|??????'.")]
        [TestCase("(???????? ?????? ?????? ???????????? ???????? (????????????||????????) ?????????? ???????? ????????????)", "Empty branches are not allowed. Absolute position: 34. Line: 0; position in line: 34. Near character: '|'. Context: '?? (????????????||????????) ????????'.")]
        [TestCase("(???????? ?????? ?????? ???????????? ???????? (????????????|????????|) ?????????? ???????? ????????????)", "Empty branches are not allowed. Absolute position: 39. Line: 0; position in line: 39. Near character: ')'. Context: '????????|????????|) ?????????? ??????'.")]
        [TestCase("(???????? ?????? ?????? ???????????? ???????? (||????????????|????????) ?????????? ???????? ????????????)", "Empty branches are not allowed. Absolute position: 27. Line: 0; position in line: 27. Near character: '|'. Context: '?????? ???????? (||????????????|????'.")]
        [TestCase("(???????? ?????? ?????? ???????????? ???????? (????????????|????????||) ?????????? ???????? ????????????)", "Empty branches are not allowed. Absolute position: 39. Line: 0; position in line: 39. Near character: '|'. Context: '????????|????????||) ?????????? ????'.")]
        [TestCase("(???????? ?????? ?????? ???????????? ???????? () ?????????? ???????? ????????????)", "Empty branches are not allowed. Absolute position: 27. Line: 0; position in line: 27. Near character: ')'. Context: '?????? ???????? () ?????????? ??????'.")]
        [TestCase("({1,2})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 1. Line: 0; position in line: 1. Near character: '{'. Context: '({1,2})'.")]
        [TestCase("({2,})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 1. Line: 0; position in line: 1. Near character: '{'. Context: '({2,})'.")]
        [TestCase("({3})", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 1. Line: 0; position in line: 1. Near character: '{'. Context: '({3})'.")]
        [TestCase("(+)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 1. Line: 0; position in line: 1. Near character: '+'. Context: '(+)'.")]
        [TestCase("(?)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 1. Line: 0; position in line: 1. Near character: '?'. Context: '(?)'.")]
        [TestCase("(*)", "Quantifier is only allowed after quantifiable lexeme. Absolute position: 1. Line: 0; position in line: 1. Near character: '*'. Context: '(*)'.")]
        [TestCase("(???????? {1,2})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '(???????? {1,2})'.")]
        [TestCase("(???????? {2,})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '(???????? {2,})'.")]
        [TestCase("(???????? {3})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '(???????? {3})'.")]
        [TestCase("(???????? +)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 6. Line: 0; position in line: 6. Near character: '+'. Context: '(???????? +)'.")]
        [TestCase("(???????? ?)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 6. Line: 0; position in line: 6. Near character: '?'. Context: '(???????? ?)'.")]
        [TestCase("(???????? *)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 6. Line: 0; position in line: 6. Near character: '*'. Context: '(???????? *)'.")]
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
        [TestCase("([???????? one]\t \t{1,2})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 14. Line: 0; position in line: 14. Near character: '{'. Context: '???? one]\t \t{1,2})'.")]
        [TestCase("([???????? one]\t \t{2,})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 14. Line: 0; position in line: 14. Near character: '{'. Context: '???? one]\t \t{2,})'.")]
        [TestCase("([???????? one]\t \t{3})", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 14. Line: 0; position in line: 14. Near character: '{'. Context: '???? one]\t \t{3})'.")]
        [TestCase("([???????? one]\t \t+)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 14. Line: 0; position in line: 14. Near character: '+'. Context: '???? one]\t \t+)'.")]
        [TestCase("([???????? one]\t \t?)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 14. Line: 0; position in line: 14. Near character: '?'. Context: '???? one]\t \t?)'.")]
        [TestCase("([???????? one]\t \t*)", "Quantifier must stand right after the lexeme it's quantifying. Absolute position: 14. Line: 0; position in line: 14. Near character: '*'. Context: '???? one]\t \t*)'.")]
        [TestCase("????????????{0}", "Cannot create a quantifier {0} with n < 1. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '????????????{0}'.")]
        [TestCase("????????????{-1}", "Cannot create a quantifier {-1} with n < 1. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '????????????{-1}'.")]
        [TestCase("????????????{-53}", "Cannot create a quantifier {-53} with n < 1. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '????????????{-53}'.")]
        [TestCase("????????????{-1,}", "Cannot create a quantifier {-1,} with n < 0. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '????????????{-1,}'.")]
        [TestCase("????????????{-53,}", "Cannot create a quantifier {-53,} with n < 0. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '????????????{-53,}'.")]
        [TestCase("????????????{-1,15}", "Cannot create a quantifier {-1,15} with n < 0. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '????????????{-1,15}'.")]
        [TestCase("????????????{15,-1}", "Cannot create a quantifier {15,-1} with n > m. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '????????????{15,-1}'.")]
        [TestCase("????????????{-67,36}", "Cannot create a quantifier {-67,36} with n < 0. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '????????????{-67,36}'.")]
        [TestCase("????????????{36,-67}", "Cannot create a quantifier {36,-67} with n > m. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '????????????{36,-67}'.")]
        [TestCase("????????????{1,0}", "Cannot create a quantifier {1,0} with n > m. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '????????????{1,0}'.")]
        [TestCase("????????????{2,1}", "Cannot create a quantifier {2,1} with n > m. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '????????????{2,1}'.")]
        [TestCase("????????????{38,24}", "Cannot create a quantifier {38,24} with n > m. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '????????????{38,24}'.")]
        [TestCase("????????????{96,53}", "Cannot create a quantifier {96,53} with n > m. Absolute position: 6. Line: 0; position in line: 6. Near character: '{'. Context: '????????????{96,53}'.")]
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
                "(??)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(????)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("????"), new QuantifierToken(1, 1), null),
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
                "(??~)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new PrefixToken("??"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(????~)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new PrefixToken("????"), new QuantifierToken(1, 1), null),
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
                "(~??~)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new InfixToken("??"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~????~)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new InfixToken("????"), new QuantifierToken(1, 1), null),
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
                "(~??)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new SuffixToken("??"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~????)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new SuffixToken("????"), new QuantifierToken(1, 1), null),
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
                "(?? ??)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(???? ????)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("????"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new LiteralToken("????"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(?? ??~ ~??~ ~??)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new PrefixToken("??"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new InfixToken("??"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new SuffixToken("??"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(???? ~????~ ????~ ~????)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("????"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new InfixToken("????"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new PrefixToken("????"), new QuantifierToken(1, 1), null),
                                new QuantifiableBranchItemToken(new SuffixToken("????"), new QuantifierToken(1, 1), null),
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
                "(??|??)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(????|????|????)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("????"), new QuantifierToken(1, 1), null),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("????"), new QuantifierToken(1, 1), null),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("????"), new QuantifierToken(1, 1), null),
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
                "((?????? ???????? ???????????????????? <someVar = sdn.RelativeTimeSpan()>) ???__id_0???)",
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
                                                        new LiteralToken("??????"),
                                                        new QuantifierToken(1, 1),
                                                        null
                                                    ),
                                                    new QuantifiableBranchItemToken(
                                                        new LiteralToken("????????"),
                                                        new QuantifierToken(1, 1),
                                                        null
                                                    ),
                                                    new QuantifiableBranchItemToken(
                                                        new LiteralToken("????????????????????"),
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
                "(???foo??? ???|~????~+|[^????])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new MarkerToken("foo"),
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(0, 1), null),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new InfixToken("????"), new QuantifierToken(1, null), null),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(true, new ILiteralSetMemberToken[]{new LiteralToken("????")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(???foo1??? (?????????2??? ??+|(???```??? ??)*))",
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
                                                    new MarkerToken("??????2"),
                                                    new QuantifiableBranchItemToken(
                                                        new LiteralToken("??"),
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
                                                                        new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
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
                "(???foo??? ??)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new MarkerToken("foo"),
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(???123??? ??)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new MarkerToken("123"),
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(???????????? ??)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new MarkerToken("??????"),
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(???.*'`\"\uD800??? ??)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new MarkerToken(".*'`\"\uD800"),
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(?????.d*??'??? ??)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new MarkerToken("??.d*??'"),
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
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
                "((??|?? ??)|(?? ??|??))",
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
                                                    new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
                                                }
                                            ),
                                            new BranchToken(
                                                new IBranchItemToken[]
                                                {
                                                    new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
                                                    new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
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
                                                    new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
                                                    new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
                                                }
                                            ),
                                            new BranchToken(
                                                new IBranchItemToken[]
                                                {
                                                    new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
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
                "(???foo1??? ??|??+|?? <foo = bar.baz(a.b)>?|[?????????? ??????????~ ~?????? ????]?|(???? ~????~ [?? ~??~ ??] (?????????2??? <baz = foo.bar(default)>{2,5} ~??+ ??{1,20}|~??~*|[^~???????? ??????????~]))*|[^?? ???????? ??????] ??????????????~ (.?|.+){11} .* .{2,23}|??????~)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new MarkerToken("foo1"),
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, null), null),
                            }
                        ),
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 1), null),
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
                                            new LiteralToken("??????????"),
                                            new PrefixToken("??????????"),
                                            new SuffixToken("??????"),
                                            new LiteralToken("????"),
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
                                                    new QuantifiableBranchItemToken(new LiteralToken("????"), new QuantifierToken(1, 1), null),
                                                    new QuantifiableBranchItemToken(new InfixToken("????"), new QuantifierToken(1, 1), null),
                                                    new QuantifiableBranchItemToken(
                                                        new LiteralSetToken(
                                                            false,
                                                            new ILiteralSetMemberToken[]
                                                            {
                                                                new LiteralToken("??"),
                                                                new InfixToken("??"),
                                                                new LiteralToken("??"),
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
                                                                        new MarkerToken("??????2"),
                                                                        new QuantifiableBranchItemToken(new RuleReferenceToken(null, "foo.bar", new IRuleArgumentToken[]{RuleDefaultArgumentToken.Instance}), new QuantifierToken(2, 5), "baz"),
                                                                        new QuantifiableBranchItemToken(new SuffixToken("??"), new QuantifierToken(1, null), null),
                                                                        new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, 20), null),
                                                                    }
                                                                ),
                                                                new BranchToken(
                                                                    new IBranchItemToken[]
                                                                    {
                                                                        new QuantifiableBranchItemToken(new InfixToken("??"), new QuantifierToken(0, null), null),
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
                                                                                    new SuffixToken("????????"),
                                                                                    new PrefixToken("??????????"),
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
                                            new LiteralToken("??"),
                                            new LiteralToken("????????"),
                                            new LiteralToken("??????"),
                                        }
                                    ),
                                    new QuantifierToken(1, 1),
                                    null
                                ),
                                new QuantifiableBranchItemToken(new PrefixToken("??????????????"), new QuantifierToken(1, 1), null),
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
                                new QuantifiableBranchItemToken(new PrefixToken("??????"), new QuantifierToken(1, 1), null),
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
                "([??])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(false, new ILiteralSetMemberToken[]{new LiteralToken("??")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([????])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(false, new ILiteralSetMemberToken[]{new LiteralToken("????")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^??])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(true, new ILiteralSetMemberToken[]{new LiteralToken("??")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^????])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(true, new ILiteralSetMemberToken[]{new LiteralToken("????")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([?? ??])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(false, new ILiteralSetMemberToken[]{new LiteralToken("??"), new LiteralToken("??")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([???? ????])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(false, new ILiteralSetMemberToken[]{new LiteralToken("????"), new LiteralToken("????")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^?? ??])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(true, new ILiteralSetMemberToken[]{new LiteralToken("??"), new LiteralToken("??")}), new QuantifierToken(1, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^???? ????])",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralSetToken(true, new ILiteralSetMemberToken[]{new LiteralToken("????"), new LiteralToken("????")}), new QuantifierToken(1, 1), null),
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
                "(??+)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(1, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(????+)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("????"), new QuantifierToken(1, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^?? ~?? ~??~ ??~]+)",
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
                                            new LiteralToken("??"),
                                            new SuffixToken("??"),
                                            new InfixToken("??"),
                                            new PrefixToken("??"),
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
                "(??~+)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new PrefixToken("??"), new QuantifierToken(1, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~??~+)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new InfixToken("??"), new QuantifierToken(1, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~??+)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new SuffixToken("??"), new QuantifierToken(1, null), null),
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
                "(???)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(0, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(?????)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("????"), new QuantifierToken(0, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^?? ~?? ~??~ ??~]?)",
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
                                            new LiteralToken("??"),
                                            new SuffixToken("??"),
                                            new InfixToken("??"),
                                            new PrefixToken("??"),
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
                "(??~?)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new PrefixToken("??"), new QuantifierToken(0, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~??~?)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new InfixToken("??"), new QuantifierToken(0, 1), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~???)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new SuffixToken("??"), new QuantifierToken(0, 1), null),
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
                "(??*)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(0, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(????*)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("????"), new QuantifierToken(0, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^?? ~?? ~??~ ??~]*)",
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
                                            new LiteralToken("??"),
                                            new SuffixToken("??"),
                                            new InfixToken("??"),
                                            new PrefixToken("??"),
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
                "(??~*)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new PrefixToken("??"), new QuantifierToken(0, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~??~*)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new InfixToken("??"), new QuantifierToken(0, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~??*)",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new SuffixToken("??"), new QuantifierToken(0, null), null),
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
                "(??{2,33})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(2, 33), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(????{2,33})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("????"), new QuantifierToken(2, 33), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(??{22,})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(22, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(??{33})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(33, 33), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(??{44,44})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new LiteralToken("??"), new QuantifierToken(44, 44), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "([^?? ~?? ~??~ ??~]{23,24})",
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
                                            new LiteralToken("??"),
                                            new SuffixToken("??"),
                                            new InfixToken("??"),
                                            new PrefixToken("??"),
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
                "(??~{23,})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new PrefixToken("??"), new QuantifierToken(23, null), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~??~{20})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new InfixToken("??"), new QuantifierToken(20, 20), null),
                            }
                        ),
                    }
                ),
            },
            new object?[]
            {
                "(~??{17,18})",
                new RegexGroupToken(
                    new []
                    {
                        new BranchToken(
                            new IBranchItemToken[]
                            {
                                new QuantifiableBranchItemToken(new SuffixToken("??"), new QuantifierToken(17, 18), null),
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