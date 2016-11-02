using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    //TODO: проверить имена
    [TestFixture]
    public class MdLanguage_Should
    {
        private MdLanguage mdLanguage;
        private HtmlLanguage htmlLanguage;

        [SetUp]
        public void SetUp()
        {
            mdLanguage = new MdLanguage();
            htmlLanguage = new HtmlLanguage();
        }


        [TestCase("a b c d", TestName = "Simple string")]
        [TestCase("a b __c d__", TestName = "Bold")]
        [TestCase("a b _c d_ ", TestName = "Italic")]
        [TestCase("a __b _c d_ e__", TestName = "Italic in bold")]
        public void CorrectParseAndBuild(string source)
        {
            var tree = mdLanguage.Parse(source);
            var builded = mdLanguage.Build(tree);
            builded.Should().Be(source);
        }

        [TestCase("\\_ab\\_", ExpectedResult = "_ab_", TestName = "Escaping")]
        [TestCase("_ ab_", ExpectedResult = "_ ab_", TestName = "Space after openned")]
        [TestCase("_ab _", ExpectedResult = "_ab _", TestName = "Space before closed")]
        [TestCase("c_ab_", ExpectedResult = "c_ab_", TestName = "No space before oppened")]
        [TestCase("_ab_c", ExpectedResult = "_ab_c", TestName = "No space after closed")]
        [TestCase("_ab", ExpectedResult = "_ab", TestName = "Singletone")]
        [TestCase("1_2_3", ExpectedResult = "1_2_3", TestName = "Number string")]
        [TestCase("a_b_c", ExpectedResult = "a_b_c", TestName = "Letter string")]
        public string NotParseItalic_When(string mdString)
        {
            var tree = mdLanguage.Parse(mdString);
            return htmlLanguage.Build(tree);
        }

        [TestCase("\\_\\_ab\\_\\_", ExpectedResult = "__ab__", TestName = "Escaping")]
        [TestCase("__ ab__", ExpectedResult = "__ ab__", TestName = "Space after openned")]
        [TestCase("__ab __", ExpectedResult = "__ab __", TestName = "Space before closed")]
        [TestCase("c__ab__", ExpectedResult = "c__ab__", TestName = "No space before oppened")]
        [TestCase("__ab__c", ExpectedResult = "__ab__c", TestName = "No space after closed")]
        [TestCase("__ab", ExpectedResult = "__ab", TestName = "Singletone")]
        [TestCase("1__2__3", ExpectedResult = "1__2__3", TestName = "Number string")]
        [TestCase("a__b__c", ExpectedResult = "a__b__c", TestName = "Letter string")]
        public string NotParseBold_When(string mdString)
        {
            var tree = mdLanguage.Parse(mdString);
            return htmlLanguage.Build(tree);
        }

        [TestCase("_a __b c d__ e_", ExpectedResult = "<em>a __b c d__ e</em>", TestName = "Bold, when bold in italic")]
        [TestCase("__a _b", ExpectedResult = "__a _b", TestName = "when bold and italic is singletone")]
        public string NotParse(string mdString)
        {
            var tree = mdLanguage.Parse(mdString);
            return htmlLanguage.Build(tree);
        }

        [TestCase("a _b c d_ e", ExpectedResult = "a <em>b c d</em> e", TestName = "Italic to em")]
        [TestCase("a __b c d__ e", ExpectedResult = "a <strong>b c d</strong> e", TestName = "Bold to strong")]
        [TestCase("a _b c d_ __e__", ExpectedResult = "a <em>b c d</em> <strong>e</strong>", TestName = "Bold next to italic")]
        [TestCase("__a _b c d_ e__", ExpectedResult = "<strong>a <em>b c d</em> e</strong>", TestName = "Italic in bold")]
        public string CorrectParse_When(string md)
        {
            var tree = mdLanguage.Parse(md);
            return htmlLanguage.Build(tree);
        }
    }
}