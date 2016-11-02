using Markdown.Syntax;
using Markdown.Utility;

namespace Markdown.Languages
{
    public class MdLanguage : Language
    {
        public static Syntax.LanguageSyntax GetSintactic()
        {
            var syntax = new LanguageSyntaxBuilder {Escape = '\\'};
            syntax += new ConstructionBuilder("bold")
            {
                Begin = new Border(CharType.Space, "__", CharType.DigitOrLetter),
                End = new Border(CharType.DigitOrLetter, "__", CharType.Space),
                IsRootableTag = true,
                NestedTags = { "italic" }
            };
            syntax += new ConstructionBuilder("italic")
            {
                Begin = new Border(CharType.Space, "_", CharType.DigitOrLetter),
                End = new Border(CharType.DigitOrLetter, "_", CharType.Space),
                IsRootableTag = true
            };
            return syntax.Build();
        }

        public MdLanguage() : base(GetSintactic())
        { }
    }
}