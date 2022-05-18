using System;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Types.Formatting
{
    public sealed class TypeFormatterException : Exception
    {
        public TypeFormatterException(string message) : base(message)
        {
        }
    }
}