using Markdown.Syntax;
using Markdown.Utility;

namespace Markdown.Languages
{
    public class MdSyntax : LanguageSyntax
    {
        private static LanguageSyntax CreateMdSyntax()
        {
            var syntax = new LanguageSyntaxBuilder('\\');
            syntax += new TagBuilder("bold")
            {
                Begin = new Template(CharType.Space, "__", CharType.Digit | CharType.Letter),
                End = new Template(CharType.Digit | CharType.Letter, "__", CharType.Space),
                IsRootable = true,
                NestedTags = { "italic" }
            };
            syntax += new TagBuilder("italic")
            {
                Begin = new Template(CharType.Space, "_", CharType.Digit | CharType.Letter),
                End = new Template(CharType.Digit | CharType.Letter, "_", CharType.Space),
                IsRootable = true
            };

            syntax += new TagBuilder("url.name")
            {
                Begin = new Template(CharType.Space, "[", CharType.Inverse | CharType.RightSquareBracket),
                End = new Template(CharType.LeftSquareBracket | CharType.Inverse, "]", CharType.LeftBracket),
                IsRootable = true,
                GroupIndex = 0,
                GroupName = "url",
                NestedTags = { "italic", "bold" }
            };
            syntax += new TagBuilder("url.address")
            {
                Begin = new Template(CharType.RightSquareBracket, "(", CharType.Any),
                End = new Template(CharType.Any, ")", CharType.Space),
                IsRootable = true,
                GroupIndex = 1,
                GroupName = "url"
            };
            syntax += new TagBuilder("paragrph")
            {
                Begin = new Template(CharType.NewLine, "\n", CharType.Any),
                End = new Template(CharType.Any, "\n", CharType.NewLine),
                IsRootable = true,
                NestedTags = { "italic", "bold", "url.name", "url.address" }
            };
            return syntax.Build();
        }

        public MdSyntax() : base(CreateMdSyntax())
        { }
    }
}