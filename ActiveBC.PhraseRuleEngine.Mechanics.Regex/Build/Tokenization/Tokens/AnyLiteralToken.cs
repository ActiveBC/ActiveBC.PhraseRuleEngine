namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens
{
    public sealed class AnyLiteralToken : ITerminalToken
    {
        public static readonly AnyLiteralToken Instance = new AnyLiteralToken();

        private AnyLiteralToken()
        {
        }

        public override string ToString()
        {
            return ".";
        }
    }
}