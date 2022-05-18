using ActiveBC.PhraseRuleEngine.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Exceptions
{
    public sealed class RegexPatternTokenizationException : PhraseRuleEngineTokenizationException
    {
        public RegexPatternTokenizationException(string message, string source)
            : base(message, source)
        {
        }
    }
}