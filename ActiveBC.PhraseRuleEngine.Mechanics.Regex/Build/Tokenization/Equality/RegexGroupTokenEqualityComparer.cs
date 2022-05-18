using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Equality;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Build.Tokenization.Equality
{
    internal sealed class RegexGroupTokenEqualityComparer : IEqualityComparer<RegexGroupToken>
    {
        public static readonly RegexGroupTokenEqualityComparer Instance = new RegexGroupTokenEqualityComparer();

        private RegexGroupTokenEqualityComparer()
        {
        }

        public bool Equals(RegexGroupToken? x, RegexGroupToken? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Branches.SequenceEqual(y.Branches, BranchTokenEqualityComparer.Instance);
        }

        public int GetHashCode(RegexGroupToken obj)
        {
            return obj.Branches.GetSequenceHashCode(BranchTokenEqualityComparer.Instance);
        }

        private sealed class BranchTokenEqualityComparer : IEqualityComparer<BranchToken>, IEqualityComparer
        {
            public static readonly BranchTokenEqualityComparer Instance = new BranchTokenEqualityComparer();

            private BranchTokenEqualityComparer()
            {
            }

            bool IEqualityComparer.Equals(object? x, object? y)
            {
                return Equals(x as BranchToken, y as BranchToken);
            }

            int IEqualityComparer.GetHashCode(object obj)
            {
                return obj is BranchToken token ? GetHashCode(token) : 0;
            }

            public bool Equals(BranchToken? x, BranchToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.Items.SequenceEqual(y.Items, BranchItemTokenEqualityComparer.Instance);
            }

            public int GetHashCode(BranchToken obj)
            {
                return obj.Items.GetSequenceHashCode(BranchItemTokenEqualityComparer.Instance);
            }
        }

        private sealed class BranchItemTokenEqualityComparer : IEqualityComparer<IBranchItemToken>, IEqualityComparer
        {
            public static readonly BranchItemTokenEqualityComparer Instance = new BranchItemTokenEqualityComparer();

            private BranchItemTokenEqualityComparer()
            {
            }

            bool IEqualityComparer.Equals(object? x, object? y)
            {
                return Equals(x as IBranchItemToken, y as IBranchItemToken);
            }

            int IEqualityComparer.GetHashCode(object obj)
            {
                return obj is IBranchItemToken token ? GetHashCode(token) : 0;
            }

            public bool Equals(IBranchItemToken? x, IBranchItemToken? y)
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
                    QuantifiableBranchItemToken token => QuantifiableBranchItemTokenEqualityComparer.Instance.Equals(token, y as QuantifiableBranchItemToken),
                    MarkerToken token => MarkerTokenEqualityComparer.Instance.Equals(token, y as MarkerToken),
                    _ => throw new ArgumentOutOfRangeException(nameof(x)),
                };
            }

            public int GetHashCode(IBranchItemToken obj)
            {
                return obj switch
                {
                    QuantifiableBranchItemToken token => QuantifiableBranchItemTokenEqualityComparer.Instance.GetHashCode(token),
                    MarkerToken token => MarkerTokenEqualityComparer.Instance.GetHashCode(token),
                    _ => throw new ArgumentOutOfRangeException(nameof(obj)),
                };
            }
        }

        private sealed class QuantifiableBranchItemTokenEqualityComparer : IEqualityComparer<QuantifiableBranchItemToken>
        {
            public static readonly QuantifiableBranchItemTokenEqualityComparer Instance = new QuantifiableBranchItemTokenEqualityComparer();

            private QuantifiableBranchItemTokenEqualityComparer()
            {
            }

            public bool Equals(QuantifiableBranchItemToken? x, QuantifiableBranchItemToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return QuantifiableTokenEqualityComparer.Instance.Equals(x.Quantifiable, y.Quantifiable) &&
                       QuantifierTokenEqualityComparer.Instance.Equals(x.Quantifier, y.Quantifier);
            }

            public int GetHashCode(QuantifiableBranchItemToken obj)
            {
                int hashCode = QuantifiableTokenEqualityComparer.Instance.GetHashCode(obj.Quantifiable);
                hashCode = (hashCode * 397) ^ QuantifierTokenEqualityComparer.Instance.GetHashCode(obj.Quantifier);
                return hashCode;
            }
        }

        private sealed class MarkerTokenEqualityComparer : IEqualityComparer<MarkerToken>
        {
            public static readonly MarkerTokenEqualityComparer Instance = new MarkerTokenEqualityComparer();

            private MarkerTokenEqualityComparer()
            {
            }

            public bool Equals(MarkerToken? x, MarkerToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.Marker == y.Marker;
            }

            public int GetHashCode(MarkerToken obj)
            {
                return obj.Marker.GetHashCode();
            }
        }

        private sealed class QuantifiableTokenEqualityComparer : IEqualityComparer<IQuantifiableToken>
        {
            public static QuantifiableTokenEqualityComparer Instance = new QuantifiableTokenEqualityComparer();

            private QuantifiableTokenEqualityComparer()
            {
            }

            public bool Equals(IQuantifiableToken? x, IQuantifiableToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                if (x.GetType() != y.GetType())
                {
                    return false;
                }

                return x switch
                {
                    AnyLiteralToken token => AnyLiteralTokenEqualityComparer.Instance.Equals(token, y as AnyLiteralToken),
                    LiteralSetToken token => LiteralSetTokenEqualityComparer.Instance.Equals(token, y as LiteralSetToken),
                    LiteralToken token => LiteralTokenEqualityComparer.Instance.Equals(token, y as LiteralToken),
                    PrefixToken token => PrefixTokenEqualityComparer.Instance.Equals(token, y as PrefixToken),
                    InfixToken token => InfixTokenEqualityComparer.Instance.Equals(token, y as InfixToken),
                    SuffixToken token => SuffixTokenEqualityComparer.Instance.Equals(token, y as SuffixToken),
                    RuleReferenceToken token => RuleReferenceTokenEqualityComparer.Instance.Equals(token, y as RuleReferenceToken),
                    RegexGroupToken token => RegexGroupTokenEqualityComparer.Instance.Equals(token, y as RegexGroupToken),
                    NerToken token => NerTokenEqualityComparer.Instance.Equals(token, y as NerToken),
                    _ => throw new ArgumentOutOfRangeException(nameof(x)),
                };
            }

            public int GetHashCode(IQuantifiableToken obj)
            {
                return obj switch
                {
                    AnyLiteralToken token => AnyLiteralTokenEqualityComparer.Instance.GetHashCode(token),
                    LiteralSetToken token => LiteralSetTokenEqualityComparer.Instance.GetHashCode(token),
                    LiteralToken token => LiteralTokenEqualityComparer.Instance.GetHashCode(token),
                    PrefixToken token => PrefixTokenEqualityComparer.Instance.GetHashCode(token),
                    InfixToken token => InfixTokenEqualityComparer.Instance.GetHashCode(token),
                    SuffixToken token => SuffixTokenEqualityComparer.Instance.GetHashCode(token),
                    RuleReferenceToken token => RuleReferenceTokenEqualityComparer.Instance.GetHashCode(token),
                    RegexGroupToken token => RegexGroupTokenEqualityComparer.Instance.GetHashCode(token),
                    NerToken token => NerTokenEqualityComparer.Instance.GetHashCode(token),
                    _ => throw new ArgumentOutOfRangeException(nameof(obj)),
                };
            }
        }

        private sealed class AnyLiteralTokenEqualityComparer : IEqualityComparer<AnyLiteralToken>
        {
            public static readonly AnyLiteralTokenEqualityComparer Instance = new AnyLiteralTokenEqualityComparer();

            private AnyLiteralTokenEqualityComparer()
            {
            }

            public bool Equals(AnyLiteralToken? x, AnyLiteralToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return true;
            }

            public int GetHashCode(AnyLiteralToken obj)
            {
                return obj.GetType().GetHashCode();
            }
        }

        private sealed class LiteralSetTokenEqualityComparer : IEqualityComparer<LiteralSetToken>
        {
            public static readonly LiteralSetTokenEqualityComparer Instance = new LiteralSetTokenEqualityComparer();

            private LiteralSetTokenEqualityComparer()
            {
            }

            public bool Equals(LiteralSetToken? x, LiteralSetToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.IsNegative == y.IsNegative && x.Members.SequenceEqual(y.Members, LiteralSetMemberTokenEqualityComparer.Instance);
            }

            public int GetHashCode(LiteralSetToken obj)
            {
                return (obj.IsNegative.GetHashCode() * 397) ^ obj.Members.GetSequenceHashCode(LiteralSetMemberTokenEqualityComparer.Instance);
            }
        }

        private sealed class LiteralSetMemberTokenEqualityComparer : IEqualityComparer<ILiteralSetMemberToken>, IEqualityComparer
        {
            public static LiteralSetMemberTokenEqualityComparer Instance = new LiteralSetMemberTokenEqualityComparer();

            private LiteralSetMemberTokenEqualityComparer()
            {
            }

            bool IEqualityComparer.Equals(object? x, object? y)
            {
                return Equals(x as ILiteralSetMemberToken, y as ILiteralSetMemberToken);
            }

            int IEqualityComparer.GetHashCode(object obj)
            {
                return obj is ILiteralSetMemberToken token ? GetHashCode(token) : 0;
            }

            public bool Equals(ILiteralSetMemberToken? x, ILiteralSetMemberToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                if (x.GetType() != y.GetType())
                {
                    return false;
                }

                return x switch
                {
                    LiteralToken token => LiteralTokenEqualityComparer.Instance.Equals(token, y as LiteralToken),
                    PrefixToken token => PrefixTokenEqualityComparer.Instance.Equals(token, y as PrefixToken),
                    InfixToken token => InfixTokenEqualityComparer.Instance.Equals(token, y as InfixToken),
                    SuffixToken token => SuffixTokenEqualityComparer.Instance.Equals(token, y as SuffixToken),
                    _ => throw new ArgumentOutOfRangeException(nameof(x)),
                };
            }

            public int GetHashCode(ILiteralSetMemberToken obj)
            {
                return obj switch
                {
                    LiteralToken token => LiteralTokenEqualityComparer.Instance.GetHashCode(token),
                    PrefixToken token => PrefixTokenEqualityComparer.Instance.GetHashCode(token),
                    InfixToken token => InfixTokenEqualityComparer.Instance.GetHashCode(token),
                    SuffixToken token => SuffixTokenEqualityComparer.Instance.GetHashCode(token),
                    _ => throw new ArgumentOutOfRangeException(nameof(obj)),
                };
            }
        }

        private sealed class LiteralTokenEqualityComparer : IEqualityComparer<LiteralToken>
        {
            public static readonly LiteralTokenEqualityComparer Instance = new LiteralTokenEqualityComparer();

            private LiteralTokenEqualityComparer()
            {
            }

            public bool Equals(LiteralToken? x, LiteralToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.Literal == y.Literal;
            }

            public int GetHashCode(LiteralToken obj)
            {
                return obj.Literal.GetHashCode();
            }
        }

        private sealed class PrefixTokenEqualityComparer : IEqualityComparer<PrefixToken>
        {
            public static readonly PrefixTokenEqualityComparer Instance = new PrefixTokenEqualityComparer();

            private PrefixTokenEqualityComparer()
            {
            }

            public bool Equals(PrefixToken? x, PrefixToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.Prefix == y.Prefix;
            }

            public int GetHashCode(PrefixToken obj)
            {
                return obj.Prefix.GetHashCode();
            }
        }

        private sealed class InfixTokenEqualityComparer : IEqualityComparer<InfixToken>
        {
            public static readonly InfixTokenEqualityComparer Instance = new InfixTokenEqualityComparer();

            private InfixTokenEqualityComparer()
            {
            }

            public bool Equals(InfixToken? x, InfixToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.Infix == y.Infix;
            }

            public int GetHashCode(InfixToken obj)
            {
                return obj.Infix.GetHashCode();
            }
        }

        private sealed class SuffixTokenEqualityComparer : IEqualityComparer<SuffixToken>
        {
            public static readonly SuffixTokenEqualityComparer Instance = new SuffixTokenEqualityComparer();

            private SuffixTokenEqualityComparer()
            {
            }

            public bool Equals(SuffixToken? x, SuffixToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.Suffix == y.Suffix;
            }

            public int GetHashCode(SuffixToken obj)
            {
                return obj.Suffix.GetHashCode();
            }
        }

        private sealed class RuleReferenceTokenEqualityComparer : IEqualityComparer<RuleReferenceToken>
        {
            public static readonly RuleReferenceTokenEqualityComparer Instance = new RuleReferenceTokenEqualityComparer();

            private RuleReferenceTokenEqualityComparer()
            {
            }

            public bool Equals(RuleReferenceToken? x, RuleReferenceToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.RuleName == y.RuleName &&
                       x.DeclaredInNamespace == y.DeclaredInNamespace &&
                       x.Arguments.SequenceEqual(y.Arguments, RuleArgumentTokenEqualityComparer.Instance);
            }

            public int GetHashCode(RuleReferenceToken obj)
            {
                int hashCode = obj.RuleName.GetHashCode();
                hashCode = (hashCode * 397) ^ (obj.DeclaredInNamespace?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ obj.Arguments.GetSequenceHashCode(RuleArgumentTokenEqualityComparer.Instance);
                return hashCode;
            }
        }

        private sealed class NerTokenEqualityComparer : IEqualityComparer<NerToken>
        {
            public static NerTokenEqualityComparer Instance = new NerTokenEqualityComparer();

            private NerTokenEqualityComparer()
            {
            }

            public bool Equals(NerToken? x, NerToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.VariableName == y.VariableName &&
                       x.CallChain == y.CallChain &&
                       x.Arguments.SequenceEqual(y.Arguments, RuleArgumentTokenEqualityComparer.Instance);
            }

            public int GetHashCode(NerToken obj)
            {
                int hashCode = obj.VariableName.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.CallChain.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Arguments.GetSequenceHashCode(RuleArgumentTokenEqualityComparer.Instance);
                return hashCode;
            }
        }

        private sealed class QuantifierTokenEqualityComparer : IEqualityComparer<QuantifierToken>
        {
            public static QuantifierTokenEqualityComparer Instance = new QuantifierTokenEqualityComparer();

            private QuantifierTokenEqualityComparer()
            {
            }

            public bool Equals(QuantifierToken? x, QuantifierToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.Min == y.Min && x.Max == y.Max;
            }

            public int GetHashCode(QuantifierToken obj)
            {
                return (obj.Min.GetHashCode() * 397) ^ obj.Max.GetHashCode();
            }
        }
    }
}