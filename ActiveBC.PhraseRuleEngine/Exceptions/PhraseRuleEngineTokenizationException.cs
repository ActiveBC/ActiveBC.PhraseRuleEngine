using System;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Exceptions
{
    public class PhraseRuleEngineTokenizationException : PhraseRuleEngineException
    {
        public PhraseRuleEngineTokenizationException(string message, Exception innerException, string source) : base(message, innerException)
        {
            ErrorIndexHelper.FillExceptionData(this.Data, source);
        }

        public PhraseRuleEngineTokenizationException(string message, string source) : base(message)
        {
            ErrorIndexHelper.FillExceptionData(this.Data, source);
        }
    }
}