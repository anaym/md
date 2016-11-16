using System;

namespace Markdown.Utility
{
    //потому что запрещены regexp-ы
    [Flags]
    public enum CharType
    {
        Space = 1 << 0,
        Digit = 1 << 1,
        Letter = 1 << 2,
        NonSpace = 1 << 3,
        LeftBracket = 1 << 4,           //(
        RightBracket = 1 << 5,          //)
        LeftSquareBracket = 1 << 6,     //[
        RightSquareBracket = 1 << 6,    //]
        //Dot = 1 << 4,
        Any = Space | Digit | Letter | NonSpace
    }
}