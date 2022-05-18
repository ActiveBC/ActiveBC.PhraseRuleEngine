using System;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens
{
    public sealed class ResolvedCSharpTypeToken : ICSharpTypeToken
    {
        public string TypeDeclaration { get; }
        public Type Type { get; }

        public ResolvedCSharpTypeToken(string typeDeclaration, Type type)
        {
            this.TypeDeclaration = typeDeclaration;
            this.Type = type;
        }

        public override string ToString()
        {
            return this.TypeDeclaration;
        }
    }
}