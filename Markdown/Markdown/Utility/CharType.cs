using System;

namespace Markdown.Utility
{
    //потому что запрещены regexp-ы
    [Flags]
    public enum CharType
    {
        Inverse = 1,
        Space = 1 << 1,
        Digit = 1 << 2,
        Letter = 1 << 3,
        NonSpace = 1 << 4,
        LeftBracket = 1 << 5,           //(
        RightBracket = 1 << 6,          //)
        LeftSquareBracket = 1 << 7,     //[
        RightSquareBracket = 1 << 8,    //]
        Any = Space | Digit | Letter | NonSpace | LeftBracket | RightBracket | LeftSquareBracket | RightSquareBracket
    }
}