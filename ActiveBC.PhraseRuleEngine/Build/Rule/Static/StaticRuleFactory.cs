using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ActiveBC.PhraseRuleEngine.Build.Rule.Static.Attributes;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Exceptions;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Build.Rule.Static
{
    public sealed class StaticRuleFactory
    {
        private readonly RuleSpaceFactory m_ruleSpaceFactory;

        public StaticRuleFactory(RuleSpaceFactory ruleSpaceFactory)
        {
            this.m_ruleSpaceFactory = ruleSpaceFactory;
        }

        public Dictionary<string, IRuleMatcher> ConvertStaticRuleContainerToRuleMatchers(Type container)
        {
            StaticRuleContainerAttribute? containerAttribute = container.GetCustomAttribute<StaticRuleContainerAttribute>();

            if (containerAttribute is null)
            {
                throw new RuleBuildException(
                    $"Type {container.FullName} is not a valid static rule container: " +
                    $"{nameof(StaticRuleContainerAttribute)} is missing"
                );
            }

            string ruleNamespace = containerAttribute.Namespace;

            return container
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Select(methodInfo => (Method: methodInfo, Attribute: methodInfo.GetCustomAttribute<StaticRuleAttribute>()))
                .Where(data => data.Attribute is not null)
                .Select(
                    tuple =>
                    {
                        string ruleName = $"{ruleNamespace}.{tuple.Attribute!.Name}";

                        return new KeyValuePair<string, IRuleMatcher>(
                            ruleName,
                            (IRuleMatcher) typeof(StaticRuleMatcherBuilder)
                                .GetMethod(nameof(StaticRuleMatcherBuilder.Build))!
                                .MakeGenericMethod(DescribeResultType(tuple.Method, ruleName))
                                .Invoke(
                                    new StaticRuleMatcherBuilder(
                                        ruleName,
                                        container,
                                        tuple.Attribute.UsedWordsProviderMethodName,
                                        tuple.Method
                                    ),
                                    Array.Empty<object>()
                                )!
                        );
                    }
                )
                .MapValue(ruleMatcher => (IRuleMatcher) this.m_ruleSpaceFactory.WrapWithCache(ruleMatcher))
                .ToDictionary();
        }

        private static Type DescribeResultType(MethodInfo method, string ruleName)
        {
            Type returnType = method.ReturnType;

            return StaticRuleMatcherBuilder.SwitchByType(
                method,
                () =>
                {
                    const int successArgumentIndex = 0;
                    Type successArgument = returnType.GenericTypeArguments[successArgumentIndex];
                    if (returnType.GenericTypeArguments[successArgumentIndex] != typeof(bool))
                    {
                        throw new RuleBuildException(
                            $"Return type of rule '{ruleName}' is not valid: tuple element '{successArgumentIndex}' " +
                            $"should be 'bool success', '{successArgument.FullName} given instead " +
                            $"(class: {method.DeclaringType!.FullName}, method: {method.Name})."
                        );
                    }

                    const int lastUsedSymbolIndexArgumentIndex = 2;
                    Type lastUsedSymbolIndexArgument =
                        returnType.GenericTypeArguments[lastUsedSymbolIndexArgumentIndex];
                    if (returnType.GenericTypeArguments[lastUsedSymbolIndexArgumentIndex] != typeof(int))
                    {
                        throw new RuleBuildException(
                            $"Return type of rule '{ruleName}' is not valid: tuple element '{lastUsedSymbolIndexArgumentIndex}' " +
                            $"should be 'int lastUsedSymbolIndex', '{lastUsedSymbolIndexArgument.FullName} given instead " +
                            $"(class: {method.DeclaringType!.FullName}, method: {method.Name})."
                        );
                    }

                    return returnType.GenericTypeArguments[1];
                },
                () =>
                {
                    Type itemType = returnType.GetGenericArguments().Single();

                    const int lastUsedSymbolIndexArgumentIndex = 1;
                    Type lastUsedSymbolIndexArgument = itemType.GenericTypeArguments[lastUsedSymbolIndexArgumentIndex];
                    if (itemType.GenericTypeArguments[lastUsedSymbolIndexArgumentIndex] != typeof(int))
                    {
                        throw new RuleBuildException(
                            $"Return type of rule '{ruleName}' is not valid: tuple element '{lastUsedSymbolIndexArgumentIndex}' " +
                            $"should be 'int lastUsedSymbolIndex', '{lastUsedSymbolIndexArgument.FullName} given instead " +
                            $"(class: {method.DeclaringType!.FullName}, method: {method.Name})."
                        );
                    }

                    return itemType.GenericTypeArguments[0];
                },
                ruleName
            );
        }
    }
}