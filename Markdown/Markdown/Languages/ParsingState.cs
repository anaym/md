using System.Collections.Generic;
using System.Linq;
using Markdown.StringParser;
using Markdown.Syntax;

namespace Markdown.Languages
{
    internal class ParsingState
    {
        public static readonly string RootTagName = null;

        // CR (krait): Всё ещё не перенастроил? :)
        //TODO: надо перенатроить решарпер, чтобы не предлагал писать поля с _
        private readonly LanguageSyntax languageSyntax;
        private readonly Stack<SyntaxNode> tagStack;
        
        public readonly EscapedString String;
        public readonly SyntaxNode Root;

        // CR (krait): Нехорошо так просто выставлять наружу голые поля. Оберни в проперти хотя бы.
        public int Position;
        public bool IsCompleted => Position >= String.Length;

        public SyntaxNode CurrentNode => tagStack.Peek();
        public string CurrentTagName => CurrentNode.TagName;
        public Tag CurrentTag
            => CurrentTagName == RootTagName ? null : languageSyntax.GetTag(CurrentTagName);
        public IEnumerable<Tag> CurrentAvaibleTags
            => languageSyntax.GetAvaibleTags(CurrentTagName);

        public bool IsNowTagClose
            => CurrentTagName != RootTagName && CurrentTag.End.IsMatch(String, Position);

        //название метода противоположно тому, что он делает. Кажется, что он что-то возвращает на стек, но он снимает последний элемент
        // CR (krait): Кажется, тут всё в порядке с названием: когда смотришь на вызов этого метода снаружи, представляешь себе именно дерево, а то, что тут внутри стек - просто детали реализации.
        public void UpInTree() => tagStack.Pop();
        // CR (krait): Лучше сделать ещё метод SyntaxNode.AddNestedNode, а то получается не очень красиво: откуда-то снаружи добавляем элементы в readonly-поле.
        public void AddNestedNode(SyntaxNode node) => CurrentNode.NestedNodes.Add(node);
        public void DownInTree() => tagStack.Push(CurrentNode.NestedNodes.Last());

        public ParsingState(string source, LanguageSyntax languageSyntax)
        {
            this.languageSyntax = languageSyntax;
            String  = new EscapedString(source, languageSyntax.EscapeChar);
            Position = 0;
            tagStack = new Stack<SyntaxNode>();
            Root = SyntaxNode.CreateTag(RootTagName);
            tagStack.Push(Root);
        }
    }
}