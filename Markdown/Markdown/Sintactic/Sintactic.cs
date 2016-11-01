using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DLibrary.Enumerations;

namespace Markdown.Sintactic
{
    public class Sintactic
    {
        private readonly ImmutableDictionary<string, Construction> constructions;
        private readonly ImmutableHashSet<string> rootTags;

        public readonly ImmutableHashSet<string> Alphabet;

        /// <exception cref="ArgumentNullException">Значение параметра <paramref name="source" />, <paramref name="keySelector" /> или <paramref name="elementSelector" /> — null.– или –Функция <paramref name="keySelector" /> возвращает null в качестве ключа.</exception>
        /// <exception cref="ArgumentException">Функция <paramref name="keySelector" /> выдает дубликаты ключей для двух элементов.</exception>
        /// <exception cref="Exception">Condition.</exception>
        public Sintactic(IEnumerable<string> rootTags, IEnumerable<Construction> constructions)
        {
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

        /// <exception cref="ArgumentNullException">Параметр <paramref name="key" /> имеет значение null.</exception>
        public IEnumerable<Construction> GetAvaibleConstructions(string nowTag)
        {
            if (nowTag == null) return GetRootAvaible();
            return constructions[nowTag].NestedTags.Select(GetConstruction);
        }
    }
}
