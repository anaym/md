using System;
using Markdown.StringParser;

namespace Markdown.Syntax
{
    public class Border
    {
        private readonly CharType prevChar;
        private readonly string border;
        private readonly CharType nextChar;

        public int Length => border.Length;

        public Border(CharType prevChar, string border, CharType nextChar)
        {
            if (border == null || border == "")
                throw new ArgumentException(nameof(border));

            this.prevChar = prevChar;
            this.border = border;
            this.nextChar = nextChar;
        }

        public bool Is(ParsedString str, int pos)
        {
            var nextCharIndex = pos + border.Length;
            var prevCharIndex = pos - 1;
            if ((str.Length - pos) < border.Length) return false;
            if (prevCharIndex >= 0 && !str[prevCharIndex].Value.Is(prevChar)) return false;
            if (nextCharIndex < str.Length && !str[nextCharIndex].Value.Is(nextChar)) return false;
            return str.IsOrdinalEqual(border, pos);
        }

        //TODO: N^2
        public int? Find(ParsedString str, int start)
        {
            for (var i = start; i < str.Length; i++)
            {
                if (Is(str, i)) return i;
            }
            return null;
        }

        public override string ToString() => border;
    }
}