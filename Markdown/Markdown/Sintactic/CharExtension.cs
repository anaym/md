using System;

namespace Markdown.Sintactic
{
    public static class CharExtension
    {
        public static bool Is(this char c, CharType type)
        {
            switch (type)
            {
                case CharType.Space:
                    return Char.IsWhiteSpace(c);
                case CharType.Digit:
                    return Char.IsDigit(c);
                case CharType.Letter:
                    return Char.IsLetter(c);
                case CharType.SpaceOrDigit:
                    return c.Is(CharType.Space) || c.Is(CharType.Digit);
                case CharType.DigitOrLetter:
                    return c.Is(CharType.Digit) || c.Is(CharType.Letter);
                case CharType.LetterOrSpace:
                    return c.Is(CharType.Letter) || c.Is(CharType.Space);
                case CharType.Any:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}