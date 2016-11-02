using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MdLanguage_Should
    {
        // уменьшить размер примера строки до самой сути "a b __c d__" --> "__a__"
        [TestCase("a b c d", TestName = "Simple string")]
        [TestCase("a b __c d__", TestName = "Bold")]
        [TestCase("a b _c d_ ", TestName = "Italic")]
        [TestCase("a __b _c d_ e__", TestName = "Italic in bold")]
        public void CorrectParseAndBuild(string source)
        {
            var language = new MdLanguage();
            var tree = language.Parse(source);
            var builded = language.Build(tree);
            builded.Should().Be(source);
        }

        //не очень читается с When
        //тестраннер перемешает тесткейсы, лучше убрать номера
        [TestCase("\\_ab\\_", "_ab_", TestName = "1. Escaping")]
        [TestCase("_ ab_", "_ ab_", TestName = "2. Space after openned")]
        [TestCase("_ab _", "_ab _", TestName = "3. Space before closed")]
        [TestCase("c_ab_", "c_ab_", TestName = "4. No space before oppened")]
        [TestCase("_ab_c", "_ab_c", TestName = "5. No space after closed")]
        [TestCase("_ab", "_ab", TestName = "6. Singletone")]
        [TestCase("1_2_3", "1_2_3", TestName = "7. Number string")]
        [TestCase("a_b_c", "a_b_c", TestName = "8. Letter string")]
        public void NotParseItalic_When(string mdString, string expectedHtmlString)
        {
            var mdLanguage = new MdLanguage();
            var htmlLanguage = new HtmlLanguage();
            var tree = mdLanguage.Parse(mdString);
            var html = htmlLanguage.Build(tree);
            html.Should().Be(expectedHtmlString);
        }

        //смотри выше
        //7 и 8 тесткейс проверяют одно и тоже, нужен отдельный тест на возможность парсить строки с цифрами
        [TestCase("\\_\\_ab\\_\\_", "__ab__", TestName = "1. Escaping")]
        [TestCase("__ ab__", "__ ab__", TestName = "2. Space after openned")]
        [TestCase("__ab __", "__ab __", TestName = "3. Space before closed")]
        [TestCase("c__ab__", "c__ab__", TestName = "4. No space before oppened")]
        [TestCase("__ab__c", "__ab__c", TestName = "5. No space after closed")]
        [TestCase("__ab", "__ab", TestName = "6. Singletone")]
        [TestCase("1__2__3", "1__2__3", TestName = "7. Number string")]
        [TestCase("a__b__c", "a__b__c", TestName = "8. Letter string")]
        public void NotParseBold_When(string mdString, string expectedHtmlString)
        {
            //maybe create mdLanguage in SetUp?
            var mdLanguage = new MdLanguage();
            var htmlLanguage = new HtmlLanguage();
            var tree = mdLanguage.Parse(mdString);
            var html = htmlLanguage.Build(tree);
            html.Should().Be(expectedHtmlString);
        }

        //TODO: SintacticNode: флаг это rawString или пустой тег
        //Use Result or ExpectedResult and return value instead of use ``expected`` parameter
        [TestCase("_a __b c d__ e_", "<em>a __b c d__ e</em>", TestName = "Bold, when bold in italic")]
        [TestCase("__a _b", "__a _b", TestName = "when bold and italic is singletone")]
        public void NotParse(string mdString, string expectedHtmlString)
        {
            var mdLanguage = new MdLanguage();
            var htmlLanguage = new HtmlLanguage();
            //
            var tree = mdLanguage.Parse(mdString);
            var html = htmlLanguage.Build(tree);
            html.Should().Be(expectedHtmlString);
        }

        //ExpectedResult!
        [TestCase("a _b c d_ e", "a <em>b c d</em> e", TestName = "Italic to em")]
        [TestCase("__a _b c d_ e__", "<strong>a <em>b c d</em> e</strong>", TestName = "Italic in bold")]
        public void CorrectParse_When(string md, string expectedHtml)
        {
            var mdLanguage = new MdLanguage();
            var htmlLanguage = new HtmlLanguage();
            var tree = mdLanguage.Parse(md);
            var html = htmlLanguage.Build(tree);
            html.Should().Be(expectedHtml);
        }
    }
}