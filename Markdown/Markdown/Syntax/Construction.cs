using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Markdown.StringParser;

namespace Markdown.Syntax
{
    public class Construction
    {
        public string Tag { get; }
        public Border Begin { get; }
        public Border End { get; }
        public IEnumerable<string> NestedTags => nestedTags;

        private readonly List<string> nestedTags;

        public bool Is(ParsedString str, int pos)
        {
            if (!Begin.Is(str, pos)) return false;
            return End.Find(str, pos + Begin.Length) != null;
        }

        //потому что запрещены regexp-ы
        /// <exception cref="ArgumentNullException"><paramref name="tag"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">Condition.</exception>
        public Construction(string tag, Border begin, Border end, IEnumerable<string> nestedTags)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            if (begin == null) throw new ArgumentNullException(nameof(begin));
            if (end == null) throw new ArgumentNullException(nameof(end));
            if (tag == "") throw new ArgumentException(nameof(tag));

            Tag = tag;
            Begin = begin;
            End = end;
            this.nestedTags = nestedTags.ToList();
        }
    }
}