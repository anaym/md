namespace Markdown.Syntax.Utility
{
    public static class SyntaxTreeOperations
    {
        public static IProcessTask Insert(this SyntaxNode root)
        {
            return new TreeProcessTask { Root = root, IsRemove = false, IsInsert = true};
        }

        public static IProcessTask Remove(this SyntaxNode root)
        {
            return new TreeProcessTask { Root = root, IsRemove = true, IsInsert = false};
        }

        public static IProcessTask Tag(this IProcessTask task, string tagName)
        {
            task.Target = SyntaxNode.CreateTag(tagName);
            return task;
        }
        public static IProcessTask RawString(this IProcessTask task, string tagName)
        {
            task.Target = SyntaxNode.CreateRawString(tagName);
            return task;
        }

        public static IProcessTask Into(this IProcessTask task, string parentTagName)
        {
            task.ParentTagNameFilter = parentTagName;
            task.EnableParentFilter = true;
            return task;
        }

        public static IProcessTask IntoRoot(this IProcessTask task) => task.Into(null);

        public static IProcessTask After(this IProcessTask task, string previousTagName)
        {
            task.AfterTagNameFilter = previousTagName;
            task.EnableAfterFilter = true;
            return task;
        }

        public static IProcessTask Before(this IProcessTask task, string nextTagName)
        {
            task.BeforeTagNameFilter = nextTagName;
            task.EnableBeforeFilter = true;
            return task;
        }

        public static IProcessTask Between(this IProcessTask task, string previousTagName, string nextTagName)
            => task.After(previousTagName).Before(nextTagName);
    }
}