using System;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Build.Rule.Source;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Cache;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Build
{
    internal sealed class RuleSpaceBuilder
    {
        private readonly IRuleDescriptionProvider m_ruleDescriptionProvider;
        private readonly IReadOnlyDictionary<string, Type> m_ruleSpaceParameterTypesByName;
        private readonly IReadOnlyDictionary<string, IRuleSource> m_ruleSourcesByName;
        private readonly IReadOnlyDictionary<string, string> m_ruleAliases;
        private readonly RuleSpaceFactory m_ruleSpaceFactory;

        private RuleSpace? m_ruleSpace;

        public RuleSpaceBuilder(
            IRuleDescriptionProvider ruleDescriptionProvider,
            IReadOnlyDictionary<string, Type> ruleSpaceParameterTypesByName,
            IReadOnlyDictionary<string, IRuleSource> ruleSourcesByName,
            IReadOnlyDictionary<string, string> ruleAliases,
            RuleSpaceFactory ruleSpaceFactory
        )
        {
            this.m_ruleDescriptionProvider = ruleDescriptionProvider;
            this.m_ruleSpaceParameterTypesByName = ruleSpaceParameterTypesByName;
            this.m_ruleSourcesByName = ruleSourcesByName;
            this.m_ruleAliases = ruleAliases;
            this.m_ruleSpaceFactory = ruleSpaceFactory;
        }

        public IRuleSpace Build()
        {
            Dictionary<string, IRuleMatcher> ruleMatchers = new Dictionary<string, IRuleMatcher>(
                this.m_ruleSourcesByName.Count + this.m_ruleAliases.Count
            );

            this.m_ruleSpace = new RuleSpace(
                this.m_ruleSpaceParameterTypesByName,
                this.m_ruleDescriptionProvider.ResultTypesByRuleName,
                ruleMatchers
            );

            FillRuleMatchers(ruleMatchers);

            return this.m_ruleSpace;
        }

        private void FillRuleMatchers(in Dictionary<string, IRuleMatcher> ruleMatchers)
        {
            foreach ((string ruleKey, IRuleSource ruleSource) in this.m_ruleSourcesByName)
            {
                IRuleMatcher ruleMatcher = ruleSource.GetRuleMatcher(this.m_ruleSpace!);

                CachingRuleMatcher cachingRuleMatcher;

                if (ruleMatcher is CachingRuleMatcher caching)
                {
                    cachingRuleMatcher = caching;
                }
                else
                {
                    cachingRuleMatcher = this.m_ruleSpaceFactory.WrapWithCache(ruleMatcher);
                }

                ruleMatchers.Add(ruleKey, cachingRuleMatcher);
            }

            ruleMatchers.AddAliases(this.m_ruleAliases);
        }
    }
}