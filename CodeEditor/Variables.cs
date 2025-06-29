namespace CodeEditor
{
    public class Variables
    {
        public static bool EditAfterCursor { get; set; } = true;

        public static Stack<Stack<(int, string)>> Undo { get; } = new Stack<Stack<(int, string)>>();

        public static Stack<Stack<(int, string)>> Redo { get; } = new Stack<Stack<(int, string)>>();

        public static Stack<(int, int, int)> InfoToShowUndo { get; } = new Stack<(int, int, int)>();

        public static Stack<(int, int)> CursorPositionUndo { get; } = new Stack<(int, int)>();

        public static Stack<(int, int, int)> InfoToShowRedo { get; } = new Stack<(int, int, int)>();

        public static Stack<(int, int)> CursorPositionRedo { get; } = new Stack<(int, int)>();

        public static Stack<Stack<bool>> UndoDeleteLine { get; } = new Stack<Stack<bool>>();

        public static Stack<Stack<bool>> UndoAddLine { get; } = new Stack<Stack<bool>>();

        public static Stack<Stack<bool>> RedoDeleteLine { get; } = new Stack<Stack<bool>>();

        public static Stack<Stack<bool>> RedoAddLine { get; } = new Stack<Stack<bool>>();

        public static string ErrorMesage { get; set; } = "";
    }
}
