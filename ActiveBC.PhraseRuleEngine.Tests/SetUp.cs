using System.Reflection;
using ActiveBC.PhraseRuleEngine.Tests.Helpers;
using NUnit.Framework;

namespace ActiveBC.PhraseRuleEngine.Tests
{
    [SetUpFixture]
    internal sealed class SetUp
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // force load pick assembly
            Assembly.Load(typeof(Pick).Assembly.GetName());
        }
    }
}