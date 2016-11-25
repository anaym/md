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

        public readonly ImmutableHashSet<string> GlobalTags;

        public readonly char EscapeChar;

        public LanguageSyntax(LanguageSyntax syntax) : this(syntax.tags.Select(p => p.Value), syntax.EscapeChar)
        { }

        public LanguageSyntax(IEnumerable<Tag> tags, char escapeChar)
        {
            EscapeChar = escapeChar;
            this.tags = tags.ToImmutableDictionary(c => c.Name, c => c);
            rootTags = this.tags.Where(t => t.Value.IsRootableTag).Select(t => t.Value.Name).ToImmutableHashSet();
            var alphabet = this.tags.Select(p => p.Key).ToImmutableHashSet();
            GlobalTags = this.tags.Where(t => t.Value.Enviroment != EnviromentType.SimpleTag).Select(p => p.Key).ToImmutableHashSet();
            var notDescribedTags = this.tags.SelectMany(p => p.Value.NestedTags).Except(alphabet).ToList();
            if (notDescribedTags.Count != 0)
            {
                throw new IncorrectSyntaxException($"Not all tags has been described: {notDescribedTags.SequenceToString()}"); 
            }

            groups = ImmutableDictionary<string, ImmutableList<Tag>>.Empty;
            foreach (var tag in this.tags.Select(p => p.Value).Where(t => t.GroupName != null))
            {
                if (!groups.ContainsKey(tag.GroupName))
                {
                    groups = groups.Add(tag.GroupName, ImmutableList<Tag>.Empty);
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
