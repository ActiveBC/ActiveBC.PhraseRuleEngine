using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Tests.Helpers
{
    internal sealed class ConstructorDumper
    {
        private readonly int m_indentLevelSize;
        private readonly char m_indentCharacter;
        private readonly int m_rowLengthLimit;

        public ConstructorDumper(int indentLevelSize = 4, char indentCharacter = ' ', int rowLengthLimit = 120)
        {
            this.m_indentLevelSize = indentLevelSize;
            this.m_indentCharacter = indentCharacter;
            this.m_rowLengthLimit = rowLengthLimit;
        }

        public string Dump(object? @object, int indent = 0)
        {
            string indentString = GetIndentString(indent);

            if (@object is null)
            {
                return $"{indentString}null";
            }

            string? knownTypeDump = DumpKnownType(@object, indent);

            if (knownTypeDump is not null)
            {
                return knownTypeDump;
            }

            Type objectType = @object.GetType();

            ConstructorInfo? constructor = objectType.GetConstructors().SingleOrDefault();

            if (constructor is null)
            {
                if (objectType.GetField("Instance", BindingFlags.Static | BindingFlags.Public) is null)
                {
                    throw new Exception($"Cannot dump instance of {objectType.FullName}.");
                }

                return $"{indentString}{objectType.Name}.Instance";
            }

            ParameterInfo[] constructorParameters = constructor.GetParameters();

            object?[] constructorArguments = constructorParameters
                .Select(parameter => parameter.Name!)
                .Select(parameterName => parameterName.Capitalize())
                .Select(propertyName => objectType.GetProperty(propertyName)!.GetValue(@object))
                .ToArray();

            string formattedArgumentsJoinedWithSpace = constructorArguments
                .Select(argument => Dump(argument))
                .JoinToString(", ");

            if (!formattedArgumentsJoinedWithSpace.Contains("\r\n"))
            {
                string formattedConstructorCall = $"{indentString}new {objectType.Name}({formattedArgumentsJoinedWithSpace})";

                if (formattedConstructorCall.Length <= this.m_rowLengthLimit)
                {
                    return formattedConstructorCall;
                }
            }

            return $"{indentString}new {objectType.Name}(\r\n" +
                   $"{constructorArguments.Select(argument => Dump(argument, indent + this.m_indentLevelSize)).JoinToString(",\r\n")}\r\n" +
                   $"{indentString})";
        }

        private string? DumpKnownType(object @object, int indent)
        {
            string indentString = GetIndentString(indent);

            string? keywordTypeDump = @object switch
            {
                string value => value.Contains("\"") || value.Contains("\r\n") ? $"@\"{value.Replace("\"", "\"\"")}\"" : $"\"{value}\"",
                bool value => value ? "true" : "false",
                byte value => $"(byte) {value}",
                sbyte value => $"(sbyte) {value}",
                char value => $"'{value}'",
                decimal value => $"{value}m",
                double value => $"{value}d",
                float value => $"{value}f",
                int value => $"{value}",
                uint value => $"{value}u",
                long value => $"{value}l",
                ulong value => $"{value}ul",
                short value => $"(short) {value}",
                ushort value => $"(ushort) {value}",
                _ => null,
            };

            if (keywordTypeDump is not null)
            {
                return $"{indentString}{keywordTypeDump}";
            }

            if (@object is IEnumerable enumerable)
            {
                Type objectType = enumerable.GetType();

                if (objectType.HasElementType)
                {
                    string[] formattedItems = (from object item in enumerable select Dump(item, indent + this.m_indentLevelSize)).ToArray();

                    if (formattedItems.Length == 0)
                    {
                        return $"{indentString}Array.Empty<{objectType.GetElementType()!.Name}>()";
                    }

                    return $"{indentString}new []\r\n" +
                           $"{indentString}{{\r\n" +
                           $"{formattedItems.JoinToString(",\r\n")}\r\n" +
                           $"{indentString}}}";
                }
            }

            return null;
        }

        private string GetIndentString(int indentSize)
        {
            return new string(this.m_indentCharacter, indentSize);
        }
    }
}