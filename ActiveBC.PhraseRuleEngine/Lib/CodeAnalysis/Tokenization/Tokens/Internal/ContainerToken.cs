namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens.Internal
{
    public sealed class ContainerToken<TValue> : IToken
    {
        public TValue Value { get; }

        public ContainerToken(TValue value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return this.Value?.ToString() ?? "";
        }
    }
}