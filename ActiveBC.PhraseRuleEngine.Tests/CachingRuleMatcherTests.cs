using System.Collections.Immutable;
using ActiveBC.PhraseRuleEngine.Evaluation.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Cache;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Input;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Projection.Arguments;
using ActiveBC.PhraseRuleEngine.Evaluation.Rule.Result;
using Moq;
using NUnit.Framework;

namespace ActiveBC.PhraseRuleEngine.Tests
{
    [TestFixture(TestOf = typeof(CachingRuleMatcher))]
    internal sealed class CachingRuleMatcherTests
    {
        [Test]
        [TestCase(300, new string[0], 42)]
        [TestCase(42, new [] {"foo"}, 300)]
        public void WorksWithEmptyCache(int matcherId, string[] sequence, int firstSymbolIndex)
        {
            RuleInput ruleInput = new RuleInput(
                sequence,
                new RuleSpaceArguments(ImmutableDictionary<string, object?>.Empty)
            );
            RuleArguments ruleArguments = new RuleArguments(ImmutableDictionary<string, object?>.Empty);
            RuleMatchResultCollection mockedResults = new RuleMatchResultCollection(0);

            Mock<IRuleMatcher> nestedRuleMatcherMock = new Mock<IRuleMatcher>();
            Mock<IRuleSpaceCache> cacheMock = new Mock<IRuleSpaceCache>();

            nestedRuleMatcherMock
                .Setup(nestedRuleMatcher => nestedRuleMatcher.MatchAndProject(ruleInput, firstSymbolIndex, ruleArguments, cacheMock.Object))
                .Returns(mockedResults);

            CachingRuleMatcher ruleMatcher = new CachingRuleMatcher(
                matcherId,
                nestedRuleMatcherMock.Object
            );

            RuleMatchResultCollection results = ruleMatcher.MatchAndProject(
                ruleInput,
                firstSymbolIndex,
                ruleArguments,
                cacheMock.Object
            );

            nestedRuleMatcherMock.Verify(nestedRuleMatcher => nestedRuleMatcher.MatchAndProject(ruleInput, firstSymbolIndex, ruleArguments, cacheMock.Object), Times.Once);
            nestedRuleMatcherMock.VerifyNoOtherCalls();

            cacheMock.Verify(cache => cache.GetResult(matcherId, ruleInput.Sequence, firstSymbolIndex, ruleArguments.Values), Times.Once);
            cacheMock.Verify(cache => cache.SetResult(matcherId, ruleInput.Sequence, firstSymbolIndex, ruleArguments.Values, mockedResults), Times.Once);
            cacheMock.VerifyNoOtherCalls();

            Assert.AreSame(mockedResults, results);
        }

        [Test]
        [TestCase(300, new string[0], 42)]
        [TestCase(42, new [] {"foo"}, 300)]
        public void WorksWithFilledCache(int matcherId, string[] sequence, int firstSymbolIndex)
        {
            RuleInput ruleInput = new RuleInput(
                sequence,
                new RuleSpaceArguments(ImmutableDictionary<string, object?>.Empty)
            );
            RuleArguments ruleArguments = new RuleArguments(ImmutableDictionary<string, object?>.Empty);
            RuleMatchResultCollection mockedResults = new RuleMatchResultCollection(0);

            Mock<IRuleMatcher> nestedRuleMatcherMock = new Mock<IRuleMatcher>();
            Mock<IRuleSpaceCache> cacheMock = new Mock<IRuleSpaceCache>();

            cacheMock
                .Setup(cache => cache.GetResult(matcherId, ruleInput.Sequence, firstSymbolIndex, ruleArguments.Values))
                .Returns(mockedResults);

            CachingRuleMatcher ruleMatcher = new CachingRuleMatcher(
                matcherId,
                nestedRuleMatcherMock.Object
            );

            RuleMatchResultCollection results = ruleMatcher.MatchAndProject(
                ruleInput,
                firstSymbolIndex,
                ruleArguments,
                cacheMock.Object
            );

            nestedRuleMatcherMock.VerifyNoOtherCalls();

            cacheMock.Verify(cache => cache.GetResult(matcherId, ruleInput.Sequence, firstSymbolIndex, ruleArguments.Values), Times.Once);
            cacheMock.VerifyNoOtherCalls();

            Assert.AreSame(mockedResults, results);
        }
    }
}