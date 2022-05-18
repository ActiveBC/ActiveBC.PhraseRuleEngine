using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens
{
    public sealed class RegexGroupToken : IQuantifiableToken, IPatternToken
    {
        public BranchToken[] Branches { get; }

        public RegexGroupToken(BranchToken[] branches)
        {
            this.Branches = branches;
        }

        public override string ToString()
        {
            return $"({this.Branches.Select(branch => branch.ToString()).JoinToString(" | ")})";
        }
    }
}