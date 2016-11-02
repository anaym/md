using System.Collections.Generic;
using System.Linq;
using Markdown.Sintactic;
using Markdown.StringParser;

namespace Markdown
{
    public class Machine
    {
        private readonly Sintactic.Sintactic sintactic;
        private readonly Stack<SintacticNode> tagStack;
        
        public readonly ParsedString String;
        public int Position;
        public readonly SintacticNode Root;

        public static readonly string RootTag = null;

        public SintacticNode NowNode => tagStack.Peek();
        public string NowTag => NowNode.Lexem;
        public Construction NowConstruction => NowTag == RootTag ? null : sintactic.GetConstruction(NowTag);
        public IEnumerable<Construction> NowAvaibleConstructions => sintactic.GetAvaibleConstructions(NowTag);

        public void BackOnStack() => tagStack.Pop();
        public void AddNested(SintacticNode node) => NowNode.NestesNodes.Add(node);
        public void ForwardOnStack() => tagStack.Push(NowNode.NestesNodes.Last());

        public Machine(string source, Sintactic.Sintactic sintactic)
        {
            this.sintactic = sintactic;
            String  = new ParsedString(source, sintactic.Escape);
            Position = 0;
            tagStack = new Stack<SintacticNode>();
            Root = new SintacticNode(RootTag);
            tagStack.Push(Root);
        }
    }
}