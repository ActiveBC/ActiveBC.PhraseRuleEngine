using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Exceptions;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Build.Tokenization
{
    // todo [code quality] refactor this method to use RuleSetTokenizationException (instead of base one)
    internal sealed class LoopBasedRuleSetTokenizer : IRuleSetTokenizer
    {
        private readonly IReadOnlyDictionary<string, IPatternTokenizer> m_patternTokenizers;
        private readonly CSharpCodeTokenizer m_cSharpCodeTokenizer;

        public LoopBasedRuleSetTokenizer(IReadOnlyDictionary<string, IPatternTokenizer> patternTokenizers)
        {
            this.m_patternTokenizers = patternTokenizers;
            this.m_cSharpCodeTokenizer = new CSharpCodeTokenizer();
        }

        public RuleSetToken Tokenize(string ruleSet, string? @namespace, bool caseSensitive)
        {
            int i = 0;
            // todo [non-realtime performance] get rid of this (now it's needed to speed-up IronMeta)
            List<char> chars = ruleSet.ToList();
            UsingToken[] usings = ParseUsings(ruleSet, chars, ref i);
            RuleToken[] rules = ParseRules(ruleSet, chars, @namespace, caseSensitive, ref i);

            if (i != ruleSet.Length)
            {
                throw new PhraseRuleEngineTokenizationException("Unable to parse rule set.", ruleSet);
            }

            return new RuleSetToken(usings, rules);
        }

        private RuleToken[] ParseRules(
            string ruleSet,
            List<char> chars,
            string? @namespace,
            bool caseSensitive,
            ref int i
        )
        {
            HashSet<string> usedRuleNames = new HashSet<string>();
            List<RuleToken> rules = new List<RuleToken>();

            this.m_cSharpCodeTokenizer.ConsumeWhitespaces(ruleSet, chars, ref i, "in very beginning");

            while (i < ruleSet.Length)
            {
                ICSharpTypeToken type = this.m_cSharpCodeTokenizer.TokenizeCSharpType(ruleSet, chars, ref i);

                this.m_cSharpCodeTokenizer.ConsumeWhitespaces(ruleSet, chars, ref i, "after rule result type", true);

                string ruleName = this.m_cSharpCodeTokenizer.ParseIdentifier(ruleSet, ref i, "rule name");

                if (!usedRuleNames.Add(ruleName))
                {
                    throw new PhraseRuleEngineTokenizationException($"Duplicate rule '{ruleName}'.", ruleSet);
                }

                this.m_cSharpCodeTokenizer.ConsumeWhitespaces(ruleSet, chars, ref i, "after rule name");

                CSharpParameterToken[] ruleParameters = this.m_cSharpCodeTokenizer.ParseParameters(ruleSet, chars, ref i, "rule parameters");

                this.m_cSharpCodeTokenizer.ConsumeWhitespaces(ruleSet, chars, ref i, "after rule parameters");

                if (i >= ruleSet.Length || ruleSet[i] != '=')
                {
                    throw new PhraseRuleEngineTokenizationException($"Unmatched character '='.{CSharpCodeTokenizer.GetDetails(ruleSet, i)}", ruleSet);
                }

                i++;

                this.m_cSharpCodeTokenizer.ConsumeWhitespaces(ruleSet, chars, ref i, "after '=' sign");

                string patternKey = this.m_cSharpCodeTokenizer.ParseIdentifier(ruleSet, ref i, "pattern key");

                this.m_cSharpCodeTokenizer.ConsumeWhitespaces(ruleSet, chars, ref i, "after pattern key");

                IPatternToken pattern = ReadPattern(patternKey, ref i);

                this.m_cSharpCodeTokenizer.ConsumeWhitespaces(ruleSet, chars, ref i, "after pattern");

                IProjectionToken projection = ReadProjection(ref i);

                this.m_cSharpCodeTokenizer.ConsumeWhitespaces(ruleSet, chars, ref i, "after projection");

                rules.Add(new RuleToken(@namespace, type, ruleName, ruleParameters, patternKey, pattern, projection));
            }

            return rules.ToArray();

            IPatternToken ReadPattern(
                string patternKey,
                ref int index
            )
            {
                if (index >= ruleSet.Length || ruleSet[index] != '#')
                {
                    throw new PhraseRuleEngineTokenizationException($"Unmatched pattern start.{CSharpCodeTokenizer.GetDetails(ruleSet, index)}", ruleSet);
                }

                index++;

                int startIndex = index;
                while (ruleSet[index] != '#')
                {
                    if (index < ruleSet.Length - 1)
                    {
                        index++;
                    }
                    else
                    {
                        throw new PhraseRuleEngineTokenizationException($"Unmatched pattern end.{CSharpCodeTokenizer.GetDetails(ruleSet, index + 1)}", ruleSet);
                    }
                }
                int length = index - startIndex;

                // todo [non-realtime performance] this can be improved by making patter tokenizer accept start index and return end one
                IPatternToken pattern = this.m_patternTokenizers[patternKey].Tokenize(
                    ruleSet.Substring(startIndex, length),
                    @namespace,
                    caseSensitive
                );

                index++;

                return pattern;
            }

            IProjectionToken ReadProjection(ref int index)
            {
                if (index + 1 < ruleSet.Length && ruleSet[index] == '=' && ruleSet[index + 1] == '>')
                {
                    index += 2;
                    this.m_cSharpCodeTokenizer.ConsumeWhitespaces(ruleSet, chars, ref index, "in non-c# projection");

                    return this.m_cSharpCodeTokenizer.ParseConstantProjection(ruleSet, ref index);
                }

                string body = this.m_cSharpCodeTokenizer.ParseMethodBody(ruleSet, chars, ref index);

                if (body == "{}")
                {
                    return VoidProjectionToken.Instance;
                }

                return new BodyBasedProjectionToken(body);
            }
        }

        private UsingToken[] ParseUsings(string ruleSet, List<char> chars, ref int i)
        {
            List<UsingToken> usings = new List<UsingToken>();

            while (i < ruleSet.Length)
            {
                char c = ruleSet[i++];

                switch (c)
                {
                    case ' ':
                    case '\r':
                    case '\n':
                    case '\t':
                    {
                        continue;
                    }
                    case 'u':
                    {
                        i--;
                        usings.Add(this.m_cSharpCodeTokenizer.ParseUsing(ruleSet, chars, ref i));
                        break;
                    }
                    default:
                    {
                        i--;

                        return usings.ToArray();
                    }
                }
            }

            throw new PhraseRuleEngineTokenizationException("Unable to read usings.", ruleSet);
        }
    }
}