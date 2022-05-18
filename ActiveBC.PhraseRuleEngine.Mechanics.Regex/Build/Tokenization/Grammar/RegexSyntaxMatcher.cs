using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Grammar;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Grammar
{
    public partial class RegexSyntaxMatcher
    {
        private readonly string? m_namespace;

        private readonly CSharpSyntaxMatcher m_cSharpSyntaxMatcher;

        public RegexSyntaxMatcher(string? @namespace, CSharpSyntaxMatcher cSharpSyntaxMatcher)
        {
            this.m_namespace = @namespace;
            this.m_cSharpSyntaxMatcher = cSharpSyntaxMatcher;
        }

        public RegexSyntaxMatcher(bool handleLeftRecursion, string? @namespace, CSharpSyntaxMatcher cSharpSyntaxMatcher)
            : base(handleLeftRecursion)
        {
            this.m_namespace = @namespace;
            this.m_cSharpSyntaxMatcher = cSharpSyntaxMatcher;
        }
    }
}