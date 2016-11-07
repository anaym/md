using FluentAssertions;
using Markdown.StringParser;
using NUnit.Framework;

namespace Markdown.Test
{
    // CR (krait): Нету теста на IsOrdinalEqual.
    [TestFixture]
    public class EscapedString_Should
    {
        // CR (krait): correctly
    //Use @"   " everywhere!
        [TestCase("", "", TestName = "is empty")]
        [TestCase("a b cdefg", "a b cdefg", TestName = "is simple")]
        [TestCase(@"a \b cdefg", "a b cdefg", TestName = "with one escaped char")]
        [TestCase(@"a \b cd\efg", "a b cdefg", TestName = "with two escaped chars")]
        [TestCase(@"a \b cd\efg \\", @"a b cdefg \", TestName = "with escape of escape char")]
        public void CorrectParseEscapedLine_WhenString(string source, string expected)
        {
            var parsed = new EscapedString(source, '\\');
            parsed.ToString().Should().Be(expected);
        }
    }
}