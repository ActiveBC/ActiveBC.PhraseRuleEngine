using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Tokens;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Lib.CodeAnalysis.Tokenization.Equality
{
    public sealed class CSharpTypeTokenEqualityComparer : IEqualityComparer<ICSharpTypeToken>, IEqualityComparer
    {
        public static readonly CSharpTypeTokenEqualityComparer Instance = new CSharpTypeTokenEqualityComparer();

        private CSharpTypeTokenEqualityComparer()
        {
        }

        bool IEqualityComparer.Equals(object? x, object? y)
        {
            return Equals(x as ICSharpTypeToken, y as ICSharpTypeToken);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return obj is ICSharpTypeToken token ? GetHashCode(token) : 0;
        }

        public bool Equals(ICSharpTypeToken? x, ICSharpTypeToken? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return x switch
            {
                ClassicCSharpTypeToken token => ClassicCSharpTypeTokenEqualityComparer.Instance.Equals(token, y as ClassicCSharpTypeToken),
                ResolvedCSharpTypeToken token => ResolvedCSharpTypeTokenEqualityComparer.Instance.Equals(token, y as ResolvedCSharpTypeToken),
                TupleCSharpTypeToken token => TupleCSharpTypeTokenEqualityComparer.Instance.Equals(token, y as TupleCSharpTypeToken),
                _ => throw new ArgumentOutOfRangeException(nameof(x)),
            };
        }

        public int GetHashCode(ICSharpTypeToken obj)
        {
            return obj switch
            {
                ClassicCSharpTypeToken token => ClassicCSharpTypeTokenEqualityComparer.Instance.GetHashCode(token),
                ResolvedCSharpTypeToken token => ResolvedCSharpTypeTokenEqualityComparer.Instance.GetHashCode(token),
                TupleCSharpTypeToken token => TupleCSharpTypeTokenEqualityComparer.Instance.GetHashCode(token),
                _ => throw new ArgumentOutOfRangeException(nameof(obj)),
            };
        }

        private sealed class ResolvedCSharpTypeTokenEqualityComparer : IEqualityComparer<ResolvedCSharpTypeToken>
        {
            public static readonly ResolvedCSharpTypeTokenEqualityComparer Instance = new ResolvedCSharpTypeTokenEqualityComparer();

            private ResolvedCSharpTypeTokenEqualityComparer()
            {
            }

            public bool Equals(ResolvedCSharpTypeToken? x, ResolvedCSharpTypeToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.Type == y.Type;
            }

            public int GetHashCode(ResolvedCSharpTypeToken obj)
            {
                return obj.Type.GetHashCode();
            }
        }

        private sealed class ClassicCSharpTypeTokenEqualityComparer : IEqualityComparer<ClassicCSharpTypeToken>
        {
            public static readonly ClassicCSharpTypeTokenEqualityComparer Instance = new ClassicCSharpTypeTokenEqualityComparer();

            private ClassicCSharpTypeTokenEqualityComparer()
            {
            }

            public bool Equals(ClassicCSharpTypeToken? x, ClassicCSharpTypeToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return CSharpTypeNameWithNamespaceTokenEqualityComparer.Instance.Equals(x.TypeDeclaration, y.TypeDeclaration) &&
                       x.GenericArguments.SequenceEqual(y.GenericArguments, CSharpTypeTokenEqualityComparer.Instance);
            }

            public int GetHashCode(ClassicCSharpTypeToken obj)
            {
                return (obj.TypeDeclaration.GetHashCode() * 397) ^ obj.GenericArguments.GetSequenceHashCode(CSharpTypeTokenEqualityComparer.Instance);
            }
        }

        private sealed class CSharpTypeNameWithNamespaceTokenEqualityComparer : IEqualityComparer<CSharpTypeNameWithNamespaceToken>
        {
            public static readonly CSharpTypeNameWithNamespaceTokenEqualityComparer Instance = new CSharpTypeNameWithNamespaceTokenEqualityComparer();

            private CSharpTypeNameWithNamespaceTokenEqualityComparer()
            {
            }

            public bool Equals(CSharpTypeNameWithNamespaceToken? x, CSharpTypeNameWithNamespaceToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.Value == y.Value;
            }

            public int GetHashCode(CSharpTypeNameWithNamespaceToken obj)
            {
                return obj.Value.GetHashCode();
            }
        }

        private sealed class TupleCSharpTypeTokenEqualityComparer : IEqualityComparer<TupleCSharpTypeToken>
        {
            public static readonly TupleCSharpTypeTokenEqualityComparer Instance = new TupleCSharpTypeTokenEqualityComparer();

            private TupleCSharpTypeTokenEqualityComparer()
            {
            }

            public bool Equals(TupleCSharpTypeToken? x, TupleCSharpTypeToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.Items.SequenceEqual(y.Items, CSharpTupleItemTokenEqualityComparer.Instance);
            }

            public int GetHashCode(TupleCSharpTypeToken obj)
            {
                return obj.Items.GetSequenceHashCode(CSharpTupleItemTokenEqualityComparer.Instance);
            }
        }

        private sealed class CSharpTupleItemTokenEqualityComparer : IEqualityComparer<CSharpTupleItemToken>, IEqualityComparer
        {
            public static readonly CSharpTupleItemTokenEqualityComparer Instance = new CSharpTupleItemTokenEqualityComparer();

            private CSharpTupleItemTokenEqualityComparer()
            {
            }

            bool IEqualityComparer.Equals(object? x, object? y)
            {
                return Equals(x as CSharpTupleItemToken, y as CSharpTupleItemToken);
            }

            int IEqualityComparer.GetHashCode(object obj)
            {
                return obj is CSharpTupleItemToken token ? GetHashCode(token) : 0;
            }

            public bool Equals(CSharpTupleItemToken? x, CSharpTupleItemToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return CSharpTypeTokenEqualityComparer.Instance.Equals(x.Type, y.Type) &&
                       CSharpIdentifierTokenEqualityComparer.Instance.Equals(x.PropertyName!, y.PropertyName!);
            }

            public int GetHashCode(CSharpTupleItemToken obj)
            {
                return (obj.Type.GetHashCode() * 397) ^ (obj.PropertyName?.GetHashCode() ?? 0);
            }
        }

        private sealed class CSharpIdentifierTokenEqualityComparer : IEqualityComparer<CSharpIdentifierToken>
        {
            public static readonly CSharpIdentifierTokenEqualityComparer Instance = new CSharpIdentifierTokenEqualityComparer();

            private CSharpIdentifierTokenEqualityComparer()
            {
            }

            public bool Equals(CSharpIdentifierToken? x, CSharpIdentifierToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.Value == y.Value;
            }

            public int GetHashCode(CSharpIdentifierToken obj)
            {
                return obj.Value.GetHashCode();
            }
        }
    }
}