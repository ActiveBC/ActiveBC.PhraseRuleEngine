using System;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Types.Resolving
{
    public sealed class TypeResolverException : Exception
    {
        public TypeResolverException(string message) : base(message)
        {
        }
    }
}