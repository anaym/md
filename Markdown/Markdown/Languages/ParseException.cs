using System;

namespace Markdown.Languages
{
    public class ParseException : Exception
    {
        public ParseException(string reason) : base($"Parsing failed: {reason}")
        { }
    }
}