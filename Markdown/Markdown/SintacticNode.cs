using System.Collections.Generic;

namespace Markdown
{
    public class SintacticNode
    {
        public readonly string Lexem;
        public readonly List<SintacticNode> NestesNodes;
        public bool IsEnd => NestesNodes.Count == 0;

        public SintacticNode(string lexem)
        {
            this.Lexem = lexem;
            NestesNodes = new List<SintacticNode>();
        }
    }
}