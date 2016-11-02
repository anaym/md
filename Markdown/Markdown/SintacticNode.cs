using System.Collections.Generic;

namespace Markdown
{
    public class SintacticNode
    {
        public readonly string Lexem;
        public readonly List<SintacticNode> NestesNodes;
        public readonly bool IsTag;

        public static SintacticNode CreateTag(string tag) => new SintacticNode(tag, true);
        public static SintacticNode CreateRawString(string rawString) => new SintacticNode(rawString, false);

        public SintacticNode(string lexem, bool isTag)
        {
            this.Lexem = lexem;
            IsTag = isTag;
            NestesNodes = new List<SintacticNode>();
        }

        public override string ToString() => Lexem;
    }
}