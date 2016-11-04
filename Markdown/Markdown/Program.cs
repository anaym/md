using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DLibrary.Enumerations;
using Markdown.Utility;

namespace Markdown
{


    class Program
    {
        public static IEnumerable<string> GetAllNamespaces(string nameSpaceFilter)
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Select(t => t.Namespace)
                .Where(n => n.StartsWith(nameSpaceFilter))
                .Where(n => n != "")
                .Distinct()
                .Select(c => c.CamelCaseToNormal());

        }

        public static IEnumerable<string> GetAllTypes(string nameSpaceFilter)
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(n => n.Namespace.StartsWith(nameSpaceFilter))
                .Select(n => n.FullName)
                .Distinct()
                .Select(c => c.CamelCaseToNormal());

        }

        public static IEnumerable<string> GetAllMethods(string nameSpaceFilter)
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(n => n.Namespace.StartsWith(nameSpaceFilter))
                .SelectMany(
                    t =>
                        t.GetMethods()
                            .Where(n => n.Name.First().IsMatch(CharType.Letter))
                            .Select(m => t.FullName + '.' + m.Name)
                            .Union(
                                t.GetFields()
                                    .Where(n => n.Name.First().IsMatch(CharType.Letter))
                                    .Select(m => t.FullName + '.' + m.Name))
                            .Union(
                                t.GetProperties()
                                    .Where(n => n.Name.First().IsMatch(CharType.Letter))
                                    .Select(m => t.FullName + '.' + m.Name)))
                .Where(n => !n.EndsWith("GetHashCode"))
                .Where(n => !n.EndsWith("Equals"))
                .Where(n => !n.EndsWith("ToString"))
                .Where(n => !n.EndsWith("GetType"))
                .Distinct()
                .Select(c => c.CamelCaseToNormal());

        }

        //Name analyzers. Это временный код, потом его не будет :)
        static void Main(string[] args)
        {
            var resultFileName = "stat.txt";
            File.WriteAllText(resultFileName, "");
            File.AppendAllLines(resultFileName, new[] {"=====NS====="});
            File.AppendAllLines(resultFileName, GetAllNamespaces("Markdown"));
            File.AppendAllLines(resultFileName, new[] {"=====TYPES====="});
            File.AppendAllLines(resultFileName, GetAllTypes("Markdown"));
            File.AppendAllLines(resultFileName, new[] {"=====MTHDS====="});
            File.AppendAllLines(resultFileName, GetAllMethods("Markdown"));

        }
    }

    public static class StringHelper
    {
        public static string CamelCaseToNormal(this string str)
        {
            var builder = new StringBuilder();
            foreach (var c in str)
            {
                if (Char.IsUpper(c) || c == '_' || c == '.')
                {
                    builder.Append(" ");
                }
                if (c != '_')
                {
                    builder.Append(Char.ToLower(c));
                }
            }
            return builder.ToString();
        }
    }
}
