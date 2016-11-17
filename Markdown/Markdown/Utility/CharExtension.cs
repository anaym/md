using System;

namespace Markdown.Utility
{
    //потому что запрещены regexp-ы
    public static class CharExtension
    {
        public static bool IsMatch(this char c, CharType type)
        {
            if (type.HasFlag(CharType.Inverse)) return !c.IsMatch(type ^ CharType.Inverse);

            if (type.HasFlag(CharType.Space)) if (Char.IsWhiteSpace(c)) return true;
            if (type.HasFlag(CharType.NonSpace)) if (!Char.IsWhiteSpace(c)) return true;
            if (type.HasFlag(CharType.Digit)) if (Char.IsDigit(c)) return true;
            if (type.HasFlag(CharType.Letter)) if (Char.IsLetter(c)) return true;

            if (type.HasFlag(CharType.LeftBracket)) if (c == '(') return true;
            if (type.HasFlag(CharType.RightBracket)) if (c == ')') return true;
            if (type.HasFlag(CharType.LeftSquareBracket)) if (c == '[') return true;
            if (type.HasFlag(CharType.RightSquareBracket)) if (c == ']') return true;

            return false;
        }
    }
}