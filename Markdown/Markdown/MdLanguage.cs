using Markdown.Sintactic;

namespace Markdown
{
    public class MdLanguage : Language
    {
        public static Sintactic.Sintactic GetSintactic()
        {
            var builder = new SintacticBuilder();
            builder.AddBorders("bold", "__", "__");
            builder.AddNestedTags("bold", "italic");
            builder.AddBorders("italic", " _", "_ ");
            builder.AddToRoot("bold", "italic");
            return builder.Build();
        }

        public MdLanguage() : base(GetSintactic())
        { }
    }
}