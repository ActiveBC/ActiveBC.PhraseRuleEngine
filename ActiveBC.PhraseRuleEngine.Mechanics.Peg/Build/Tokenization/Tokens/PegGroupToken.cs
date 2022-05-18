using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens
{
    public sealed class PegGroupToken : IQuantifiableToken, IPatternToken
    {
        public BranchToken[] Branches { get; }

        public PegGroupToken(BranchToken[] branches)
        {
            this.Branches = branches;
        }

        public override string ToString()
        {
            return $"({this.Branches.Select(branch => branch.ToString()).JoinToString(" | ")})";
        }
    }
}