namespace ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens
{
    public sealed class ConstantProjectionToken : IProjectionToken
    {
        public object? Constant { get; }

        public ConstantProjectionToken(object? constant)
        {
            this.Constant = constant;
        }

        public override string ToString()
        {
            return $"=> {Format()}";

            string Format()
            {
                return this.Constant switch
                {
                    string => @$"""{this.Constant}""",
                    _ => $"{this.Constant}",
                };
            }
        }
    }
}