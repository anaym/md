namespace Markdown.StringParser
{
    public struct ParsedChar
    {
        public readonly char Value;
        public readonly bool IsEscaped;

        public ParsedChar(char value, bool isEscaped)
        {
            Value = value;
            IsEscaped = isEscaped;
        }
    }
}