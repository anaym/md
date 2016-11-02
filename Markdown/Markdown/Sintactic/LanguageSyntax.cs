using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DLibrary.Enumerations;

namespace Markdown.Syntax
{
    public class LanguageSyntax
    {
        private readonly ImmutableDictionary<string, Construction> constructions;
        private readonly ImmutableHashSet<string> rootTags;

        public readonly char Escape;
        public readonly ImmutableHashSet<string> Alphabet;

        public LanguageSyntax(IEnumerable<string> rootTags, IEnumerable<Construction> constructions, char escape)
        {
            Escape = escape;
            this.rootTags = rootTags.ToImmutableHashSet();
            this.constructions = constructions.ToImmutableDictionary(c => c.Tag, c => c);
            Alphabet = this.constructions.Select(p => p.Key).Union(this.rootTags).ToImmutableHashSet();
            var notDescribedTags = Alphabet.Except(this.constructions.Select(p => p.Key));
            if (notDescribedTags.Count != 0)
            {
                throw new Exception($"Not all tags describes in as construction: {notDescribedTags.SequenceToString()}");
            }
        }

        public Construction GetConstruction(string tag)
        { 
            return constructions[tag];
        }

        public IEnumerable<Construction> GetRootAvaible()
        {
            return rootTags.Select(GetConstruction);
        }

        public IEnumerable<Construction> GetAvaibleConstructions(string nowTag)
        {
            if (nowTag == null) return GetRootAvaible();
            return constructions[nowTag].NestedTags.Select(GetConstruction);
        }
    }
}
