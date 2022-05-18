using System;

namespace ActiveBC.PhraseRuleEngine.Build.Rule.Static.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class StaticRuleAttribute: Attribute
    {
        public string Name { get; }
        public string UsedWordsProviderMethodName { get; }

        public StaticRuleAttribute(string name, string usedWordsProviderMethodName)
        {
            this.Name = name;
            this.UsedWordsProviderMethodName = usedWordsProviderMethodName;
        }
    }
}
