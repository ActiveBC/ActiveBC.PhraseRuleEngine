using ActiveBC.PhraseRuleEngine.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Exceptions
{
    public sealed class PegProcessorBuildException : RuleBuildException
    {
        public PegProcessorBuildException(string message) : base(message)
        {
        }
    }
}