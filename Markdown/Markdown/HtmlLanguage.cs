using Markdown.Sintactic;

namespace Markdown
{
    public class HtmlLanguage : Language
    {
        public static Sintactic.Sintactic GetSintactic()
        {
            var builder = new SintacticBuilder();
            builder.AddBorders("bold", "<strong>", "</strong>");
            builder.AddNestedTags("bold", "italic");
            builder.AddBorders("italic", "<em>", "</em>");
            builder.AddToRoot("bold", "italic");
            return builder.Build();
        }

        public HtmlLanguage() : base(GetSintactic())
        {
        }
    }
}