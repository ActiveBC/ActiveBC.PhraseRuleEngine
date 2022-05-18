namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens
{
    public sealed class CSharpIdentifierToken : IToken
    {
        public string Value { get; }

        public CSharpIdentifierToken(string value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}