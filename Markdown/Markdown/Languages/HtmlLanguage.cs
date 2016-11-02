using Markdown.Syntax;
using Markdown.Utility;

namespace Markdown.Languages
{
    public class HtmlLanguage : Language
    {
        public static LanguageSyntax GetSintactic()
        {
            var syntax = new LanguageSyntaxBuilder();
            syntax += new ConstructionBuilder("bold")
            {
                Begin = new Border(CharType.Any, "<strong>", CharType.Any),
                End = new Border(CharType.Any, "</strong>", CharType.Any),
                IsRootableTag = true,
                NestedTags = {"italic"}
            };
            syntax += new ConstructionBuilder("italic")
            {
                Begin = new Border(CharType.Any, "<em>", CharType.Any),
                End = new Border(CharType.Any, "</em>", CharType.Any),
                IsRootableTag = true
            };
            return syntax.Build();
        }

        public HtmlLanguage() : base(GetSintactic())
        {
        }
    }
}