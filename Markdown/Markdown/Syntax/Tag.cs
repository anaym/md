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
        public string GroupName { get; }
        public int GroupIndex { get; }

        private readonly List<string> nestedTags;

        public bool IsAt(ParsedString str, int pos)
        {
            if (pos < 0) return false;
            if (!Begin.IsMatch(str, pos)) return false;
            return End.Find(str, pos + Begin.Length) != null;
        }

        //потому что запрещены regexp-ы
        public Tag(string name, Template begin, Template end, IEnumerable<string> nestedTags, int groupIndex, string groupName)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (begin == null) throw new ArgumentNullException(nameof(begin));
            if (end == null) throw new ArgumentNullException(nameof(end));
            if (name == "") throw new ArgumentException(nameof(name));

            Name = name;
            Begin = begin;
            End = end;
            GroupIndex = groupIndex;
            this.nestedTags = nestedTags?.ToList() ?? new List<string>();
            GroupName = groupName;
        }
    }
}