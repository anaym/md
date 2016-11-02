using System.Collections.Generic;
using System.Linq;
using Markdown.Syntax;
using Markdown.StringParser;

namespace Markdown
{
    public class Machine
    {
        public static readonly string RootTag = null;

        //TODO: надо перенатроить решарпер, чтобы не предлагал писать поля с _
        private readonly Syntax.LanguageSyntax languageSyntax;
        private readonly Stack<SintacticNode> tagStack;
        
        public readonly ParsedString String;
        public readonly SintacticNode Root;

        public int Position;

        public SintacticNode NowNode => tagStack.Peek();
        public string NowTag => NowNode.Lexem;
        public Construction NowConstruction => NowTag == RootTag ? null : languageSyntax.GetConstruction(NowTag);
        public IEnumerable<Construction> NowAvaibleConstructions => languageSyntax.GetAvaibleConstructions(NowTag);

        public void BackOnStack() => tagStack.Pop();
        public void AddNestedNode(SintacticNode node) => NowNode.NestesNodes.Add(node);
        public void ForwardOnStack() => tagStack.Push(NowNode.NestesNodes.Last());

        public Machine(string source, Syntax.LanguageSyntax languageSyntax)
        {
            this.languageSyntax = languageSyntax;
            String  = new ParsedString(source, languageSyntax.Escape);
            Position = 0;
            tagStack = new Stack<SintacticNode>();
            Root = SintacticNode.CreateTag(RootTag);
            tagStack.Push(Root);
        }
    }
}