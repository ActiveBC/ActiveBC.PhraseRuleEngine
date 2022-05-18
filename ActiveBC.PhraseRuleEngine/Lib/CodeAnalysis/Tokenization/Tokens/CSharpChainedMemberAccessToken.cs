using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens
{
    public sealed class CSharpChainedMemberAccessToken : IToken
    {
        public string[] Value { get; }

        public CSharpChainedMemberAccessToken(string[] value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return this.Value.JoinToString(".");
        }
    }
}