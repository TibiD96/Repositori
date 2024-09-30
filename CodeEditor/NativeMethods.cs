using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CodeEditor
{
    public class NativeMethods
    {
        [DllImport("c-sharp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr tree_sitter_c_sharp();

        [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_parser_new();

        [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_parser_delete(IntPtr parser);

        [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ts_parser_set_language(IntPtr parser, IntPtr language);

        [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_parser_language(IntPtr parser);
    }
}
