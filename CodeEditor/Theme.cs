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
                { "function", ConsoleColor.Blue },
                { "type", ConsoleColor.Green },
                { "number", ConsoleColor.DarkYellow },
                { "string", ConsoleColor.DarkYellow },
                { "comment", ConsoleColor.DarkGray },
                { "preprocessor", ConsoleColor.DarkCyan },
                { "default", ConsoleColor.White }
            };
        }

        public static readonly HashSet<string> HighlightedNodeTypes = new HashSet<string>
        {
            "class_declaration",
            "predefined_type",
            "struct_declaration",
            "interface_declaration",
            "enum_declaration",
            "method_declaration",
            "property_declaration",
            "identifier",
            "type_identifier",
            "number_literal",
            "integer_literal",
            "float_literal",
            "string_literal",
            "verbatim_string_literal",
            "keyword",
            "comment",
            "preprocessor_statement",
            "attribute",
            "namespace_declaration",
            "using_directive",
        };
    }
}
