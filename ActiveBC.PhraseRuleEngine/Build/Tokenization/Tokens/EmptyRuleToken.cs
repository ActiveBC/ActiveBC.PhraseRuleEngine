using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens
{
    public sealed class EmptyRuleToken : IRuleToken
    {
        public string? Namespace { get; }
        public ICSharpTypeToken ReturnType { get; }
        public string Name { get; }
        public CSharpParameterToken[] RuleParameters { get; }
        public IProjectionToken Projection { get; }

        public EmptyRuleToken(
            string? @namespace,
            ICSharpTypeToken returnType,
            string name,
            CSharpParameterToken[] ruleParameters,
            IProjectionToken projection
        )
        {
            this.Namespace = @namespace;
            this.ReturnType = returnType;
            this.Name = name;
            this.RuleParameters = ruleParameters;
            this.Projection = projection;
        }

        public override string ToString()
        {
            return $"{this.ReturnType} {this.Name} = " +
                   $"<none>" +
                   $"{this.Projection}";
        }
    }
}