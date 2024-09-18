using System;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace CodeEditor
{
    public class Parser
    {
        public delegate void TSLogger(TSLogType logType, string message);

        public enum TSInputEncoding
        {
            TSInputEncodingUTF8,
            TSInputEncodingUTF16
        }

        public enum TSSymbolType
        {
            TSSymbolTypeRegular,
            TSSymbolTypeAnonymous,
            TSSymbolTypeAuxiliary,
        }

        public enum TSLogType
        {
            TSLogTypeParse,
            TSLogTypeLex,
        }

        public enum TSQuantifier
        {
            TSQuantifierZero = 0,
            TSQuantifierZeroOrOne,
            TSQuantifierZeroOrMore,
            TSQuantifierOne,
            TSQuantifierOneOrMore,
        }

        public enum TSQueryPredicateStepType
        {
            TSQueryPredicateStepTypeDone,
            TSQueryPredicateStepTypeCapture,
            TSQueryPredicateStepTypeString,
        }

        public enum TSQueryError
        {
            TSQueryErrorNone = 0,
            TSQueryErrorSyntax,
            TSQueryErrorNodeType,
            TSQueryErrorField,
            TSQueryErrorCapture,
            TSQueryErrorStructure,
            TSQueryErrorLanguage,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TSPoint
        {
            public uint row;
            public uint column;

            public TSPoint(uint row, uint column)
            {
                this.row = row;
                this.column = column;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TSRange
        {
            public TSPoint start_point;
            public TSPoint end_point;
            public uint start_byte;
            public uint end_byte;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TSInputEdit
        {
            public uint start_byte;
            public uint old_end_byte;
            public uint new_end_byte;
            public TSPoint start_point;
            public TSPoint old_end_point;
            public TSPoint new_end_point;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TSQueryCapture
        {
            public TSNode node;
            public uint index;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TSQueryMatch
        {
            public uint id;
            public ushort pattern_index;
            public ushort capture_count;
            public IntPtr captures;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TSQueryPredicateStep
        {
            public TSQueryPredicateStepType type;
            public uint value_id;
        }

        internal sealed class TSParser : IDisposable
        {
            public TSParser()
            {
                Ptr = NativeMethods.ts_parser_new();
            }

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            private delegate void TSLogCallback(IntPtr payload, TSLogType logType, [MarshalAs(UnmanagedType.LPUTF8Str)] string message);

            private IntPtr Ptr { get; set; }

            public void Dispose()
            {
                if (Ptr == IntPtr.Zero)
                {
                    return;
                }

                NativeMethods.ts_parser_delete(Ptr);
                Ptr = IntPtr.Zero;
            }

            public bool SetLanguage(TSLanguage language)
            {
                return NativeMethods.ts_parser_set_language(Ptr, language.Ptr);
            }

            public TSLanguage? Language()
            {
                var ptr = NativeMethods.ts_parser_language(Ptr);
                return ptr != IntPtr.Zero ? new TSLanguage(ptr) : null;
            }

            public bool SetIncludedRanges(TSRange[] ranges)
            {
                return NativeMethods.ts_parser_set_included_ranges(Ptr, ranges, (uint)ranges.Length);
            }

            public TSRange[] IncludedRanges()
            {
                uint length;
                return NativeMethods.ts_parser_included_ranges(Ptr, out length);
            }

            public TSTree? ParseString(TSTree oldTree, string input)
            {
                var ptr = NativeMethods.ts_parser_parse_string_encoding(Ptr, oldTree != null ? oldTree.Ptr : IntPtr.Zero, input, (uint)input.Length * 2, TSInputEncoding.TSInputEncodingUTF16);
                return ptr != IntPtr.Zero ? new TSTree(ptr) : null;
            }

            public void Reset()
            {
                NativeMethods.ts_parser_reset(Ptr);
            }

            public void SetTimeoutMicros(ulong timeout)
            {
                NativeMethods.ts_parser_set_timeout_micros(Ptr, timeout);
            }

            public ulong TimeoutMicros()
            {
                return NativeMethods.ts_parser_timeout_micros(Ptr);
            }

            public void SetLogger(TSLogger logger)
            {
                var code = new TSLoggerCode(logger);
                var data = new TSLoggerData { Log = logger != null ? new TSLogCallback(code.LogCallback) : null };
                NativeMethods.ts_parser_set_logger(Ptr, data);
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct TSLoggerData
            {
                internal TSLogCallback Log;
                private IntPtr payload;
            }

            private class TSLoggerCode
            {
                private readonly TSLogger logger;

                internal TSLoggerCode(TSLogger logger)
                {
                    this.logger = logger;
                }

                internal void LogCallback(IntPtr payload, TSLogType logType, string message)
                {
                    logger(logType, message);
                }
            }

            private class NativeMethods
            {
                [DllImport("tree-sitter-cpp.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr tree_sitter_cpp();

                [DllImport("tree-sitter-c-sharp.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr tree_sitter_c_sharp();

                [DllImport("tree-sitter-rust.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr tree_sitter_rust();

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr ts_parser_new();

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern void ts_parser_delete(IntPtr parser);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                [return: MarshalAs(UnmanagedType.I1)]
                public static extern bool ts_parser_set_language(IntPtr parser, IntPtr language);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr ts_parser_language(IntPtr parser);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern bool ts_parser_set_included_ranges(IntPtr parser, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] TSRange[] ranges, uint length);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                [return: MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
                public static extern TSRange[] ts_parser_included_ranges(IntPtr parser, out uint length);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr ts_parser_parse_string(IntPtr parser, IntPtr oldTree, [MarshalAs(UnmanagedType.LPUTF8Str)] string input, uint length);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr ts_parser_parse_string_encoding(IntPtr parser, IntPtr oldTree, [MarshalAs(UnmanagedType.LPWStr)] string input, uint length, TSInputEncoding encoding);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern void ts_parser_reset(IntPtr parser);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern void ts_parser_set_timeout_micros(IntPtr parser, ulong timeout);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern ulong ts_parser_timeout_micros(IntPtr parser);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern void ts_parser_set_cancellation_flag(IntPtr parser, ref IntPtr flag);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr ts_parser_cancellation_flag(IntPtr parser);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern void ts_parser_set_logger(IntPtr parser, TSLoggerData logger);
            }
        }

        internal sealed class TSLanguage : IDisposable
        {

            public string[] Symbols;
            public string[] Fields;
            public Dictionary<string, ushort> FieldIds;

            public TSLanguage(IntPtr ptr)
            {
                Ptr = ptr;

                Symbols = new string[Symbol_count() + 1];
                for (ushort i = 0; i < Symbols.Length; i++)
                {
                    Symbols[i] = Marshal.PtrToStringAnsi(NativeMethods.ts_language_symbol_name(Ptr, i));
                }

                Fields = new string[Field_count() + 1];
                FieldIds = new Dictionary<string, ushort>((int)Field_count() + 1);

                for (ushort i = 0; i < Fields.Length; i++)
                {
                    Fields[i] = Marshal.PtrToStringAnsi(NativeMethods.ts_language_field_name_for_id(Ptr, i));
                    if (Fields[i] != null)
                    {
                        FieldIds.Add(Fields[i], i);
                    }
                }

#if false
            for (int i = 0; i < symbols.Length; i++) {
                for (int j = 0; j < i; j++) {
                    Debug.Assert(symbols[i] != symbols[j]);
                }
            }

            for (int i = 0; i < fields.Length; i++) {
                for (int j = 0; j < i; j++) {
                    Debug.Assert(fields[i] != fields[j]);
                }
            }
#endif
            }

            internal IntPtr Ptr { get; private set; }

            public void Dispose()
            {
                if (Ptr == IntPtr.Zero)
                {
                    return;
                }

                Ptr = IntPtr.Zero;
            }

            public TSQuery Query_new(string source, out uint error_offset, out TSQueryError error_type)
            {
                var ptr = NativeMethods.ts_query_new(Ptr, source, (uint)source.Length, out error_offset, out error_type);
                return ptr != IntPtr.Zero ? new TSQuery(ptr) : null;
            }

            public uint Symbol_count()
            {
                return NativeMethods.ts_language_symbol_count(Ptr);
            }

            public string Symbol_name(ushort symbol)
            {
                return (symbol != ushort.MaxValue) ? Symbols[symbol] : "ERROR";
            }

            public ushort Symbol_for_name(string str, bool is_named)
            {
                return NativeMethods.ts_language_symbol_for_name(Ptr, str, (uint)str.Length, is_named);
            }

            public uint Field_count()
            {
                return NativeMethods.ts_language_field_count(Ptr);
            }

            public string Field_name_for_id(ushort fieldId)
            {
                return Fields[fieldId];
            }

            public ushort Field_id_for_name(string str)
            {
                return NativeMethods.ts_language_field_id_for_name(Ptr, str, (uint)str.Length);
            }

            public TSSymbolType Symbol_type(ushort symbol)
            {
                return NativeMethods.ts_language_symbol_type(Ptr, symbol);
            }

            private class NativeMethods
            {
                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr ts_query_new(IntPtr language, [MarshalAs(UnmanagedType.LPUTF8Str)] string source, uint source_len, out uint error_offset, out TSQueryError error_type);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern uint ts_language_symbol_count(IntPtr language);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr ts_language_symbol_name(IntPtr language, ushort symbol);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern ushort ts_language_symbol_for_name(IntPtr language, [MarshalAs(UnmanagedType.LPUTF8Str)] string str, uint length, bool is_named);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern uint ts_language_field_count(IntPtr language);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr ts_language_field_name_for_id(IntPtr language, ushort fieldId);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern ushort ts_language_field_id_for_name(IntPtr language, [MarshalAs(UnmanagedType.LPUTF8Str)] string str, uint length);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSSymbolType ts_language_symbol_type(IntPtr language, ushort symbol);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern uint ts_language_version(IntPtr language);
            }
        }

        public sealed class TSTree : IDisposable
        {
            public TSTree(IntPtr ptr)
            {
                Ptr = ptr;
            }

            internal IntPtr Ptr { get; private set; }

            public void Dispose()
            {
                if (Ptr == IntPtr.Zero)
                {
                    return;
                }

                ts_tree_delete(Ptr);
                Ptr = IntPtr.Zero;
            }

            public TSTree copy()
            {
                var ptr = ts_tree_copy(Ptr);
                return ptr != IntPtr.Zero ? new TSTree(ptr) : null;
            }

            public TSNode root_node() { return ts_tree_root_node(Ptr); }
            public TSNode root_node_with_offset(uint offsetBytes, TSPoint offsetPoint) { return ts_tree_root_node_with_offset(Ptr, offsetBytes, offsetPoint); }
            public TSLanguage language()
            {
                var ptr = ts_tree_language(Ptr);
                return ptr != IntPtr.Zero ? new TSLanguage(ptr) : null;
            }
            public void edit(TSInputEdit edit) { ts_tree_edit(Ptr, ref edit); }

            [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr ts_tree_copy(IntPtr tree);

            [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
            private static extern void ts_tree_delete(IntPtr tree);

            [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
            private static extern TSNode ts_tree_root_node(IntPtr tree);

            [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
            private static extern TSNode ts_tree_root_node_with_offset(IntPtr tree, uint offsetBytes, TSPoint offsetPoint);

            [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr ts_tree_language(IntPtr tree);

            [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr ts_tree_included_ranges(IntPtr tree, out uint length);

            [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
            private static extern void ts_tree_included_ranges_free(IntPtr ranges);

            [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
            private static extern void ts_tree_edit(IntPtr tree, ref TSInputEdit edit);

            [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr ts_tree_get_changed_ranges(IntPtr old_tree, IntPtr new_tree, out uint length);
            #endregion
        }
    }
}
