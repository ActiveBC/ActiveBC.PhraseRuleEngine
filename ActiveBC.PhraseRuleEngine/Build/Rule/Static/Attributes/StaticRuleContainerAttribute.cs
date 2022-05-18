using System;

namespace ActiveBC.PhraseRuleEngine.Build.Rule.Static.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class StaticRuleContainerAttribute: Attribute
    {
        public string Namespace { get; }

        public StaticRuleContainerAttribute(string @namespace)
        {
            this.Namespace = @namespace;
        }
    }
}