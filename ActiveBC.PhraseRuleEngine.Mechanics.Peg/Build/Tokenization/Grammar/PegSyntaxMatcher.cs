using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Grammar;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Grammar
{
    public partial class PegSyntaxMatcher
    {
        private readonly string? m_namespace;

        private readonly CSharpSyntaxMatcher m_cSharpSyntaxMatcher;

        public PegSyntaxMatcher(string? @namespace, CSharpSyntaxMatcher cSharpSyntaxMatcher)
        {
            this.m_namespace = @namespace;
            this.m_cSharpSyntaxMatcher = cSharpSyntaxMatcher;
        }

        public PegSyntaxMatcher(bool handleLeftRecursion, string? @namespace, CSharpSyntaxMatcher cSharpSyntaxMatcher)
            : base(handleLeftRecursion)
        {
            this.m_namespace = @namespace;
            this.m_cSharpSyntaxMatcher = cSharpSyntaxMatcher;
        }
    }
}