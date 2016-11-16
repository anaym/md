using System;

// CR: Be consistent about namespaces
namespace Markdown.Languages.Exteptions
{
    public class ParseException : Exception
    {
        public ParseException(string reason) : base($"Parsing failed: {reason}")
        { }
    }
}