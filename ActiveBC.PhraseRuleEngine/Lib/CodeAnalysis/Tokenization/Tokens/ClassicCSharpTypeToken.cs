using System.Linq;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens
{
    public sealed class ClassicCSharpTypeToken : ICSharpTypeToken
    {
        public CSharpTypeNameWithNamespaceToken TypeDeclaration { get; }
        public ICSharpTypeToken[] GenericArguments { get; }

        public ClassicCSharpTypeToken(
            CSharpTypeNameWithNamespaceToken typeDeclaration,
            ICSharpTypeToken[] genericArguments
        )
        {
            this.TypeDeclaration = typeDeclaration;
            this.GenericArguments = genericArguments;
        }

        public override string ToString()
        {
            return $"{this.TypeDeclaration}{FormatGenericArguments()}";

            string FormatGenericArguments()
            {
                return this.GenericArguments.Length > 0
                    ? $"<{this.GenericArguments.Select(type => type.ToString()).JoinToString(", ")}>"
                    : "";
            }
        }
    }
}