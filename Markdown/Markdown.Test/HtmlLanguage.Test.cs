using System;
using System.Collections.Generic;
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
    public class HtmlLanguage_Should
    {
        private HtmlLanguageSyntax htmlLanguageSyntax;

        [SetUp]
        public void SetUp()
        {
            htmlLanguageSyntax = new HtmlLanguageSyntax();
        }

        [TestCase("a b c d", TestName = "is simple string")]
        [TestCase("a b <strong>c d</strong>", TestName =  "is string with bold-tag")]
        [TestCase("a b <em>c d</em> ", TestName = "is string with italic-tag")]
        [TestCase("a <strong>b <em>c d</em> e</strong>", TestName = "is string with italic-tag in bold-tag")]
        [TestCase("a <em>b <strong>c d</strong> e</em>", TestName = "is italic-tag and bold-tag permutation string")]

        [TestCase("<a href=\"a\">text</a>", TestName = "is string with url")]
        public void CorrectlyRebuild(string source)
        {
            var tree = htmlLanguageSyntax.Parse(source);
            var build = htmlLanguageSyntax.Build(tree);
            build.Should().Be(source);
        }

        #region Bold && Italic

        #region Not Parse Incorrect Strings
        [TestCase(@"\<em>ab</em>", TestName = "italic-tag")]
        [TestCase(@"\<strong>ab</strong>", TestName = "bold-tag")]
        public void NotParse_WhenSourceIsStringWithEscaped(string mdString)
        {
            var tree = htmlLanguageSyntax.Parse(mdString);
            tree.Size.Should().Be(2);
            var raw = tree.NestedNodes.First();
            raw.IsRawString.Should().BeTrue();
        }

        [Test]
        public void NotParse_WhenThereIsNoClosingTag()
        {
            var tree = htmlLanguageSyntax.Parse("<em>a <strong>b");
            tree.NestedNodes.ShouldAllBeEquivalentTo(new [] {SyntaxNode.CreateRawString("<em>a <strong>b") }, o => o.WithStrictOrdering());
        }
        #endregion

        #region Correct Parse
        [Test]
        public void CorrectlyParse_StringWithItalic()
        {
            var md = "a <em>b c d</em> e";
            var tree = htmlLanguageSyntax.Parse(md);
            var expected = Enumerable.Empty<SyntaxNode>()
                .ConnectRaw("a ")
                .ConnectTag("italic", SyntaxNode.CreateRawString("b c d"))
                .ConnectRaw(" e");
            tree.NestedNodes.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Test]
        public void CorrectlyParse_StringWithBold()
        {
            var md = "a <strong>b c d</strong> e";
            var tree = htmlLanguageSyntax.Parse(md);
            var expected = Enumerable.Empty<SyntaxNode>()
                .ConnectRaw("a ")
                .ConnectTag("bold", SyntaxNode.CreateRawString("b c d"))
                .ConnectRaw(" e");
            tree.NestedNodes.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Test]
        public void CorrectlyParse_StringWithItalicInBold()
        {
            var md = "c <strong>a <em>b 1 2</em> e</strong> d";
            var tree = htmlLanguageSyntax.Parse(md);
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
            var md = "c <strong>a b 1</strong> <em>2 e</em> d";
            var tree = htmlLanguageSyntax.Parse(md);
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

        [Test]
        public void CorrectBuildUrl_WithCssCLass()
        {
            htmlLanguageSyntax = new HtmlLanguageSyntax(cssClass:"noname");
            var tags = Enumerable.Empty<SyntaxNode>()
                .ConnectTag("url.address", SyntaxNode.CreateRawString("/addr"))
                .ConnectTag("url.name", SyntaxNode.CreateRawString("nm"));
            var root = SyntaxNode.CreateTag(null);
            root.AddManyNestedNode(tags);
            htmlLanguageSyntax.Build(root, metaUrl:"meta").Should().Be("<a href=\"meta/addr\" class=\"noname\">nm</a>");
        }

        #endregion
    }
}