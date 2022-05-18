using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Tests.Helpers.Dummy
{
    internal sealed class DummyPatternToken : IPatternToken
    {
        public string Pattern { get; }

        public DummyPatternToken(string pattern)
        {
            this.Pattern = pattern;
        }

        public override string ToString()
        {
            return this.Pattern;
        }
    }
}