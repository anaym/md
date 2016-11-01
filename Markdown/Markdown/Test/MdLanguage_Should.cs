using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MdLanguage_Should
    {
        [TestCase("a b c d", TestName = "Simple string")]
        [TestCase("a b __c d__", TestName = "Bold")]
        [TestCase("a b _c d_", TestName = "Shor bold")]
        [TestCase("a __b _c d_ e__", TestName = "Shor bold in bold")]
        public void CorrectParseAndBuild(string source)
        {
            var language = new MdLanguage();
            var tree = language.Parse(source);
            var builded = language.Build(tree);
            builded.Should().Be(source);
        }

        [TestCase("a __b c d__ e", "a <strong>b c d</strong> e", TestName = "Bold to strong")]
        [TestCase("a _b c d_ e", "a <em>b c d</em> e", TestName = "Short bold to strong")]
        [TestCase("__a _b c d_ e__", "<strong>a <em>b c d</em> e</strong>", TestName = "Short bold in bold")]
        [TestCase("_a __b c d__ e_", "a <em>b c d</em>", TestName = "__ in short bold")]
        public void CorrectParseAndBuildToHtml(string md, string expectedHtml)
        {
            var mdLanguage = new MdLanguage();
            var htmlLanguage = new HtmlLanguage();
            var tree = mdLanguage.Parse(md);
            var html = htmlLanguage.Build(tree);
            html.Should().Be(expectedHtml);
        }
    }
}