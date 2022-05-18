using System;
using System.Collections.Generic;
using System.Linq;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using ActiveBC.PhraseRuleEngine.Exceptions;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;

namespace ActiveBC.PhraseRuleEngine.Evaluation.Cache
{
    public sealed class RuleSpaceCache : IManageableRuleSpaceCache
    {
        private readonly IDictionary<Key, RuleMatchResultCollection> m_results;

        public RuleSpaceCache(int capacity = 0)
        {
            this.m_results = new Dictionary<Key, RuleMatchResultCollection>(capacity);
        }

        public void Clear()
        {
            this.m_results.Clear();
        }

        public RuleMatchResultCollection? GetResult(
            int ruleId,
            string[] inputSequence,
            int nextSymbolIndex,
            IReadOnlyDictionary<string, object?>? ruleArguments
        )
        {
            this.m_results.TryGetValue(new Key(ruleId, inputSequence, ruleArguments, nextSymbolIndex), out RuleMatchResultCollection? result);

            return result;
        }

        public void SetResult(
            int ruleId,
            string[] inputSequence,
            int nextSymbolIndex,
            IReadOnlyDictionary<string, object?>? ruleArguments,
            RuleMatchResultCollection result
        )
        {
            try
            {
                this.m_results.Add(new Key(ruleId, inputSequence, ruleArguments, nextSymbolIndex), result);
            }
            catch (Exception exception)
            {
                throw new RuleMatchException("Error during adding rule match result to cache.", exception);
            }
        }

        private readonly struct Key : IEquatable<Key>
        {
            private readonly int m_ruleId;
            private readonly string[] m_inputSequence;
            private readonly IReadOnlyDictionary<string, object?>? m_ruleArguments;
            private readonly int m_nextSymbolIndex;

            public Key(
                int ruleId,
                string[] inputSequence,
                IReadOnlyDictionary<string, object?>? ruleArguments,
                int nextSymbolIndex
            )
            {
                this.m_ruleId = ruleId;
                this.m_inputSequence = inputSequence;
                this.m_nextSymbolIndex = nextSymbolIndex;
                this.m_ruleArguments = ruleArguments;
            }

            public bool Equals(Key other)
            {
                return this.m_ruleId == other.m_ruleId &&
                       this.m_nextSymbolIndex == other.m_nextSymbolIndex &&
                       this.m_inputSequence.SequenceEqual(other.m_inputSequence) &&
                       this.m_ruleArguments is null == other.m_ruleArguments is null &&
                       (this.m_ruleArguments?.SequenceEqual(other.m_ruleArguments!) ?? true);
            }

            public override bool Equals(object? obj)
            {
                return obj is Key other && Equals(other);
            }

            public override int GetHashCode()
            {
                int hash = 21;
                hash = hash * 17 + this.m_ruleId.GetHashCode();
                hash = hash * 17 + this.m_nextSymbolIndex.GetHashCode();
                hash = hash * 17 + this.m_inputSequence.GetSequenceHashCode<string>();
                hash = hash * 17 + (this.m_ruleArguments?.GetDictionaryHashCode() ?? 0);
                return hash;
            }
        }
    }
}