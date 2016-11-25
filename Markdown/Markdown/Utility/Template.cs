using System;
using Markdown.StringParser;

namespace Markdown.Utility
{
    //потому что запрещены regexp-ы
    public class Template
    {
        private readonly CharType prevCharTemplate;
        private readonly CharType nextCharTemplate;

        public readonly string Lexem;
        public int Length => Lexem.Length;

        public Template(CharType prevCharTemplate, string lexem, CharType nextCharTemplate)
        {
            if (lexem == null)
                throw new ArgumentException(nameof(lexem));

            this.prevCharTemplate = prevCharTemplate;
            this.Lexem = lexem;
            this.nextCharTemplate = nextCharTemplate;
        }

        public bool IsMatch(ParsedString str, int startPosition = 0)
        {
            if ((str.Length - startPosition) < Lexem.Length) return false;
            if (!IsMatch(str, startPosition + Lexem.Length, nextCharTemplate)) return false;
            if (!IsMatch(str, startPosition - 1, prevCharTemplate)) return false;
            return str.SubstringOrdinalEqual(Lexem, startPosition);
        }

        public int? Find(ParsedString str, int findStart)
        {
            while (findStart >= 0)
            {
                if (IsMatch(str, findStart)) return findStart;
                findStart = str.StringWithoutEscaping.IndexOf(Lexem, findStart + 1, StringComparison.InvariantCulture);
            }
            return null;
        }

        private bool IsMatch(ParsedString str, int pos, CharType expectedCharType)
        {
            if (pos < 0 || pos >= str.Length) return true;
            return str[pos].Value.IsMatch(expectedCharType);
        }

        public override string ToString() => $"{prevCharTemplate}{Lexem}{nextCharTemplate}";
    }
}