using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLibrary.Enumerations;
using Markdown.Sintactic;

namespace Markdown
{
    public class Language
    {
        public readonly Sintactic.Sintactic Sintactic;

        /// <exception cref="Exception">Condition.</exception>
        /// <exception cref="ArgumentNullException">Значение параметра <paramref name="source" /> или <paramref name="selector" /> — null.</exception>
        /// <exception cref="ArgumentException">Функция <paramref name="keySelector" /> выдает дубликаты ключей для двух элементов.</exception>
        public Language(Sintactic.Sintactic sintactic)
        {
            Sintactic = sintactic;
        }

        public SintacticNode Parse(string text)
        {
            var tagStack = new Stack<SintacticNode> ();
            var root = new SintacticNode(null);
            tagStack.Push(root);
            while (text.Length > 0)
            {
                text = ReadNext(tagStack, text);
            }
            return root;
        }

        private string ReadNext(Stack<SintacticNode> tagStack, string text)
        {
            return ReadEndOfThisTag(tagStack, text) ?? ReadNestedTag(tagStack, text) ?? ReadRawText(tagStack, text);
        }

        private string ReadEndOfThisTag(Stack<SintacticNode> tagStack, string text)
        {
            var now = tagStack.Peek();
            if (now.Lexem == null)
                return null;
            var end = Sintactic.GetConstruction(now.Lexem).End;
            if (!text.StartsWith(end)) return null;
            tagStack.Pop();
            return text.Substring(end.Length);
        }

        private string ReadNestedTag(Stack<SintacticNode> tagStack, string text)
        {
            var now = tagStack.Peek();
            var next = Sintactic
                .GetAvaibleConstructions(now.Lexem)
                .OrderByDescending(c => c.Begin.Length)
                .FirstOrDefault(c => text.StartsWith(c.Begin));
            if (next == null) return null;

            text = text.Substring(next.Begin.Length);
            var newNode = new SintacticNode(next.Tag);
            now.NestesNodes.Add(newNode);
            tagStack.Push(newNode);
            return text;
        }

        private string ReadRawText(Stack<SintacticNode> tagStack, string text)
        {
            var end = FindRawTextEnd(tagStack, text);
            if (end < 0)
                throw new Exception($"Incorrect string: end of tags is not founded: {tagStack.SequenceToString(t => t.Lexem)}");
            var raw = text.Substring(0, end);
            var now = tagStack.Peek();
            now.NestesNodes.Add(new SintacticNode(raw));
            return text.Substring(end);
        }

        private int FindRawTextEnd(Stack<SintacticNode> tagStack, string text)
        {
            var now = tagStack.Peek();
            var begin = Sintactic
                .GetAvaibleConstructions(now.Lexem)
                .Select(c => text.IndexOf(c.Begin, StringComparison.Ordinal))
                .Where(i => i >= 0)
                .MinOrDefault(-1);
            var end = now.Lexem == null ? text.Length : text.IndexOf(Sintactic.GetConstruction(now.Lexem).End, StringComparison.Ordinal);
            if (end < 0)
                return begin;
            if (begin < 0)
                return end;
            return Math.Min(begin, end);
        }

        public string Build(SintacticNode tree)
        {
            if (tree.IsEnd)
                return tree.Lexem;
            if (tree.Lexem == null)
                return tree.NestesNodes.SequenceToString(Build, "", "", "");
            var construction = Sintactic.GetConstruction(tree.Lexem);
            return construction.Begin + tree.NestesNodes.SequenceToString(Build, "", "", "") + construction.End;
        }
    }
}
