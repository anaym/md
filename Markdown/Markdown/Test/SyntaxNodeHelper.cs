using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal static class SyntaxNodeHelper
    {
        public static IEnumerable<SyntaxNode> ConnectRaw(this IEnumerable<SyntaxNode> nodes, string raw)
            => nodes.Union(new[] {SyntaxNode.CreateRawString(raw)});

        public static IEnumerable<SyntaxNode> ConnectTag(this IEnumerable<SyntaxNode> nodes, string tag, IEnumerable<SyntaxNode> nested)
        {
            var node = SyntaxNode.CreateTag(tag);
            foreach (var i in nested) node.AddNestedNode(i);
            return nodes.Union(new[] {node});
        }

        public static IEnumerable<SyntaxNode> ConnectTag(this IEnumerable<SyntaxNode> nodes, string tag, params SyntaxNode[] nested)
            => nodes.ConnectTag(tag, (IEnumerable<SyntaxNode>) nested);
    }
}