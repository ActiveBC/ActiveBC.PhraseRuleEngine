using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ActiveBC.PhraseRuleEngine.Build
{
    public interface IRuleDescriptionProvider
    {
        Dictionary<string, Type> ResultTypesByRuleName { get; }

        Type this[string ruleKey] { get; }

        bool TryGet(string ruleKey, [MaybeNullWhen(false)] out Type type);
    }
}