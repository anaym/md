using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Syntax
{
    public class LanguageSyntaxBuilder : IEnumerable<ConstructionBuilder>
    {
        public readonly Dictionary<string, ConstructionBuilder> Constructions;
        public readonly HashSet<string> RootTags;

        public char Escape;

        public LanguageSyntaxBuilder()
        {
            Constructions = new Dictionary<string, ConstructionBuilder>();
            RootTags = new HashSet<string>();
        }

        public LanguageSyntax Build() => new LanguageSyntax(RootTags, Constructions.Select(p => p.Value.Build()), Escape);

        #region Operations
        public void Add(ConstructionBuilder construction)
        {
            if (Constructions.ContainsKey(construction.Tag))
                Constructions[construction.Tag] = construction;
            else
                Constructions.Add(construction.Tag, construction);
            if (construction.IsRootableTag)
                RootTags.Add(construction.Tag);
        }
        public void Remove(ConstructionBuilder construction) => Remove(construction.Tag);
        public void Remove(string tag) => Constructions.Remove(tag);
        public ConstructionBuilder Get(string tag) => Constructions[tag];
        public bool Contains(ConstructionBuilder construction) => Contains(construction.Tag);
        public bool Contains(string tag) => Constructions.ContainsKey(tag);
        #endregion

        #region Operators

        public static LanguageSyntaxBuilder operator +(LanguageSyntaxBuilder self, ConstructionBuilder construction)
        {
            self.Add(construction);
            return self;
        }

        public static LanguageSyntaxBuilder operator -(LanguageSyntaxBuilder self, ConstructionBuilder construction)
        {
            self.Remove(construction);
            return self;
        }

        #endregion

        public IEnumerator<ConstructionBuilder> GetEnumerator()
        {
            return Constructions.Select(p => p.Value).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
