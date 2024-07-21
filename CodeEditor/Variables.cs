namespace CodeEditor
{
    public class Variables
    {
        public static bool EditAfterCursor { get; set; } = true;

        public static List<string[]> Undo { get; } = new List<string[]>();

        public static List<string[]> Redo { get; } = new List<string[]>();
    }
}
