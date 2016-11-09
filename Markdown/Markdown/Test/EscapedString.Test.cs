using FluentAssertions;
using Markdown.StringParser;
using NUnit.Framework;

namespace Markdown.Test
{
    [TestFixture]
    public class EscapedString_Should
    {
        [TestCase("", "", TestName = "is empty")]
        [TestCase("a b cdefg", "a b cdefg", TestName = "is simple")]
        [TestCase(@"a \b cdefg", "a b cdefg", TestName = "with one escaped char")]
        [TestCase(@"a \b cd\efg", "a b cdefg", TestName = "with two escaped chars")]
        [TestCase(@"a \b cd\efg \\", @"a b cdefg \", TestName = "with escape of escape char")]
        public void CorrectlyParseEscapedLine_WhenString(string source, string expected)
        {
            var parsed = new EscapedString(source, '\\');
            parsed.ToString().Should().Be(expected);
        }

        [TestCase("abcde", 1, "bcd", TestName = "simple string")]
        [TestCase(@"a\bbcde", 2, "bcd", TestName = "escaped string")]
        public void OrdinalEqual_WhenSourceIs(string src, int compareStart, string other)
        {
            var parsed = new EscapedString(src, '\\');
            parsed.SubstringOrdinalEqual(other, compareStart).Should().BeTrue();
        }

        [TestCase("abcde", 1, "ecd", TestName = "simple string")]
        [TestCase(@"a\bcde", 1, "bcd", TestName = "escaped string")]
        [TestCase(@"\\", 0, @"\", TestName = "one escape char")]
        public void NotOrdinalEqual_WhenSourceIs(string src, int compareStart, string other)
        {
            var parsed = new EscapedString(src, '\\');
            parsed.SubstringOrdinalEqual(other, compareStart).Should().BeFalse();
        }
    }
}