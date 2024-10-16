using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CodeEditor
{
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

    public struct HighlightedNode
    {
        public string Type;
        public uint StartByte;
        public uint EndByte;
    }

    public delegate void TSLogger(TSLogType logType, string message);

    public sealed class TSParser : IDisposable
    {
        private const string treeSitter = "libtree-sitter";

        static TSParser()
        {
            LibraryChooser();
        }


        private IntPtr Ptr { get; set; }

        public TSParser()
        {
            Ptr = ts_parser_new();
        }

        public void Dispose()
        {
            if (Ptr != IntPtr.Zero)
            {
                ts_parser_delete(Ptr);
                Ptr = IntPtr.Zero;
            }
        }

        public bool set_language(TSLanguage language) 
        { 
            return ts_parser_set_language(Ptr, language.Ptr); 
        }

        public TSLanguage language()
        {
            var ptr = ts_parser_language(Ptr);
            return ptr != IntPtr.Zero ? new TSLanguage(ptr) : null!;
        }

        public bool set_included_ranges(TSRange[] ranges)
        {
            return ts_parser_set_included_ranges(Ptr, ranges, (uint)ranges.Length);
        }
        public TSRange[] included_ranges()
        {
            uint length;
            return ts_parser_included_ranges(Ptr, out length);
        }

        public TSTree parse_string(TSTree oldTree, string input)
        {
            var ptr = ts_parser_parse_string_encoding(Ptr, oldTree != null ? oldTree.Ptr : IntPtr.Zero,
                                                        input, (uint)input.Length * 2, TSInputEncoding.TSInputEncodingUTF16);
            return ptr != IntPtr.Zero ? new TSTree(ptr) : null!;
        }

        public void reset() { ts_parser_reset(Ptr); }
        public void set_timeout_micros(ulong timeout) { ts_parser_set_timeout_micros(Ptr, timeout); }
        public ulong timeout_micros() { return ts_parser_timeout_micros(Ptr); }
        public void set_logger(TSLogger logger)
        {
            var code = new _TSLoggerCode(logger);
            var data = new _TSLoggerData { Log = logger != null ? new TSLogCallback(code.LogCallback) : null! };
            ts_parser_set_logger(Ptr, data);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct _TSLoggerData
        {
            private IntPtr Payload;
            internal TSLogCallback Log;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void TSLogCallback(IntPtr payload, TSLogType logType, [MarshalAs(UnmanagedType.LPUTF8Str)] string message);

        private class _TSLoggerCode
        {
            private TSLogger logger;

            internal _TSLoggerCode(TSLogger logger)
            {
                this.logger = logger;
            }

            internal void LogCallback(IntPtr payload, TSLogType logType, string message)
            {
                logger(logType, message);
            }
        }

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_parser_new();

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_parser_delete(IntPtr parser);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ts_parser_set_language(IntPtr parser, IntPtr language);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_parser_language(IntPtr parser);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_parser_set_included_ranges(IntPtr parser, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] TSRange[] ranges, uint length);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
        private static extern TSRange[] ts_parser_included_ranges(IntPtr parser, out uint length);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_parser_parse_string(IntPtr parser, IntPtr oldTree, [MarshalAs(UnmanagedType.LPUTF8Str)] string input, uint length);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_parser_parse_string_encoding(IntPtr parser, IntPtr oldTree, [MarshalAs(UnmanagedType.LPWStr)] string input, uint length, TSInputEncoding encoding);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_parser_reset(IntPtr parser);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_parser_set_timeout_micros(IntPtr parser, ulong timeout);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong ts_parser_timeout_micros(IntPtr parser);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_parser_set_cancellation_flag(IntPtr parser, ref IntPtr flag);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_parser_cancellation_flag(IntPtr parser);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_parser_set_logger(IntPtr parser, _TSLoggerData logger);

        private static void LibraryChooser()
        {
            string treeSitterLib;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                treeSitterLib = $"{treeSitter}.dll"; ;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                treeSitterLib = $"{treeSitter}.so";
            }
            else
            {
                throw new PlatformNotSupportedException("Platform not supported.");
            }
        }
    }
    
    public sealed class TSTree : IDisposable
    {
        private const string treeSitter = "libtree-sitter";

        static TSTree()
        {
            LibraryChooser();
        }

        internal IntPtr Ptr { get; private set; }

        public TSTree(IntPtr ptr)
        {
            Ptr = ptr;
        }

        public void Dispose()
        {
            if (Ptr != IntPtr.Zero)
            {
                ts_tree_delete(Ptr);
                Ptr = IntPtr.Zero;
            }
        }

        public TSTree copy()
        {
            var ptr = ts_tree_copy(Ptr);
            return ptr != IntPtr.Zero ? new TSTree(ptr) : null!;
        }
        public TSNode root_node() { return ts_tree_root_node(Ptr); }
        public TSNode root_node_with_offset(uint offsetBytes, TSPoint offsetPoint) { return ts_tree_root_node_with_offset(Ptr, offsetBytes, offsetPoint); }
        public TSLanguage language()
        {
            var ptr = ts_tree_language(Ptr);
            return ptr != IntPtr.Zero ? new TSLanguage(ptr) : null!;
        }
        public void edit(TSInputEdit edit) { ts_tree_edit(Ptr, ref edit); }

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_tree_copy(IntPtr tree);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_tree_delete(IntPtr tree);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_tree_root_node(IntPtr tree);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_tree_root_node_with_offset(IntPtr tree, uint offsetBytes, TSPoint offsetPoint);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_tree_language(IntPtr tree);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_tree_included_ranges(IntPtr tree, out uint length);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_tree_included_ranges_free(IntPtr ranges);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_tree_edit(IntPtr tree, ref TSInputEdit edit);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_tree_get_changed_ranges(IntPtr old_tree, IntPtr new_tree, out uint length);

        private static void LibraryChooser()
        {
            string treeSitterLib;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                treeSitterLib = $"{treeSitter}.dll"; ;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                treeSitterLib = $"{treeSitter}.so";
            }
            else
            {
                throw new PlatformNotSupportedException("Platform not supported.");
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TSNode
    {
        private uint context0;
        private uint context1;
        private uint context2;
        private uint context3;
        public IntPtr id;
        private IntPtr tree;

        private const string treeSitter = "libtree-sitter";

        static TSNode()
        {
            LibraryChooser();
        }

        public void clear() { id = IntPtr.Zero; tree = IntPtr.Zero; }
        public bool is_zero() { return (id == IntPtr.Zero && tree == IntPtr.Zero); }
        public string type() { return Marshal.PtrToStringAnsi(ts_node_type(this))!; }
        public string type(TSLanguage lang) { return lang.symbol_name(symbol()); }
        public ushort symbol() { return ts_node_symbol(this); }
        public uint start_offset() { return ts_node_start_byte(this) / sizeof(ushort); }
        public TSPoint start_point() { var pt = ts_node_start_point(this); return new TSPoint(pt.row, pt.column / sizeof(ushort)); }
        public uint end_offset() { return ts_node_end_byte(this) / sizeof(ushort); }
        public TSPoint end_point() { var pt = ts_node_end_point(this); return new TSPoint(pt.row, pt.column / sizeof(ushort)); }
        public string to_string() { var dat = ts_node_string(this); var str = Marshal.PtrToStringAnsi(dat); ts_node_string_free(dat); return str!; }
        public bool is_null() { return ts_node_is_null(this); }
        public bool is_named() { return ts_node_is_named(this); }
        public bool is_missing() { return ts_node_is_missing(this); }
        public bool is_extra() { return ts_node_is_extra(this); }
        public bool has_changes() { return ts_node_has_changes(this); }
        public bool has_error() { return ts_node_has_error(this); }
        public TSNode parent() { return ts_node_parent(this); }
        public TSNode child(uint index) { return ts_node_child(this, index); }
        public IntPtr field_name_for_child(uint index) { return ts_node_field_name_for_child(this, index); }
        public uint child_count() { return ts_node_child_count(this); }
        public TSNode named_child(uint index) { return ts_node_named_child(this, index); }
        public uint named_child_count() { return ts_node_named_child_count(this); }
        public TSNode child_by_field_name(string field_name) { return ts_node_child_by_field_name(this, field_name, (uint)field_name.Length); }
        public TSNode child_by_field_id(ushort fieldId) { return ts_node_child_by_field_id(this, fieldId); }
        public TSNode next_sibling() { return ts_node_next_sibling(this); }
        public TSNode prev_sibling() { return ts_node_prev_sibling(this); }
        public TSNode next_named_sibling() { return ts_node_next_named_sibling(this); }
        public TSNode prev_named_sibling() { return ts_node_prev_named_sibling(this); }
        public TSNode first_child_for_offset(uint offset) { return ts_node_first_child_for_byte(this, offset * sizeof(ushort)); }
        public TSNode first_named_child_for_offset(uint offset) { return ts_node_first_named_child_for_byte(this, offset * sizeof(ushort)); }
        public TSNode descendant_for_offset_range(uint start, uint end) { return ts_node_descendant_for_byte_range(this, start * sizeof(ushort), end * sizeof(ushort)); }
        public TSNode descendant_for_point_range(TSPoint start, TSPoint end) { return ts_node_descendant_for_point_range(this, start, end); }
        public TSNode named_descendant_for_offset_range(uint start, uint end) { return ts_node_named_descendant_for_byte_range(this, start * sizeof(ushort), end * sizeof(ushort)); }
        public TSNode named_descendant_for_point_range(TSPoint start, TSPoint end) { return ts_node_named_descendant_for_point_range(this, start, end); }
        public bool eq(TSNode other) { return ts_node_eq(this, other); }

        public string text(string data)
        {
            uint beg = start_offset();
            uint end = end_offset();
            return data.Substring((int)beg, (int)(end - beg));
        }

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_node_type(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort ts_node_symbol(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_node_start_byte(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSPoint ts_node_start_point(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_node_end_byte(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSPoint ts_node_end_point(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_node_string(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_node_string_free(IntPtr str);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_node_is_null(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_node_is_named(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_node_is_missing(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_node_is_extra(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_node_has_changes(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_node_has_error(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_parent(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_child(TSNode node, uint index);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_node_field_name_for_child(TSNode node, uint index);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_node_child_count(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_named_child(TSNode node, uint index);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_node_named_child_count(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_child_by_field_name(TSNode self, [MarshalAs(UnmanagedType.LPUTF8Str)] string field_name, uint field_name_length);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_child_by_field_id(TSNode self, ushort fieldId);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_next_sibling(TSNode self);
        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_prev_sibling(TSNode self);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_next_named_sibling(TSNode self);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_prev_named_sibling(TSNode self);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_first_child_for_byte(TSNode self, uint byteOffset);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_first_named_child_for_byte(TSNode self, uint byteOffset);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_descendant_for_byte_range(TSNode self, uint startByte, uint endByte);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_descendant_for_point_range(TSNode self, TSPoint startPoint, TSPoint endPoint);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_named_descendant_for_byte_range(TSNode self, uint startByte, uint endByte);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_node_named_descendant_for_point_range(TSNode self, TSPoint startPoint, TSPoint endPoint);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_node_eq(TSNode node1, TSNode node2);

        private static void LibraryChooser()
        {
            string treeSitterLib;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                treeSitterLib = $"{treeSitter}.dll"; ;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                treeSitterLib = $"{treeSitter}.so";
            }
            else
            {
                throw new PlatformNotSupportedException("Platform not supported.");
            }
        }
    }

    public sealed class TSCursor : IDisposable
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct TSTreeCursor
        {
            private IntPtr Tree;
            private IntPtr Id;
            private uint Context0;
            private uint Context1;
        }


        private IntPtr Ptr;
        private TSTreeCursor cursor;
        public TSLanguage lang { get; private set; }

        public TSCursor(TSTreeCursor cursor, TSLanguage lang)
        {
            this.cursor = cursor;
            this.lang = lang;
            Ptr = new IntPtr(1);
        }

        public TSCursor(TSNode node, TSLanguage lang)
        {
            this.cursor = ts_tree_cursor_new(node);
            this.lang = lang;
            Ptr = new IntPtr(1);
        }

        public void Dispose()
        {
            if (Ptr != IntPtr.Zero)
            {
                ts_tree_cursor_delete(ref cursor);
                Ptr = IntPtr.Zero;
            }
        }

        private const string treeSitter = "libtree-sitter";

        static TSCursor()
        {
            LibraryChooser();
        }

        public void reset(TSNode node) { ts_tree_cursor_reset(ref cursor, node); }
        public TSNode current_node() { return ts_tree_cursor_current_node(ref cursor); }
        public string current_field() { return lang.fields[current_field_id()]; }
        public string current_symbol()
        {
            ushort symbol = ts_tree_cursor_current_node(ref cursor).symbol();
            return (symbol != UInt16.MaxValue) ? lang.symbols[symbol] : "ERROR";
        }
        public ushort current_field_id() { return ts_tree_cursor_current_field_id(ref cursor); }
        public bool goto_parent() { return ts_tree_cursor_goto_parent(ref cursor); }
        public bool goto_next_sibling() { return ts_tree_cursor_goto_next_sibling(ref cursor); }
        public bool goto_first_child() { return ts_tree_cursor_goto_first_child(ref cursor); }
        public long goto_first_child_for_offset(uint offset) { return ts_tree_cursor_goto_first_child_for_byte(ref cursor, offset * sizeof(ushort)); }
        public long goto_first_child_for_point(TSPoint point) { return ts_tree_cursor_goto_first_child_for_point(ref cursor, point); }
        public TSCursor copy() { return new TSCursor(ts_tree_cursor_copy(ref cursor), lang); }

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSTreeCursor ts_tree_cursor_new(TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_tree_cursor_delete(ref TSTreeCursor cursor);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_tree_cursor_reset(ref TSTreeCursor cursor, TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSNode ts_tree_cursor_current_node(ref TSTreeCursor cursor);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_tree_cursor_current_field_name(ref TSTreeCursor cursor);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort ts_tree_cursor_current_field_id(ref TSTreeCursor cursor);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_tree_cursor_goto_parent(ref TSTreeCursor cursor);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_tree_cursor_goto_next_sibling(ref TSTreeCursor cursor);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_tree_cursor_goto_first_child(ref TSTreeCursor cursor);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern long ts_tree_cursor_goto_first_child_for_byte(ref TSTreeCursor cursor, uint byteOffset);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern long ts_tree_cursor_goto_first_child_for_point(ref TSTreeCursor cursor, TSPoint point);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSTreeCursor ts_tree_cursor_copy(ref TSTreeCursor cursor);

        private static void LibraryChooser()
        {
            string treeSitterLib;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                treeSitterLib = $"{treeSitter}.dll"; ;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                treeSitterLib = $"{treeSitter}.so";
            }
            else
            {
                throw new PlatformNotSupportedException("Platform not supported.");
            }
        }

    }

    public sealed class TSQuery : IDisposable
    {
        internal IntPtr Ptr { get; private set; }

        public TSQuery(IntPtr ptr)
        {
            Ptr = ptr;
        }

        public void Dispose()
        {
            if (Ptr != IntPtr.Zero)
            {
                ts_query_delete(Ptr);
                Ptr = IntPtr.Zero;
            }
        }

        private const string treeSitter = "libtree-sitter";

        static TSQuery()
        {
            LibraryChooser();
        }

        public uint pattern_count() { return ts_query_pattern_count(Ptr); }
        public uint capture_count() { return ts_query_capture_count(Ptr); }
        public uint string_count() { return ts_query_string_count(Ptr); }
        public uint start_offset_for_pattern(uint patternIndex) { return ts_query_start_byte_for_pattern(Ptr, patternIndex) / sizeof(ushort); }
        public IntPtr predicates_for_pattern(uint patternIndex, out uint length) { return ts_query_predicates_for_pattern(Ptr, patternIndex, out length); }
        public bool is_pattern_rooted(uint patternIndex) { return ts_query_is_pattern_rooted(Ptr, patternIndex); }
        public bool is_pattern_non_local(uint patternIndex) { return ts_query_is_pattern_non_local(Ptr, patternIndex); }
        public bool is_pattern_guaranteed_at_offset(uint offset) { return ts_query_is_pattern_guaranteed_at_step(Ptr, offset / sizeof(ushort)); }
        public string capture_name_for_id(uint id, out uint length) { return Marshal.PtrToStringAnsi(ts_query_capture_name_for_id(Ptr, id, out length))!; }
        public TSQuantifier capture_quantifier_for_id(uint patternId, uint captureId) { return ts_query_capture_quantifier_for_id(Ptr, patternId, captureId); }
        public string string_value_for_id(uint id, out uint length) { return Marshal.PtrToStringAnsi(ts_query_string_value_for_id(Ptr, id, out length))!; }
        public void disable_capture(string captureName) { ts_query_disable_capture(Ptr, captureName, (uint)captureName.Length); }
        public void disable_pattern(uint patternIndex) { ts_query_disable_pattern(Ptr, patternIndex); }

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_delete(IntPtr query);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_query_pattern_count(IntPtr query);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_query_capture_count(IntPtr query);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_query_string_count(IntPtr query);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_query_start_byte_for_pattern(IntPtr query, uint patternIndex);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_query_predicates_for_pattern(IntPtr query, uint patternIndex, out uint length);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_query_is_pattern_rooted(IntPtr query, uint patternIndex);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_query_is_pattern_non_local(IntPtr query, uint patternIndex);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_query_is_pattern_guaranteed_at_step(IntPtr query, uint byteOffset);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_query_capture_name_for_id(IntPtr query, uint id, out uint length);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSQuantifier ts_query_capture_quantifier_for_id(IntPtr query, uint patternId, uint captureId);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_query_string_value_for_id(IntPtr query, uint id, out uint length);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_disable_capture(IntPtr query, [MarshalAs(UnmanagedType.LPUTF8Str)] string captureName, uint captureNameLength);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_disable_pattern(IntPtr query, uint patternIndex);

        private static void LibraryChooser()
        {
            string treeSitterLib;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                treeSitterLib = $"{treeSitter}.dll"; ;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                treeSitterLib = $"{treeSitter}.so";
            }
            else
            {
                throw new PlatformNotSupportedException("Platform not supported.");
            }
        }
    }

    public sealed class TSQueryCursor : IDisposable
    {
        private IntPtr Ptr { get; set; }

        private TSQueryCursor(IntPtr ptr)
        {
            Ptr = ptr;
        }

        public TSQueryCursor()
        {
            Ptr = ts_query_cursor_new();
        }

        public void Dispose()
        {
            if (Ptr != IntPtr.Zero)
            {
                ts_query_cursor_delete(Ptr);
                Ptr = IntPtr.Zero;
            }
        }

        private const string treeSitter = "libtree-sitter";

        static TSQueryCursor()
        {
            LibraryChooser();
        }

        public void exec(TSQuery query, TSNode node) { ts_query_cursor_exec(Ptr, query.Ptr, node); }
        public bool did_exceed_match_limit() { return ts_query_cursor_did_exceed_match_limit(Ptr); }
        public uint match_limit() { return ts_query_cursor_match_limit(Ptr); }
        public void set_match_limit(uint limit) { ts_query_cursor_set_match_limit(Ptr, limit); }
        public void set_range(uint start, uint end) { ts_query_cursor_set_byte_range(Ptr, start * sizeof(ushort), end * sizeof(ushort)); }
        public void set_point_range(TSPoint start, TSPoint end) { ts_query_cursor_set_point_range(Ptr, start, end); }
        public bool next_match(out TSQueryMatch match, out TSQueryCapture[]? captures)
        {
            captures = null;
            if (ts_query_cursor_next_match(Ptr, out match))
            {
                if (match.capture_count > 0)
                {
                    captures = new TSQueryCapture[match.capture_count];
                    for (ushort i = 0; i < match.capture_count; i++)
                    {
                        var intPtr = match.captures + Marshal.SizeOf(typeof(TSQueryCapture)) * i;
                        captures[i] = Marshal.PtrToStructure<TSQueryCapture>(intPtr);
                    }
                }
                return true;
            }
            return false;
        }
        public void remove_match(uint id) { ts_query_cursor_remove_match(Ptr, id); }
        public bool next_capture(out TSQueryMatch match, out uint index) { return ts_query_cursor_next_capture(Ptr, out match, out index); }

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_query_cursor_new();

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_cursor_delete(IntPtr cursor);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_cursor_exec(IntPtr cursor, IntPtr query, TSNode node);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_query_cursor_did_exceed_match_limit(IntPtr cursor);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_query_cursor_match_limit(IntPtr cursor);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_cursor_set_match_limit(IntPtr cursor, uint limit);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_cursor_set_byte_range(IntPtr cursor, uint start_byte, uint end_byte);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_cursor_set_point_range(IntPtr cursor, TSPoint start_point, TSPoint end_point);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_query_cursor_next_match(IntPtr cursor, out TSQueryMatch match);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ts_query_cursor_remove_match(IntPtr cursor, uint id);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ts_query_cursor_next_capture(IntPtr cursor, out TSQueryMatch match, out uint capture_index);

        private static void LibraryChooser()
        {
            string treeSitterLib;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                treeSitterLib = $"{treeSitter}.dll"; ;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                treeSitterLib = $"{treeSitter}.so";
            }
            else
            {
                throw new PlatformNotSupportedException("Platform not supported.");
            }
        }
    }

    public sealed class TSLanguage : IDisposable
    {
        internal IntPtr Ptr { get; private set; }

        private const string treeSitter = "libtree-sitter";

        static TSLanguage()
        {
            LibraryChooser();
        }

        public TSLanguage(IntPtr ptr)
        {
            Ptr = ptr;

            symbols = new String[symbol_count() + 1];
            for (ushort i = 0; i < symbols.Length; i++)
            {
                symbols[i] = Marshal.PtrToStringAnsi(ts_language_symbol_name(Ptr, i))!;
            }

            fields = new String[field_count() + 1];
            fieldIds = new Dictionary<string, ushort>((int)field_count() + 1);

            for (ushort i = 0; i < fields.Length; i++)
            {
                fields[i] = Marshal.PtrToStringAnsi(ts_language_field_name_for_id(Ptr, i))!;
                if (fields[i] != null)
                {
                    fieldIds.Add(fields[i], i);
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

        public void Dispose()
        {
            if (Ptr != IntPtr.Zero)
            {
                Ptr = IntPtr.Zero;
            }
        }

        public TSQuery query_new(string source, out uint error_offset, out TSQueryError error_type)
        {
            var ptr = ts_query_new(Ptr, source, (uint)source.Length, out error_offset, out error_type);
            return ptr != IntPtr.Zero ? new TSQuery(ptr) : null!;
        }

        public String[] symbols;
        public String[] fields;
        public Dictionary<String, ushort> fieldIds;

        public uint symbol_count() { return ts_language_symbol_count(Ptr); }
        public string symbol_name(ushort symbol) { return (symbol != UInt16.MaxValue) ? symbols[symbol] : "ERROR"; }
        public ushort symbol_for_name(string str, bool is_named) { return ts_language_symbol_for_name(Ptr, str, (uint)str.Length, is_named); }
        public uint field_count() { return ts_language_field_count(Ptr); }
        public string field_name_for_id(ushort fieldId) { return fields[fieldId]; }
        public ushort field_id_for_name(string str) { return ts_language_field_id_for_name(Ptr, str, (uint)str.Length); }
        public TSSymbolType symbol_type(ushort symbol) { return ts_language_symbol_type(Ptr, symbol); }

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_query_new(IntPtr language, [MarshalAs(UnmanagedType.LPUTF8Str)] string source, uint source_len, out uint error_offset, out TSQueryError error_type);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_language_symbol_count(IntPtr language);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_language_symbol_name(IntPtr language, ushort symbol);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort ts_language_symbol_for_name(IntPtr language, [MarshalAs(UnmanagedType.LPUTF8Str)] string str, uint length, bool is_named);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_language_field_count(IntPtr language);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ts_language_field_name_for_id(IntPtr language, ushort fieldId);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort ts_language_field_id_for_name(IntPtr language, [MarshalAs(UnmanagedType.LPUTF8Str)] string str, uint length);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern TSSymbolType ts_language_symbol_type(IntPtr language, ushort symbol);

        [DllImport(treeSitter, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ts_language_version(IntPtr language);

        private static void LibraryChooser()
        {
            string treeSitterLib;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                treeSitterLib = $"{treeSitter}.dll"; ;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                treeSitterLib = $"{treeSitter}.so";
            }
            else
            {
                throw new PlatformNotSupportedException("Platform not supported.");
            }
        }
    }
}
