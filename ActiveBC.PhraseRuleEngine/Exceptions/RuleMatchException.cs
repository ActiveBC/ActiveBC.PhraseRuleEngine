using System;

namespace ActiveBC.PhraseRuleEngine.Exceptions
{
    public class RuleMatchException : PhraseRuleEngineException
    {
        public RuleMatchException(string message) : base(message)
        {
        }

        public RuleMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}