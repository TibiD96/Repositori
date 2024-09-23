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

        [StructLayout(LayoutKind.Sequential)]
        public struct TSNode
        {
            public IntPtr Id;
            private IntPtr tree;
            private uint context0;
            private uint context1;
            private uint context2;
            private uint context3;

            public void Clear()
            {
                Id = IntPtr.Zero;
                tree = IntPtr.Zero;
            }

            public bool IsZero()
            {
                return Id == IntPtr.Zero && tree == IntPtr.Zero;
            }

            public string Type()
            {
                return Marshal.PtrToStringAnsi(NativeMethods.ts_node_type(this));
            }

            internal string Type(TSLanguage lang)
            {
                return lang.symbol_name(symbol());
            }

            public ushort symbol()
            {
                return NativeMethods.ts_node_symbol(this);
            }

            public uint start_offset()
            {
                return NativeMethods.ts_node_start_byte(this) / sizeof(ushort);
            }

            public TSPoint start_point()
            {
                var pt = NativeMethods.ts_node_start_point(this); return new TSPoint(pt.row, pt.column / sizeof(ushort));
            }

            public uint end_offset()
            {
                return NativeMethods.ts_node_end_byte(this) / sizeof(ushort);
            }

            public TSPoint end_point()
            {
                var pt = NativeMethods.ts_node_end_point(this); return new TSPoint(pt.row, pt.column / sizeof(ushort));
            }

            public string to_string()
            {
                var dat = NativeMethods.ts_node_string(this); var str = Marshal.PtrToStringAnsi(dat); ts_node_string_free(dat); return str;
            }

            public bool is_null()
            {
                return NativeMethods.ts_node_is_null(this);
            }

            public bool is_named()
            {
                return NativeMethods.ts_node_is_named(this);
            }

            public bool is_missing()
            {
                return NativeMethods.ts_node_is_missing(this);
            }

            public bool is_extra()
            {
                return NativeMethods.ts_node_is_extra(this);
            }

            public bool has_changes()
            {
                return NativeMethods.ts_node_has_changes(this);
            }

            public bool has_error()
            {
                return NativeMethods.ts_node_has_error(this);
            }

            public TSNode parent()
            {
                return NativeMethods.ts_node_parent(this);
            }

            public TSNodechild(uint index)
            {
                return NativeMethods.ts_node_child(this, index);
            }

            public IntPtr field_name_for_child(uint index)
            {
                return NativeMethods.ts_node_field_name_for_child(this, index);
            }

            public uint child_count()
            {
                return NativeMethods.ts_node_child_count(this);
            }

            public TSNode named_child(uint index)
            {
                return NativeMethods.ts_node_named_child(this, index);
            }

            public uint named_child_count()
            {
                return NativeMethods.ts_node_named_child_count(this);
            }

            public TSNode child_by_field_name(string field_name)
            {
                return NativeMethods.ts_node_child_by_field_name(this, field_name, (uint)field_name.Length);
            }

            public TSNode child_by_field_id(ushort fieldId)
            {
                return NativeMethods.ts_node_child_by_field_id(this, fieldId);
            }

            public TSNode next_sibling()
            {
                return NativeMethods.ts_node_next_sibling(this);
            }

            public TSNode prev_sibling()
            {
                return NativeMethods.ts_node_prev_sibling(this);
            }

            public TSNode next_named_sibling()
            {
                return NativeMethods.ts_node_next_named_sibling(this);
            }

            public TSNode prev_named_sibling()
            {
                return NativeMethods.ts_node_prev_named_sibling(this);
            }

            public TSNode first_child_for_offset(uint offset)
            {
                return NativeMethods.ts_node_first_child_for_byte(this, offset * sizeof(ushort));
            }

            public TSNode first_named_child_for_offset(uint offset)
            {
                return NativeMethods.ts_node_first_named_child_for_byte(this, offset * sizeof(ushort));
            }

            public TSNode descendant_for_offset_range(uint start, uint end)
            {
                return NativeMethods.ts_node_descendant_for_byte_range(this, start * sizeof(ushort), end * sizeof(ushort));
            }

            public TSNode descendant_for_point_range(TSPoint start, TSPoint end)
            {
                return NativeMethods.ts_node_descendant_for_point_range(this, start, end);
            }

            public TSNode named_descendant_for_offset_range(uint start, uint end)
            {
                return NativeMethods.ts_node_named_descendant_for_byte_range(this, start * sizeof(ushort), end * sizeof(ushort));
            }

            public TSNode named_descendant_for_point_range(TSPoint start, TSPoint end)
            {
                return NativeMethods.ts_node_named_descendant_for_point_range(this, start, end);
            }

            public bool eq(TSNode other)
            {
                return NativeMethods.ts_node_eq(this, other);
            }

            public string text(string data)
            {
                uint beg = start_offset();
                uint end = end_offset();
                return data.Substring((int)beg, (int)(end - beg));
            }

            private class NativeMethods
            {
                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr ts_node_type(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern ushort ts_node_symbol(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern uint ts_node_start_byte(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSPoint ts_node_start_point(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern uint ts_node_end_byte(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSPoint ts_node_end_point(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr ts_node_string(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern void ts_node_string_free(IntPtr str);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern bool ts_node_is_null(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern bool ts_node_is_named(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern bool ts_node_is_missing(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern bool ts_node_is_extra(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern bool ts_node_has_changes(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern bool ts_node_has_error(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_node_parent(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_node_child(TSNode node, uint index);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr ts_node_field_name_for_child(TSNode node, uint index);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern uint ts_node_child_count(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_node_named_child(TSNode node, uint index);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern uint ts_node_named_child_count(TSNode node);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_node_child_by_field_name(TSNode self, [MarshalAs(UnmanagedType.LPUTF8Str)] string field_name, uint field_name_length);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_node_child_by_field_id(TSNode self, ushort fieldId);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_node_next_sibling(TSNode self);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_node_prev_sibling(TSNode self);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_node_next_named_sibling(TSNode self);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_node_prev_named_sibling(TSNode self);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_node_first_child_for_byte(TSNode self, uint byteOffset);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_node_first_named_child_for_byte(TSNode self, uint byteOffset);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_node_descendant_for_byte_range(TSNode self, uint startByte, uint endByte);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_node_descendant_for_point_range(TSNode self, TSPoint startPoint, TSPoint endPoint);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_node_named_descendant_for_byte_range(TSNode self, uint startByte, uint endByte);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_node_named_descendant_for_point_range(TSNode self, TSPoint startPoint, TSPoint endPoint);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern bool ts_node_eq(TSNode node1, TSNode node2);
            }
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

        internal sealed class TSTree : IDisposable
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

                NativeMethods.ts_tree_delete(Ptr);
                Ptr = IntPtr.Zero;
            }

            public TSTree Copy()
            {
                var ptr = NativeMethods.ts_tree_copy(Ptr);
                return ptr != IntPtr.Zero ? new TSTree(ptr) : null;
            }

            public TSNode root_node()
            {
                return NativeMethods.ts_tree_root_node(Ptr);
            }

            public TSNode root_node_with_offset(uint offsetBytes, TSPoint offsetPoint)
            {
                return NativeMethods.ts_tree_root_node_with_offset(Ptr, offsetBytes, offsetPoint);
            }

            public TSLanguage Language()
            {
                var ptr = NativeMethods.ts_tree_language(Ptr);
                return ptr != IntPtr.Zero ? new TSLanguage(ptr) : null;
            }

            public void Edit(TSInputEdit edit)
            {
                NativeMethods.ts_tree_edit(Ptr, ref edit);
            }

            private class NativeMethods
            {
                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr ts_tree_copy(IntPtr tree);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern void ts_tree_delete(IntPtr tree);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_tree_root_node(IntPtr tree);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern TSNode ts_tree_root_node_with_offset(IntPtr tree, uint offsetBytes, TSPoint offsetPoint);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr ts_tree_language(IntPtr tree);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr ts_tree_included_ranges(IntPtr tree, out uint length);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern void ts_tree_included_ranges_free(IntPtr ranges);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern void ts_tree_edit(IntPtr tree, ref TSInputEdit edit);

                [DllImport("tree-sitter.dll", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr ts_tree_get_changed_ranges(IntPtr old_tree, IntPtr new_tree, out uint length);
            }
        }
    }
}
