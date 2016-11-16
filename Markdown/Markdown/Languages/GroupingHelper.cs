using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Syntax;

namespace Markdown.Languages
{
    internal static class GroupingHelper
    {
        public static IEnumerable<IEnumerable<SyntaxNode>> OrderInGroups(this IEnumerable<IReadOnlyList<SyntaxNode>> groups, LanguageSyntax syntax)
        {
            return
                groups.Select(
                    group =>
                        group.First().IsRawString || syntax.GetTag(group.First().TagName).GroupName == null
                            ? group
                            : group.OrderBy(n => syntax.GetTag(n.TagName).GroupIndex) as IEnumerable<SyntaxNode>);
        }

        public static IEnumerable<IReadOnlyList<SyntaxNode>> CutToGroups(this IEnumerable<SyntaxNode> nodes, LanguageSyntax syntax)
        {
            string currentGroup = null;
            var result = new List<List<SyntaxNode>>();
            foreach (var node in nodes)
            {
                var group = node.IsTag ? syntax.GetTag(node.TagName).GroupName : null;
                if (node.IsRawString || group != currentGroup)
                {
                    SaveResult(result, node, false);
                }
                else
                {
                    SaveResult(result, node, true);   
                }
                currentGroup = group;
            }
            return result;
        }

        private static void SaveResult(List<List<SyntaxNode>> result, SyntaxNode current, bool append)
        {
            if (!append)
            {
                result.Add(new List<SyntaxNode> {current});
            }
            else
            {
                result.Last().Add(current);
            }
        }
    }
}