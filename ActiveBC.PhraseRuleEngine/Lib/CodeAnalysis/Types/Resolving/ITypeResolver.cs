using System;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Types.Resolving
{
    public interface ITypeResolver
    {
        Type Resolve(
            ICSharpTypeToken typeDeclaration,
            IReadOnlySet<string> usingNamespaces,
            IAssembliesProvider assembliesProvider
        );
    }
}