using System;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Types.Formatting
{
    public interface ITypeFormatter
    {
        string GetStringRepresentation(Type type);
    }
}