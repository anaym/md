using System.Linq;
using FluentAssertions;
using Markdown.Languages;
using Markdown.Languages.Exteptions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MdLanguage_Should
    {
        private MdLanguage mdLanguage;

        [SetUp]
        public void SetUp()
        {
            mdLanguage = new MdLanguage();
        }

        [TestCase("a b c d", TestName = "without tags")]
        [TestCase("a b __c d__", TestName =  "with bold-tag")]
        [TestCase("a b _c d_ ", TestName = "with italic-tag")]
        [TestCase("a __b _c d_ e__", TestName = "with italic-tag in bold-tag")]
        [TestCase("a _b __c d__ e_", TestName = "with italic-tag and bold-tag permutation string")]
        public void CorrectlyRebuild_String(string source)
        {
            var tree = mdLanguage.Parse(source);
            var build = mdLanguage.Build(tree);
            build.Should().Be(source);
        }

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
            var tree = mdLanguage.Parse(mdString);
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
            var tree = mdLanguage.Parse(mdString);
            tree.Size.Should().Be(2);
            var raw = tree.NestedNodes.First();
            raw.IsRawString.Should().BeTrue();
            if (!mdString.Contains('\\')) raw.TagName.Should().Be(mdString);

        }

        [Test]
        public void NotParseBold_WhenItInsideItalic()
        {
            var tree = mdLanguage.Parse("_a __b c d__ e_");
            var expected = Enumerable.Empty<SyntaxNode>()
                .ConnectTag("italic", SyntaxNode.CreateRawString("a __b c d__ e"));
            tree.NestedNodes.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Test]
        public void NotParse_WhenThereIsNoClosingTag()
        {
            var tree = mdLanguage.Parse("__a _b");
            tree.NestedNodes.ShouldAllBeEquivalentTo(new [] {SyntaxNode.CreateRawString("__a _b")}, o => o.WithStrictOrdering());
        }

        [Test]
        public void ThrowWhen_StringWith_OverlayTags()
        {
            Assert.Throws<ParseException>(() => mdLanguage.Parse("__a _b c__ d_"));
        }
        #endregion

        #region Correct Parse
        [Test]
        public void CorrectlyParse_StringWithItalic()
        {
            var md = "a _b c d_ e";
            var tree = mdLanguage.Parse(md);
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
            var tree = mdLanguage.Parse(md);
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
            var tree = mdLanguage.Parse(md);
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
            var tree = mdLanguage.Parse(md);
            var expected = Enumerable.Empty<SyntaxNode>()
                .ConnectRaw("c ")
                .ConnectTag("bold", SyntaxNode.CreateRawString("a b 1"))
                .ConnectRaw(" ")
                .ConnectTag("italic", SyntaxNode.CreateRawString("2 e"))
                .ConnectRaw(" d");
            tree.NestedNodes.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }
        #endregion
    }
}