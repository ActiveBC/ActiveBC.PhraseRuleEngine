using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Models
{
    public sealed class FunctionCreationData
    {
        public IReadOnlySet<string> Usings { get; }
        public string ReturnTypeDeclaration { get; }
        public string Name { get; }
        public IEnumerable<VariableCreationData> Parameters { get; }
        public string Body { get; }

        public FunctionCreationData(
            IReadOnlySet<string> usings,
            string returnTypeDeclaration,
            string name,
            IEnumerable<VariableCreationData> parameters,
            string body
        )
        {
            this.Usings = usings;
            this.ReturnTypeDeclaration = returnTypeDeclaration;
            this.Name = name;
            this.Parameters = parameters;
            this.Body = body;
        }
    }
}