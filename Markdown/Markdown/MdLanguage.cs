using Markdown.Sintactic;

namespace Markdown
{
    public class MdLanguage : Language
    {
        public static Sintactic.Sintactic GetSintactic()
        {
            var builder = new SintacticBuilder();
            builder.AddBorders("bold", new Border(CharType.Any, "__", CharType.Any), new Border(CharType.Any, "__", CharType.Any));
            builder.AddNestedTags("bold", "italic");
            builder.AddBorders("italic", new Border(CharType.Any, "_", CharType.Any), new Border(CharType.Any, "_", CharType.Any));
            builder.AddToRoot("bold", "italic");
            return builder.Build();
        }

        public MdLanguage() : base(GetSintactic())
        { }
    }
}