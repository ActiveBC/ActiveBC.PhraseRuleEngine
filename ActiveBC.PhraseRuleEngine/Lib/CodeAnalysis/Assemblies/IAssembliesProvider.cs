using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies
{
    /// <summary>
    /// A class that represents a bag for the assemblies that should be used during code compilation and type resolving.
    /// </summary>
    /// <remarks>
    /// Take care of the consistency between <see cref="GetAssemblies"/> and <see cref="GetMetadataReferences"/> methods
    /// when implementing this interface.
    /// </remarks>
    // todo [refactoring] consider using approach described here: https://github.com/jaredpar/basic-reference-assemblies
    public interface IAssembliesProvider
    {
        IEnumerable<Assembly> GetAssemblies();
        IEnumerable<MetadataReference> GetMetadataReferences();
    }
}