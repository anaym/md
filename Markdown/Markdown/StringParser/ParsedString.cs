using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.StringParser
{
    public class ParsedString : IEnumerable<ParsedChar>
    {
        public readonly string StringWithoutEscaping;
        private readonly ParsedChar[] parsedChar;

        public ParsedString(string str, char escapeChar)
        {
            var escaped = false;
            parsedChar = str
                .Select(c => escaped ? ReadEscaped(c, out escaped) : ReadNonEscaped(c, escapeChar, out escaped))
                .Where(p => p != null)
                .Select(p => p.Value)
                .ToArray();
            StringWithoutEscaping = string.Join("", parsedChar.Select(c => c.Value));
        }

        private static ParsedChar? ReadNonEscaped(char c, char escapeChar, out bool escaped)
        {
            escaped = c == escapeChar;
            return escaped ? (ParsedChar?)null : new ParsedChar(c, false);
        }

        private static ParsedChar? ReadEscaped(char c, out bool escaped)
        {
            escaped = false;
            return new ParsedChar(c, true);
        }

        public int Length => parsedChar.Length;

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

        public ParsedChar this[int index] => parsedChar[index];

        public IEnumerator<ParsedChar> GetEnumerator()
        {
            return (IEnumerator<ParsedChar>)parsedChar.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return parsedChar.GetEnumerator();
        }

        public override string ToString() => string.Join("", parsedChar.Select(c => c.Value));
    }
}