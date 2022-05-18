using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Reflection
{
    public interface IUsedWordsProvider
    {
        IEnumerable<string> GetUsedWords();
    }
}