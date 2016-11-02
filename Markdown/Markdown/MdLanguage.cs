using Markdown.Sintactic;

namespace Markdown
{
    public class MdLanguage : Language
    {
        public static Sintactic.Sintactic GetSintactic()
        {
            var builder = new SintacticBuilder();
            builder.Escape = '\\';
            builder.AddBorders("bold", new Border(CharType.Space, "__", CharType.DigitOrLetter), new Border(CharType.DigitOrLetter, "__", CharType.Space));
            builder.AddNestedTags("bold", "italic");
            builder.AddBorders("italic", new Border(CharType.Space, "_", CharType.DigitOrLetter), new Border(CharType.DigitOrLetter, "_", CharType.Space));
            builder.AddToRoot("bold", "italic");
            return builder.Build();
        }

        public MdLanguage() : base(GetSintactic())
        { }
    }
}