using System;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens
{
    public sealed class RuleSetToken : IToken
    {
        public UsingToken[] Usings { get; }
        public RuleToken[] Rules { get; }

        public RuleSetToken(UsingToken[] usings, RuleToken[] rules)
        {
            this.Usings = usings;
            this.Rules = rules;
        }

        public override string ToString()
        {
            return $"{(this.Usings.Length > 0 ? $"{this.Usings.Select(@using => @using.ToString()).JoinToString(Environment.NewLine)}{Environment.NewLine}{Environment.NewLine}" : "")}" +
                   $"{this.Rules.Select(rule => rule.ToString()).JoinToString(Environment.NewLine)}";
        }
    }
}