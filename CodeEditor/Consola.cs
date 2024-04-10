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
                    WriteIndex(lineNumber, lineIndex, currentLine, fastTravelMode);
                    Console.WriteLine(line.Substring(currentStartColumn, currentEndColumn));
                }
                else
                {
                    WriteIndex(lineNumber, lineIndex, currentLine, fastTravelMode);
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

        public static string GenerateLineIndex(bool fastTravelMode, int currentLine, int lineNumber, string maximumNumberOfLines)
        {
            ArgumentNullException(maximumNumberOfLines);
            string lineIndex = Convert.ToString(lineNumber);
            int offset = maximumNumberOfLines.Length;
            if (fastTravelMode)
            {
                if (lineNumber != currentLine)
                {
                    lineIndex = Convert.ToString(Math.Abs(currentLine - lineNumber));

                    while (maximumNumberOfLines.Length - 1 + offset - 1 != lineIndex.Length)
                    {
                        lineIndex = " " + lineIndex;
                    }

                    return lineIndex;
                }

                while (maximumNumberOfLines.Length - 1 + offset - 1 != lineIndex.Length)
                {
                    if (lineIndex.Length == maximumNumberOfLines.Length)
                    {
                        lineIndex = lineIndex + " ";
                        break;
                    }

                    lineIndex = " " + lineIndex;
                }

                return lineIndex;
            }

            lineIndex = Convert.ToString(lineNumber + 1);
            while (maximumNumberOfLines.Length > lineIndex.Length)
            {
                lineIndex = " " + lineIndex;
            }

            return lineIndex;
        }

        private static void ArgumentNullException(string maximumNumberOfLines)
        {
            if (maximumNumberOfLines != null)
            {
                return;
            }

            throw new ArgumentNullException(maximumNumberOfLines);
        }

        private static void WriteIndex(int lineNumber, string lineIndex, int currentLine, bool fastTravelMode)
        {
            if (lineNumber == currentLine && fastTravelMode)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(lineIndex);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(lineIndex);
            }

            Console.ResetColor();
        }
    }
}