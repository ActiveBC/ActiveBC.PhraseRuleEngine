using System;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Models;
using NUnit.Framework;

namespace ActiveBC.PhraseRuleEngine.Tests.Lib.CodeAnalysis
{
    [TestFixture(TestOf = typeof(CodeEmitter))]
    internal sealed class CodeEmitterTests
    {
        private CodeEmitter? m_codeEmitter;

        [SetUp]
        public void SetUp()
        {
            this.m_codeEmitter = new CodeEmitter("foo_", "ActiveBC.Foo", "bar_", "faz_");
        }

        [Test]
        [TestCaseSource(nameof(CreatesFunction_Mixed))]
        public void CreatesFunction(
            string returnTypeDeclaration,
            string body,
            IEnumerable<VariableCreationData> parameters,
            object?[] arguments,
            object? expectedResult
        )
        {
            Func<object?[], object?> function = this.m_codeEmitter!.CreateFunction(
                new FunctionCreationData(
                    new HashSet<string>()
                    {
                        "System.Linq"
                    },
                    returnTypeDeclaration,
                    "Function",
                    parameters,
                    body
                ),
                new LoadedAssembliesProvider()
            );

            object? result = function.Invoke(arguments);

            Assert.AreEqual(expectedResult, result);
        }

        public static object?[][] CreatesFunction_Mixed()
        {
            return new[]
            {
                new object?[]
                {
                    "string",
                    @" { return new string(word.ToCharArray().Reverse().ToArray()); }",
                    new []
                    {
                        new VariableCreationData("word", "string"),
                    },
                    new []
                    {
                        "слово",
                    },
                    "оволс"
                }
            };
        }
    }
}