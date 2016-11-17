using System.Collections.Generic;
using System.Linq;

namespace Markdown.Syntax.Utility
{
    internal class TreeProcessTask : IProcessTask
    {
        public SyntaxNode Root { get; set; }
        public SyntaxNode Target { get; set; }
        public bool IsRemove { get; set; }
        public bool IsInsert { get; set; }
        public string ParentTagNameFilter { get; set; }
        public string AfterTagNameFilter { get; set; }
        public string BeforeTagNameFilter { get; set; }
        public bool EnableParentFilter { get; set; }
        public bool EnableAfterFilter { get; set; }
        public bool EnableBeforeFilter { get; set; }
        public SyntaxNode Do()
        {
            return Do(Root);
        }

        private SyntaxNode Do(SyntaxNode root)
        {
            if (root.IsRawString) return root;
            var nested = IsRemove ? Remove(root) : root.NestedNodes.ToList();
            nested = IsInsert ? Insert(root) : root.NestedNodes.ToList();
            var tag = SyntaxNode.CreateTag(root.TagName);
            tag.AddManyNestedNode(nested);
            return tag;
        }

        private List<SyntaxNode> Remove(SyntaxNode parent)
        {
            var newNested = new List<SyntaxNode>();
            var nested = parent.NestedNodes.ToList();
            for (int i = 0; i < nested.Count; i++)
            {
                SyntaxNode prev = i == 0 ? null : nested[i - 1];
                SyntaxNode next = i + 1 == nested.Count ? null : nested[i + 1];

                if (!IsGoodState(parent.TagName, prev?.TagName, next?.TagName) ||
                    Target.IsRawString != nested[i].IsRawString || 
                    Target.TagName != nested[i].TagName)
                {
                    newNested.Add(Do(nested[i]));
                }
            }
            return newNested;
        }

        private List<SyntaxNode> Insert(SyntaxNode parent)
        {
            var newNested = new List<SyntaxNode>();
            var nested = parent.NestedNodes.ToList();
            for (int i = 0; i < nested.Count + 1; i++)
            {
                SyntaxNode prev = i == 0 ? null : nested[i - 1];
                SyntaxNode next = i >= nested.Count ? null : nested[i];
                if (IsGoodState(parent.TagName, prev?.TagName, next?.TagName))
                    newNested.Add(Target);
                if (i < nested.Count)
                    newNested.Add(Do(nested[i]));
            }
            return newNested;
        }

        private bool IsGoodState(string parent, string prev, string next)
        {
            if (EnableAfterFilter && AfterTagNameFilter != prev) return false;
            if (EnableBeforeFilter && BeforeTagNameFilter != next) return false;
            if (EnableParentFilter && ParentTagNameFilter != parent) return false;
            return true;
        }
    }
}