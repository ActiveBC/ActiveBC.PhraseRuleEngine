using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Grammar;

namespace ActiveBC.PhraseRuleEngine.Build.Tokenization.Grammar
{
    public partial class RuleSetSyntaxMatcher
    {
        private readonly string? m_namespace;
        private readonly IReadOnlyDictionary<string, IPatternTokenizer> m_patternParsers;
        private readonly CSharpSyntaxMatcher m_cSharpSyntaxMatcher;
        private readonly bool m_caseSensitive;

        public RuleSetSyntaxMatcher(
            string? @namespace,
            IReadOnlyDictionary<string, IPatternTokenizer> patternParsers,
            CSharpSyntaxMatcher cSharpSyntaxMatcher,
            bool caseSensitive
        )
        {
            this.m_namespace = @namespace;
            this.m_patternParsers = patternParsers;
            this.m_cSharpSyntaxMatcher = cSharpSyntaxMatcher;
            this.m_caseSensitive = caseSensitive;
        }

        public RuleSetSyntaxMatcher(
            bool handleLeftRecursion,
            string? @namespace,
            IReadOnlyDictionary<string, IPatternTokenizer> patternParsers,
            CSharpSyntaxMatcher cSharpSyntaxMatcher,
            bool caseSensitive
        )
            : base(handleLeftRecursion)
        {
            this.m_namespace = @namespace;
            this.m_patternParsers = patternParsers;
            this.m_cSharpSyntaxMatcher = cSharpSyntaxMatcher;
            this.m_caseSensitive = caseSensitive;
        }
    }
}