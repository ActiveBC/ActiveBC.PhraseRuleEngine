namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens
{
    public sealed class InfixToken : ITerminalToken, ILiteralSetMemberToken
    {
        public string Infix { get; }

        public InfixToken(string infix)
        {
            this.Infix = infix;
        }

        public override string ToString()
        {
            return $"~{this.Infix}~";
        }
    }
}