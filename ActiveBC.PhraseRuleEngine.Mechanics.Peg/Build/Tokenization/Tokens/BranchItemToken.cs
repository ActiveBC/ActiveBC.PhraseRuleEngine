using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens
{
    public sealed class BranchItemToken : IToken
    {
        public IQuantifiableToken Quantifiable { get; }
        public QuantifierToken Quantifier { get; }
        public string? VariableName { get; }
        public LookaheadToken? Lookahead { get; }

        public BranchItemToken(
            IQuantifiableToken quantifiable,
            QuantifierToken quantifier,
            string? variableName,
            LookaheadToken? lookahead
        )
        {
            this.Quantifiable = quantifiable;
            this.Quantifier = quantifier;
            this.VariableName = variableName;
            this.Lookahead = lookahead;
        }

        public override string ToString()
        {
            return $"{this.Lookahead}" +
                   $"{this.Quantifiable}" +
                   $"{this.Quantifier}" +
                   $"{(this.VariableName is not null ? $":{this.VariableName}" : "")}";
        }
    }
}