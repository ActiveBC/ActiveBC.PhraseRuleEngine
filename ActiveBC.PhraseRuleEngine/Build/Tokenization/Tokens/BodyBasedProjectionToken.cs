namespace ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens
{
    public sealed class BodyBasedProjectionToken : IProjectionToken
    {
        public string Body { get; }

        public BodyBasedProjectionToken(string body)
        {
            this.Body = body;
        }

        public override string ToString()
        {
            return this.Body;
        }
    }
}