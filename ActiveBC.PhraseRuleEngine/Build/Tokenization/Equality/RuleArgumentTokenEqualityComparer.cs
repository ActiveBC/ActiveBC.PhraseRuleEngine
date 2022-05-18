using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Build.Tokenization.Tokens.Arguments;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Build.Tokenization.Equality
{
    public sealed class RuleArgumentTokenEqualityComparer : IEqualityComparer<IRuleArgumentToken>, IEqualityComparer
    {
        public static readonly RuleArgumentTokenEqualityComparer Instance = new RuleArgumentTokenEqualityComparer();

        private RuleArgumentTokenEqualityComparer()
        {
        }

        bool IEqualityComparer.Equals(object? x, object? y)
        {
            return Equals(x as IRuleArgumentToken, y as IRuleArgumentToken);
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return obj is IRuleArgumentToken token ? GetHashCode(token) : 0;
        }

        public bool Equals(IRuleArgumentToken? x, IRuleArgumentToken? y)
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
                RuleDefaultArgumentToken => true,
                RuleChainedMemberAccessArgumentToken token => RuleChainedMemberAccessArgumentTokenEqualityComparer.Instance.Equals(token, y as RuleChainedMemberAccessArgumentToken),
                _ => throw new ArgumentOutOfRangeException(nameof(x)),
            };
        }

        public int GetHashCode(IRuleArgumentToken obj)
        {
            return obj switch
            {
                RuleDefaultArgumentToken => typeof(RuleDefaultArgumentToken).GetHashCode(),
                RuleChainedMemberAccessArgumentToken token => RuleChainedMemberAccessArgumentTokenEqualityComparer.Instance.GetHashCode(token),
                _ => throw new ArgumentOutOfRangeException(nameof(obj)),
            };
        }

        private sealed class RuleChainedMemberAccessArgumentTokenEqualityComparer : IEqualityComparer<RuleChainedMemberAccessArgumentToken>
        {
            public static readonly RuleChainedMemberAccessArgumentTokenEqualityComparer Instance = new RuleChainedMemberAccessArgumentTokenEqualityComparer();

            private RuleChainedMemberAccessArgumentTokenEqualityComparer()
            {
            }

            public bool Equals(RuleChainedMemberAccessArgumentToken? x, RuleChainedMemberAccessArgumentToken? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                {
                    return false;
                }

                return x.CallChain.SequenceEqual(y.CallChain);
            }

            public int GetHashCode(RuleChainedMemberAccessArgumentToken obj)
            {
                return obj.CallChain.GetSequenceHashCode<string>();
            }
        }
    }
}