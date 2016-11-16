using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Syntax;

namespace Markdown.Languages
{
    internal static class GroupingHelper
    {
        public static SyntaxNode BuildIncomletGroups(this SyntaxNode root, Language language)
        {
            if (root.IsRawString) return root;
            var newRoot = SyntaxNode.CreateTag(root.TagName);
            var nested = root.NestedNodes.Select(n => n.BuildIncomletGroups(language)).ToList();
            var processed = nested.CutToGroups(language.Syntax).BuildIncomletGroups(language).ConcatRaw();
            newRoot.AddManyNestedNode(processed.SelectMany(a => a));
            return newRoot;
        }

        public static IEnumerable<IReadOnlyCollection<SyntaxNode>> ConcatRaw(this IEnumerable<IReadOnlyList<SyntaxNode>> groups)
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

        public static IEnumerable<IReadOnlyList<SyntaxNode>> BuildIncomletGroups(this IEnumerable<IReadOnlyList<SyntaxNode>> groups, Language language)
        {
            foreach (var group in groups)
            {
                if (group.IsGroup(language.Syntax))
                {
                    var groupName = language.Syntax.GetTag(group.First().TagName).GroupName;
                    var expected = language.Syntax.GetTagInGroup(groupName).ToList();
                    var buffer = new List<SyntaxNode>();
                    foreach (var node in group)
                    {
                        if (expected.Count == 0)
                        {
                            yield return buffer;
                            buffer = new List<SyntaxNode>();
                            expected = language.Syntax.GetTagInGroup(groupName).ToList();
                        }
                        buffer.Add(node);
                        if (node.TagName != expected.First().Name)
                        {
                            var tree = SyntaxNode.CreateTag(null);
                            tree.AddManyNestedNode(buffer);
                            var str = SyntaxNode.CreateRawString(language.Build(tree));
                            yield return new List<SyntaxNode> {str};
                            buffer = new List<SyntaxNode>();
                            expected = language.Syntax.GetTagInGroup(groupName).ToList();
                        }
                        else
                        {
                            expected.RemoveAt(0);
                        }
                    }
                    if (expected.Count == 0)
                        yield return buffer;
                    else
                    {
                        var tree = SyntaxNode.CreateTag(null);
                        tree.AddManyNestedNode(buffer);
                        var str = SyntaxNode.CreateRawString(language.Build(tree));
                        yield return new List<SyntaxNode> { str };
                    }
                }
                else
                {
                    yield return group;
                }
            }
        }

        public static IEnumerable<IEnumerable<SyntaxNode>> OrderInGroups(this IEnumerable<IReadOnlyList<SyntaxNode>> groups, LanguageSyntax syntax)
        {
            return groups.Select(group => !group.IsGroup(syntax) ? group : group.OrderBy(n => syntax.GetTag(n.TagName).GroupIndex) as IEnumerable<SyntaxNode>);
        }

        public static bool IsGroup(this IReadOnlyList<SyntaxNode> nodes, LanguageSyntax syntax)
        {
            return nodes.First().IsTag && syntax.GetTag(nodes.First().TagName).GroupName != null;
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
                    SaveResult(result, node, currentGroup != null);   
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