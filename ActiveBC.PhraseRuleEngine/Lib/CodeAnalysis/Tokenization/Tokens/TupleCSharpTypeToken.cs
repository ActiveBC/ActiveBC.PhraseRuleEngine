using System.Linq;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens
{
    public sealed class TupleCSharpTypeToken : ICSharpTypeToken
    {
        public CSharpTupleItemToken[] Items { get; }

        public TupleCSharpTypeToken(CSharpTupleItemToken[] items)
        {
            this.Items = items;
        }

        public override string ToString()
        {
            return $"({Items.Select(item => item.ToString()).JoinToString(", ")})";
        }
    }
}