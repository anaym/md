using System.Linq;
using FluentAssertions;
using Markdown.Converters;
using Markdown.Syntax;
using Markdown.Syntax.Builtins;
using Markdown.Syntax.Utility;
using NUnit.Framework;

namespace Markdown.Test
{
    [TestFixture]
    public class MdLanguage_Should
    {
        private MdLanguageSyntax mdLanguageSyntax;

        [SetUp]
        public void SetUp()
        {
            mdLanguageSyntax = new MdLanguageSyntax();
        }

        #region Test Cases
        [TestCase("a b c d", TestName = "without tags")]
        [TestCase("a b __c d__", TestName = "with bold-tag")]
        [TestCase("a b _c d_ ", TestName = "with italic-tag")]
        [TestCase("a __b _c d_ e__", TestName = "with italic-tag in bold-tag")]
        [TestCase("a _b __c d__ e_", TestName = "with italic-tag and bold-tag permutation string")]

        [TestCase("a [a](b)", TestName = "with link")]
        [TestCase("a [a](b) [c](d)", TestName = "with two links")]
        [TestCase("a [a]()", TestName = "with link without address")]
        [TestCase("a [](b)", TestName = "with link without name")]

        [TestCase("a\n\nb", TestName = "with two paragraphs")]
        #endregion
        public void CorrectlyRebuild_String(string source)
        {
            var tree = mdLanguageSyntax.ParseMultiline(source);
            var build = mdLanguageSyntax.Build(tree);
            build.Should().Be(source);
        }

        #region Bold && Italic

        #region Not Parse Incorrect Strings
        [TestCase(@"\_ab\_", TestName = "is string with escaped tag")]
        [TestCase("_ ab_",  TestName = "is string with space after tag begin")]
        [TestCase("_ab _", TestName = "is string with space before tag end")]
        [TestCase("c_ab_", TestName = "is string without space before tag begin")]
        [TestCase("_ab_c", TestName = "is string without space after tag end")]
        [TestCase("_ab", TestName = "is string with only one tag bracket")]
        [TestCase("1_2_3",  TestName = "is digit-string")]
        [TestCase("a_b_c", TestName = "is letter-string")]
        [TestCase("_a_b_c", TestName = "is letter-string starting with _")]
        [TestCase("a_b_c_", TestName = "is letter-string ending with _")]
        public void NotParseItalic_WhenSource(string mdString)
        {
            var tree = mdLanguageSyntax.ParseMultiline(mdString);
            tree.Size.Should().Be(2);
            var raw = tree.NestedNodes.First();
            raw.IsRawString.Should().BeTrue();
            if (!mdString.Contains('\\')) raw.TagName.Should().Be(mdString);
        }

        [TestCase(@"\_\_ab\_\_", TestName = "is string with escaped tag")]
        [TestCase("__ ab__", TestName = "is string with space after tag begin")]
        [TestCase("__ab __", TestName = "is string with space before tag end")]
        [TestCase("c__ab__", TestName = "is string without space before tag begin")]
        [TestCase("__ab__c", TestName = "is string without space after tag end")]
        [TestCase("__ab", TestName = "is string with only one tag bracket")]
        [TestCase("1__2__3", TestName = "is digit-string")]
        [TestCase("a__b__c", TestName = "is letter-string")]
        public void NotParseBold_WhenSource(string mdString)
        {
            var tree = mdLanguageSyntax.ParseMultiline(mdString);
            tree.Size.Should().Be(2);
            var raw = tree.NestedNodes.First();
            raw.IsRawString.Should().BeTrue();
            if (!mdString.Contains('\\')) raw.TagName.Should().Be(mdString);

        }

        [Test]
        public void NotParseBold_WhenItInsideItalic()
        {
            var tree = mdLanguageSyntax.ParseMultiline("_a __b c d__ e_");
            var expected = Enumerable.Empty<SyntaxNode>()
                .ConnectTag("italic", SyntaxNode.CreateRawString("a __b c d__ e"));
            tree.NestedNodes.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Test]
        public void NotParse_WhenThereIsNoClosingTag()
        {
            var tree = mdLanguageSyntax.ParseMultiline("__a _b");
            tree.NestedNodes.ShouldAllBeEquivalentTo(new [] {SyntaxNode.CreateRawString("__a _b")}, o => o.WithStrictOrdering());
        }

        [Test]
        public void ThrowWhen_StringWith_OverlayTags()
        {
            Assert.Throws<ParseException>(() => mdLanguageSyntax.ParseMultiline("__a _b c__ d_"));
        }
        #endregion

