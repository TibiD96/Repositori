namespace CodeEditor
{
    public class Variables
    {
        public static bool EditAfterCursor { get; set; } = true;

        public static Stack<Stack<(int, string)>> Undo { get; } = new Stack<Stack<(int, string)>>();

        public static Stack<Stack<(int, string)>> Redo { get; } = new Stack<Stack<(int, string)>>();

        public static Stack<(int, int, int)> InfoToShow { get; } = new Stack<(int, int, int)>();

        public static Stack<(int, int)> CursorPosition { get; } = new Stack<(int, int)>();
    }
}
