namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens
{
    public sealed class SuffixToken : ITerminalToken, ILiteralSetMemberToken
    {
        public string Suffix { get; }

        public SuffixToken(string suffix)
        {
            this.Suffix = suffix;
        }

        public override string ToString()
        {
            return $"~{this.Suffix}";
        }
    }
}