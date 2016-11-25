using Markdown.Syntax;

namespace Markdown.Converters
{
    public static class LanguageSyntaxHelper
    {
        public static SyntaxNode Parse(this LanguageSyntax syntax, string line)
        {
            return new SyntaxTreeParser(syntax).Parse(line);
        }

        public static SyntaxNode ParseMultiline(this LanguageSyntax syntax, string text)
        {
            return new MultilineSyntaxTreeParser(syntax).ParseMultiline(text);
        }

        public static string Build(this LanguageSyntax syntax, SyntaxNode tree)
        {
            return new SyntaxTreeBuilder(syntax).Build(tree);
        }

        public static string Build(this LanguageSyntax syntax, SyntaxNode tree, string metaUrl)
        {
            return new SyntaxTreeBuilder(syntax).Build(tree, metaUrl);
        }
    }
}