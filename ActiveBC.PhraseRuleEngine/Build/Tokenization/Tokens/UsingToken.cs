using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens
{
    public sealed class UsingToken : IToken
    {
        public string Namespace { get; }

        public UsingToken(string @namespace)
        {
            this.Namespace = @namespace;
        }

        public override string ToString()
        {
            return $"using {Namespace};";
        }
    }
}