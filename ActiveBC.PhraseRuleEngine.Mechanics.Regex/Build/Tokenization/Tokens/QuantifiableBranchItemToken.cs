namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens
{
    public sealed class QuantifiableBranchItemToken : IBranchItemToken
    {
        public IQuantifiableToken Quantifiable { get; }
        public QuantifierToken Quantifier { get; }
        public string? VariableName { get; }

        public QuantifiableBranchItemToken(
            IQuantifiableToken quantifiable,
            QuantifierToken quantifier,
            string? variableName
        )
        {
            this.Quantifiable = quantifiable;
            this.Quantifier = quantifier;
            this.VariableName = variableName;
        }

        public override string ToString()
        {
            return $"{this.Quantifiable}" +
                   $"{this.Quantifier}" +
                   $"{(this.VariableName is not null ? $":{this.VariableName}" : "")}";
        }
    }
}