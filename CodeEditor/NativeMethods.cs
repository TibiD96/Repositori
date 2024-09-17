using System;
using System.Runtime.InteropServices;

namespace CodeEditor
{
    internal static class NativeMethods
    {
        [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ts_parser_new();

        [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ts_parser_delete(IntPtr parser);

        [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ts_parser_set_language(IntPtr parser, IntPtr language);

        [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern IntPtr ts_parser_parse_string(IntPtr parser, IntPtr tree, string source_code, uint length);

        [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ts_tree_root_node(IntPtr tree);

        [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ts_node_child_count(IntPtr node);

        [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ts_node_child(IntPtr node, uint index);

        [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ts_node_type(IntPtr node);

        [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ts_node_start_byte(IntPtr node);

        [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ts_node_end_byte(IntPtr node);

        [DllImport("c-sharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr tree_sitter_c_sharp();
    }
}
