using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens
{
    public sealed class VoidProjectionToken : IProjectionToken
    {
        public static readonly VoidProjectionToken Instance = new VoidProjectionToken();
        public static readonly ICSharpTypeToken ReturnType = new ResolvedCSharpTypeToken("void", typeof(void));

        private VoidProjectionToken()
        {
        }

        public override string ToString()
        {
            return "{}";
        }
    }
}