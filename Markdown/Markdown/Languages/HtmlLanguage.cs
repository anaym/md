using System.Linq;
using Markdown.Syntax;
using Markdown.Utility;

namespace Markdown.Languages
{
    public class HtmlLanguage : Language
    {
        private static LanguageSyntax CreateHtmlSyntax(string cssClass)
        {
            var cssInject = string.IsNullOrWhiteSpace(cssClass) ? "" : $" class=\"{cssClass}\"";
            var syntax = new LanguageSyntaxBuilder('\\');
            syntax += new TagBuilder("bold")
            {
                Begin = new Template(CharType.Any, $"<strong{cssInject}>", CharType.Any),
                End = new Template(CharType.Any, "</strong>", CharType.Any),
                IsRootableTag = true,
                NestedTags = {"italic"}
            };
            syntax += new TagBuilder("italic")
            {
                Begin = new Template(CharType.Any, $"<em{cssInject}>", CharType.Any),
                End = new Template(CharType.Any, "</em>", CharType.Any),
                IsRootableTag = true
            };

            syntax += new TagBuilder("url.address")
            {
                Begin = new Template(CharType.Any, "<a href=\"", CharType.Any),
                //TODO: могут быть проблемы при html-парсинге. Но их без нормальных регэкспов решать нехочу. Да и при корректном юрл все норм
                End = new Template(CharType.Any, "\"", CharType.Any),
                IsRootableTag = true,
                GroupIndex = 0,
                GroupName = "url",
                NestedTags = { "italic", "bold" }
            };
            syntax += new TagBuilder("url.name")
            {
                Begin = new Template(CharType.Any, $"{cssInject}>", CharType.Any),
                End = new Template(CharType.Any, "</a>", CharType.Any),
                IsRootableTag = true,
                GroupIndex = 1,
                GroupName = "url"
            };
            return syntax.Build();
        }

        public HtmlLanguage(string cssClass = null) : base(CreateHtmlSyntax(cssClass))
        {
        }

        public string Build(SyntaxNode tree, string metaUrl)
        {
            var newTree = InsertMetaUrl(tree, metaUrl);
            return base.Build(newTree);
        }

        private SyntaxNode InsertMetaUrl(SyntaxNode root, string metaUrl)
        {
            if (root.IsRawString) return root;
            var tag = SyntaxNode.CreateTag(root.TagName);
            foreach (var nested in root.NestedNodes)
            {
                if (nested.TagName == "url.address")
                {
                    if (nested.NestedNodes.Count() == 0 || nested.NestedNodes.First().TagName.StartsWith("/"))
                    {
                        var url = SyntaxNode.CreateTag("url.address");
                        url.AddNestedNode(SyntaxNode.CreateRawString(metaUrl));
                        url.AddManyNestedNode(nested.NestedNodes);
                        tag.AddNestedNode(url);
                    }
                    else
                    {
                        tag.AddNestedNode(InsertMetaUrl(nested, metaUrl));
                    }
                }
                else
                {
                    tag.AddNestedNode(InsertMetaUrl(nested, metaUrl));
                }
            }
            return tag;
        }
    }
}