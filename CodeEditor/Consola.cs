namespace CodeEditor
{
    public class Consola
    {
        public static void Menu()
        {
            Console.WriteLine("0. Exit application");
            Console.WriteLine("1. Show from file");
            Console.WriteLine("2. Show Menu");
        }

        public static void ShowContentOfFile(string fullPath, int startingLine = 0, int startingColumn = 0)
        {
            int visibleAreaWidth = Console.WindowWidth;
            int visibleAreaHight = Console.WindowHeight;
            string line;
            int currentEndColumn;
            int currentStartColumn;
            ClearConsole();
            string[] lines = File.ReadAllLines(fullPath);

            for (int i = startingLine; i < Math.Min(lines.Length, startingLine + visibleAreaHight); i++)
            {
                line = lines[i];
                currentStartColumn = Math.Max(0, Math.Min(startingColumn, line.Length));
                currentEndColumn = line.Length - currentStartColumn <= visibleAreaWidth ? line.Length - currentStartColumn : visibleAreaWidth;

                if (i < Math.Min(lines.Length, startingLine + visibleAreaHight) - 1)
                {
                    Console.WriteLine(line.Substring(currentStartColumn, currentEndColumn));
                }
                else
                {
                    Console.Write(line.Substring(currentStartColumn, currentEndColumn));
                }
            }
        }

        public static void ClearConsole()
        {
            for (int i = Console.WindowHeight - 1; i >= 0; i--)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, 0);
        }
    }
}
