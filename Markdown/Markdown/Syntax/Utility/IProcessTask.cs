namespace Markdown.Syntax.Utility
{
    public interface IProcessTask
    {
        SyntaxNode Root { get; set; }
        SyntaxNode Target { get; set; }
        bool IsRemove { get; set; }
        bool IsInsert { get; set; }
        string ParentTagNameFilter { get; set; }
        string AfterTagNameFilter { get; set; }
        string BeforeTagNameFilter { get; set; }
        bool EnableParentFilter { get; set; }
        bool EnableAfterFilter { get; set; }
        bool EnableBeforeFilter { get; set; }

        SyntaxNode Do();
    }
}