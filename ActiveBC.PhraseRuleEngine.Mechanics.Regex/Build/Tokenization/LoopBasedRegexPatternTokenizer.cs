using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ActiveBC.PhraseRuleEngine.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization;
using ActiveBC.PhraseRuleEngine.Lib.Common;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization
{
    public sealed class LoopBasedRegexPatternTokenizer : IPatternTokenizer
    {
        private readonly StringInterner m_stringInterner;

        public LoopBasedRegexPatternTokenizer(StringInterner stringInterner)
        {
            this.m_stringInterner = stringInterner;
        }

        public IPatternToken Tokenize(string pattern, string? @namespace, bool caseSensitive)
        {
            HashSet<string> usedMarkers = new HashSet<string>();

            Stack<List<List<IBranchItemToken>>> groups = new Stack<List<List<IBranchItemToken>>>();

            List<List<IBranchItemToken>> currentBranchItems;

            CreateNewCurrentGroup();

            bool lastReadTokenIsQuantifier = false;
            int i = 0;
            while (i < pattern.Length)
            {
                char c = caseSensitive ? pattern[i++] : pattern[i++].ToLowerFastRusEng();

                switch (c)
                {
                    case '(':
                    {
                        groups.Push(currentBranchItems);

                        CreateNewCurrentGroup();

                        lastReadTokenIsQuantifier = false;
                        break;
                    }
                    case '|':
                    {
                        ThrowIfBranchIsEmpty(LastBranchOfCurrentGroup(), pattern, i - 1);

                        AddNewBranchToCurrentGroup();

                        lastReadTokenIsQuantifier = false;

                        break;
                    }
                    case ')':
                    {
                        ThrowIfBranchIsEmpty(LastBranchOfCurrentGroup(), pattern, i - 1);

                        RegexGroupToken closedGroup = TryGatherLexemesFromBranchesToGroup(currentBranchItems);

                        if (groups.Count < 1)
                        {
                            throw new RegexPatternTokenizationException(
                                $"Too many closing brackets.{CSharpCodeTokenizer.GetDetails(pattern, i - 1)}",
                                pattern
                            );
                        }

                        currentBranchItems = groups.Pop();

                        AddItemToCurrentBranch(closedGroup);

                        lastReadTokenIsQuantifier = false;

                        break;
                    }
                    case '*':
                    {
                        QuantifyLastBranchItem(new QuantifierToken(0, null));

                        lastReadTokenIsQuantifier = true;

                        break;
                    }
                    case '+':
                    {
                        QuantifyLastBranchItem(new QuantifierToken(1, null));

                        lastReadTokenIsQuantifier = true;

                        break;
                    }
                    case '?':
                    {
                        QuantifyLastBranchItem(new QuantifierToken(0, 1));

                        lastReadTokenIsQuantifier = true;

                        break;
                    }
                    case '{':
                    {
                        i--;
                        int firstQuantifierCharIndex = i;
                        QuantifyLastBranchItem(QuantifierReader.ReadQuantifier(pattern, ref i), firstQuantifierCharIndex);

                        lastReadTokenIsQuantifier = true;

                        break;
                    }
                    case MarkerToken.MarkerStart:
                    {
                        i--;
                        LastBranchOfCurrentGroup().Add(ParseMarker(pattern, ref i, usedMarkers));

                        lastReadTokenIsQuantifier = false;

                        break;
                    }
                    case '.':
                    {
                        AddItemToCurrentBranch(AnyLiteralToken.Instance);

                        ThrowIfCharIsLetter(pattern, i);

                        lastReadTokenIsQuantifier = false;

                        break;
                    }
                    case '[':
                    {
                        IQuantifiableToken literalSet = ReadLiteralsSet(pattern, ref i, caseSensitive);
                        AddItemToCurrentBranch(literalSet);

                        lastReadTokenIsQuantifier = false;

                        break;
                    }
                    case '<':
                    {
                        i--;
                        (RuleReferenceToken Reference, string? VariableName) ner = NerReader.ReadNerDeclaration(pattern, ref i, @namespace);

                        AddItemToCurrentBranch(
                            ner.Reference,
                            ner.VariableName
                        );

                        lastReadTokenIsQuantifier = false;

                        break;
                    }
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                    {
                        break;
                    }
                    case '~':
                    {
                        string word = ParseWordLiteral(pattern, ref i, caseSensitive);
                        if (i < pattern.Length && pattern[i] == '~')
                        {
                            ++i;
                            AddItemToCurrentBranch(new InfixToken(word));
                            ThrowIfCharIsLetter(pattern, i);
                        }
                        else
                        {
                            AddItemToCurrentBranch(new SuffixToken(word));
                        }

                        lastReadTokenIsQuantifier = false;

                        break;
                    }
                    default:
                    {
                        if (IsCharRussian(c) || IsCharEngOrDigit(c))
                        {
                            --i;
                            string word = ParseWordLiteral(pattern, ref i, caseSensitive);
                            if (i < pattern.Length && pattern[i] == '~')
                            {
                                ++i;
                                AddItemToCurrentBranch(new PrefixToken(word));
                                ThrowIfCharIsLetter(pattern, i);
                            }
                            else
                            {
                                AddItemToCurrentBranch(new LiteralToken(word));
                            }
                        }
                        else
                        {
                            throw new RegexPatternTokenizationException(
                                $"Unexpected char.{CSharpCodeTokenizer.GetDetails(pattern, i - 1)}",
                                pattern
                            );
                        }

                        lastReadTokenIsQuantifier = false;

                        break;
                    }
                }
            }

            RegexGroupToken currentGroup = TryGatherLexemesFromBranchesToGroup(currentBranchItems, true);

            if (groups.Count > 0)
            {
                throw new RegexPatternTokenizationException(
                    $"Not closed open parentheses.{CSharpCodeTokenizer.GetDetails(pattern, i - 1)}",
                    pattern
                );
            }

            return currentGroup;

            void CreateNewCurrentGroup()
            {
                currentBranchItems = new List<List<IBranchItemToken>>();
                AddNewBranchToCurrentGroup();
            }

            void AddNewBranchToCurrentGroup()
            {
                currentBranchItems.Add(new List<IBranchItemToken>());
            }

            void AddItemToCurrentBranch(IQuantifiableToken quantifiable, string? variableName = null)
            {
                LastBranchOfCurrentGroup().Add(new QuantifiableBranchItemToken(quantifiable, new QuantifierToken(1, 1), variableName));
            }

            void QuantifyLastBranchItem(QuantifierToken quantifier, int? firstQuantifierChar = null)
            {
                firstQuantifierChar ??= i - 1;

                ThrowIfNotInQuantifierPosition(LastBranchOfCurrentGroup(), pattern, firstQuantifierChar.Value, lastReadTokenIsQuantifier);

                List<IBranchItemToken> lastBranchOfCurrentGroup = LastBranchOfCurrentGroup();
                int lastBranchItemIndex = lastBranchOfCurrentGroup.Count - 1;
                IBranchItemToken lastBranchItem = lastBranchOfCurrentGroup[lastBranchItemIndex];

                if (lastBranchItem is QuantifiableBranchItemToken quantifiableBranchItem)
                {
                    lastBranchOfCurrentGroup[lastBranchItemIndex] = new QuantifiableBranchItemToken(
                        quantifiableBranchItem.Quantifiable,
                        quantifier,
                        null
                    );
                }
                else
                {
                    throw new RegexPatternTokenizationException(
                        $"Cannot apply quantifier '{quantifier}': " +
                        $"last branch item '{lastBranchItem.ToString()}' is not quantifiable." +
                        $"{CSharpCodeTokenizer.GetDetails(pattern, firstQuantifierChar.Value)}",
                        pattern
                    );
                }

                ThrowIfCharIsLetter(pattern, i);
            }

            List<IBranchItemToken> LastBranchOfCurrentGroup()
            {
                return currentBranchItems[currentBranchItems.Count - 1];
            }
        }

        private static void ThrowIfBranchIsEmpty(List<IBranchItemToken> currentBranchItems, string pattern, int i)
        {
            if (currentBranchItems.Count == 0)
            {
                throw new RegexPatternTokenizationException(
                    $"Empty branches are not allowed.{CSharpCodeTokenizer.GetDetails(pattern, i)}",
                    pattern
                );
            }
        }

        private RegexGroupToken TryGatherLexemesFromBranchesToGroup(List<List<IBranchItemToken>> branches, bool simplify = false)
        {
            if (simplify && branches.Count == 1 && branches[0].Count == 1)
            {
                IBranchItemToken branchItem = branches[0][0];

                if (branchItem is QuantifiableBranchItemToken { Quantifiable: RegexGroupToken group } quantifiableBranchItem &&
                    quantifiableBranchItem.Quantifier.Min == 1 && quantifiableBranchItem.Quantifier.Max == 1
                )
                {
                    return group;
                }
            }

            return new RegexGroupToken(branches.Select(branch => new BranchToken(branch.ToArray())).ToArray());
        }

        private static void ThrowIfNotInQuantifierPosition(
            List<IBranchItemToken> currentBranchItems,
            string pattern,
            int quantifierFirstCharIndex,
            bool lastReadTokenIsQuantifier
        )
        {
            IBranchItemToken? branchItem = currentBranchItems.LastOrDefault();

            if (branchItem is not QuantifiableBranchItemToken || lastReadTokenIsQuantifier)
            {
                throw new RegexPatternTokenizationException(
                    $"Quantifier is only allowed after quantifiable lexeme." +
                    $"{CSharpCodeTokenizer.GetDetails(pattern, quantifierFirstCharIndex)}",
                    pattern
                );
            }

            char previousChar = pattern[quantifierFirstCharIndex - 1];

            if (previousChar == ' ' || previousChar == '\t' || previousChar == '\n' || previousChar == '\r')
            {
                throw new RegexPatternTokenizationException(
                    $"Quantifier must stand right after the lexeme it's quantifying." +
                    $"{CSharpCodeTokenizer.GetDetails(pattern, quantifierFirstCharIndex)}",
                    pattern
                );
            }
        }

        private static void ThrowIfCharIsLetter(string pattern, int i)
        {
            if (i >= pattern.Length)
            {
                return;
            }

            char c = pattern[i];

            if (IsCharRussian(c) || IsCharEngOrDigit(c))
            {
                throw new RegexPatternTokenizationException(
                    $"Found word start in invalid position.{CSharpCodeTokenizer.GetDetails(pattern, i)}",
                    pattern
                );
            }
        }

        private static bool IsCharRussian(char c)
        {
            return c
                is >= 'а' and <= 'я'
                or 'ё'
                or '-'
                or >= 'А' and <= 'Я'
                or 'Ё';
        }

        private static bool IsCharEngOrDigit(char c)
        {
            return c
                is >= 'a' and <= 'z'
                or '-'
                or '\''
                or >= '0' and <= '9'
                or >= 'A' and <= 'Z';
        }

        private MarkerToken ParseMarker(string pattern, ref int i, HashSet<string> usedMarkers)
        {
            int initialPosition = i;

            ++i;
            while (i < pattern.Length && pattern[i] != MarkerToken.MarkerEnd)
            {
                ++i;
            }

            if (i == pattern.Length - 1 && pattern[i] != MarkerToken.MarkerEnd)
            {
                throw new RegexPatternTokenizationException(
                    $"Not found end of marker.{CSharpCodeTokenizer.GetDetails(pattern, initialPosition)}",
                    pattern
                );
            }

            int markerLength = i - initialPosition - 1;
            if (markerLength == 0)
            {
                throw new RegexPatternTokenizationException(
                    $"Empty marker.{CSharpCodeTokenizer.GetDetails(pattern, initialPosition)}",
                    pattern
                );
            }

            ++i;

            string marker = pattern.Substring(initialPosition + 1, markerLength);
            if (!usedMarkers.Add(marker))
            {
                throw new RegexPatternTokenizationException(
                    $"Duplicated marker '{marker}'.{CSharpCodeTokenizer.GetDetails(pattern, initialPosition)}",
                    pattern
                );
            }

            return new MarkerToken(marker);
        }

        private LiteralSetToken ReadLiteralsSet(string pattern, ref int i, bool caseSensitive)
        {
            int startIndex = i - 1;
            bool negate;
            List<ILiteralSetMemberToken> members = new List<ILiteralSetMemberToken>();
            if (i < pattern.Length && pattern[i] == '^')
            {
                negate = true;
                ++i;
            }
            else
            {
                negate = false;
            }

            while (i < pattern.Length)
            {
                char c = caseSensitive ? pattern[i++] : pattern[i++].ToLowerFastRusEng();

                switch (c)
                {
                    case ']':
                    {
                        return new LiteralSetToken(negate, members.ToArray());
                    }
                    case '[':
                    case '^':
                    case '\\':
                    {
                        throw new RegexPatternTokenizationException(
                            $"Unhandled char in literal set.{CSharpCodeTokenizer.GetDetails(pattern, i - 1)}",
                            pattern
                        );
                    }
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                    {
                        break;
                    }
                    case '~':
                    {
                        string word = ParseWordLiteral(pattern, ref i, caseSensitive);
                        if (i < pattern.Length && pattern[i] == '~')
                        {
                            ++i;
                            members.Add(new InfixToken(word));

                            ThrowIfCharIsLetter(pattern, i);
                        }
                        else
                        {
                            members.Add(new SuffixToken(word));
                        }
                        break;
                    }
                    default:
                    {
                        if (IsCharRussian(c) || IsCharEngOrDigit(c))
                        {
                            --i;
                            string word = ParseWordLiteral(pattern, ref i, caseSensitive);
                            if (i < pattern.Length && pattern[i] == '~')
                            {
                                ++i;
                                members.Add(new PrefixToken(word));
                                ThrowIfCharIsLetter(pattern, i);
                            }
                            else
                            {
                                members.Add(new LiteralToken(word));
                            }
                        }
                        else
                        {
                            throw new RegexPatternTokenizationException(
                                $"Unhandled char in literal set.{CSharpCodeTokenizer.GetDetails(pattern, i - 1)}",
                                pattern
                            );
                        }

                        break;
                    }
                }
            }

            throw new RegexPatternTokenizationException(
                $"Unbound character set.{CSharpCodeTokenizer.GetDetails(pattern, startIndex)}",
                pattern
            );
        }

        private string ParseWordLiteral(string pattern, ref int i, bool caseSensitive)
        {
            if (i >= pattern.Length)
            {
                throw new RegexPatternTokenizationException(
                    $"Empty word.{CSharpCodeTokenizer.GetDetails(pattern, i)}",
                    pattern
                );
            }

            int startIndex = i;
            //Check for first symbol to declare language
            bool isRussianLanguageWordDeclare = IsCharRussian(pattern[i]);

            if (isRussianLanguageWordDeclare)
            {
                //Check word till symbol not in Russian
                while (i < pattern.Length && IsCharRussian(pattern[i]))
                {
                    ++i;
                }

                //Check next symbol to opposite language (it would English or Digit symbol)
                if (i < pattern.Length && IsCharEngOrDigit(pattern[i]))
                {
                    throw new RegexPatternTokenizationException(
                        $"Unhandled char. Check word for different language usage." +
                        $"{CSharpCodeTokenizer.GetDetails(pattern, i)}",
                        pattern
                    );
                }
            }
            else
            {
                //Check word till symbol not in English or Digit
                while (i < pattern.Length && IsCharEngOrDigit(pattern[i]))
                {
                    ++i;
                }

                //Check next symbol to opposite language (it would Russian)
                if (i < pattern.Length && IsCharRussian(pattern[i]))
                {
                    throw new RegexPatternTokenizationException(
                        $"Unhandled char. Check word for different language usage." +
                        $"{CSharpCodeTokenizer.GetDetails(pattern, i)}",
                        pattern
                    );
                }
            }

            string word = pattern.Substring(startIndex, i - startIndex);

            if (!caseSensitive)
            {
                word = word.ToLowerFastRusEng();
            }

            if (word.Length == 0)
            {
                throw new RegexPatternTokenizationException(
                    $"Empty word.{CSharpCodeTokenizer.GetDetails(pattern, startIndex)}",
                    pattern
                );
            }

            word = this.m_stringInterner.InternString(word);

            return word;
        }

        private static class QuantifierReader
        {
            /// <summary>
            /// This method reads quantifiers {n,m}, {n}, {n,}
            /// </summary>
            /// <param name="pattern">String with regular expression to process</param>
            /// <param name="i">Index of the quantifier opening character "{"</param>
            /// <returns>Quantifier lexeme</returns>
            public static QuantifierToken ReadQuantifier(string pattern, ref int i)
            {
                int initialPosition = i;
                StringBuilder nStrBuilder = new StringBuilder();
                StringBuilder mStrBuilder = new StringBuilder();
                bool reachedComma = false;
                bool reachedClosing = false;
                bool noCharactersAfterComma = false;

                // We are not interested in '{' opening character
                i++;

                while (i < pattern.Length && !reachedClosing)
                {
                    char c = pattern[i++];
                    switch (c)
                    {
                    case ',':
                        reachedComma = true;
                        noCharactersAfterComma = true;
                        break;
                    case '}':
                        reachedClosing = true;
                        break;
                    default:
                        if (reachedComma)
                        {
                            noCharactersAfterComma = false;
                            mStrBuilder.Append(c);
                        }
                        else
                        {
                            nStrBuilder.Append(c);
                        }
                        break;
                    }
                }

                if (!reachedClosing)
                {
                    throw new RegexPatternTokenizationException(
                        $"Unmatched opening bracket '{{'.{CSharpCodeTokenizer.GetDetails(pattern, initialPosition)}",
                        pattern
                    );
                }

                string nStr = nStrBuilder.ToString();
                if (!Int32.TryParse(nStr, out int n))
                {
                    throw new RegexPatternTokenizationException(
                        $"Expected an integer parameter.{CSharpCodeTokenizer.GetDetails(pattern, initialPosition + 1)}",
                        pattern
                    );
                }

#pragma warning disable S2583
                if (noCharactersAfterComma)
#pragma warning restore S2583
                {
                    return ConstructNonLessThanQuantifier(n, pattern, initialPosition);
                }

#pragma warning disable S2583
                if (reachedComma)
#pragma warning restore S2583
                {
                    string mStr = mStrBuilder.ToString();
                    if (!Int32.TryParse(mStr, out int m))
                    {
                        throw new RegexPatternTokenizationException(
                            $"Expected an integer parameter." +
                            $"{CSharpCodeTokenizer.GetDetails(pattern, initialPosition + 1)}",
                            pattern
                        );
                    }

                    return ConstructRangeQuantifier(n, m, pattern, initialPosition);
                }

                return ConstructExactQuantifier(n, pattern, initialPosition);
            }

            private static QuantifierToken ConstructRangeQuantifier(int n, int m, string pattern, int position)
            {
                if (n < 0)
                {
                    throw new RegexPatternTokenizationException(
                        $"Cannot create a quantifier {{{n},{m}}} with n < 0." +
                        $"{CSharpCodeTokenizer.GetDetails(pattern, position)}",
                        pattern
                    );
                }

                if (m < n)
                {
                    throw new RegexPatternTokenizationException(
                        $"Cannot create a quantifier {{{n},{m}}} with n > m." +
                        $"{CSharpCodeTokenizer.GetDetails(pattern, position)}",
                        pattern
                    );
                }

                return new QuantifierToken(n, m);
            }

            private static QuantifierToken ConstructExactQuantifier(int n, string pattern, int position)
            {
                if (n < 1)
                {
                    throw new RegexPatternTokenizationException(
                        $"Cannot create a quantifier {{{n}}} with n < 1." +
                        $"{CSharpCodeTokenizer.GetDetails(pattern, position)}",
                        pattern
                    );
                }

                return new QuantifierToken(n, n);
            }

            private static QuantifierToken ConstructNonLessThanQuantifier(int n, string pattern, int position)
            {
                if (n < 0)
                {
                    throw new RegexPatternTokenizationException(
                        $"Cannot create a quantifier {{{n},}} with n < 0." +
                        $"{CSharpCodeTokenizer.GetDetails(pattern, position)}",
                        pattern
                    );
                }

                return new QuantifierToken(n, null);
            }
        }

        private static class NerReader
        {
            private static readonly System.Text.RegularExpressions.Regex s_generalNerRegex = new System.Text.RegularExpressions.Regex(@"<\s*(?:(?<var>[a-zA-Z_][a-zA-Z_0-9]*)\s*=\s*)?(?<call>[a-zA-Z_][a-zA-Z_0-9]*(?:\.[a-zA-Z_][a-zA-Z_0-9]*)*)(?:\s*\(\s*(?:(?<arg>(?:default|[a-zA-Z_][a-zA-Z_0-9]*(?:\.[a-zA-Z_][a-zA-Z_0-9]*)*))(?:\s*,\s*(?<arg>(?:default|[a-zA-Z_][a-zA-Z_0-9]*(?:\.[a-zA-Z_][a-zA-Z_0-9]*)*)))*)?\s*\))?\s*>", RegexOptions.Compiled);

            public static (RuleReferenceToken Reference, string? VariableName) ReadNerDeclaration(string pattern, ref int position, string? @namespace)
            {
                Match match = s_generalNerRegex.Match(pattern, position);
                if (!match.Success)
                {
                    throw new RegexPatternTokenizationException(
                        $"Failed to parse NER inclusion at all.{CSharpCodeTokenizer.GetDetails(pattern, position)}",
                        pattern
                    );
                }
                if (match.Index != position)
                {
                    throw new RegexPatternTokenizationException(
                        $"Failed to parse NER inclusion.{CSharpCodeTokenizer.GetDetails(pattern, position)}",
                        pattern
                    );
                }

                position = match.Index + match.Length;

                string? variableName = match.Groups["var"].Success ? match.Groups["var"].Value : null;
                string callChain = match.Groups["call"].Value;
                IRuleArgumentToken[] arguments = match
                    .Groups["arg"]
                    .Captures
#if NETFRAMEWORK
                    .Cast<Capture>()
#endif
                    .Select<Capture, IRuleArgumentToken>(
                        capture =>
                        {
                            if (capture.Value == "default")
                            {
                                return RuleDefaultArgumentToken.Instance;
                            }

                            return new RuleChainedMemberAccessArgumentToken(
                                capture.Value.Split('.', StringSplitOptions.RemoveEmptyEntries)
                            );
                        }
                    )
                    .ToArray();

                return (new RuleReferenceToken(@namespace, callChain, arguments), variableName);
            }
        }
    }
}