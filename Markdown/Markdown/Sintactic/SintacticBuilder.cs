using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Sintactic
{
    public class SintacticBuilder
    {
        public readonly Dictionary<string, ConstructionBuilder> Constructions;
        public readonly HashSet<string> RootTags;

        public char Escape;

        public SintacticBuilder()
        {
            Constructions = new Dictionary<string, ConstructionBuilder>();
            RootTags = new HashSet<string>();
        }

        public void AddTag(string tag)
        {
            Constructions.Add(tag, new ConstructionBuilder(tag));
        }

        public void AddToRoot(params string[] tags)
        {
            foreach (var tag in tags)
            {
                RootTags.Add(tag);
            }
        }
        public void AddBorders(string tag, Border begin, Border end)
        {
            if (!Constructions.ContainsKey(tag))
                AddTag(tag);
            Constructions[tag].Begin = begin;
            Constructions[tag].End = end;
        }

        public void AddNestedTags(string tag, params string[] nested)
        {
            if (!Constructions.ContainsKey(tag))
                AddTag(tag);
            Constructions[tag].NestedTags.AddRange(nested);
        }

        public void AddConstruction(Construction construction)
        {
            Constructions.Add(construction.Tag, new ConstructionBuilder(construction));
        }

        public Sintactic Build() => new Sintactic(RootTags, Constructions.Select(p => p.Value.Build()), Escape);
    }
}
