using System.Collections.Generic;
using System.Xml;

namespace Markdown
{
    public class SyntaxNode
    {
        public readonly string Lexem;
        public readonly List<SyntaxNode> NestesNodes;
        public readonly bool IsTag;

        public static SyntaxNode CreateTag(string tag) => new SyntaxNode(tag, true);
        public static SyntaxNode CreateRawString(string rawString) => new SyntaxNode(rawString, false);

        public SyntaxNode(string lexem, bool isTag)
        {
            this.Lexem = lexem;
            IsTag = isTag;
            NestesNodes = new List<SyntaxNode>();
        }

        public override string ToString() => Lexem;
    }
}