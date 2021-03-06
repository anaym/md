﻿using System.Linq;
using Markdown.Syntax;

namespace Markdown.Languages
{
    public class SyntaxTreeCompiler
    {
        public readonly LanguageSyntax Syntax;

        public SyntaxTreeCompiler(LanguageSyntax syntax)
        {
            Syntax = syntax;
        }

        public virtual string Build(SyntaxNode tree)
        {
            if (tree.IsRawString)
                return tree.TagName;
            if (tree.TagName == null)
                return string.Join("", tree.NestedNodes.OrderTagsInGroups(Syntax).Select(Build));
            var construction = Syntax.GetTag(tree.TagName);
            var nestedString = string.Join("", tree.NestedNodes.OrderTagsInGroups(Syntax).Select(Build));
            return construction.Begin.Lexem + nestedString + construction.End.Lexem;
        }

        public string Build(SyntaxNode tree, string metaUrl)
        {
            var newTree = InsertMetaUrl(tree, metaUrl);
            return Build(newTree);
        }

        private SyntaxNode InsertMetaUrl(SyntaxNode root, string metaUrl)
        {
            if (root.IsRawString) return root;
            var tag = SyntaxNode.CreateTag(root.TagName);
            foreach (var nested in root.NestedNodes)
            {
                if (nested.TagName == "url.address")
                {
                    if (nested.NestedNodes.Count() == 0 || nested.NestedNodes.First().TagName.StartsWith("/"))
                    {
                        var url = SyntaxNode.CreateTag("url.address");
                        url.AddNestedNode(SyntaxNode.CreateRawString(metaUrl));
                        url.AddManyNestedNode(nested.NestedNodes);
                        tag.AddNestedNode(url);
                    }
                    else
                    {
                        tag.AddNestedNode(InsertMetaUrl(nested, metaUrl));
                    }
                }
                else
                {
                    tag.AddNestedNode(InsertMetaUrl(nested, metaUrl));
                }
            }
            return tag;
        }
    }
}