using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DLibrary.Enumerations;

namespace Markdown.Syntax
{
    public class LanguageSyntax
    {
        public static readonly string RootTagName = null;


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
                throw new IncorrectSyntaxException($"Not all tags has been described: {notDescribedTags.SequenceToString()}"); 
            }
        }

        public Tag GetTag(string tag)
        { 
            return tags[tag];
        }

        public IEnumerable<Tag> GetTagsAvailableInRoot()
        {
            return rootTags.Select(GetTag);
        }

        public IEnumerable<Tag> GetAvailableTags(string currentTag)
        {
            if (currentTag == RootTagName) return GetTagsAvailableInRoot();
            return tags[currentTag].NestedTags.Select(GetTag);
        }
    }
}
