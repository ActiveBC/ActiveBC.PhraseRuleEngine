using System.Collections.Generic;
using System.Linq;

namespace ActiveBC.PhraseRuleEngine.Mechanics.Regex.Benchmarking
{
    internal static class DataProvider
    {
        public static (string Regex, string[] Phrases)[] Classic { get; } = GetCases(CollectClassicCases());
        public static (string Regex, string[] Phrases)[] Real { get; } = GetCases(CollectRealCases());

        private static (string Regex, string[] Phrases)[] GetCases(IEnumerable<(string Regex, string Phrase)> cases)
        {
            return cases
                .GroupBy(@case => @case.Regex)
                .Select(group => (group.Key, group.Select(@case => @case.Phrase).ToArray()))
                .ToArray();
        }

        private static IEnumerable<(string Regex, string Phrase)> CollectClassicCases()
        {
            yield return ("(один два три четыре пять)", "один два три четыре пять");
            yield return ("(один два три? четыре пять)", "один два три четыре пять");
            yield return ("(один два три? четыре пять)", "один два четыре пять");
            yield return ("(один два | четыре пять)", "один два");
            yield return ("(один два | четыре пять)", "четыре пять");
            yield return ("(один | два | три | четыре | пять)", "один");
            yield return ("(один | два | три | четыре | пять)", "два");
            yield return ("(один | два | три | четыре | пять)", "три");
            yield return ("(один | два | три | четыре | пять)", "четыре");
            yield return ("(один | два | три | четыре | пять)", "пять");
            yield return ("(один [два три четыре]? пять)", "один пять");
            yield return ("(один [два три четыре]? пять)", "один два пять");
            yield return ("(один [два три четыре]? пять)", "один три пять");
            yield return ("(один [два три четыре]? пять)", "один четыре пять");
        }

        private static IEnumerable<(string Regex, string Phrase)> CollectRealCases()
        {
            yield break;
        }
    }
}