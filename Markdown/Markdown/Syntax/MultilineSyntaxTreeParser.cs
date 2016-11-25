using Markdown.Languages;

namespace Markdown.Syntax
{
    public class MultilineSyntaxTreeParser : SyntaxTreeParser
    {
        public MultilineSyntaxTreeParser(LanguageSyntax syntax) : base(syntax)
        {
        }

        //public override SyntaxNode Parse(string text)
        //{
        //    var lines = text.Split('\n');
        //    string prev = null;
        //    Tag opennedGlobalTag = null;
        //    for (int i = 0; i < lines.Length; i++)
        //    {
        //        var current = lines[i];
        //        var next = i < lines.Length - 1 ? lines[i + 1] : null;
        //        var env = EnviromentTypeHelper.GetType(prev, next);
        //        if (opennedGlobalTag == null)
        //        {
        //            FindBeginOfTag(current, env);
        //        }
        //        else
        //        {
                            
        //        }
        //        prev = current;
        //    }
        //}

        //private void FindBeginOfTag(string line, EnviromentType enviroment)
        //{
        //    var available = 
        //}

        //private void FindEndOfOpennedTag(string line)
        //{
            
        //}
    }
}