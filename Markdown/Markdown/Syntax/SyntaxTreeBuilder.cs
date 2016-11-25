using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Syntax
{
    public class LanguageSyntaxBuilder
    {
        public readonly HashSet<TagBuilder> Tags;
        public readonly char EscapeChar;

        public LanguageSyntaxBuilder(char escapeChar)
        {
            EscapeChar = escapeChar;
            Tags = new HashSet<TagBuilder>();
        }

        public LanguageSyntax Build() => new LanguageSyntax(Tags.Select(p => p.Build()), EscapeChar);

        public void Add(TagBuilder tag)
        {
            Tags.Add(tag);
        }

        public static LanguageSyntaxBuilder operator +(LanguageSyntaxBuilder self, TagBuilder tag)
        {
            self.Add(tag);
            return self;
        }
    }
}
