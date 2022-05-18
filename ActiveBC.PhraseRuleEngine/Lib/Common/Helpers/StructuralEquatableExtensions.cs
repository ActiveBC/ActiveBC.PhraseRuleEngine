using System.Collections;
using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Lib.Common.Helpers
{
    public static class StructuralEquatableExtensions
    {
        public static int GetSequenceHashCode<TValue>(this IStructuralEquatable structuralEquatable)
        {
            return structuralEquatable.GetSequenceHashCode(EqualityComparer<TValue>.Default);
        }

        public static int GetSequenceHashCode(this IStructuralEquatable structuralEquatable, IEqualityComparer equalityComparer)
        {
            return structuralEquatable.GetHashCode(equalityComparer);
        }

        public static int GetDictionaryHashCode<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary)
            where TKey : notnull
        {
            return dictionary.GetDictionaryHashCode(EqualityComparer<KeyValuePair<TKey, TValue>>.Default);
        }

        public static int GetDictionaryHashCode<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> dictionary,
            IEqualityComparer equalityComparer
        )
            where TKey : notnull
        {
            int hash = 0;

            foreach (KeyValuePair<TKey,TValue> pair in dictionary)
            {
                hash = hash * 17 + equalityComparer.GetHashCode(pair);
            }

            return hash;
        }
    }
}