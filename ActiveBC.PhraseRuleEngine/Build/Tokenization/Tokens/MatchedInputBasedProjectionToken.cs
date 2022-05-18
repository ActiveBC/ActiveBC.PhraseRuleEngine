using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens
{
    public sealed class MatchedInputBasedProjectionToken : IProjectionToken
    {
        public static readonly MatchedInputBasedProjectionToken Instance = new MatchedInputBasedProjectionToken();
        public static readonly ICSharpTypeToken ReturnType = new ResolvedCSharpTypeToken("string", typeof(string));

        private MatchedInputBasedProjectionToken()
        {
        }

        public override string ToString()
        {
            return "{ return input; }";
        }
    }
}