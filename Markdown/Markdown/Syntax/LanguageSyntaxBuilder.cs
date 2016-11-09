using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Syntax
{
    public class LanguageSyntaxBuilder
    {
        public readonly HashSet<TagBuilder> Tags;
        public readonly HashSet<string> RootTags;

        public readonly char EscapeChar;

        public LanguageSyntaxBuilder(char escapeChar)
        {
            EscapeChar = escapeChar;
            Tags = new HashSet<TagBuilder>();
            RootTags = new HashSet<string>();
        }

        public LanguageSyntax Build() => new LanguageSyntax(RootTags, Tags.Select(p => p.Build()), EscapeChar);

        public void Add(TagBuilder tag)
        {
            Tags.Add(tag);
            if (tag.IsRootableTag)
                RootTags.Add(tag.TagName);
        }

        public static LanguageSyntaxBuilder operator +(LanguageSyntaxBuilder self, TagBuilder tag)
        {
            self.Add(tag);
            return self;
        }
    }
}
