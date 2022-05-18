using System;
using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Lib.Common.Helpers
{
    public sealed class EntityEqualityComparer<TValue, TEntity> : IEqualityComparer<TValue>
        where TEntity : notnull
    {
        private readonly Func<TValue, TEntity> m_entityGetter;

        public EntityEqualityComparer(Func<TValue, TEntity> entityGetter)
        {
            this.m_entityGetter = entityGetter;
        }

        public bool Equals(TValue? x, TValue? y)
        {
            if (x is null != y is null)
            {
                return false;
            }

            if (x is null && y is null)
            {
                return true;
            }

            return this.m_entityGetter(x!).Equals(this.m_entityGetter(y!));
        }

        public int GetHashCode(TValue obj)
        {
            return this.m_entityGetter(obj!).GetHashCode();
        }
    }
}