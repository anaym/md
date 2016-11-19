using System;
using System.Linq;
using Markdown.Syntax;

namespace Markdown.Languages
{
    public class Language
    {
        public readonly LanguageSyntax Syntax;

        public Language(LanguageSyntax syntax)
        {
            Syntax = syntax;
        }

        public virtual SyntaxNode Parse(string line)
        {
            var parsingState = new ParsingState(line, Syntax);
            while (!parsingState.IsCompleted)
            {
                parsingState.Position = ReadNextTag(parsingState);
            }
            if (!parsingState.AllTagsClosed)
                throw new ParseException($"Not all tags have been closed: {{{string.Join(", ", parsingState.CurrentOpenedTags)}}}");
            return parsingState.Root.RevertParseForIncompleteGroups(this);
        }

        public virtual string Build(SyntaxNode tree)
        {
            if (tree.IsRawString)
                return tree.TagName;
            if (tree.TagName == null)
                return string.Join("", tree.NestedNodes.OrderTagsInGroups(Syntax).Select(Build));
            var construction = Syntax.GetTag(tree.TagName);
            var nestedString = string.Join("", tree.NestedNodes.OrderTagsInGroups(Syntax).Select(Build));
            return construction.Begin.Lexem + nestedString + construction.End.Lexem;
        }

        private int ReadNextTag(ParsingState parsingState)
        {
            var pos = ReadEndOfCurrentTag(parsingState) ?? 
                ReadBeginOfNestedTag(parsingState) ?? 
                ReadRawText(parsingState);
            if (pos == null)
                throw new InvalidOperationException("Bad string");
            return pos.Value;
        }

        private static int? ReadEndOfCurrentTag(ParsingState parsingState)
        {
            if (!parsingState.IsNowTagClose)
                return null;

            var newIndex = parsingState.CurrentTag.End.Length + parsingState.Position;
            parsingState.UpInTree();
            return newIndex;
        }

        private static int? ReadBeginOfNestedTag(ParsingState parsingState)
        {
            var next = parsingState
                .CurrentAvailableTags
                .FirstOrDefault(c => c.IsAt(parsingState.String, parsingState.Position));

            if (next == null)
                return null;
            parsingState.AddNestedNode(SyntaxNode.CreateTag(next.Name));
            parsingState.DownInTree();
            return parsingState.Position + parsingState.CurrentTag.Begin.Length;
        }

        private static int? ReadRawText(ParsingState parsingState)
        {
            var end = FindEndOfRawText(parsingState);
            var raw = parsingState.String.ToString().Substring(parsingState.Position, end - parsingState.Position);
            parsingState.AddNestedNode(SyntaxNode.CreateRawString(raw));
            return end;
        }

        private static int FindEndOfRawText(ParsingState parsingState)
        {
            var beginNested = FindBeginOfNested(parsingState);

            var endNow = parsingState.CurrentTagName == LanguageSyntax.RootTagName
                ? null
                : parsingState.CurrentTag.End.Find(parsingState.String, parsingState.Position);

            if (endNow == null)
            {
                return beginNested ?? parsingState.String.Length;
            }
            return beginNested == null ? endNow.Value : Math.Min(endNow.Value, beginNested.Value);
        }

        private static int? FindBeginOfNested(ParsingState parsingState)
        {
            var beginableNested = parsingState
                .CurrentAvailableTags
                .Where(t => t.IsAt(parsingState.String, t.Begin.Find(parsingState.String, parsingState.Position) ?? -1))
                .ToList();

            if (beginableNested.Count == 0) return null;

            return beginableNested.Select(t => t.Begin.Find(parsingState.String, parsingState.Position)).Min();
        }
    }
}
