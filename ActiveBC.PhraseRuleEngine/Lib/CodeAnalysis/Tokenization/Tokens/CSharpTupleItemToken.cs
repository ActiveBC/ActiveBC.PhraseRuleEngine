namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens
{
    public sealed class CSharpTupleItemToken : IToken
    {
        public ICSharpTypeToken Type { get; }
        public CSharpIdentifierToken? PropertyName { get; }

        public CSharpTupleItemToken(ICSharpTypeToken type, CSharpIdentifierToken? propertyName)
        {
            this.Type = type;
            this.PropertyName = propertyName;
        }

        public override string ToString()
        {
            return $"{this.Type.ToString()}{(this.PropertyName is not null ? $" {this.PropertyName}" : "")}";
        }
    }
}