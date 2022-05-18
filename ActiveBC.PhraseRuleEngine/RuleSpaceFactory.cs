using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build;
using ActiveBC.PhraseRuleEngine.Build.InputProcessing;
using ActiveBC.PhraseRuleEngine.Build.InputProcessing.Models;
using ActiveBC.PhraseRuleEngine.Build.Rule.Projection;
using ActiveBC.PhraseRuleEngine.Build.Rule.Projection.Models;
using ActiveBC.PhraseRuleEngine.Build.Rule.Source;
using ActiveBC.PhraseRuleEngine.Build.Rule.Static;
using ActiveBC.PhraseRuleEngine.Build.Tokenization;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Evaluation;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Exceptions;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Types.Formatting;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Types.Resolving;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine
{
    /// <summary>
    /// This class represents the logic of compiling rule space from its sources.
    /// </summary>
    public sealed class RuleSpaceFactory
    {
        private readonly IProjectionCompiler m_projectionCompiler;
        private readonly ITypeResolver m_typeResolver;
        private readonly IReadOnlyDictionary<Type, IInputProcessorFactory> m_inputProcessorFactoriesByPatternTokenType;
        private int m_lastUsedCachedMatcherId = 0;

        public StaticRuleFactory StaticRuleFactory { get; }
        public IReadOnlyDictionary<string, IPatternTokenizer> PatternTokenizers { get; }
        public IRuleSetTokenizer RuleSetTokenizer { get; }

        public RuleSpaceFactory(IReadOnlyCollection<MechanicsBundle> mechanicsCollection)
        {
            this.m_projectionCompiler = new ProjectionCompiler(
                new CachedTypeFormatter(new TypeFormatter()),
                new CodeEmitter(
                    "ActiveBC.PhraseRuleEngine.Projections",
                    $"{typeof(ProjectionCompiler).Namespace!}.Generated",
                    "ProjectionContainer_",
                    "ApplyProjection_"
                )
            );
            this.m_typeResolver = new TypeResolver();
            this.m_inputProcessorFactoriesByPatternTokenType = mechanicsCollection
                .Select(mechanics => new KeyValuePair<Type, IInputProcessorFactory>(mechanics.TokenType, mechanics.Factory))
                .ToDictionaryWithKnownCapacity(mechanicsCollection.Count);
            this.StaticRuleFactory = new StaticRuleFactory(this);
            this.PatternTokenizers = mechanicsCollection.ToDictionary(mechanics => mechanics.Key, mechanics => mechanics.Tokenizer);
            this.RuleSetTokenizer = new LoopBasedRuleSetTokenizer(this.PatternTokenizers);
        }

        /// <summary>
        /// This method allows to create a rule space from different sources:
        /// rule set, rules, already built (standalone) matchers, and rule spaces, built before.
        /// </summary>
        /// <param name="ruleSets">
        /// Collection of rule sets to include to new rule space.
        /// The keys for the provided rules will be extracted from the rule tokens.
        /// </param>
        /// <param name="rulesByName">
        /// Collection of rules to include to new rule space.
        /// The keys for the provided rules will be extracted from the rule tokens.
        /// </param>
        /// <param name="standaloneMatchersByName">
        /// Collection of previously compiled standalone rule matchers to include to new rule space.
        /// The keys for the provided rules will be extracted from the this dictionary keys.
        /// </param>
        /// <param name="includedRuleSpaces">
        /// Dictionary of previously compiled rule spaces to include to new rule space,
        /// where key are prefixes of the respective values' matchers in new rule space.
        /// </param>
        /// <param name="ruleSpaceParameterTypes">
        /// Description of the rule space parameters.
        /// The values of these types can be passed as respective arguments when calling rule matcher.
        /// </param>
        /// <param name="assembliesProvider">
        /// todo remove or describe
        /// </param>
        /// <param name="rootRuleName">
        /// The conventional value of rule set "root rule" name.
        /// Root rule (if exists) is invoked when the rule set is referenced "as whole" (by it's namespace).
        /// </param>
        /// <returns>Compiled rule space.</returns>
        public IRuleSpace CreateWithAliases(
            IReadOnlyCollection<RuleSetToken> ruleSets,
            IReadOnlyCollection<IRuleToken> rulesByName,
            IReadOnlyDictionary<string, IRuleMatcher> standaloneMatchersByName,
            IReadOnlyDictionary<string, IRuleSpace> includedRuleSpaces,
            IReadOnlyDictionary<string, Type> ruleSpaceParameterTypes,
            IAssembliesProvider assembliesProvider,
            string rootRuleName = "Root"
        )
        {
            IReadOnlyDictionary<string, IRuleToken> ruleTokensByName = CreateRuleTokens(ruleSets, rulesByName);

            IReadOnlyDictionary<string, IRuleMatcher> matchersByName = Enumerable
                .Empty<IEnumerable<KeyValuePair<string, IRuleMatcher>>>()
                .Append(standaloneMatchersByName)
                .Append(
                    includedRuleSpaces
                        .SelectMany(
                            pair => pair
                                .Value
                                .RuleMatchersByName
                                .MapKey(key => $"{pair.Key}.{key}")
                        )
                )
                .MergeWithKnownCapacity(
                    standaloneMatchersByName.Count + includedRuleSpaces.Aggregate(0, (count, ruleSpace) => count + ruleSpace.Value.RuleResultTypesByName.Count),
                    true
                );

            IReadOnlyDictionary<string, string> aliases = CreateAliases(ruleTokensByName, rootRuleName);

            IReadOnlyDictionary<string, IReadOnlySet<string>> usingsByRuleName = GetUsingsByRuleName(ruleSets, aliases);

            RuleDescriptionProvider ruleDescriptionProvider = new RuleDescriptionProvider(
                this.m_typeResolver,
                assembliesProvider,
                ruleTokensByName,
                matchersByName,
                aliases,
                usingsByRuleName
            );

            IReadOnlyDictionary<string, CapturedVariablesParameters> capturedVariablesParametersByRuleName = GetCapturedVariablesParametersByRuleName(ruleTokensByName, matchersByName, ruleDescriptionProvider, aliases);
            IReadOnlyDictionary<string, RuleParameters> ruleParametersByRuleName = GetRuleParametersByRuleName(ruleTokensByName, usingsByRuleName, assembliesProvider, aliases);

            IReadOnlyDictionary<string, IRuleProjection> projectionsByRuleName = CreateProjections(
                    ruleDescriptionProvider,
                    assembliesProvider,
                    ruleTokensByName,
                    usingsByRuleName,
                    capturedVariablesParametersByRuleName,
                    ruleParametersByRuleName,
                    ruleSpaceParameterTypes,
                    aliases
                );

            IReadOnlyDictionary<string, IRuleSource> ruleSources = Enumerable
                .Empty<KeyValuePair<string, IRuleSource>>()
                .Concat(CreateTokenBasedRuleSources(ruleTokensByName, capturedVariablesParametersByRuleName, ruleParametersByRuleName, ruleDescriptionProvider, projectionsByRuleName))
                .Concat(CreateMatcherBasedRuleSource(matchersByName))
                .ToDictionaryWithKnownCapacity(ruleTokensByName.Count + matchersByName.Count);

            RuleSpaceBuilder ruleSpaceBuilder = new RuleSpaceBuilder(
                ruleDescriptionProvider,
                ruleSpaceParameterTypes,
                ruleSources,
                aliases,
                this
            );

            return ruleSpaceBuilder.Build();
        }

        public IRuleMatcher AddRule(
            IRuleSpace ruleSpace,
            IRuleToken rule,
            IAssembliesProvider assembliesProvider
        )
        {
            string ruleKey = rule.Name;

            IRuleMatcher ruleMatcher = CreateSingleRule();

            ruleSpace[ruleKey] = ruleMatcher;

            return ruleMatcher;

            IRuleMatcher CreateSingleRule()
            {
                Type resultType = this
                    .m_typeResolver
                    .Resolve(rule.ReturnType, ImmutableHashSet<string>.Empty, assembliesProvider);

                RuleParameters ruleParameters = GetRuleParameters(
                    rule,
                    assembliesProvider,
                    ImmutableHashSet<string>.Empty
                );

                RuleCapturedVariables ownCapturedVariables = CollectOwnCapturedVariables(
                    rule,
                    new RuleSpaceBasedDescriptionProvider(ruleSpace)
                );

                CapturedVariablesParameters capturedVariablesParameters = new CapturedVariablesParameters(
                    YieldAllCapturedVariables(ownCapturedVariables).ToDictionary(true)
                );

                IRuleSource ruleSource = CreateRuleSource(
                    rule,
                    ruleParameters,
                    capturedVariablesParameters,
                    resultType,
                    this
                        .m_projectionCompiler
                        .CreateProjection(
                            ruleKey,
                            CreateProjectionCompilationData(
                                ruleKey,
                                rule,
                                ImmutableHashSet<string>.Empty,
                                resultType,
                                capturedVariablesParameters,
                                ruleParameters,
                                new RuleSpaceParameters(ruleSpace.RuleSpaceParameterTypesByName)
                            ),
                            assembliesProvider
                        )
                );

                return WrapWithCache(ruleSource.GetRuleMatcher(ruleSpace));

                IEnumerable<KeyValuePair<string, Type>> YieldAllCapturedVariables(RuleCapturedVariables capturedVariables)
                {
                    foreach (KeyValuePair<string, Type> ownVariable in capturedVariables.OwnVariables)
                    {
                        yield return ownVariable;
                    }

                    foreach (string referencedRuleKey in capturedVariables.ReferencedRules)
                    {
                        if (ruleSpace.RuleMatchersByName.TryGetValue(referencedRuleKey, out IRuleMatcher? referencedMatcher))
                        {
                            foreach (KeyValuePair<string, Type> referencedVariable in referencedMatcher.ResultDescription.CapturedVariablesTypes)
                            {
                                yield return referencedVariable;
                            }
                        }
                        else
                        {
                            throw new RuleBuildException(
                                $"Rule '{referencedMatcher}' referenced from rule '{ruleKey}' doesn't exist."
                            );
                        }
                    }
                }
            }
        }

        internal CachingRuleMatcher WrapWithCache(IRuleMatcher source)
        {
            return new CachingRuleMatcher(++this.m_lastUsedCachedMatcherId, source);
        }

        private IReadOnlyDictionary<string, CapturedVariablesParameters> GetCapturedVariablesParametersByRuleName(
            IReadOnlyDictionary<string, IRuleToken> ruleTokensByName,
            IReadOnlyDictionary<string, IRuleMatcher> matcherByName,
            IRuleDescriptionProvider ruleDescriptionProvider,
            IReadOnlyDictionary<string, string> aliases
        )
        {
            Dictionary<string, RuleCapturedVariables> capturedVariablesByRuleKey = new Dictionary<string, RuleCapturedVariables>(
                ruleTokensByName.Count
            );

            foreach ((string ruleKey, IRuleToken ruleToken) in ruleTokensByName)
            {
                capturedVariablesByRuleKey.Add(
                    ruleKey,
                    CollectOwnCapturedVariables(ruleToken, ruleDescriptionProvider)
                );
            }

            // the process is split into two parts intentionally:
            // first we collect all own variables (i.e. declared directly in the rule)
            // second we collect all the variables (including variables from referenced rules)
            return capturedVariablesByRuleKey
                .MapValue(
                    (ruleKey, capturedVariables) => new CapturedVariablesParameters(
                        YieldAllCapturedVariables(
                            ruleKey,
                            capturedVariables,
                            new HashSet<string>()
                            {
                                ruleKey,
                            }
                        )
                        .ToDictionary(true)
                    )
                )
                .ToDictionaryWithKnownCapacity(ruleTokensByName.Count + aliases.Count)
                .AddAliases(aliases);

            IEnumerable<KeyValuePair<string, Type>> CollectCapturedVariables(
                string originalRuleKey,
                string referencedRuleKey,
                in ISet<string> discoveredReferences
            )
            {
                if (discoveredReferences.Contains(referencedRuleKey))
                {
                    return Enumerable.Empty<KeyValuePair<string, Type>>();
                }

                discoveredReferences.Add(referencedRuleKey);

                if (capturedVariablesByRuleKey.TryGetValue(referencedRuleKey, out RuleCapturedVariables? capturedVariables))
                {
                    return YieldAllCapturedVariables(originalRuleKey, capturedVariables, discoveredReferences);
                }

                if (matcherByName.TryGetValue(referencedRuleKey, out IRuleMatcher? ruleMatcher))
                {
                    return ruleMatcher.ResultDescription.CapturedVariablesTypes;
                }

                return Enumerable.Empty<KeyValuePair<string, Type>>();
            }

            IEnumerable<KeyValuePair<string, Type>> YieldAllCapturedVariables(
                string originalRuleKey,
                RuleCapturedVariables capturedVariables,
                ISet<string> discoveredReferences
            )
            {
                foreach (KeyValuePair<string, Type> ownVariable in capturedVariables.OwnVariables)
                {
                    yield return ownVariable;
                }

                foreach (string referencedRuleKey in capturedVariables.ReferencedRules)
                {
                    foreach (KeyValuePair<string, Type> referencedVariable in CollectCapturedVariables(originalRuleKey, referencedRuleKey, discoveredReferences))
                    {
                        yield return referencedVariable;
                    }
                }
            }
        }

        private RuleCapturedVariables CollectOwnCapturedVariables(
            IRuleToken ruleToken,
            IRuleDescriptionProvider ruleDescriptionProvider
        )
        {
            return ruleToken switch
            {
                EmptyRuleToken => new RuleCapturedVariables(ImmutableDictionary<string, Type>.Empty, Array.Empty<string>()),
                RuleToken rule => ExtractOwnCapturedVariables(rule),
                _ => throw new ArgumentOutOfRangeException(nameof(ruleToken))
            };

            RuleCapturedVariables ExtractOwnCapturedVariables(RuleToken rule)
            {
                return ExtractVariables(rule.Pattern, rule.GetFullName());
            }

            RuleCapturedVariables ExtractVariables(IPatternToken pattern, string ruleName)
            {
                Type key = pattern.GetType();

                if (this.m_inputProcessorFactoriesByPatternTokenType.TryGetValue(key, out IInputProcessorFactory? factory))
                {
                    try
                    {
                        return factory.ExtractOwnCapturedVariables(pattern, ruleDescriptionProvider);
                    }
                    catch (Exception exception)
                    {
                        throw new RuleBuildException(
                            $"Cannot get variables from rule '{ruleName}'.",
                            exception
                        );
                    }
                }

                throw new RuleBuildException(
                    $"Input processor for pattern token '{key.FullName}' does not exist."
                );
            }
        }

        private IReadOnlyDictionary<string, RuleParameters> GetRuleParametersByRuleName(
            IReadOnlyDictionary<string, IRuleToken> ruleTokensByName,
            IReadOnlyDictionary<string, IReadOnlySet<string>> usingsByRuleName,
            IAssembliesProvider assembliesProvider,
            IReadOnlyDictionary<string, string> aliases
        )
        {
            return ruleTokensByName
                .MapValue(
                    (ruleKey, rule) => GetRuleParameters(
                        rule,
                        assembliesProvider,
                        usingsByRuleName.GetValueOrDefault(ruleKey, ImmutableHashSet<string>.Empty)
                    )
                )
                .ToDictionaryWithKnownCapacity(ruleTokensByName.Count + aliases.Count)
                .AddAliases(aliases);
        }

        private RuleParameters GetRuleParameters(
            IRuleToken rule,
            IAssembliesProvider assembliesProvider,
            IReadOnlySet<string> usings
        )
        {
            return new RuleParameters(
                rule
                    .RuleParameters
                    .ToDictionary(
                        ruleParameter => ruleParameter.Name,
                        ruleParameter => this
                            .m_typeResolver
                            .Resolve(ruleParameter.Type, usings, assembliesProvider)
                    )
            );
        }

        private static IEnumerable<KeyValuePair<string, IRuleSource>> CreateMatcherBasedRuleSource(
            IReadOnlyDictionary<string, IRuleMatcher> matcherBasedRulesByName
        )
        {
            return matcherBasedRulesByName.MapValue(matcher => (IRuleSource) new MatcherBasedRuleSource(matcher));
        }

        private IEnumerable<KeyValuePair<string, IRuleSource>> CreateTokenBasedRuleSources(
            IReadOnlyDictionary<string, IRuleToken> ruleTokensByName,
            IReadOnlyDictionary<string, CapturedVariablesParameters> capturedVariablesParametersByRuleName,
            IReadOnlyDictionary<string, RuleParameters> ruleParametersByRuleName,
            IRuleDescriptionProvider ruleDescriptionProvider,
            IReadOnlyDictionary<string, IRuleProjection> projectionsByRuleName
        )
        {
            return ruleTokensByName
                .MapValue(
                    (ruleKey, ruleToken) => CreateRuleSource(
                        ruleToken,
                        ruleParametersByRuleName[ruleKey],
                        capturedVariablesParametersByRuleName[ruleKey],
                        ruleDescriptionProvider[ruleKey],
                        projectionsByRuleName[ruleKey]
                    )
                );
        }

        private IRuleSource CreateRuleSource(
            IRuleToken ruleToken,
            RuleParameters ruleParameters,
            CapturedVariablesParameters capturedVariablesParameters,
            Type resultType,
            IRuleProjection projection
        )
        {
            return ruleToken switch
            {
                EmptyRuleToken emptyRule => new EmptyRuleTokenBasedRuleSource(
                    emptyRule,
                    ruleParameters,
                    new RuleMatchResultDescription(resultType, capturedVariablesParameters.Values)
               ),
                RuleToken rule => new RuleTokenBasedRuleSource(
                    rule,
                    this.m_inputProcessorFactoriesByPatternTokenType[rule.Pattern.GetType()],
                    ruleParameters,
                    capturedVariablesParameters,
                    new RuleMatchResultDescription(resultType, capturedVariablesParameters.Values),
                    projection
                ),
                _ => throw new ArgumentOutOfRangeException(nameof(ruleToken))
            };
        }

        private IReadOnlyDictionary<string, IRuleProjection> CreateProjections(
            IRuleDescriptionProvider ruleDescriptionProvider,
            IAssembliesProvider assembliesProvider,
            IReadOnlyDictionary<string, IRuleToken> ruleTokens,
            IReadOnlyDictionary<string, IReadOnlySet<string>> usingsByRuleName,
            IReadOnlyDictionary<string, CapturedVariablesParameters> capturedVariablesParametersByRuleName,
            IReadOnlyDictionary<string, RuleParameters> ruleParametersByRuleName,
            IReadOnlyDictionary<string, Type> ruleSpaceParametersTypes,
            IReadOnlyDictionary<string, string> aliases
        )
        {
            return this.m_projectionCompiler
                .CreateProjections(
                    ruleTokens
                        .MapValue(
                            (ruleKey, rule) => CreateProjectionCompilationData(
                                ruleKey,
                                rule,
                                usingsByRuleName.GetValueOrDefault(ruleKey, ImmutableHashSet<string>.Empty),
                                ruleDescriptionProvider[ruleKey],
                                capturedVariablesParametersByRuleName[ruleKey],
                                ruleParametersByRuleName[ruleKey],
                                new RuleSpaceParameters(ruleSpaceParametersTypes)
                            )
                        )
                        .ToDictionaryWithKnownCapacity(ruleTokens.Count),
                    assembliesProvider,
                    aliases.Count
                )
                .AddAliases(aliases);
        }

        private static IProjectionCompilationData CreateProjectionCompilationData(
            string ruleKey,
            IRuleToken rule,
            IReadOnlySet<string> usings,
            Type resultType,
            CapturedVariablesParameters capturedVariablesParameters,
            RuleParameters ruleParameters,
            RuleSpaceParameters ruleSpaceParameters
        )
        {
            return rule.Projection switch
            {
                BodyBasedProjectionToken bodyBasedProjection => new BodyBasedProjectionCompilationData(
                    usings,
                    resultType,
                    new ProjectionParameters(
                        typeof(string[]),
                        capturedVariablesParameters,
                        ruleParameters,
                        ruleSpaceParameters
                    ),
                    bodyBasedProjection.Body
                ),
                MatchedInputBasedProjectionToken => MatchedInputBasedProjectionCompilationData.Instance,
                VoidProjectionToken => VoidProjectionCompilationData.Instance,
                ConstantProjectionToken constantProjection => new ConstantProjectionCompilationData(
                    constantProjection.Constant
                ),
                _ => throw new RuleBuildException(
                    $"Rule {ruleKey} has unknown projection type {rule.Projection.GetType().FullName}"
                )
            };
        }

        private static Dictionary<string, IRuleToken> CreateRuleTokens(
            IReadOnlyCollection<RuleSetToken> ruleSets,
            IReadOnlyCollection<IRuleToken> rulesByName
        )
        {
            int capacity = rulesByName.Count + ruleSets.Aggregate(0, (count, ruleSet) => count + ruleSet.Rules.Length);

            return ruleSets
                .SelectMany(ruleSet => ruleSet.Rules)
                .Concat(rulesByName)
                .Select(rule => new KeyValuePair<string, IRuleToken>(rule.GetFullName(), rule))
                .ToDictionaryWithKnownCapacity(
                    capacity,
                    false,
                    (ruleKey, argumentException) => new RuleBuildException($"Duplicate rule '{ruleKey}'.", argumentException)
                );
        }

        private static IReadOnlyDictionary<string, string> CreateAliases(
            IReadOnlyDictionary<string, IRuleToken> ruleTokensByName,
            string rootRuleName
        )
        {
            bool Filter(IRuleToken rule) => rule.Namespace is not null && rule.Name == rootRuleName;

            int capacity = ruleTokensByName.Aggregate(0, (count, pair) => Filter(pair.Value) ? ++count : count);

            return ruleTokensByName
                .WhereValue(Filter)
                .MapValue(rule => rule.Namespace!)
                .SwapKeysAndValues()
                .ToDictionaryWithKnownCapacity(capacity);
        }

        private static IReadOnlyDictionary<string, IReadOnlySet<string>> GetUsingsByRuleName(
            IReadOnlyCollection<RuleSetToken> ruleSets,
            IReadOnlyDictionary<string, string> aliases
        )
        {
            int capacity = aliases.Count + ruleSets.Aggregate(0, (count, ruleSet) => count + ruleSet.Rules.Length);

            return ruleSets
                .SelectMany(
                    ruleSet =>
                    {
                        IReadOnlySet<string> usings = ruleSet.Usings.Select(@using => @using.Namespace).ToHashSet();

                        return ruleSet
                            .Rules
                            .Select(rule => new KeyValuePair<string, IReadOnlySet<string>>(rule.GetFullName(), usings));
                    }
                )
                .ToDictionaryWithKnownCapacity(capacity)
                .AddAliases(aliases);
        }

        private sealed class RuleSpaceBasedDescriptionProvider : IRuleDescriptionProvider
        {
            private readonly IRuleSpace m_ruleSpace;

            public Type this[string ruleKey] => this.m_ruleSpace.RuleResultTypesByName[ruleKey];

            public Dictionary<string, Type> ResultTypesByRuleName => this.m_ruleSpace.RuleResultTypesByName.ToDictionary();
            public RuleSpaceBasedDescriptionProvider(IRuleSpace ruleSpace)
            {
                this.m_ruleSpace = ruleSpace;
            }

            public bool TryGet(string ruleKey, [MaybeNullWhen(false)] out Type type)
            {
                return this.m_ruleSpace.RuleResultTypesByName.TryGetValue(ruleKey, out type);
            }
        }
        private sealed class RuleDescriptionProvider : IRuleDescriptionProvider
        {
            private readonly ITypeResolver m_typeResolver;
            private readonly IAssembliesProvider m_assembliesProvider;

            private readonly IReadOnlyDictionary<string, IRuleToken> m_tokenBasedRules;
            private readonly IReadOnlyDictionary<string, IRuleMatcher> m_matcherBasedRules;
            private readonly IReadOnlyDictionary<string, string> m_aliases;
            private readonly IReadOnlyDictionary<string, IReadOnlySet<string>> m_usingsByRuleName;

            private Dictionary<string, Type>? m_returnTypesByRuleName;

            public Dictionary<string, Type> ResultTypesByRuleName
            {
                get
                {
                    return this.m_returnTypesByRuleName ??= new[]
                        {
                            this
                                .m_tokenBasedRules
                                .MapValue(
                                    (ruleKey, ruleToken) => this.m_typeResolver.Resolve(
                                        ruleToken.ReturnType,
                                        this
                                            .m_usingsByRuleName
                                            .GetValueOrDefault(ruleKey, ImmutableHashSet<string>.Empty),
                                        this.m_assembliesProvider
                                    )
                                ),
                            this.m_matcherBasedRules.MapValue(matcher => matcher.ResultDescription.ResultType)
                        }
                        .MergeWithKnownCapacity(
                            this.m_tokenBasedRules.Count + this.m_matcherBasedRules.Count + this.m_aliases.Count
                        )
                        .AddAliases(this.m_aliases);
                }
            }

            public Type this[string ruleKey]
            {
                get
                {
                    if (this.ResultTypesByRuleName.TryGetValue(ruleKey, out Type? ruleResultType))
                    {
                        return ruleResultType;
                    }

                    throw new RuleBuildException($"Description for rule {ruleKey} not found.");
                }
            }

            public RuleDescriptionProvider(
                ITypeResolver typeResolver,
                IAssembliesProvider assembliesProvider,
                IReadOnlyDictionary<string, IRuleToken> tokenBasedRules,
                IReadOnlyDictionary<string, IRuleMatcher> matcherBasedRules,
                IReadOnlyDictionary<string, string> aliases,
                IReadOnlyDictionary<string, IReadOnlySet<string>> usingsByRuleName
            )
            {
                this.m_typeResolver = typeResolver;
                this.m_assembliesProvider = assembliesProvider;
                this.m_tokenBasedRules = tokenBasedRules;
                this.m_matcherBasedRules = matcherBasedRules;
                this.m_aliases = aliases;
                this.m_usingsByRuleName = usingsByRuleName;
            }

            public bool TryGet(string ruleKey, [MaybeNullWhen(false)] out Type type)
            {
                return this.ResultTypesByRuleName.TryGetValue(ruleKey, out type);
            }
        }
    }
}