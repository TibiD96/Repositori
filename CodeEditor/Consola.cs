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
            const int visibleAreaWidth = 119;
            const int visibleAreaHight = 30;
            string line;
            int currentEndColumn;
            int currentStartColumn;
            ClearConsole();
            ConsoleSizeing();
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

        public static void ConsoleSizeing()
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                Console.SetWindowSize(120, 30);
            }
            else
            {
                Console.Write($"\x1b[8;{30};{120}t");
            }
        }

        public static void ClearConsole()
        {
            if (Console.CursorTop >= 29)
            {
                for (int i = Console.CursorTop; i >= 0; i--)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write(new string(' ', Console.WindowWidth));
                }
            }
            else
            {
                for (int i = 29; i >= 0; i--)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write(new string(' ', Console.WindowWidth));
                }
            }

            Console.SetCursorPosition(0, 0);
        }
    }
}
