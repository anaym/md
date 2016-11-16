using System.Collections.Generic;
using System.Security.Policy;

namespace Markdown.Utility
{
    public static class LinqExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> seq)
        {
            return new HashSet<T>(seq);
        }
    }
}