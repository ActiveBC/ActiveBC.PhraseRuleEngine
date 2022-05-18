using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Models;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis
{
    public sealed class CodeEmitter
    {
        private readonly string m_assemblyPrefix;
        private readonly string m_codeNamespace;
        private readonly string m_classPrefix;
        private readonly string m_methodPrefix;

        public CodeEmitter(string assemblyPrefix, string codeNamespace, string classPrefix, string methodPrefix)
        {
            this.m_assemblyPrefix = assemblyPrefix;
            this.m_codeNamespace = codeNamespace;
            this.m_classPrefix = classPrefix;
            this.m_methodPrefix = methodPrefix;
        }

        public Func<object?[], object?> CreateFunction(
            FunctionCreationData data,
            IAssembliesProvider assembliesProvider
        )
        {
            const string key = "default";

            IDictionary<string, Func<object?[], object?>> functions = CreateFunctions(
                new Dictionary<string, FunctionCreationData>(1)
                {
                    { key, data }
                },
                assembliesProvider
            );

            return functions[key];
        }

        public IDictionary<string, Func<object?[], object?>> CreateFunctions(
            IReadOnlyDictionary<string, FunctionCreationData> dataByMethodName,
            IAssembliesProvider assembliesProvider
        )
        {
            (Assembly Assembly, AssemblyMetadata Metadata, string ReformattedCode) compilationResult = CompileAssembly(
                dataByMethodName
                    .MapValue(
                        data => CreateNamespace(
                            data.Usings,
                            CreateStaticClass(
                                CreateClassName(data.Name),
                                CreateStaticMethod(
                                    CreateMethodName(data.Name),
                                    data.ReturnTypeDeclaration,
                                    data.Parameters,
                                    data.Body
                                )
                            )
                        )
                    )
                    .SelectValues()
                    .JoinToString("\r\n\r\n\r\n"),
                assembliesProvider.GetMetadataReferences()
            );

            return dataByMethodName
                .MapValue<string, FunctionCreationData, Func<object?[], object?>>(
                    functionCreationData =>
                    {
                        MethodInfo methodInfo = compilationResult
                            .Assembly
                            .GetType($"{this.m_codeNamespace}.{CreateClassName(functionCreationData.Name)}")!
                            .GetMethod(CreateMethodName(functionCreationData.Name))!;

                        return arguments => methodInfo.Invoke(null, arguments)!;
                    }
                )
                .ToDictionaryWithKnownCapacity(dataByMethodName.Count);
        }

        private string CreateClassName(string key)
        {
            return $"{this.m_classPrefix}{key}";
        }

        private string CreateMethodName(string key)
        {
            return $"{this.m_methodPrefix}{key}";
        }

        private string CreateNamespace(IReadOnlySet<string> usingNamespaces, string content)
        {
            return $@"namespace {this.m_codeNamespace}
{{
{usingNamespaces.Select(@namespace => $"    using {@namespace};").JoinToString("\r\n")}

{content}
}}";
        }

        private static string CreateStaticClass(string name, string content)
        {
            return $@"    public static class {name}
    {{
{content}
    }}";
        }

        private static string CreateStaticMethod(string methodName, string returnType, IEnumerable<VariableCreationData> parameters, string body)
        {
            return $@"        public static {returnType} {methodName}({parameters.Select(parameter => $"{parameter.TypeDeclaration} {parameter.Name}").JoinToString(", ")})
        {body}";
        }

        private (Assembly Assembly, AssemblyMetadata Metadata, string ReformattedCode) CompileAssembly(
            string code,
            IEnumerable<MetadataReference> metadataReferences
        )
        {
            string reformattedCode = Reformat(code);

            SyntaxTree syntaxTree = Parse(reformattedCode);

            CSharpCompilation compilation = CSharpCompilation.Create(
                $"{this.m_assemblyPrefix}_{Guid.NewGuid():N}",
                new [] { syntaxTree },
                metadataReferences,
                new CSharpCompilationOptions(
                    OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release
                )
            );

            using MemoryStream stream = new MemoryStream();
            using MemoryStream streamPdb = new MemoryStream();

            EmitResult result = compilation.Emit(
                stream,
                streamPdb,
                options: new EmitOptions(debugInformationFormat: DebugInformationFormat.PortablePdb)
            );

            if (!result.Success)
            {
                List<Diagnostic> failures = result
                    .Diagnostics
                    .Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error)
                    .ToList();

                throw new CodeEmitException("Cannot create assembly", reformattedCode, failures);
            }

            stream.Seek(0, SeekOrigin.Begin);
            streamPdb.Seek(0, SeekOrigin.Begin);

            byte[] rawAssembly = stream.ToArray();
            byte[] rawPdb = streamPdb.ToArray();

            Assembly compiledAssembly = Assembly.Load(rawAssembly, rawPdb);

            AssemblyMetadata assemblyMetadata = AssemblyMetadata.CreateFromImage(ImmutableArray.Create(rawAssembly));

            return (compiledAssembly, assemblyMetadata, reformattedCode);
        }

        private static SyntaxTree Parse(string code)
        {
            return CSharpSyntaxTree.ParseText(
                code,
                new CSharpParseOptions(LanguageVersion.Latest, DocumentationMode.None)
            );
        }

        private string Reformat(string code)
        {
            return Parse(code).GetRoot().NormalizeWhitespace().ToFullString();
        }
    }
}