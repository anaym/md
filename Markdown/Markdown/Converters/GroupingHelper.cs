using System.Collections.Generic;
using System.Linq;
using Markdown.Syntax;
using Utility.Linq;

namespace Markdown.Converters
{
    internal static class GroupingHelper
    {
        public static SyntaxNode RevertParseForIncompleteGroups(this SyntaxNode root, LanguageSyntax syntax)
        {
            if (root.IsRawString) return root;
            var newRoot = SyntaxNode.CreateTag(root.TagName);
            var nested = root.NestedNodes.Select(n => n.RevertParseForIncompleteGroups(syntax)).ToList();
            var processed = nested.RevertParseForIncompleteGroups(syntax).ConcatRaw();
            newRoot.AddManyNestedNode(processed.SelectMany(a => a));
            return newRoot;
        }

        public static IEnumerable<SyntaxNode> OrderTagsInGroups(this IEnumerable<SyntaxNode> nodes, LanguageSyntax syntax)
        {
            return nodes.CutToGroups(syntax).OrderInGroups(syntax).SelectMany(g => g);
        }

        private static IEnumerable<IReadOnlyCollection<SyntaxNode>> ConcatRaw(this IEnumerable<IReadOnlyList<SyntaxNode>> groups)
        {
            string buffer = null;
            foreach (var group in groups)
            {
                if (group.First().IsRawString)
                {
                    if (buffer == null) buffer = "";
                    buffer += group.First().TagName;
                }
                else
                {
                    if (buffer != null) yield return new List<SyntaxNode> {SyntaxNode.CreateRawString(buffer)};
                    buffer = null;
                    yield return group;
                }
            }
            if (buffer != null) yield return new List<SyntaxNode> { SyntaxNode.CreateRawString(buffer) };
        }

        private static IEnumerable<IReadOnlyList<SyntaxNode>> RevertParseForIncompleteGroups(this IEnumerable<SyntaxNode> tags, LanguageSyntax syntax)
        {
            var groups = tags.CutToGroups(syntax).ToList();
            foreach (var group in groups)
            {
                if (group.First().IsRawString || syntax.GetTag(group.First().TagName).GroupName == null)
                {
                    foreach (var node in group)
                    {
                        yield return new List<SyntaxNode> {node};
                    }
                }
                else
                {
                    var groupName = syntax.GetTag(group.First().TagName).GroupName;
                    var expected = syntax.GetTagInGroup(groupName);
                    if (group.Count != expected.Count())
                    {
                        var str = group.Select(syntax.Build).SequenceToString("", "", "");
                        yield return new List<SyntaxNode> {SyntaxNode.CreateRawString(str)};
                    }
                    else
                    {
                        yield return group;
                    }
                }
            }
        }

        private static IEnumerable<IEnumerable<SyntaxNode>> OrderInGroups(this IEnumerable<IReadOnlyList<SyntaxNode>> groups, LanguageSyntax syntax)
        {
            return groups.Select(group => !group.IsGroup(syntax) ? group : group.OrderBy(n => syntax.GetTag(n.TagName).GroupIndex) as IEnumerable<SyntaxNode>);
        }

        private static bool IsGroup(this IReadOnlyList<SyntaxNode> nodes, LanguageSyntax syntax)
        {
            return nodes.First().IsTag && syntax.GetTag(nodes.First().TagName).GroupName != null;
        }

        private static IEnumerable<IReadOnlyList<SyntaxNode>> CutToGroups(this IEnumerable<SyntaxNode> nodes, LanguageSyntax syntax)
        {
            return nodes
                .GroupWithSaveOrderBy(n => n.IsRawString ? null : syntax.GetTag(n.TagName).GroupName)
                .Select(g => g.ToList())
                .SelectMany(g => g.CutToFullGroups(syntax));
        }

        private static IEnumerable<IReadOnlyList<SyntaxNode>> CutToFullGroups(this List<SyntaxNode> group, LanguageSyntax syntax)
        {
            if (group.Count == 0) return new List<IReadOnlyList<SyntaxNode>>();
            var groupName = group.First().IsRawString ? null : syntax.GetTag(group.First().TagName).GroupName;
            if (groupName == null) return new[] { group.ToList() };
            var expected = syntax.GetTagInGroup(groupName).Select(t => t.Name).ToList();
            var tags = group.ToList();
            if (tags.Count < expected.Count) return new[] { group.ToList() };

            var isFullCombination = tags.IsFullCombination(expected);
            var length = isFullCombination ? expected.Count : 1;

            var result = new List<IReadOnlyList<SyntaxNode>>();
            result.Add(tags.SubEnumerable(0, length).ToList());
            result.AddRange(tags.SubEnumerable(length, tags.Count - length).ToList().CutToFullGroups(syntax));

            return result;
        }

        private static bool IsFullCombination(this IEnumerable<SyntaxNode> nodes, IEnumerable<string> expectedTags)
        {
            return nodes.Zip(expectedTags, (a, b) => a.IsTag && a.TagName == b).All(p => p);
        }
    }
}