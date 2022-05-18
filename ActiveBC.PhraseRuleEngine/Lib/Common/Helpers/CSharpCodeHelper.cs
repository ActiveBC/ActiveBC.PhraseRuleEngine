using System;
using System.Collections.Generic;
using System.Reflection;

namespace ActiveBC.PhraseRuleEngine.Lib.Common.Helpers
{
    public static class CSharpCodeHelper
    {
        public static (object List, Action<object?> AddMethod) CreateGenericList(Type elementType)
        {
            Type targetListType = typeof(List<>).MakeGenericType(elementType);

            object targetList = Activator.CreateInstance(targetListType)!;

            MethodInfo addMethod = targetListType.GetMethod("Add")!;

            return (targetList, item => addMethod.Invoke(targetList, new []{item}));
        }
    }
}