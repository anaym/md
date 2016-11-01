using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Markdown.Sintactic
{
    public class Construction
    {
        public string Tag { get; }
        public string Begin { get; }
        public string End { get; }
        public IEnumerable<string> NestedTags => nestedTags;

        private readonly List<string> nestedTags;

        /// <exception cref="ArgumentNullException"><paramref name="tag"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">Condition.</exception>
        public Construction(string tag, string begin, string end, IEnumerable<string> nestedTags)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            if (begin == null) throw new ArgumentNullException(nameof(begin));
            if (end == null) throw new ArgumentNullException(nameof(end));
            if (end == "") throw new ArgumentException(nameof(end));
            if (begin == "") throw new ArgumentException(nameof(begin));
            if (tag == "") throw new ArgumentException(nameof(tag));

            Tag = tag;
            Begin = begin;
            End = end;
            this.nestedTags = nestedTags.ToList();
        }
    }
}