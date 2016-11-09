using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.StringParser
{
    public class EscapedString : IEnumerable<Char>
    {
        public readonly string ParsedString;

        private readonly Char[] chars;

        public EscapedString(string str, char escapeChar)
        {
            var escaped = false;
            chars = str
                .Select(c => escaped ? ReadEscaped(c, out escaped) : ReadNonEscaped(c, escapeChar, out escaped))
                .Where(p => p != null)
                .Select(p => p.Value)
                .ToArray();
            ParsedString = string.Join("", chars.Select(c => c.Value));
        }

        private static Char? ReadNonEscaped(char c, char escapeChar, out bool escaped)
        {
            escaped = c == escapeChar;
            return escaped ? (Char?)null : new Char(c, false);
        }

        private static Char? ReadEscaped(char c, out bool escaped)
        {
            escaped = false;
            return new Char(c, true);
        }

        public int Length => chars.Length;

        public bool SubstringOrdinalEqual(string other, int substringStart = 0)
        {
            if (other.Length > (Length - substringStart)) return false;
            
            for (int i = 0; i < other.Length; i++)
            {
                if (this[i + substringStart].IsEscaped) return false;
                if (this[i + substringStart].Value != other[i]) return false;
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

        public override string ToString() => string.Join("", chars.Select(c => c.Value));
    }
}