namespace ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments
{
    public sealed class RuleDefaultArgumentToken : IRuleArgumentToken
    {
        public static readonly RuleDefaultArgumentToken Instance = new RuleDefaultArgumentToken();

        private RuleDefaultArgumentToken()
        {
        }

        public override string ToString()
        {
            return "default";
        }
    }
}