using Markdown.Utility;

namespace Markdown.Syntax.Builtins
{
    public class MdLanguageSyntax : LanguageSyntax
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

            syntax += new TagBuilder("url.name")
            {
                Begin = new Template(CharType.Space, "[", CharType.Inverse | CharType.RightSquareBracket),
                End = new Template(CharType.LeftSquareBracket | CharType.Inverse, "]", CharType.LeftBracket),
                IsRootableTag = true,
                GroupIndex = 0,
                GroupName = "url",
                NestedTags = { "italic", "bold" }
            };
            syntax += new TagBuilder("url.address")
            {
                Begin = new Template(CharType.RightSquareBracket, "(", CharType.Any),
                End = new Template(CharType.Any, ")", CharType.Space),
                IsRootableTag = true,
                GroupIndex = 1,
                GroupName = "url"
            };
            return syntax.Build();
        }

        public MdLanguageSyntax() : base(CreateMdSyntax())
        { }
    }
}