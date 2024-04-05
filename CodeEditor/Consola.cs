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

        public static void ShowContentOfFile(string[] lines, int currentLine, bool fastTravelMode, int startingLine = 0, int startingColumn = 0)
        {
            Console.CursorVisible = false;
            int visibleAreaWidth = Console.WindowWidth;
            int visibleAreaHight = Console.WindowHeight;
            int lineNumber;
            string line;
            int currentEndColumn;
            int currentStartColumn;

            if (lines == null)
            {
                return;
            }

            string maximumNumberOfLines = Convert.ToString(lines.Length - 1);

            ClearConsole();

            for (int i = startingLine; i < Math.Min(lines.Length, startingLine + visibleAreaHight); i++)
            {
                line = lines[i];
                lineNumber = i;
                string lineIndex = GenerateLineIndex(fastTravelMode, currentLine, lineNumber, maximumNumberOfLines) + " ";
                currentStartColumn = Math.Max(0, Math.Min(startingColumn, line.Length));
                currentEndColumn = line.Length - currentStartColumn <= visibleAreaWidth - lineIndex.Length ? line.Length - currentStartColumn : visibleAreaWidth - lineIndex.Length;
                if (i < Math.Min(lines.Length, startingLine + visibleAreaHight) - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(lineIndex);
                    Console.ResetColor();
                    Console.WriteLine(line.Substring(currentStartColumn, currentEndColumn));
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(lineIndex);
                    Console.ResetColor();
                    Console.Write(line.Substring(currentStartColumn, currentEndColumn));
                }
            }

            Console.CursorVisible = true;
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

        private static string GenerateLineIndex(bool fastTravelMode, int currentLine, int lineNumber, string maximumNumberOfLines)
        {
            string lineIndex = Convert.ToString(lineNumber);
            string curentLineGap = Convert.ToString(currentLine);
            if (fastTravelMode)
            {
                if (lineNumber != currentLine)
                {
                    lineIndex = Convert.ToString(Math.Abs(currentLine - lineNumber));

                    while (maximumNumberOfLines.Length + curentLineGap.Length - 1 > lineIndex.Length)
                    {
                        lineIndex = " " + lineIndex;
                    }

                    return lineIndex;
                }

                while (maximumNumberOfLines.Length + curentLineGap.Length - 1 > lineIndex.Length)
                {
                    lineIndex = curentLineGap.Length == maximumNumberOfLines.Length ? lineIndex + " " : " " + lineIndex + " ";
                }

                return lineIndex;
            }

            lineIndex = Convert.ToString(lineNumber + 1);
            for (int i = maximumNumberOfLines.Length; i > lineIndex.Length;)
            {
                lineIndex = " " + lineIndex;
            }

            return lineIndex;
        }
    }
}