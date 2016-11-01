using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Sintactic
{
    public class ConstructionBuilder
    {
        public string Tag { get; set; }
        public string Begin { get; set; }
        public string End { get; set; }
        public List<string> NestedTags { get; }

        public ConstructionBuilder(string tag, string begin, string end, IEnumerable<string> nestedTags)
        {
            Tag = tag;
            Begin = begin;
            End = end;
            NestedTags = nestedTags.ToList();
        }

        public ConstructionBuilder(string tag = null, string begin = null, string end = null)
            : this(tag, begin, end, new string[0])
        { }

        public ConstructionBuilder(Construction construction)
            : this(construction.Tag, construction.Begin, construction.End, construction.NestedTags)
        { }

        public Construction Build() => new Construction(Tag, Begin, End, NestedTags);
    }
}
