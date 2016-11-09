using System.Collections.Generic;
using System.Linq;

namespace Markdown
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
        public void AddNestedNode(SyntaxNode node) => nestedNodes.Add(node);

        public SyntaxNode(string tagName, bool isTag)
        {
            this.TagName = tagName;
            IsTag = isTag;
            nestedNodes = new List<SyntaxNode>();
        }

        public override string ToString() => TagName;
    }
}