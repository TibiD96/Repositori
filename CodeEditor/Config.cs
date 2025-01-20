namespace CodeEditor
{
    public class Config
    {
        public static bool FastTravel { get; } = true;

        public static List<string> Commands { get; } = ["edit", "quit", "quit!", "write"];
    }
}
