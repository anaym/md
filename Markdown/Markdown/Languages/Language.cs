using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Languages.Exteptions;
using Markdown.Syntax;
using Markdown.Utility;

namespace Markdown.Languages
{
    public class Language
    {
        public readonly LanguageSyntax Syntax;

        public Language(LanguageSyntax syntax)
        {
            Syntax = syntax;
        }

        public SyntaxNode Parse(string line)
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
            // CR: Never ever-ever use tuples. E.g. here you can use anonymus
            // objects. Also LINQ is only making this piece of code worse.
            // Try rewriting it in a more readable way
            var beginNested = parsingState
                .CurrentAvailableTags
                .Select(c => Tuple.Create(c, c.Begin.Find(parsingState.String, parsingState.Position)))
                .Where(p => p.Item2 != null)
                .OrderBy(p => p.Item2.Value)
                .FirstOrDefault(p => p.Item1.IsAt(parsingState.String, p.Item2.Value))
                ?.Item2;

            // CR: 170 chars line is too much, try to stick to 130
            var endNow = parsingState.CurrentTagName == LanguageSyntax.RootTagName ? null : parsingState.CurrentTag.End.Find(parsingState.String, parsingState.Position);

            if (endNow == null)
            {
                return beginNested ?? parsingState.String.Length;
            }
            // Nit: Redundant else
            else
            {
                return beginNested == null ? endNow.Value : Math.Min(endNow.Value, beginNested.Value);
            }
        }

        // CR: Public methods should be before private ones
        public string Build(SyntaxNode tree)
        {
            if (tree.IsRawString)
                return tree.TagName;
            if (tree.TagName == null)
                return string.Join("", tree.NestedNodes.OrderTagsInGroups(Syntax).Select(Build));
            var construction = Syntax.GetTag(tree.TagName);
            // CR: Extract variable
            return construction.Begin.Lexem + string.Join("", tree.NestedNodes.OrderTagsInGroups(Syntax).Select(Build)) + construction.End.Lexem;
        }
    }
}
