using System;

namespace Markdown.Syntax
{
    public class IncorrectSyntaxException : Exception
    {
        public IncorrectSyntaxException(string reason) : base($"Incorrect syntax: {reason}")
        { }
    }
}