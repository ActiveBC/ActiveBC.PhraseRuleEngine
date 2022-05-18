using System;

namespace ActiveBC.PhraseRuleEngine.Exceptions
{
    /// <summary>
    /// Represents the base class for all the exceptions that are intentionally thrown by the library.
    /// </summary>
    public abstract class PhraseRuleEngineException : Exception
    {
        protected PhraseRuleEngineException(string message) : base(message)
        {
        }

        protected PhraseRuleEngineException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}