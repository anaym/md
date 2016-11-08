using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.StringParser;
using Markdown.Utility;

namespace Markdown.Syntax
{
    public class Tag
    {
        public string Name { get; }
        public Template Begin { get; }
        public Template End { get; }
        public IEnumerable<string> NestedTags => nestedTags;

        private readonly List<string> nestedTags;

        public bool IsAt(EscapedString str, int pos)
        {
            if (!Begin.IsMatch(str, pos)) return false;
            return End.Find(str, pos + Begin.Length) != null;
        }

        //потому что запрещены regexp-ы
        public Tag(string name, Template begin, Template end, IEnumerable<string> nestedTags)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (begin == null) throw new ArgumentNullException(nameof(begin));
            if (end == null) throw new ArgumentNullException(nameof(end));
            if (name == "") throw new ArgumentException(nameof(name));

            Name = name;
            Begin = begin;
            End = end;
            this.nestedTags = nestedTags.ToList();
        }
    }
}