using System.Collections;
using System.Collections.Generic;
using ActiveBC.PhraseRuleEngine.Lib.Common.Helpers;
using NUnit.Framework;

namespace ActiveBC.PhraseRuleEngine.Tests
{
    [TestFixture(TestOf = typeof(ErrorIndexHelper))]
    internal sealed class ErrorIndexHelperTests
    {
        [Test]
        [TestCase("foo", "bar  baz  baf  bac", 0, 0, 0, 'b', "bar  b")]
        [TestCase("foo", "bar  baz  baf  bac", 1, 0, 1, 'a', "bar  ba")]
        [TestCase("foo", "bar  baz  baf  bac", 8, 0, 8, ' ', "  baz  baf ")]
        [TestCase("foo", "bar  baz  baf  bac", 11, 0, 11, 'a', "az  baf  ba")]
        [TestCase("foo", "bar  baz  baf  bac", 16, 0, 16, 'a', "af  bac")]
        [TestCase("foo", "bar  baz  baf  bac", 17, 0, 17, 'c', "f  bac")]
        [TestCase("foo", "bar\r\nbaz\r\nbaf\r\nbac", 0, 0, 0, 'b', "bar")]
        [TestCase("foo", "bar\r\nbaz\r\nbaf\r\nbac", 1, 0, 1, 'a', "bar")]
        [TestCase("foo", "bar\r\nbaz\r\nbaf\r\nbac", 2, 0, 2, 'r', "bar")]
        [TestCase("foo", "bar\r\nbaz\r\nbaf\r\nbac", 3, 0, 3, null, null)]
        [TestCase("foo", "bar\r\nbaz\r\nbaf\r\nbac", 4, 0, 4, null, null)]
        [TestCase("foo", "bar\r\nbaz\r\nbaf\r\nbac", 5, 1, 0, 'b', "baz")]
        [TestCase("foo", "bar\r\nbaz\r\nbaf\r\nbac", 6, 1, 1, 'a', "baz")]
        [TestCase("foo", "bar\r\nbaz\r\nbaf\r\nbac", 7, 1, 2, 'z', "baz")]
        [TestCase("foo", "bar\r\nbaz\r\nbaf\r\nbac", 8, 1, 3, null, null)]
        [TestCase("foo", "bar\r\nbaz\r\nbaf\r\nbac", 9, 1, 4, null, null)]
        [TestCase("foo", "bar\r\nbaz\r\nbaf\r\nbac", 17, 3, 2, 'c', "bac")]
        [TestCase("foo", "", 0, 0, 0, null, null)]
        [TestCase("foo", "\r\n", 0, 0, 0, null, null)]
        [TestCase("foo", "\r\n", 1, 0, 1, null, null)]
        [TestCase("foo", "\r\n\r\n", 0, 0, 0, null, null)]
        [TestCase("foo", "\r\n\r\n", 1, 0, 1, null, null)]
        [TestCase("foo", "\r\n\r\n", 2, 1, 0, null, null)]
        [TestCase("foo", "\r\n\r\n", 3, 1, 1, null, null)]
        public void Foo(
            string message,
            string source,
            int errorIndex,
            int expectedLineIndex,
            int expectedPositionInLine,
            char? expectedCharacter,
            string? expectedContext
        )
        {
            IDictionary data = new Dictionary<string, object>();

            ErrorIndexHelper.FillExceptionData(data, source, null, errorIndex);
            Assert.IsTrue(data.Contains("source"));
            Assert.AreEqual(source, data["source"]);

            Assert.IsTrue(data.Contains("error_index"));
            Assert.AreEqual(errorIndex, data["error_index"]);

            Assert.IsTrue(data.Contains("error_line_index"));
            Assert.AreEqual(expectedLineIndex, data["error_line_index"]);

            Assert.IsTrue(data.Contains("error_position_in_line"));
            Assert.AreEqual(expectedPositionInLine, data["error_position_in_line"]);

            bool hasErrorCharacter = data.Contains("error_character");
            if (expectedCharacter is not null)
            {
                Assert.IsTrue(hasErrorCharacter);
                Assert.AreEqual(expectedCharacter.Value, data["error_character"]);
            }
            else
            {
                Assert.IsFalse(hasErrorCharacter);
            }

            bool hasErrorContext = data.Contains("error_context");
            if (expectedContext is not null)
            {
                Assert.IsTrue(hasErrorContext);
                Assert.AreEqual(expectedContext, data["error_context"]);
            }
            else
            {
                Assert.IsFalse(hasErrorContext);
            }
        }
    }
}