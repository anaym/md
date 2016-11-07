using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DLibrary.Enumerations;

namespace Markdown.Syntax
{
    public class LanguageSyntax
    {
        private readonly ImmutableDictionary<string, Tag> tags;
        private readonly ImmutableHashSet<string> rootTags;

        public readonly char EscapeChar;
        public readonly ImmutableHashSet<string> Alphabet;

        public LanguageSyntax(IEnumerable<string> rootTags, IEnumerable<Tag> tags, char escapeChar)
        {
            EscapeChar = escapeChar;
            this.rootTags = rootTags.ToImmutableHashSet();
            this.tags = tags.ToImmutableDictionary(c => c.Name, c => c);
            Alphabet = this.tags.Select(p => p.Key).Union(this.rootTags).ToImmutableHashSet();
            var notDescribedTags = Alphabet.Except(this.tags.Select(p => p.Key));
            if (notDescribedTags.Count != 0)
            {
                // CR (krait): Загадочное сообщение.
                throw new InvalidOperationException($"Not all tags describes in as construction: {notDescribedTags.SequenceToString()}"); 
            }
        }

        public Tag GetTag(string tag)
        { 
            return tags[tag];
        }

        // CR (krait): available
        public IEnumerable<Tag> GetTagsAvaibleInRoot()
        {
            return rootTags.Select(GetTag);
        }

        // CR (krait): currentTag
        public IEnumerable<Tag> GetAvaibleTags(string nowTag)
        {
            // CR (krait): Для имени root-тега где-то была объявлена константа. Надо уж либо её везде использовать, либо её убрать и использовать null.
            if (nowTag == null) return GetTagsAvaibleInRoot();
            return tags[nowTag].NestedTags.Select(GetTag);
        }
    }
}
