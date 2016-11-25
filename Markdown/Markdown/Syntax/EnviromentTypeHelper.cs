namespace Markdown.Syntax
{
    public static class EnviromentTypeHelper
    {
        public static EnviromentType GetType(string prev, string next)
        {
            EnviromentType prevType = EnviromentType.PreviousLineIsMissing;
            EnviromentType nextType = EnviromentType.NextLineLineIsMissing;
            if (prev != null)
            {
                prevType = string.IsNullOrWhiteSpace(prev)
                    ? EnviromentType.PreviousLineIsEmpty
                    : EnviromentType.PreviousIsNonEmpty;
            }
            if (next != null)
            {
                nextType = string.IsNullOrWhiteSpace(next)
                    ? EnviromentType.NexLineIsEmpty
                    : EnviromentType.NextIsNonEmpty;
            }
            return prevType | nextType;
        }
    }
}