namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens
{
    public sealed class CSharpTypeNameWithNamespaceToken : IToken
    {
        public string Value { get; }

        public CSharpTypeNameWithNamespaceToken(string value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}