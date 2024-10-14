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
                { "Statement", ConsoleColor.Magenta },
                { "Keywords", ConsoleColor.Green },
                { "Characters/Strings", ConsoleColor.DarkYellow },
                { "Identifiers", ConsoleColor.Blue },
                { "Comments", ConsoleColor.Gray},
                { "default", ConsoleColor.White }
            };
        }

        public static readonly HashSet<string> HighlightedNodeTypes = new HashSet<string>
        {
            "class",
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
            "namespace_declaration",
            "using",
            "implicit_type",
            "if",
            "foreach",
            "modifier",
            "for",
            "else",
            "continue",
            "while",
            "new",
            "return",
            "break",
            "do",
            "in",
            "namespace"
        };
    }
}
