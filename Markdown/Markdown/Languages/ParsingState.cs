using System.Collections.Generic;
using System.Linq;
using Markdown.StringParser;
using Markdown.Syntax;

namespace Markdown.Languages
{
    internal class ParsingState
    {
        private readonly LanguageSyntax languageSyntax;
        private readonly Stack<SyntaxNode> tagStack;

        public IEnumerable<string> CurrentOpenedTags => tagStack.Select(n => n.TagName).Where(s => !string.IsNullOrEmpty(s));

        public readonly EscapedString String;
        public readonly SyntaxNode Root;

        public int Position { get; set; }
        public bool AllTagsClosed => tagStack.Count <= 1;
        public bool IsCompleted => Position >= String.Length;

        public SyntaxNode CurrentNode => tagStack.Peek();
        public string CurrentTagName => CurrentNode.TagName;
        public Tag CurrentTag
            => CurrentTagName == LanguageSyntax.RootTagName ? null : languageSyntax.GetTag(CurrentTagName);
        
        public IEnumerable<Tag> CurrentAvailableTags
            => languageSyntax.GetAvailableTags(CurrentTagName);

        public bool IsNowTagClose
            => CurrentTagName != LanguageSyntax.RootTagName && CurrentTag.End.IsMatch(String, Position);

        public void UpInTree() => tagStack.Pop();
        public void AddNestedNode(SyntaxNode node) => CurrentNode.AddNestedNode(node);
        public void DownInTree() => tagStack.Push(CurrentNode.NestedNodes.Last());

        public ParsingState(string source, LanguageSyntax languageSyntax)
        {
            this.languageSyntax = languageSyntax;
            String  = new EscapedString(source, languageSyntax.EscapeChar);
            Position = 0;
            tagStack = new Stack<SyntaxNode>();
            Root = SyntaxNode.CreateTag(LanguageSyntax.RootTagName);
            tagStack.Push(Root);
        }
    }
}