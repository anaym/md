using System;
using System.Linq;
using DLibrary.Enumerations;
using Markdown.Syntax;

namespace Markdown.Languages
{
    public class Language
    {
        public readonly LanguageSyntax Syntax;

        public Language(LanguageSyntax syntax)
        {
            Syntax = syntax;
        }

        public SyntaxNode Parse(string line)
        {
            var machine = new Machine(line, Syntax);
            for (machine.Position = 0; machine.Position < machine.String.Length; machine.Position = ReadNextTag(machine))
            { }
            return machine.Root;
        }

        private int ReadNextTag(Machine machine)
        {
            var pos = ReadEndOfThisTag(machine) ?? ReadBeginOfNestedTag(machine) ?? ReadRawText(machine);
            if (pos == null)
                throw new Exception("Bad string");
            return pos.Value;
        }

        private int? ReadEndOfThisTag(Machine machine)
        {
            if (machine.NowTag == Machine.RootTag)
                return null;
            if (machine.NowConstruction.End.Is(machine.String, machine.Position))
            {
                var ni = machine.NowConstruction.End.Length + machine.Position;
                machine.BackOnStack();
                return ni;
            }
            return null;
        }

        private int? ReadBeginOfNestedTag(Machine machine)
        {
            var next = machine
                .NowAvaibleConstructions
                .FirstOrDefault(c => c.Is(machine.String, machine.Position));

            if (next == null) return null;
            machine.AddNestedNode(SyntaxNode.CreateTag(next.Tag));
            machine.ForwardOnStack();
            return machine.Position + machine.NowConstruction.Begin.Length;
        }

        private int? ReadRawText(Machine machine)
        {
            var end = FindEndOfRawText(machine);
            var raw = machine.String.ToString().Substring(machine.Position, end - machine.Position);
            machine.AddNestedNode(SyntaxNode.CreateRawString(raw));
            return end;
        }

        private int FindEndOfRawText(Machine machine)
        {
            var beginNested = machine
                .NowAvaibleConstructions
                .Select(c => Tuple.Create(c, c.Begin.Find(machine.String, machine.Position)))
                .Where(p => p.Item2 != null)
                .OrderBy(p => p.Item2.Value)
                .FirstOrDefault(p => p.Item1.Is(machine.String, p.Item2.Value))
                ?.Item2;

            var endNow = machine.NowTag == Machine.RootTag ? null : machine.NowConstruction.End.Find(machine.String, machine.Position);

            if (endNow == null)
            {
                return beginNested ?? machine.String.Length;
            }
            else
            {
                return beginNested == null ? endNow.Value : Math.Min(endNow.Value, beginNested.Value);
            }
        }

        public string Build(SyntaxNode tree)
        {
            if (!tree.IsTag)
                return tree.Lexem;
            if (tree.Lexem == null)
                return tree.NestesNodes.SequenceToString(Build, "", "", "");
            var construction = Syntax.GetConstruction(tree.Lexem);
            return construction.Begin + tree.NestesNodes.SequenceToString(Build, "", "", "") + construction.End;
        }
    }
}
