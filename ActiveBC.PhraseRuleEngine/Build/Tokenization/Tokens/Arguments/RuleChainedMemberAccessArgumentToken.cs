using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments
{
    public sealed class RuleChainedMemberAccessArgumentToken : IRuleArgumentToken
    {
        public string[] CallChain { get; }

        public RuleChainedMemberAccessArgumentToken(string[] callChain)
        {
            this.CallChain = callChain;
        }

        public override string ToString()
        {
            return this.CallChain.JoinToString(".");
        }
    }
}