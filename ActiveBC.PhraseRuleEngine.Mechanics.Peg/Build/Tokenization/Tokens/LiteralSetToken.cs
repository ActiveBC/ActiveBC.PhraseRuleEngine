using System.Linq;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens
{
    public sealed class LiteralSetToken : ITerminalToken
    {
        public bool IsNegative { get; }
        public ILiteralSetMemberToken[] Members { get; }

        public LiteralSetToken(bool isNegative, ILiteralSetMemberToken[] members)
        {
            this.IsNegative = isNegative;
            this.Members = members;
        }

        public override string ToString()
        {
            return $"[{(this.IsNegative ? "^" : "")}{this.Members.Select(member => member.ToString()).JoinToString(" ")}]";
        }
    }
}