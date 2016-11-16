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
        private readonly ImmutableDictionary<string, ImmutableList<Tag>> groups;

        public readonly char EscapeChar;
        public readonly ImmutableHashSet<string> Alphabet;

        public readonly ImmutableHashSet<string> Groups;


        public LanguageSyntax(IEnumerable<string> rootTags, IEnumerable<Tag> tags,  char escapeChar)
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

            groups = ImmutableDictionary<string, ImmutableList<Tag>>.Empty;
            Groups = ImmutableHashSet<string>.Empty;
            foreach (var tag in this.tags.Select(p => p.Value).Where(t => t.GroupName != null))
            {
                if (!groups.ContainsKey(tag.GroupName))
                {
                    groups = groups.Add(tag.GroupName, ImmutableList<Tag>.Empty);
                    Groups = Groups.Add(tag.GroupName);
                }
                groups = groups.SetItem(tag.GroupName, groups[tag.GroupName].Add(tag));
            }
        }

        public Tag GetTag(string tag)
        { 
            return tags[tag];
        }

        public IEnumerable<Tag> GetTagInGroup(string groupName)
        {
            return groups[groupName].OrderBy(i => i.GroupIndex);
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
