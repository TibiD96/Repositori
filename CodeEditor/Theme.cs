using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEditor
{
    public class Theme
    {
        public Dictionary<string, ConsoleColor> ThemeColors { get; private set; }

        public Theme()
        {
            ThemeColors = new Dictionary<string, ConsoleColor>
            {
                { "keyword", ConsoleColor.DarkMagenta },
                { "Statements", ConsoleColor.Magenta },
                { "Modifiers", ConsoleColor.Blue },
                { "number", ConsoleColor.DarkYellow }
            };
        }

        public static readonly HashSet<string> HighlightedNodeTypes = new HashSet<string>
        {
            "class_declaration",
            "struct_declaration",
            "interface_declaration",
            "method_declaration",
            "property_declaration",
            "variable_declaration",
            "identifier",
            "type_identifier",
            "number_literal",
            "integer_literal",
            "float_literal",
            "string_literal",
            "verbatim_string_literal",
            "keyword",
            "if",
            "for",
            "modifier"
        };
    }
}
