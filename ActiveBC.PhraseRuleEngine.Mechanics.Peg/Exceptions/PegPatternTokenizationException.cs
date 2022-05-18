using ActiveBC.PhraseRuleEngine.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Exceptions
{
    public sealed class PegPatternTokenizationException : PhraseRuleEngineTokenizationException
    {
        public PegPatternTokenizationException(string message, string source)
            : base(message, source)
        {
        }
    }
}