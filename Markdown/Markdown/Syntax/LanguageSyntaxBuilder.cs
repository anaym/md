using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Syntax
{
    public class LanguageSyntaxBuilder : IEnumerable<TagBuilder>
    {
        public readonly Dictionary<string, TagBuilder> Constructions;
        public readonly HashSet<string> RootTags;

        public char EscapeChar;

        public LanguageSyntaxBuilder()
        {
            Constructions = new Dictionary<string, TagBuilder>();
            RootTags = new HashSet<string>();
        }

        public LanguageSyntax Build() => new LanguageSyntax(RootTags, Constructions.Select(p => p.Value.Build()), EscapeChar);

        #region Operations
        public void Add(TagBuilder tag)
        {
            if (Constructions.ContainsKey(tag.TagName))
                Constructions[tag.TagName] = tag;
            else
                Constructions.Add(tag.TagName, tag);
            if (tag.IsRootableTag)
                RootTags.Add(tag.TagName);
        }
        public void Remove(TagBuilder tag) => Remove(tag.TagName);
        public void Remove(string tag) => Constructions.Remove(tag);
        public TagBuilder Get(string tag) => Constructions[tag];
        public bool Contains(TagBuilder tag) => Contains(tag.TagName);
        public bool Contains(string tag) => Constructions.ContainsKey(tag);
        #endregion

        #region Operators

        public static LanguageSyntaxBuilder operator +(LanguageSyntaxBuilder self, TagBuilder tag)
        {
            self.Add(tag);
            return self;
        }

        public static LanguageSyntaxBuilder operator -(LanguageSyntaxBuilder self, TagBuilder tag)
        {
            self.Remove(tag);
            return self;
        }

        #endregion

        public IEnumerator<TagBuilder> GetEnumerator()
        {
            return Constructions.Select(p => p.Value).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
