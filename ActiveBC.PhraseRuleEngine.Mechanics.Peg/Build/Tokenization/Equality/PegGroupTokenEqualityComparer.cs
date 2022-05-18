using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Equality;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Tokens;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Peg.Build.Tokenization.Equality
{
    public sealed class PegGroupTokenEqualityComparer : IEqualityComparer<PegGroupToken>
    {
        public static readonly PegGroupTokenEqualityComparer Instance = new PegGroupTokenEqualityComparer();

        public bool Equals(PegGroupToken? x, PegGroupToken? y)
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

        public int GetHashCode(PegGroupToken obj)
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

        private sealed class BranchItemTokenEqualityComparer : IEqualityComparer<BranchItemToken>, IEqualityComparer
        {
            public static readonly BranchItemTokenEqualityComparer Instance = new BranchItemTokenEqualityComparer();

            private BranchItemTokenEqualityComparer()
            {
            }

            bool IEqualityComparer.Equals(object? x, object? y)
            {
                return Equals(x as BranchItemToken, y as BranchItemToken);
            }

            int IEqualityComparer.GetHashCode(object obj)
            {
                return obj is BranchItemToken token ? GetHashCode(token) : 0;
            }

            public bool Equals(BranchItemToken? x, BranchItemToken? y)
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
                       QuantifierTokenEqualityComparer.Instance.Equals(x.Quantifier, y.Quantifier) &&
                       x.VariableName == y.VariableName &&
                       LookaheadTokenEqualityComparer.Instance.Equals(x.Lookahead!, y.Lookahead!);
            }

            public int GetHashCode(BranchItemToken obj)
            {
                int hashCode = QuantifiableTokenEqualityComparer.Instance.GetHashCode(obj.Quantifiable);
                hashCode = (hashCode * 397) ^ QuantifierTokenEqualityComparer.Instance.GetHashCode(obj.Quantifier);
                hashCode = (hashCode * 397) ^ (obj.VariableName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (obj.Lookahead is not null ? LookaheadTokenEqualityComparer.Instance.GetHashCode(obj.Lookahead) : 0);
                return hashCode;
            }
        }

        private sealed class LookaheadTokenEqualityComparer : IEqualityComparer<LookaheadToken>
        {
            public static readonly LookaheadTokenEqualityComparer Instance = new LookaheadTokenEqualityComparer();

            private LookaheadTokenEqualityComparer()
            {
            }

            public bool Equals(LookaheadToken? x, LookaheadToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.IsNegative == y.IsNegative;
            }

            public int GetHashCode(LookaheadToken obj)
            {
                return obj.IsNegative.GetHashCode();
            }
        }

        private sealed class QuantifiableTokenEqualityComparer : IEqualityComparer<IQuantifiableToken>
        {
            public static readonly QuantifiableTokenEqualityComparer Instance = new QuantifiableTokenEqualityComparer();

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
                    PegGroupToken token => PegGroupTokenEqualityComparer.Instance.Equals(token, y as PegGroupToken),
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
                    PegGroupToken token => PegGroupTokenEqualityComparer.Instance.GetHashCode(token),
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
            public static readonly LiteralSetMemberTokenEqualityComparer Instance = new LiteralSetMemberTokenEqualityComparer();

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

        private sealed class QuantifierTokenEqualityComparer : IEqualityComparer<QuantifierToken>
        {
            public static readonly QuantifierTokenEqualityComparer Instance = new QuantifierTokenEqualityComparer();

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