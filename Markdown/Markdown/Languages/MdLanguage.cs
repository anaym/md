using Markdown.Syntax;
using Markdown.Utility;

namespace Markdown.Languages
{
    public class MdLanguage : Language
    {
        private static LanguageSyntax CreateMdSyntax()
        {
            var syntax = new LanguageSyntaxBuilder('\\');
            syntax += new TagBuilder("bold")
            {
                Begin = new Template(CharType.Space, "__", CharType.Digit | CharType.Letter),
                End = new Template(CharType.Digit | CharType.Letter, "__", CharType.Space),
                IsRootableTag = true,
                NestedTags = { "italic" }
            };
            syntax += new TagBuilder("italic")
            {
                Begin = new Template(CharType.Space, "_", CharType.Digit | CharType.Letter),
                End = new Template(CharType.Digit | CharType.Letter, "_", CharType.Space),
                IsRootableTag = true
            };
            return syntax.Build();
        }

        public MdLanguage() : base(CreateMdSyntax())
        { }
    }
}