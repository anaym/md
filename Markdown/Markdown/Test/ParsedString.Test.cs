using FluentAssertions;
using Markdown.StringParser;
using NUnit.Framework;

namespace Markdown.Test
{
    [TestFixture]
    public class ParsedString_Should
    {
        [TestCase("", "", TestName = "is empty")]
        [TestCase("a b cdefg", "a b cdefg", TestName = "is simple")]
        [TestCase("a \\b cdefg", "a b cdefg", TestName = "with one escaped char")]
        [TestCase("a \\b cd\\efg", "a b cdefg", TestName = "with two escaped chars")]
        [TestCase("a \\b cd\\efg \\\\", "a b cdefg \\", TestName = "with escape of escape char")]
        public void CorrectParseEscapedLine_WhenString(string source, string expected)
        {
            var pased = new ParsedString(source, '\\');
            pased.ToString().Should().Be(expected);
        }
    }
}