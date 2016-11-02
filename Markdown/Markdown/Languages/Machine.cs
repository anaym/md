using System.Collections.Generic;
using System.Linq;
using Markdown.StringParser;
using Markdown.Syntax;

namespace Markdown.Languages
{
    internal class Machine
    {
        public static readonly string RootTag = null;

        //TODO: надо перенатроить решарпер, чтобы не предлагал писать поля с _
        private readonly Syntax.LanguageSyntax languageSyntax;
        private readonly Stack<SyntaxNode> tagStack;
        
        public readonly ParsedString String;
        public readonly SyntaxNode Root;

        public int Position;

        public SyntaxNode NowNode => tagStack.Peek();
        public string NowTag => NowNode.Lexem;
        public Construction NowConstruction => NowTag == RootTag ? null : languageSyntax.GetConstruction(NowTag);
        public IEnumerable<Construction> NowAvaibleConstructions => languageSyntax.GetAvaibleConstructions(NowTag);

        public void BackOnStack() => tagStack.Pop();
        public void AddNestedNode(SyntaxNode node) => NowNode.NestesNodes.Add(node);
        public void ForwardOnStack() => tagStack.Push(NowNode.NestesNodes.Last());

        public Machine(string source, Syntax.LanguageSyntax languageSyntax)
        {
            this.languageSyntax = languageSyntax;
            String  = new ParsedString(source, languageSyntax.Escape);
            Position = 0;
            tagStack = new Stack<SyntaxNode>();
            Root = SyntaxNode.CreateTag(RootTag);
            tagStack.Push(Root);
        }
    }
}