using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters;
using ActiveBC.PhraseRuleEngine.Exceptions;

namespace ActiveBC.PhraseRuleEngine.Build.Rule.Static
{
    internal sealed class StaticRuleMatcherBuilder
    {
        private readonly string m_ruleName;
        private readonly Type m_container;
        private readonly string m_usedWordsProviderMethodName;
        private readonly MethodInfo m_method;

        public StaticRuleMatcherBuilder(string ruleName, Type container, string usedWordsProviderMethodName, MethodInfo method)
        {
            this.m_ruleName = ruleName;
            this.m_container = container;
            this.m_usedWordsProviderMethodName = usedWordsProviderMethodName;
            this.m_method = method;
        }

        public IRuleMatcher Build<TResult>()
        {
            return new StaticRuleMatcher<TResult>(
                FindUsedWordsProvider(),
                CreateRuleEvaluator<TResult>(),
                DescribeRuleParameters()
            );
        }

        public static TReturn SwitchByType<TReturn>(
            MethodInfo method,
            Func<TReturn> single,
            Func<TReturn> multiple,
            string ruleName
        )
        {
            if (method.ReturnType.GetGenericTypeDefinition() == typeof(ValueTuple<,,>))
            {
                return single();
            }

            if (method.ReturnType.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
                method.ReturnType.GetGenericArguments().Single().GetGenericTypeDefinition() == typeof(ValueTuple<,>)
            )
            {
                return multiple();
            }

            throw new RuleBuildException(
                $"Return type of rule '{ruleName}' is not valid. " +
                $"Valid types are: " +
                $"'{typeof(ValueTuple<,,>).FullName}', " +
                $"'{typeof(IEnumerable<>).FullName}'. " +
                $"Value of type '{method.ReturnType.FullName}' given instead " +
                $"(class: {method.DeclaringType!.FullName}, method: {method.Name})."
            );
        }

        private static TReturn SwitchByType<TReturn, TResult>(
            MethodInfo method,
            Func<TReturn> single,
            Func<TReturn> multiple,
            string ruleName
        )
        {
            if (method.ReturnType == typeof(ValueTuple<bool, TResult, int>))
            {
                return single();
            }

            if (method.ReturnType == typeof(IEnumerable<ValueTuple<TResult, int>>))
            {
                return multiple();
            }

            throw new RuleBuildException(
                $"Return type of rule '{ruleName}' is not valid. " +
                $"Valid types are: " +
                $"'{typeof(ValueTuple<bool, TResult, int>).FullName}', " +
                $"'{typeof(IEnumerable<ValueTuple<TResult, int>>).FullName}'. " +
                $"Value of type '{method.ReturnType.FullName}' given instead " +
                $"(class: {method.DeclaringType!.FullName}, method: {method.Name})."
            );
        }

        private Func<IEnumerable<string>> FindUsedWordsProvider()
        {
            MethodInfo? methodInfo = this.m_container.GetMethod(
                this.m_usedWordsProviderMethodName,
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
            );

            if (methodInfo is null)
            {
                throw new RuleBuildException(
                    $"Used words provider for static rule '{this.m_ruleName}' not found: " +
                    $"function '{this.m_container.FullName}.{this.m_usedWordsProviderMethodName}' does not exist."
                );
            }

            if (methodInfo.GetParameters().Length > 0)
            {
                throw new RuleBuildException(
                    $"Used words provider for static rule '{this.m_ruleName}' is not valid: " +
                    $"function '{this.m_container.FullName}.{this.m_usedWordsProviderMethodName}' should be parameterless."
                );
            }

            if (methodInfo.ReturnType != typeof(IEnumerable<string>))
            {
                throw new RuleBuildException(
                    $"Used words provider for static rule '{this.m_ruleName}' is not valid: " +
                    $"function '{this.m_container.FullName}.{this.m_usedWordsProviderMethodName}' should return instance of {nameof(IEnumerable<string>)}."
                );
            }

            return () => (IEnumerable<string>) methodInfo.Invoke(null, Array.Empty<object>())!;
        }

        private Func<object?[], IEnumerable<(TResult Result, int LastUsedSymbolIndex)>> CreateRuleEvaluator<TResult>()
        {
            return SwitchByType<Func<object?[], IEnumerable<(TResult Result, int LastUsedSymbolIndex)>>, TResult>(
                this.m_method,
                () => arguments => WrapSingleResultMethod<TResult>(this.m_method, arguments),
                () => arguments => (IEnumerable<ValueTuple<TResult, int>>) this.m_method.Invoke(null, arguments)!,
                this.m_ruleName
            );
        }

        private static IEnumerable<(TResult Result, int LastUsedSymbolIndex)> WrapSingleResultMethod<TResult>(MethodInfo method, object?[] arguments)
        {
            (bool Success, TResult Result, int LastUsedSymbolIndex) result = ((bool, TResult, int)) method.Invoke(null, arguments)!;

            if (result.Success)
            {
                yield return (result.Result, result.LastUsedSymbolIndex);
            }
        }

        private RuleParameters DescribeRuleParameters()
        {
            if (!this.m_method.IsStatic)
            {
                throw new RuleBuildException(
                    $"Static rule '{this.m_ruleName}' can only be built from static method " +
                    $"(class: {this.m_method.DeclaringType!.FullName}, method: {this.m_method.Name})."
                );
            }

            ParameterInfo[] parameters = this.m_method.GetParameters();

            const int constantParametersCount = 2;
            if (parameters.Length < constantParametersCount)
            {
                throw new RuleBuildException(
                    $"Source method of rule '{this.m_ruleName}' should contain " +
                    $"at least two parameters 'string[] sequence' and 'int startIndex' " +
                    $"(class: {this.m_method.DeclaringType!.FullName}, method: {this.m_method.Name})."
                );
            }

            const int sequenceParameterIndex = 0;
            ParameterInfo sequenceParameter = parameters[sequenceParameterIndex];

            if (sequenceParameter.ParameterType != typeof(string[]))
            {
                throw new RuleBuildException(
                    $"Parameter '{sequenceParameterIndex}' of the source method of rule '{this.m_ruleName}' " +
                    $"should be 'string[] sequence', " +
                    $"'{sequenceParameter.ParameterType.FullName} {sequenceParameter.Name}' given instead " +
                    $"(class: {this.m_method.DeclaringType!.FullName}, method: {this.m_method.Name})."
                );
            }

            const int startIndexParameterIndex = 1;
            ParameterInfo startIndexParameter = parameters[startIndexParameterIndex];

            if (startIndexParameter.ParameterType != typeof(int))
            {
                throw new RuleBuildException(
                    $"Parameter '{startIndexParameterIndex}' of the source method of rule '{this.m_ruleName}' " +
                    $"should be 'int startIndex', " +
                    $"'{startIndexParameter.ParameterType.FullName} {startIndexParameter.Name}' given instead " +
                    $"(class: {this.m_method.DeclaringType!.FullName}, method: {this.m_method.Name})."
                );
            }

            return new RuleParameters(
                parameters
                    .Skip(constantParametersCount)
                    .ToDictionary(parameter => parameter.Name!, parameter => parameter.ParameterType)
            );
        }
    }
}