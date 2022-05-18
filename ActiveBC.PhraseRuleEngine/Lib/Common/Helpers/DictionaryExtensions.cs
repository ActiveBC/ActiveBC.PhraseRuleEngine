using System;
using System.Collections.Generic;
using System.Linq;

namespace ActiveBC.PhraseRuleEngine.Lib.Common.Helpers
{
    public static class DictionaryExtensions
    {
        public static IEnumerable<KeyValuePair<TNewKey, TValue>> MapKey<TKey, TValue, TNewKey>(
            this IEnumerable<KeyValuePair<TKey, TValue>> dictionary,
            Func<TKey, TValue, TNewKey> keyProjection
        )
            where TKey : notnull
        {
            return dictionary
                .Select(old => new KeyValuePair<TNewKey, TValue>(keyProjection(old.Key, old.Value), old.Value));
        }

        public static IEnumerable<KeyValuePair<TNewKey, TValue>> MapKey<TKey, TValue, TNewKey>(
            this IEnumerable<KeyValuePair<TKey, TValue>> dictionary,
            Func<TKey, TNewKey> keyProjection
        )
            where TKey : notnull
        {
            return dictionary
                .Select(old => new KeyValuePair<TNewKey, TValue>(keyProjection(old.Key), old.Value));
        }

        public static IEnumerable<KeyValuePair<TKey, TNewValue>> MapValue<TKey, TValue, TNewValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> dictionary,
            Func<TValue, TNewValue> valueProjection
        )
            where TKey : notnull
        {
            return dictionary
                .Select(old => new KeyValuePair<TKey, TNewValue>(old.Key, valueProjection(old.Value)));
        }

        public static IEnumerable<KeyValuePair<TKey, TNewValue>> MapValue<TKey, TValue, TNewValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> dictionary,
            Func<TKey, TValue, TNewValue> valueProjection
        )
            where TKey : notnull
        {
            return dictionary
                .Select(old => new KeyValuePair<TKey, TNewValue>(old.Key, valueProjection(old.Key, old.Value)));
        }

        public static IEnumerable<KeyValuePair<TKey, TNewValue>> MapValue<TKey, TValue, TNewValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> dictionary,
            Func<int, TKey, TValue, TNewValue> valueProjection
        )
            where TKey : notnull
        {
            return dictionary
                .Select((old, i) => new KeyValuePair<TKey, TNewValue>(old.Key, valueProjection(i, old.Key, old.Value)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> WhereKey<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> dictionary,
            Predicate<TKey> keyPredicate
        )
            where TKey : notnull
        {
            return dictionary.Where(pair => keyPredicate(pair.Key));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> WhereValue<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> dictionary,
            Predicate<TValue> valuePredicate
        )
            where TKey : notnull
        {
            return dictionary.Where(pair => valuePredicate(pair.Value));
        }

        public static IEnumerable<TValue> SelectValues<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> dictionary
        )
            where TKey : notnull
        {
            return dictionary.Select(pair => pair.Value);
        }

        public static IEnumerable<KeyValuePair<TValue, TKey>> SwapKeysAndValues<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            where TKey : notnull
            where TValue : notnull
        {
            return dictionary.Select(old => new KeyValuePair<TValue, TKey>(old.Value, old.Key));
        }

        public static Dictionary<TKey, TValue> AddAliases<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            IReadOnlyDictionary<TKey, TKey> aliasesMap
        ) where TKey : notnull
        {
            foreach ((TKey targetKey, TKey sourceKey) in aliasesMap)
            {
                if (dictionary.TryGetValue(sourceKey, out TValue? sourceValue))
                {
                    dictionary[targetKey] = sourceValue;
                }
            }

            return dictionary;
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> pairs,
            bool ignoreDuplicates = false
        )
            where TKey : notnull
        {
            return pairs.ToDictionaryWithKnownCapacity(0, ignoreDuplicates);
        }

        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
            this IEnumerable<IEnumerable<KeyValuePair<TKey, TValue>>> dictionaries,
            bool ignoreDuplicates = false
        )
            where TKey : notnull
        {
            return dictionaries.MergeWithKnownCapacity(0, ignoreDuplicates);
        }

        public static Dictionary<TKey, TValue> ToDictionaryWithKnownCapacity<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> dictionary,
            int capacity,
            bool ignoreDuplicates = false,
            Func<TKey, ArgumentException, Exception>? rethrowFactory = null
        )
            where TKey : notnull
        {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>(capacity);

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                if (ignoreDuplicates)
                {
                    result.TryAdd(pair.Key, pair.Value);
                }
                else
                {
                    if (rethrowFactory is not null)
                    {
                        try
                        {
                            result.Add(pair.Key, pair.Value);
                        }
                        catch (ArgumentException exception)
                        {
                            throw rethrowFactory(pair.Key, exception);
                        }
                    }
                    else
                    {
                        result.Add(pair.Key, pair.Value);
                    }
                }
            }

            return result;
        }

        public static Dictionary<TKey, TValue> MergeWithKnownCapacity<TKey, TValue>(
            this IEnumerable<IEnumerable<KeyValuePair<TKey, TValue>>> dictionaries,
            int capacity,
            bool ignoreDuplicates = false
        )
            where TKey : notnull
        {
            return dictionaries
                .SelectMany(dictionary => dictionary)
                .ToDictionaryWithKnownCapacity(capacity, ignoreDuplicates);
        }

        public static Dictionary<TKey, TValue>? MergeNullablesWithKnownCapacity<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>>? dictionary,
            IEnumerable<KeyValuePair<TKey, TValue>>? other,
            int capacity,
            bool ignoreDuplicates = false
        )
            where TKey : notnull
        {
            if (dictionary is null && other is null)
            {
                return null;
            }

            if (dictionary is not null && other is null)
            {
                return dictionary.ToDictionary();
            }

            if (dictionary is null && other is not null)
            {
                return other.ToDictionary();
            }

            return new []
            {
                dictionary!,
                other!
            }
            .MergeWithKnownCapacity(capacity, ignoreDuplicates);
        }
    }
}