namespace CodeEditor
{
    public class Config
    {
        public static bool FastTravel { get; } = true;

        public static List<string> Commands { get; } = ["e", "edit", "q", "quit", "q!", "quit!", "w", "write"];
    }
}
