using System;
using Markdown.StringParser;

namespace Markdown.Utility
{
    // CR (krait): Это не RegExp, не надо вводить в заблуждение :)
    //потому что запрещены regexp-ы
    public class RegExp
    {
        private readonly CharType prevCharTemplate;
        private readonly string regexp;
        private readonly CharType nextCharTemplate;

        public int Length => regexp.Length;

        public RegExp(CharType prevCharTemplate, string regexp, CharType nextCharTemplate)
        {
            if (string.IsNullOrEmpty(regexp))
                throw new ArgumentException(nameof(regexp));

            this.prevCharTemplate = prevCharTemplate;
            this.regexp = regexp;
            this.nextCharTemplate = nextCharTemplate;
        }

        public bool IsMatch(EscapedString str, int startPosition = 0)
        {
            if ((str.Length - startPosition) < regexp.Length) return false;
            if (!IsMatch(str, startPosition + regexp.Length, nextCharTemplate)) return false;
            if (!IsMatch(str, startPosition - 1, prevCharTemplate)) return false;
            return str.IsOrdinalEqual(regexp, startPosition);
        }

        private bool IsMatch(EscapedString str, int pos, CharType template)
        {
            if (pos < 0 || pos >= str.Length) return true;
            if (str[pos].IsEscaped) return false;
            return str[pos].Value.IsMatch(template);
        }

        //TODO: N^2
        public int? Find(EscapedString str, int start)
        {
            // CR (krait): Правильно стоит тудушка, сделай поиск поэффективнее.
            for (var i = start; i < str.Length; i++)
            {
                if (IsMatch(str, i)) return i;
            }
            return null;
        }

        // CR (krait): А чего бы не добавить сюда prevChar и nextChar?
        public override string ToString() => regexp;
    }
}