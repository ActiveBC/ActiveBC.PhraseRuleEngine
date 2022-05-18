using System;
using ActiveBC.PhraseRuleEngine.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Exceptions
{
    public sealed class RegexProcessorBuildException : RuleBuildException
    {
        public RegexProcessorBuildException(string message) : base(message)
        {
        }

        public RegexProcessorBuildException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}