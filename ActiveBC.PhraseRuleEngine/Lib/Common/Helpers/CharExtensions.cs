using System.Runtime.CompilerServices;

namespace ActiveBC.PhraseRuleEngine.Lib.Common.Helpers
{
    public static class CharExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ToLowerFastRusEng(this char c)
        {
            if (c is >= 'A' and <= 'Z')
            {
                return (char) (c - 65 + 97);
            }

            if (c is >= 'А' and <= 'Я')
            {
                return (char) (c - 1040 + 1072);
            }

            return c == 'Ё' ? 'ё' : c;
        }
    }
}