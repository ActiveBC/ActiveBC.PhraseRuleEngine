using System.Linq;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens
{
    public sealed class BranchToken : IToken
    {
        public BranchItemToken[] Items { get; }

        public BranchToken(BranchItemToken[] items)
        {
            this.Items = items;
        }

        public override string ToString()
        {
            return this.Items.Select(item => item.ToString()).JoinToString(" ");
        }
    }
}