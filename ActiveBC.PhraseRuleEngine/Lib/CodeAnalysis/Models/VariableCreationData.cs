namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Models
{
    public sealed class VariableCreationData
    {
        public string Name { get; }
        public string TypeDeclaration { get; }

        public VariableCreationData(string name, string typeDeclaration)
        {
            this.Name = name;
            this.TypeDeclaration = typeDeclaration;
        }
    }
}