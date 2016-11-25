using System;

namespace Markdown.Converters
{
    public class ParseException : Exception
    {
        public ParseException(string reason) : base($"Parsing failed: {reason}")
        { }
    }
}