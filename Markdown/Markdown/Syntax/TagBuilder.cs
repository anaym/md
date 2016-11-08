using System.Collections.Generic;
using System.Linq;
using Markdown.Utility;

namespace Markdown.Syntax
{
    public class TagBuilder
    {
        public string TagName { get; set; }
        public Template Begin { get; set; }
        public Template End { get; set; }
        public List<string> NestedTags { get; }
        public bool IsRootableTag;

        public TagBuilder(string tagName, Template begin, Template end, IEnumerable<string> nestedTags)
        {
            TagName = tagName;
            Begin = begin;
            End = end;
            NestedTags = nestedTags.ToList();
        }

        public TagBuilder(string tagName = null, Template begin = null, Template end = null)
            : this(tagName, begin, end, new string[0])
        { }

        public TagBuilder(Tag tag)
            : this(tag.Name, tag.Begin, tag.End, tag.NestedTags)
        { }

        public Tag Build() => new Tag(TagName, Begin, End, NestedTags);
    }
}
