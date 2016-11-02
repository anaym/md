using Markdown.Syntax;

namespace Markdown
{
    public class HtmlLanguage : Language
    {
        public static Syntax.LanguageSyntax GetSintactic()
        {
            var builder = new SyntaxBuilder();
            builder.AddBorders("bold", new Border(CharType.Any, "<strong>", CharType.Any), new Border(CharType.Any,  "</strong>", CharType.Any));
            builder.AddNestedTags("bold", "italic");
            builder.AddBorders("italic", new Border(CharType.Any, "<em>", CharType.Any), new Border(CharType.Any, "</em>", CharType.Any));
            builder.AddToRoot("bold", "italic");
            return builder.Build();
        }

        public HtmlLanguage() : base(GetSintactic())
        {
        }
    }
}