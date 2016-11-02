using Markdown.Sintactic;

namespace Markdown
{
    public class HtmlLanguage : Language
    {
        public static Sintactic.Sintactic GetSintactic()
        {
            var builder = new SintacticBuilder();
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