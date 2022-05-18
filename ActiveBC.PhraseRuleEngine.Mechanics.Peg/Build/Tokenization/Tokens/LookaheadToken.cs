using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens
{
    // todo [realtime performance] introduce two instances and make constructor private
    public sealed class LookaheadToken : IToken
    {
        public bool IsNegative { get; }

        public LookaheadToken(bool isNegative)
        {
            this.IsNegative = isNegative;
        }

        public override string ToString()
        {
            return this.IsNegative ? "!" : "&";
        }
    }
}