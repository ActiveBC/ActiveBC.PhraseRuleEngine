namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens
{
    public sealed class MarkerToken : IBranchItemToken
    {
        public const char MarkerStart = '「';
        public const char MarkerEnd = '」';

        public string Marker { get; }

        public MarkerToken(string marker)
        {
            this.Marker = marker;
        }

        public override string ToString()
        {
            return $"{MarkerStart}{Marker}{MarkerEnd}";
        }
    }
}