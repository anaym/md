using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Markdown.StringParser;
using Markdown.Syntax;

namespace Markdown.Converters
{
    public class MultilineSyntaxTreeParser : SyntaxTreeParser
    {
        public MultilineSyntaxTreeParser(LanguageSyntax syntax) : base(syntax)
        {
        }

        public SyntaxNode ParseMultiline(string text)
        {
            var lines = text.Split('\n');
            if (lines.Length < 2)
                return Parse(text);
            string prev = null;
            Tag currentTag = null;
            var result = new List<SyntaxNode>();
            var buffer = new List<SyntaxNode>();
            for (int i = 0; i < lines.Length; i++)
            {
                var next = i < lines.Length - 1 ? lines[i + 1] : null;
                var current = lines[i] + '\n';
                var env = EnviromentTypeHelper.GetType(prev, next);
                var parsed = new ParsedString(current, Syntax.EscapeChar);

                ReadNext(ref currentTag, parsed, env, current, buffer, result);
                if (currentTag == null)
                {
                    currentTag = ReadNewGlobalTag(parsed, env);
                    if (currentTag == null)
                    {
                        result.AddRange(Parse(current).NestedNodes);
                    }
                    else
                    {
                        current = current.Substring(currentTag.Begin.Length, current.Length);
                        parsed = new ParsedString(current, Syntax.EscapeChar);
                        ReadNext(ref currentTag, parsed, env, current, buffer, result);
                    }
                }
                prev = current;
            }
            if (currentTag != null)
            {
                result.Add(SyntaxNode.CreateTag(currentTag.Name).AddManyNestedNode(buffer));
            }
            return SyntaxNode.CreateTag(null).AddManyNestedNode(result);
        }

        private void ReadNext(ref Tag currentTag, ParsedString parsed, EnviromentType env, string current, List<SyntaxNode> buffer, List<SyntaxNode> result)
        {
            if (currentTag != null)
            {
                if (ReadCloseOfTag(parsed, currentTag, env))
                {
                    buffer.AddRange(Parse(current.Substring(0, current.Length - currentTag.End.Length)).NestedNodes);
                    result.Add(SyntaxNode.CreateTag(currentTag.Name).AddManyNestedNode(buffer));
                    buffer.Clear();
                    currentTag = null;
                }
                else
                {
                    buffer.AddRange(Parse(current.Substring(0, current.Length - 1)).NestedNodes);
                }
            }
        }

        private Tag ReadNewGlobalTag(ParsedString parsed, EnviromentType enviroment)
        {
            var available = Syntax.GlobalTags.Select(Syntax.GetTag).Where(t => t.Enviroment.HasFlag(enviroment)).ToList();
            var started = available.Where(t => t.Begin.IsMatch(parsed)).OrderByDescending(t => t.Begin.Length).ToList();
            return started.FirstOrDefault();
        }

        private bool ReadCloseOfTag(ParsedString parsed, Tag current, EnviromentType enviroment)
        {
            if (current.Enviroment.HasFlag(enviroment))
            {
                if (current.End.IsMatch(parsed, parsed.Length - current.End.Length))
                {
                    return true;
                }
            }
            return false;
        }
    }
}