using System;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Types.Resolving
{
    // todo [non-realtime performance] caching may be added here
    public sealed class TypeResolver : ITypeResolver
    {
        public Type Resolve(
            ICSharpTypeToken typeDeclaration,
            IReadOnlySet<string> usingNamespaces,
            IAssembliesProvider assembliesProvider
        )
        {
            return typeDeclaration switch
            {
                ResolvedCSharpTypeToken token => token.Type,
                ClassicCSharpTypeToken token => FindClassic(token, usingNamespaces, assembliesProvider),
                TupleCSharpTypeToken token => FindTuple(token, usingNamespaces, assembliesProvider),
                _ => throw new TypeResolverException(
                    $"Unknown {nameof(ICSharpTypeToken)} implementation '{typeDeclaration.GetType().FullName}'"
                ),
            };
        }

        private Type FindClassic(
            ClassicCSharpTypeToken typeDeclaration,
            IReadOnlySet<string> usingNamespaces,
            IAssembliesProvider assembliesProvider
        )
        {
            Type? keywordType = typeDeclaration.TypeDeclaration.Value switch
            {
                "object" => typeof(object),
                "string" => typeof(string),
                "bool" => typeof(bool),
                "byte" => typeof(byte),
                "sbyte" => typeof(sbyte),
                "char" => typeof(char),
                "decimal" => typeof(decimal),
                "double" => typeof(double),
                "float" => typeof(float),
                "int" => typeof(int),
                "uint" => typeof(uint),
                "long" => typeof(long),
                "ulong" => typeof(ulong),
                "short" => typeof(short),
                "ushort" => typeof(ushort),
                "void" => typeof(void),
                _ => null,
            };

            if (keywordType is not null)
            {
                if (typeDeclaration.GenericArguments.Length > 0)
                {
                    throw new TypeResolverException(
                        $"Keyword type {typeDeclaration.TypeDeclaration} doesn't have generic parameters."
                    );
                }

                return keywordType;
            }

            Type? rawType = FindRaw();

            if (rawType is null)
            {
                throw new TypeResolverException(
                    $"Unable to find type '{typeDeclaration}' " +
                    $"(using namespaces: {usingNamespaces.Select(@namespace => $"'{@namespace}'").JoinToString(", ")})."
                );
            }

            if (typeDeclaration.GenericArguments.Length > 0)
            {
                return rawType.MakeGenericType(
                    typeDeclaration
                        .GenericArguments
                        .Select(genericArgumentType => Resolve(genericArgumentType, usingNamespaces, assembliesProvider))
                        .ToArray()
                );
            }

            return rawType;

            Type? FindRaw()
            {
                string systemTypeName = $"{typeDeclaration.TypeDeclaration}" +
                                        $"{(typeDeclaration.GenericArguments.Length > 0 ? $"`{typeDeclaration.GenericArguments.Length}" : "")}";

                bool containsNamespace = typeDeclaration.TypeDeclaration.Value.Contains('.');
                if (containsNamespace)
                {
                    return Type.GetType(systemTypeName) ??
                           throw new TypeResolverException($"Unable to find type '{typeDeclaration}' (without usings).");
                }

                return FindInNamespaces() ?? FindInAssemblies();

                Type? FindInNamespaces()
                {
                    Type? foundType = null;
                    foreach (string usingNamespace in usingNamespaces)
                    {
                        Type? type = Type.GetType($"{usingNamespace}.{systemTypeName}");

                        if (type is not null)
                        {
                            // this is some kind of imitation of CS0104
                            // see https://docs.microsoft.com/en-us/dotnet/csharp/misc/cs0104
                            if (foundType is not null)
                            {
                                throw new TypeResolverException(
                                    $"Type declaration '{typeDeclaration}' is ambiguous " +
                                    $"between {foundType.FullName} " +
                                    $"and {type.FullName}."
                                );
                            }

                            foundType = type;
                        }
                    }

                    return foundType;
                }

                Type? FindInAssemblies()
                {
                    // todo [non-realtime performance] we may intersect using namespaces and namespaces from types
                    return assembliesProvider
                        .GetAssemblies()
                        .SelectMany(assembly => assembly.GetTypes())
                        .Where(type => type.Namespace is null || usingNamespaces.Contains(type.Namespace))
                        .FirstOrDefault(type => type.Name == systemTypeName);
                }
            }
        }

        private Type FindTuple(
            TupleCSharpTypeToken type,
            IReadOnlySet<string> usingNamespaces,
            IAssembliesProvider assembliesProvider
        )
        {
            Type[] possibleTupleDefinitions = new []
            {
                typeof(ValueTuple<>),
                typeof(ValueTuple<,>),
                typeof(ValueTuple<,,>),
                typeof(ValueTuple<,,,>),
                typeof(ValueTuple<,,,,>),
                typeof(ValueTuple<,,,,,>),
                typeof(ValueTuple<,,,,,,>),
                typeof(ValueTuple<,,,,,,,>),
            };

            int itemsCount = type.Items.Length;

            if (itemsCount == 0 || itemsCount > 8)
            {
                throw new TypeResolverException(
                    $"Tuple type declaration '{type}' contains invalid number of items. " +
                    $"Supported items count range is [1-8]."
                );
            }

            Type tupleType = possibleTupleDefinitions[itemsCount - 1];

            return tupleType
                .MakeGenericType(
                    type
                        .Items
                        .Select(pair => Resolve(pair.Type, usingNamespaces, assembliesProvider))
                        .ToArray()
                );
        }
    }
}