using System.Collections.Generic;

namespace Markdown
{
    public class SyntaxNode
    {
        public readonly string TagName;
        public readonly List<SyntaxNode> NestedNodes;
        public readonly bool IsTag;
        public bool IsRawString => !IsTag;

        public static SyntaxNode CreateTag(string tag) => new SyntaxNode(tag, true);
        public static SyntaxNode CreateRawString(string rawString) => new SyntaxNode(rawString, false);

        public SyntaxNode(string tagName, bool isTag)
        {
            this.TagName = tagName;
            IsTag = isTag;
            NestedNodes = new List<SyntaxNode>();
        }

        public override string ToString() => TagName;
    }
}