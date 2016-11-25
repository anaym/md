using System.Linq;
using Markdown.Syntax;

namespace Markdown.Converters
{
    public class SyntaxTreeBuilder
    {
        public readonly LanguageSyntax Syntax;

        public SyntaxTreeBuilder(LanguageSyntax syntax)
        {
            Syntax = syntax;
        }

        public virtual string Build(SyntaxNode tree)
        {
            return BuildTree(tree).TrimEnd('\n');
        }

        public string Build(SyntaxNode tree, string metaUrl)
        {
            var newTree = InsertMetaUrl(tree, metaUrl);
            return Build(newTree);
        }

        protected virtual string BuildTree(SyntaxNode tree)
        {
            if (tree.IsRawString)
                return tree.TagName;
            if (tree.TagName == null)
                return string.Join("", tree.NestedNodes.OrderTagsInGroups(Syntax).Select(BuildTree));
            var construction = Syntax.GetTag(tree.TagName);
            var nestedString = string.Join("", tree.NestedNodes.OrderTagsInGroups(Syntax).Select(BuildTree));
            return construction.Begin.Lexem + nestedString + construction.End.Lexem;
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