using System.Collections.Generic;
using System.Linq;

namespace Markdown.Syntax
{
    public class SyntaxNode
    {
        public readonly string TagName;
        private readonly List<SyntaxNode> nestedNodes;
        public readonly bool IsTag;
        public bool IsRawString => !IsTag;

        public int Size => 1 + NestedNodes.Sum(n => n.Size);

        public static SyntaxNode CreateTag(string tag) => new SyntaxNode(tag, true);
        public static SyntaxNode CreateRawString(string rawString) => new SyntaxNode(rawString, false);

        public IEnumerable<SyntaxNode> NestedNodes => nestedNodes;
        public SyntaxNode AddNestedNode(SyntaxNode node)
        {
            nestedNodes.Add(node);
            return this;
        }

        public SyntaxNode AddManyNestedNode(IEnumerable<SyntaxNode> node)
        {
            nestedNodes.AddRange(node);
            return this;
        }

        public SyntaxNode(string tagName, bool isTag)
        {
            TagName = tagName;
            IsTag = isTag;
            nestedNodes = new List<SyntaxNode>();
        }

        public override string ToString() => TagName;
    }
}