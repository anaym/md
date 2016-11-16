﻿using System.Collections.Generic;
using System.Linq;

// CR: Be consistent about namespace names
namespace Markdown
{
    public class SyntaxNode
    {
        public readonly string TagName;
        private readonly List<SyntaxNode> nestedNodes;
        public readonly bool IsTag;
        public bool IsRawString => !IsTag;

        public int Size => 1 + NestedNodes.Sum(n => n.Size);

        public static SyntaxNode CreateTag(string tag) => new SyntaxNode(tag, true);
        public static SyntaxNode CreateRawString(string rawString) => new SyntaxNode(rawString, false);

        public IEnumerable<SyntaxNode> NestedNodes => nestedNodes;
        public void AddNestedNode(SyntaxNode node) => nestedNodes.Add(node);
        public void AddManyNestedNode(IEnumerable<SyntaxNode> node) => nestedNodes.AddRange(node);

        public SyntaxNode(string tagName, bool isTag)
        {
            // CR: Be consistent about using/not using this.
            this.TagName = tagName;
            IsTag = isTag;
            nestedNodes = new List<SyntaxNode>();
        }

        public override string ToString() => TagName;
    }
}