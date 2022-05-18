namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens
{
    public sealed class PrefixToken : ITerminalToken, ILiteralSetMemberToken
    {
        public string Prefix { get; }

        public PrefixToken(string prefix)
        {
            this.Prefix = prefix;
        }

        public override string ToString()
        {
            return $"{this.Prefix}~";
        }
    }
}