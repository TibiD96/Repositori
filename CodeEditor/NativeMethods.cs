using System;
using System.Runtime.InteropServices;

namespace CodeEditor
{
    internal static class NativeMethods
    {
        [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ts_parser_new();
    }
}
