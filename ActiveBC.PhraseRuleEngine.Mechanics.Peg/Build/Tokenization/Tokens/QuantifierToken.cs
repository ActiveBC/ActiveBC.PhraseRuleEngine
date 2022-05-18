using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens
{
    public sealed class QuantifierToken : IToken
    {
        public int Min { get; }
        public int? Max { get; }

        public QuantifierToken(int min, int? max)
        {
            this.Min = min;
            this.Max = max;
        }

        public override string ToString()
        {
            return this.Min switch
            {
                1 when this.Max == 1 => string.Empty,
                1 when this.Max is null => "+",
                0 when this.Max == 1 => "?",
                0 when this.Max is null => "*",
                _ => $"{{{this.Min.ToString()},{this.Max?.ToString()}}}"
            };
        }
    }
}