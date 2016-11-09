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
            if (string.IsNullOrEmpty(lexem))
                throw new ArgumentException(nameof(lexem));

            this.prevCharTemplate = prevCharTemplate;
            this.Lexem = lexem;
            this.nextCharTemplate = nextCharTemplate;
        }

        public bool IsMatch(EscapedString str, int startPosition = 0)
        {
            if ((str.Length - startPosition) < Lexem.Length) return false;
            if (!IsMatch(str, startPosition + Lexem.Length, nextCharTemplate)) return false;
            if (!IsMatch(str, startPosition - 1, prevCharTemplate)) return false;
            return str.SubstringOrdinalEqual(Lexem, startPosition);
        }

        private bool IsMatch(EscapedString str, int pos, CharType template)
        {
            if (pos < 0 || pos >= str.Length) return true;
            if (str[pos].IsEscaped) return false;
            return str[pos].Value.IsMatch(template);
        }

        public int? Find(EscapedString str, int start)
        {
            for (int begin = start; 
                begin >= 0; 
                begin = str.ParsedString.IndexOf(Lexem, begin, StringComparison.InvariantCulture))
            {
                if (IsMatch(str, begin)) return begin;
                begin += 1;
            }
            return null;
        }

        public override string ToString() => $"{prevCharTemplate}{Lexem}{nextCharTemplate}";
    }
}