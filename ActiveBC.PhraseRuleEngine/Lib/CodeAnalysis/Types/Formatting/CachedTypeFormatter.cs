using System;
using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Types.Formatting
{
    public class CachedTypeFormatter : ITypeFormatter
    {
        private readonly ITypeFormatter m_typeFormatter;
        private readonly IDictionary<Type, string> m_cache;

        public CachedTypeFormatter(ITypeFormatter typeFormatter, int capacity = 0)
        {
            this.m_typeFormatter = typeFormatter;
            this.m_cache = new Dictionary<Type, string>(capacity);
        }

        public string GetStringRepresentation(Type type)
        {
            if (!this.m_cache.TryGetValue(type, out string? typeName))
            {
                typeName = this.m_typeFormatter.GetStringRepresentation(type);

                this.m_cache.Add(type, typeName);
            }

            return typeName;
        }
    }
}