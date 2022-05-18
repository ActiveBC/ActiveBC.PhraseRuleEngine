using System;

namespace ActiveBC.PhraseRuleEngine.Exceptions
{
    public class RuleBuildException : PhraseRuleEngineException
    {
        public RuleBuildException(string message) : base(message)
        {
        }

        public RuleBuildException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}