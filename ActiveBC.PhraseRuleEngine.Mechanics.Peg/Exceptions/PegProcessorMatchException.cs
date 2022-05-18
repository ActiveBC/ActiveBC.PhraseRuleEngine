using System;
using ActiveBC.PhraseRuleEngine.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Exceptions
{
    public sealed class PegProcessorMatchException : RuleMatchException
    {
        public PegProcessorMatchException(string message) : base(message)
        {
        }

        public PegProcessorMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}