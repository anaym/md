using Markdown.Syntax;
using Markdown.Utility;

namespace Markdown.Languages
{
    public class HtmlLanguage : Language
    {
        private static LanguageSyntax CreateHtmlSyntax()
        {
            var syntax = new LanguageSyntaxBuilder('\\');
            syntax += new TagBuilder("bold")
            {
                Begin = new Template(CharType.Any, "<strong>", CharType.Any),
                End = new Template(CharType.Any, "</strong>", CharType.Any),
                IsRootableTag = true,
                NestedTags = {"italic"}
            };
            syntax += new TagBuilder("italic")
            {
                Begin = new Template(CharType.Any, "<em>", CharType.Any),
                End = new Template(CharType.Any, "</em>", CharType.Any),
                IsRootableTag = true
            };

            syntax += new TagBuilder("url.address")
            {
                Begin = new Template(CharType.Any, "<a href=\"", CharType.Any),
                //TODO: могут быть проблемы при html-парсинге. Но их без нормальных регэкспов решать нехочу. Да и при корректном юрл все норм
                End = new Template(CharType.Any, "\"", CharType.Any),
                IsRootableTag = true,
                GroupIndex = 0,
                GroupName = "url"
            };
            syntax += new TagBuilder("url.name")
            {
                Begin = new Template(CharType.Any, ">>", CharType.Any),
                End = new Template(CharType.Any, "</a>", CharType.Any),
                IsRootableTag = true,
                GroupIndex = 1,
                GroupName = "url"
            };
            return syntax.Build();
        }

        public HtmlLanguage() : base(CreateHtmlSyntax())
        {
        }
    }
}