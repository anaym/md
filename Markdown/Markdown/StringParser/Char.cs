namespace Markdown.StringParser
{
    // CR (krait): Почему не структура?
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