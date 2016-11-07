using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Syntax
{
    // CR (krait): Паттерн работы с билдером прост: создать, подобавлять чего-то, сбилдить. Получать что-то, кроме результата билда, нам незачем. Зачем же тогда реализовывать IEnumerable?
    public class LanguageSyntaxBuilder : IEnumerable<TagBuilder>
    {
        // CR (krait): Старое название осталось?
        // CR (krait): Зачем Dictionary, если используются в итоге только Value?
        public readonly Dictionary<string, TagBuilder> Constructions;
        public readonly HashSet<string> RootTags;

        // CR (krait): Более аккуратно будет принять эту штуку в конструкторе.
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
            // CR (krait): Кажется, эти 4 строчки эквивалентны простому Constructions[tag.TagName] = tag;
            if (Constructions.ContainsKey(tag.TagName))
                Constructions[tag.TagName] = tag;
            else
                Constructions.Add(tag.TagName, tag);
            if (tag.IsRootableTag)
                RootTags.Add(tag.TagName);
        }

        // CR (krait): Зачем нужна операция удаления в билдере?
        public void Remove(TagBuilder tag) => Remove(tag.TagName);
        public void Remove(string tag) => Constructions.Remove(tag);
        // CR (krait): А эти операции зачем?
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
