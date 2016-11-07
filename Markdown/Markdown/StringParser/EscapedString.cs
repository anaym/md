using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DLibrary.Enumerations;

namespace Markdown.StringParser
{
    public class EscapedString : IEnumerable<Char>
    {
        private readonly Char[] chars;

        public EscapedString(string str, char escapeChar)
        {
            var escaped = false;
            chars = str
                .Select(c => escaped ? ReadEscaped(c, escapeChar, out escaped) : ReadNonEscaped(c, escapeChar, out escaped))
                .Where(p => p != null)
                .ToArray();
        }

        private static Char ReadNonEscaped(char c, char escapeChar, out bool escaped)
        {
            escaped = c == escapeChar;
            return escaped ? null : new Char(c, false);
        }

        // CR (krait): Зачем нужен аргумент escape?
        private static Char ReadEscaped(char c, char escape, out bool escaped)
        {
            escaped = false;
            return new Char(c, true);
        }

        public int Length => chars.Length;

        public bool IsOrdinalEqual(string other, int start = 0)
        {
            // CR (krait): А если other длиннее this?
            for (int i = 0; i < other.Length; i++)
            {
                if (this[i + start].IsEscaped) return false;
                if (this[i + start].Value != other[i]) return false;
            }
            return true;
        }

        public Char this[int index] => chars[index];

        public IEnumerator<Char> GetEnumerator()
        {
            return (IEnumerator<Char>)chars.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return chars.GetEnumerator();
        }

        // CR (krait): Можно легко обойтись без использования библиотеки: string.Join()
        public override string ToString() => chars.SequenceToString(c => c.Value.ToString(), "", "", "");
    }
}