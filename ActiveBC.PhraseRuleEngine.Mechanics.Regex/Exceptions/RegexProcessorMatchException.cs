using System;
using ActiveBC.PhraseRuleEngine.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Exceptions
{
    public sealed class RegexProcessorMatchException : RuleMatchException
    {
        public RegexProcessorMatchException(string message) : base(message)
        {
        }

        public RegexProcessorMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}