using FluentAssertions;
using Markdown.Languages;
using NUnit.Framework;

namespace Markdown
{
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


        [TestCase("a b c d", TestName = "is simple string")]
        [TestCase("a b __c d__", TestName =  "is string with bold-tag")]
        [TestCase("a b _c d_ ", TestName = "is string with italic-tag")]
        [TestCase("a __b _c d_ e__", TestName = "is string with italic-tag in bold-tag")]
        [TestCase("a _b __c d__ e_", TestName = "is italic-tag and bold-tag permutation string")]
        public void NotMutateInformation_WhenSource(string source)
        {
            var tree = mdLanguage.Parse(source);
            var builded = mdLanguage.Build(tree);
            builded.Should().Be(source);
        }

        [TestCase("\\_ab\\_", ExpectedResult = "_ab_", TestName = "is string with escaped tag")]
        [TestCase("_ ab_", ExpectedResult = "_ ab_", TestName = "is string with space after tag begin")]
        [TestCase("_ab _", ExpectedResult = "_ab _", TestName = "is string with space before tag end")]
        [TestCase("c_ab_", ExpectedResult = "c_ab_", TestName = "is string without space before tag begin")]
        [TestCase("_ab_c", ExpectedResult = "_ab_c", TestName = "is string without space after tag end")]
        [TestCase("_ab", ExpectedResult = "_ab", TestName = "is string with only one tag bracket")]
        [TestCase("1_2_3", ExpectedResult = "1_2_3", TestName = "is digit-string")]
        [TestCase("a_b_c", ExpectedResult = "a_b_c", TestName = "is letter-string")]
        public string NotParseItalic_WhenSource(string mdString)
        {
            var tree = mdLanguage.Parse(mdString);
            return htmlLanguage.Build(tree);
        }

        [TestCase("\\_\\_ab\\_\\_", ExpectedResult = "__ab__", TestName = "is string with escaped tag")]
        [TestCase("__ ab__", ExpectedResult = "__ ab__", TestName = "is string with space after tag begin")]
        [TestCase("__ab __", ExpectedResult = "__ab __", TestName = "is string with space before tag end")]
        [TestCase("c__ab__", ExpectedResult = "c__ab__", TestName = "is string without space before tag begin")]
        [TestCase("__ab__c", ExpectedResult = "__ab__c", TestName = "is string without space after tag end")]
        [TestCase("__ab", ExpectedResult = "__ab", TestName = "is string with only one tag bracket")]
        [TestCase("1__2__3", ExpectedResult = "1__2__3", TestName = "is digit-string")]
        [TestCase("a__b__c", ExpectedResult = "a__b__c", TestName = "is letter-string")]
        public string NotParseBold_WhenSource(string mdString)
        {
            var tree = mdLanguage.Parse(mdString);
            return htmlLanguage.Build(tree);
        }

        [TestCase("_a __b c d__ e_", ExpectedResult = "<em>a __b c d__ e</em>", TestName = "bold-tag in italic-tag")]
        [TestCase("__a _b", ExpectedResult = "__a _b", TestName = "when bold and italic are singletones")]
        public string NotParse_When(string mdString)
        {
            var tree = mdLanguage.Parse(mdString);
            return htmlLanguage.Build(tree);
        }

        [TestCase("a _b c d_ e", ExpectedResult = "a <em>b c d</em> e", TestName = "italic-tag")]
        [TestCase("a __b c d__ e", ExpectedResult = "a <strong>b c d</strong> e", TestName = "bold-tag")]
        [TestCase("a _b c d_ __e__", ExpectedResult = "a <em>b c d</em> <strong>e</strong>", TestName = "italic and bold tags")]
        [TestCase("__a _b c d_ e__", ExpectedResult = "<strong>a <em>b c d</em> e</strong>", TestName = "italic-tag in bold-tag")]
        public string CorrectParse_StringWith(string md)
        {
            var tree = mdLanguage.Parse(md);
            return htmlLanguage.Build(tree);
        }
    }
}