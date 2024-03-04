namespace CodeEditor
{
    public class Consola
    {
        private static int startingLine;
        private static int startingColumn;

        public static void Menu()
        {
            Console.WriteLine("0. Exit application");
            Console.WriteLine("1. Show from file");
            Console.WriteLine("2. Show Menu");
        }

        public static void ShowContentOfFile(string fullPath)
        {
            int visibleAreaWidth = Console.WindowWidth;
            int visibleAreaHight = Console.WindowHeight;
            string line;
            int currentEndColumn;
            int currentStartColumn;
            string[] lines = File.ReadAllLines(fullPath);
            if (startingLine > lines.Length - visibleAreaHight)
            {
                startingLine--;
                return;
            }

            ClearConsole();

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

        public static void MoveWindow(int horizontalPosition, int verticalPosition, ConsoleKeyInfo arrowDirection, string fullPath)
        {
            switch (arrowDirection.Key)
            {
                case ConsoleKey.UpArrow:
                    if (verticalPosition == 0 && startingLine != 0)
                    {
                        startingLine--;
                        ShowContentOfFile(fullPath);
                    }

                    break;

                case ConsoleKey.DownArrow:
                    startingLine++;
                    ShowContentOfFile(fullPath);

                    break;

                case ConsoleKey.LeftArrow:
                    if (horizontalPosition == 0 && startingColumn != 0)
                    {
                        startingColumn--;
                        ShowContentOfFile(fullPath);
                    }

                    break;

                case ConsoleKey.RightArrow:
                    startingColumn++;
                    ShowContentOfFile(fullPath);

                    break;
            }
        }
    }
}
