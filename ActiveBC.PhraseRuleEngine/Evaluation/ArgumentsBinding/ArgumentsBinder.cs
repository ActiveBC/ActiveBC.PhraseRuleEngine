using System;
using System.Linq;
using System.Reflection;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Parameters;
using ActiveBC.PhraseRuleEngine.Exceptions;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Evaluation.ArgumentsBinding
{
    public static class ArgumentsBinder
    {
        public static RuleArguments BindRuleArguments(
            RuleParameters ruleParameters,
            RuleSpaceArguments ruleSpaceArguments,
            IRuleArgumentToken[] argumentBindings
        )
        {
            return new RuleArguments(
                ruleParameters
                    .Values
                    .MapValue(
                        (index, _, parameterType) =>
                        {
                            if (index < argumentBindings.Length)
                            {
                                IRuleArgumentToken argument = argumentBindings[index];

                                return argument switch
                                {
                                    RuleDefaultArgumentToken => GetDefaultValue(parameterType),
                                    RuleChainedMemberAccessArgumentToken binding => GetFromRuleSpace(binding, ruleSpaceArguments),
                                    _ => throw new ArgumentOutOfRangeException(nameof(argument))
                                };
                            }

                            return GetDefaultValue(parameterType);
                        }
                    )
                    .ToDictionaryWithKnownCapacity(ruleParameters.Values.Count)
            );
        }

        private static object? GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        private static object? GetFromRuleSpace(RuleChainedMemberAccessArgumentToken binding, RuleSpaceArguments ruleSpaceArguments)
        {
            string formattedCallChain = binding.CallChain.JoinToString(".");

            string rootObjectName = binding.CallChain.First();
            if (!ruleSpaceArguments.Values.TryGetValue(rootObjectName, out object? rootObject))
            {
                throw new RuleMatchException(
                    $"Object '{rootObjectName}' is not part of rule space arguments " +
                    $"(call chain: {formattedCallChain})."
                );
            }

            object? targetObject = rootObject;
            string targetObjectName = rootObjectName;
            foreach (string memberName in binding.CallChain.Skip(1))
            {
                targetObject = GetFromObject(targetObject, targetObjectName, memberName, formattedCallChain);
                targetObjectName = memberName;
            }

            return targetObject;
        }

        private static object? GetFromObject(object? instance, string instanceName, string memberName, string formattedCallChain)
        {
            if (instance is null)
            {
                throw new RuleMatchException(
                    $"Object '{instance}' is null " +
                    $"and cannot be used as a source of {nameof(RuleChainedMemberAccessArgumentToken)} binding " +
                    $"(call chain: {formattedCallChain})."
                );
            }

            PropertyInfo? property = instance.GetType().GetProperty(memberName);

            if (property is null)
            {
                throw new RuleMatchException(
                    $"Object '{instanceName}' does not contain property {memberName} " +
                    $"(call chain: {formattedCallChain})."
                );
            }

            return property.GetValue(instance);
        }
    }
}