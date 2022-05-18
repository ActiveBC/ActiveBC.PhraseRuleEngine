namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens
{
    public sealed class CSharpParameterToken : IToken
    {
        public ICSharpTypeToken Type { get; }
        public string Name { get; }

        public CSharpParameterToken(ICSharpTypeToken type, string name)
        {
            this.Type = type;
            this.Name = name;
        }

        public override string ToString()
        {
            return $"{this.Type.ToString()} {this.Name}";
        }
    }
}