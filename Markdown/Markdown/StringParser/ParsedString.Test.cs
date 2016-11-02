using FluentAssertions;
using Markdown.StringParser;
using NUnit.Framework;

namespace Markdown.StringParser
{

    [TestFixture]
    public class ParsedString_Should
    {
        [TestCase("", "", TestName = "Empty string")]
        [TestCase("a b cdefg", "a b cdefg", TestName = "Simple string")]
        [TestCase("a \\b cdefg", "a b cdefg", TestName = "One escaped")]
        [TestCase("a \\b cd\\efg", "a b cdefg", TestName = "Two escaped")]
        [TestCase("a \\b cd\\efg \\\\", "a b cdefg \\", TestName = "Escaped escape char")]
        public void CorrectParseEscapedLine(string source, string expected)
        {
            var pased = new ParsedString(source, '\\');
            pased.ToString().Should().Be(expected);
        }
    }
}