using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Assemblies
{
    public sealed class AssembliesProvider : IAssembliesProvider
    {
        private readonly IReadOnlyCollection<Assembly> m_assemblies;
        private readonly IReadOnlyCollection<MetadataReference> m_metadataReferences;

        public AssembliesProvider(IReadOnlyCollection<Assembly> assemblies, IReadOnlyCollection<MetadataReference> metadataReferences)
        {
            this.m_assemblies = assemblies;
            this.m_metadataReferences = metadataReferences;
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            return this.m_assemblies;
        }

        public IEnumerable<MetadataReference> GetMetadataReferences()
        {
            return this.m_metadataReferences;
        }
    }
}