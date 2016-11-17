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
        public string GroupName { get; set; }
        public int GroupIndex { get; set; }
        public bool IsRootableTag;

        public TagBuilder(string tagName, Template begin, Template end, IEnumerable<string> nestedTags, int groupIndex, string groupName)
        {
            TagName = tagName;
            Begin = begin;
            End = end;
            GroupName = groupName;
            NestedTags = nestedTags.ToList();
        }

        public TagBuilder(string tagName = null)
            : this(tagName, null, null, new string[0], 0, null)
        { }

        public Tag Build() => new Tag(TagName, Begin, End, NestedTags, GroupIndex, GroupName);
    }
}
