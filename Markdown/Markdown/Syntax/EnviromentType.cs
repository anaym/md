using System;

namespace Markdown.Syntax
{
    [Flags]
    public enum EnviromentType
    {
        SimpleTag = 0,
        PreviousLineIsEmpty = 1 << 1,
        NexLineIsEmpty = 1 << 2,
        PreviousLineIsMissing = 1 << 3,
        NextLineLineIsMissing = 1 << 4,
        PreviousIsNonEmpty = 1 << 5,
        NextIsNonEmpty = 1 << 6,
        Any = Int32.MaxValue
    }
}