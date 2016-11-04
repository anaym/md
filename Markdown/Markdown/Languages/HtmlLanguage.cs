using Markdown.Syntax;
using Markdown.Utility;

namespace Markdown.Languages
{
    public class HtmlLanguage : Language
    {
        private static LanguageSyntax CreateHtmlSyntax()
        {
            var syntax = new LanguageSyntaxBuilder();
            syntax += new TagBuilder("bold")
            {
                Begin = new RegExp(CharType.Any, "<strong>", CharType.Any),
                End = new RegExp(CharType.Any, "</strong>", CharType.Any),
                IsRootableTag = true,
                NestedTags = {"italic"}
            };
            syntax += new TagBuilder("italic")
            {
                Begin = new RegExp(CharType.Any, "<em>", CharType.Any),
                End = new RegExp(CharType.Any, "</em>", CharType.Any),
                IsRootableTag = true
            };
            return syntax.Build();
        }

        public HtmlLanguage() : base(CreateHtmlSyntax())
        {
        }
    }
}