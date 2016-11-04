namespace Markdown.StringParser
{
    public class Char
    {
        public readonly char Value;
        public readonly bool IsEscaped;

        public Char(char value, bool isEscaped)
        {
            Value = value;
            IsEscaped = isEscaped;
        }
    }
}