        #region Correct Parse
        [Test]
        public void CorrectlyParse_StringWithItalic()
        {
            var md = "a _b c d_ e";
            var tree = mdLanguageSyntax.ParseMultiline(md);
            var expected = Enumerable.Empty<SyntaxNode>()
                .ConnectRaw("a ")
                .ConnectTag("italic", SyntaxNode.CreateRawString("b c d"))
                .ConnectRaw(" e");
            tree.NestedNodes.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Test]
        public void CorrectlyParse_StringWithBold()
        {
            var md = "a __b c d__ e";
            var tree = mdLanguageSyntax.ParseMultiline(md);
            var expected = Enumerable.Empty<SyntaxNode>()
                .ConnectRaw("a ")
                .ConnectTag("bold", SyntaxNode.CreateRawString("b c d"))
                .ConnectRaw(" e");
            tree.NestedNodes.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Test]
        public void CorrectlyParse_StringWithItalicInBold()
        {
            var md = "c __a _b 1 2_ e__ d";
            var tree = mdLanguageSyntax.ParseMultiline(md);
            var boldInside = Enumerable.Empty<SyntaxNode>()
                .ConnectRaw("a ")
                .ConnectTag("italic", SyntaxNode.CreateRawString("b 1 2"))
                .ConnectRaw(" e");
            var expected = Enumerable.Empty<SyntaxNode>()
                .ConnectRaw("c ")
                .ConnectTag("bold", boldInside)
                .ConnectRaw(" d");
            tree.NestedNodes.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }
        
        [Test]
        public void CorrectlyParse_StringWithItalicAndBold()
        {
            var md = "c __a b 1__ _2 e_ d";
            var tree = mdLanguageSyntax.ParseMultiline(md);
            var expected = Enumerable.Empty<SyntaxNode>()
                .ConnectRaw("c ")
                .ConnectTag("bold", SyntaxNode.CreateRawString("a b 1"))
                .ConnectRaw(" ")
                .ConnectTag("italic", SyntaxNode.CreateRawString("2 e"))
                .ConnectRaw(" d");
            tree.NestedNodes.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }
        #endregion

        #endregion

        #region URL

        #region Not Parse Incorrect Strings

        [TestCase(@"a](b)", TestName = "string without tag-name left bracket")]
        [TestCase(@"[a(b)", TestName = "string without tag-name right bracket")]
        [TestCase(@"[a]b)", TestName = "string without tag-address left bracket")]
        [TestCase(@"[a](b", TestName = "string without tag-address right bracket")]
        [TestCase(@"[](b)", TestName = "string without tag-name")]
        public void NotParseUrl_When(string mdString)
        {
            var tree = mdLanguageSyntax.ParseMultiline(mdString);
            tree.Size.Should().Be(2);
            var raw = tree.NestedNodes.First();
            raw.IsRawString.Should().BeTrue();
        }

        #endregion

        #region CorrectParse

        [Test]
        public void CorrectlyParse_StringWithUrl_WhenUrlIsDoubleSlash()
        {
            var md = @"a [\\ ]() e";
            var tree = mdLanguageSyntax.ParseMultiline(md);
            var expected = Enumerable.Empty<SyntaxNode>()
                .ConnectRaw("a ")
                .ConnectTag("url.name", SyntaxNode.CreateRawString("\\ "))
                .ConnectTag("url.address", Enumerable.Empty<SyntaxNode>())
                .ConnectRaw(" e");
            tree.NestedNodes.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
            var res = new HtmlLanguageSyntax().Build(tree);
        }

        [Test]
        public void CorrectlyParse_StringWithUrl_WhenUrlContinsNameAndAddress()
        {
            var md = "a [name](address) e";
            var tree = mdLanguageSyntax.ParseMultiline(md);
            var expected = Enumerable.Empty<SyntaxNode>()
                .ConnectRaw("a ")
                .ConnectTag("url.name", SyntaxNode.CreateRawString("name"))
                .ConnectTag("url.address", SyntaxNode.CreateRawString("address"))
                .ConnectRaw(" e");
            tree.NestedNodes.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
            var res = new HtmlLanguageSyntax().Build(tree);
        }

        [Test]
        public void CorrectlyParse_StringWithUrl_WhenUrlContinsOnlyName()
        {
            var md = "a [name]() e";
            var tree = mdLanguageSyntax.ParseMultiline(md);
            var expected = Enumerable.Empty<SyntaxNode>()
                .ConnectRaw("a ")
                .ConnectTag("url.name", SyntaxNode.CreateRawString("name"))
                .ConnectTag("url.address", Enumerable.Empty<SyntaxNode>())
                .ConnectRaw(" e");
            tree.NestedNodes.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Test]
        public void CorrectlyParse_StringWithTwoUrls()
        {
            var md = "a [n1](a1) [n2](a2) e";
            var tree = mdLanguageSyntax.ParseMultiline(md);
            var expected = Enumerable.Empty<SyntaxNode>()
                .ConnectRaw("a ")
                .ConnectTag("url.name", SyntaxNode.CreateRawString("n1"))
                .ConnectTag("url.address", SyntaxNode.CreateRawString("a1"))
                .ConnectRaw(" ")
                .ConnectTag("url.name", SyntaxNode.CreateRawString("n2"))
                .ConnectTag("url.address", SyntaxNode.CreateRawString("a2"))
                .ConnectRaw(" e");
            tree.NestedNodes.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        #endregion

        #endregion

        #region Paragraph

        [Test]
        public void CorrectlyParse_StringWithTwoParagraphs()
        {
            var md = "a\n\nb";
            var tree = mdLanguageSyntax.ParseMultiline(md);
            var expected = Enumerable.Empty<SyntaxNode>()
                .ConnectTag("paragraph", SyntaxNode.CreateRawString("a"))
                .ConnectRaw("\n")
                .ConnectTag("paragraph", SyntaxNode.CreateRawString("b"));
            tree.NestedNodes.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        #endregion
    }
}