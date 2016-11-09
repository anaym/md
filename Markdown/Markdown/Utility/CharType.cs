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
        //Dot = 1 << 4,
        Any = Space | Digit | Letter | NonSpace
    }
}