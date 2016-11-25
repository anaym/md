using System.Linq;
using Markdown.Syntax;
using Markdown.Utility;

namespace Markdown.Languages
{
    public class HtmlSyntax : LanguageSyntax
    {
        private static LanguageSyntax CreateHtmlSyntax(string cssClass)
        {
            var cssInject = string.IsNullOrWhiteSpace(cssClass) ? "" : $" class=\"{cssClass}\"";
            var syntax = new LanguageSyntaxBuilder('\\');
            syntax += new TagBuilder("bold")
            {
                Begin = new Template(CharType.Any, $"<strong{cssInject}>", CharType.Any),
                End = new Template(CharType.Any, "</strong>", CharType.Any),
                IsRootable = true,
                NestedTags = {"italic"}
            };
            syntax += new TagBuilder("italic")
            {
                Begin = new Template(CharType.Any, $"<em{cssInject}>", CharType.Any),
                End = new Template(CharType.Any, "</em>", CharType.Any),
                IsRootable = true
            };

            syntax += new TagBuilder("url.address")
            {
                Begin = new Template(CharType.Any, "<a href=\"", CharType.Any),
                End = new Template(CharType.Any, "\"", CharType.Any),
                IsRootable = true,
                GroupIndex = 0,
                GroupName = "url",
                NestedTags = { "italic", "bold" }
            };
            syntax += new TagBuilder("url.name")
            {
                Begin = new Template(CharType.Any, $"{cssInject}>", CharType.Any),
                End = new Template(CharType.Any, "</a>", CharType.Any),
                IsRootable = true,
                GroupIndex = 1,
                GroupName = "url"
            };
            return syntax.Build();
        }

        public HtmlSyntax(string cssClass = null) : base(CreateHtmlSyntax(cssClass))
        {
        }
    }
}