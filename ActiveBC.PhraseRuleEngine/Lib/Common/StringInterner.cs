using System.Collections.Concurrent;

namespace ActiveBC.PhraseRuleEngine.Lib.Common
{
    public sealed class StringInterner
    {
        private readonly ConcurrentDictionary<string, string> m_knownStrings;

        public StringInterner()
        {
            this.m_knownStrings = new ConcurrentDictionary<string, string>();
        }

        public string InternString(string @string)
        {
            return this.m_knownStrings.GetOrAdd(@string, @string);
        }

        public void Clear()
        {
            this.m_knownStrings.Clear();
        }
    }
}