using System;

namespace Markdown.Languages.Exteptions
{
    public class ParseException : Exception
    {
        public ParseException(string reason) : base($"Parse is failed: {reason}")
        { }
    }
}