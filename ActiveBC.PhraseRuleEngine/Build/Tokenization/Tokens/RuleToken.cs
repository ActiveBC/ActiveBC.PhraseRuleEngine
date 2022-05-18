using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens
{
    public sealed class RuleToken : IRuleToken
    {
        public string? Namespace { get; }
        public ICSharpTypeToken ReturnType { get; }
        public string Name { get; }
        public CSharpParameterToken[] RuleParameters { get; }
        public string PatternKey { get; }
        public IPatternToken Pattern { get; }
        public IProjectionToken Projection { get; }

        public RuleToken(
            string? @namespace,
            ICSharpTypeToken returnType,
            string name,
            CSharpParameterToken[] ruleParameters,
            string patternKey,
            IPatternToken pattern,
            IProjectionToken projection
        )
        {
            this.Namespace = @namespace;
            this.ReturnType = returnType;
            this.Name = name;
            this.RuleParameters = ruleParameters;
            this.PatternKey = patternKey;
            this.Pattern = pattern;
            this.Projection = projection;
        }

        public override string ToString()
        {
            return $"{this.ReturnType} {this.Name} = {this.PatternKey}#{this.Pattern}# {this.Projection}";
        }
    }
}