using System;

namespace CodeEditor
{
    public class Consola
    {
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

        public static void ShowDirectoryContent(string[] fileFromDirectory)
        {
            const int searchBarDim = 4;
            DrawContour();
            int startingLine = Console.WindowHeight - (searchBarDim + 1);
            if (fileFromDirectory == null)
            {
                return;
            }

            ClearResultsWindow();
            for (int i = 0; i < fileFromDirectory.Length && startingLine != 0; i++)
            {
                Console.Write(Path.GetFileName(fileFromDirectory[i]));
                startingLine--;
                Console.SetCursorPosition(1, startingLine);
            }
        }

        public static void ShowValidResults(List<string> validFiles, string search, string[] totatlNumberOfFiles)
        {
            if (validFiles == null || totatlNumberOfFiles == null || search == null)
            {
                return;
            }

            int corsorLeftPosition;
            const int searchBarDim = 4;
            int startingLine = Console.WindowHeight - (searchBarDim + 1);

            ClearResultsWindow();

            for (int i = 0; i < validFiles.Count && startingLine != 0; i++)
            {
                int startingIndex = Path.GetFileName(validFiles[i]).IndexOf(search, StringComparison.OrdinalIgnoreCase);
                if (startingIndex >= 0)
                {
                    string firstSection = Path.GetFileName(validFiles[i]).Substring(0, startingIndex);
                    string colored = Path.GetFileName(validFiles[i]).Substring(firstSection.Length, search.Length);
                    string lastSection = Path.GetFileName(validFiles[i]).Substring(startingIndex + search.Length);
                    Console.Write(firstSection);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(colored);
                    Console.ResetColor();
                    Console.Write(lastSection);
                    startingLine--;
                    Console.SetCursorPosition(1, startingLine);
                }
                else
                {
                    HilightChar(validFiles[i], search);
                    startingLine--;
                    Console.SetCursorPosition(1, startingLine);
                }
            }

            corsorLeftPosition = Console.WindowWidth - (Convert.ToString(totatlNumberOfFiles.Length).Length + Convert.ToString(validFiles.Count).Length + 2);
            Console.SetCursorPosition(search.Length + 1, Console.WindowHeight - 2);
            Console.Write(new string(' ', Console.WindowWidth - (2 + search.Length)));
            Console.SetCursorPosition(corsorLeftPosition, Console.WindowHeight - 2);
            Console.Write(validFiles.Count + "/" + totatlNumberOfFiles.Length);
        }

        public static void ClearResultsWindow()
        {
            const int searchBarDim = 4;
            int startingLine = Console.WindowHeight - (searchBarDim + 1);
            Console.SetCursorPosition(0, startingLine);
            for (int i = startingLine; i > 0; i--)
            {
                Console.SetCursorPosition(1, i);
                Console.Write(new string(' ', Console.WindowWidth - 2));
            }

            Console.SetCursorPosition(1, startingLine);
        }

        public static void DrawContour()
        {
            const int lowerLineResults = 3;
            for (int i = 0; i <= Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("│");
            }

            Console.SetCursorPosition(0, 0);
            Console.Write("┌");
            Console.SetCursorPosition(Console.WindowWidth - 1, 0);
            Console.Write("┐");
            Console.SetCursorPosition(1, 0);
            Console.Write(new string('─', Console.WindowWidth - 2));
            Console.SetCursorPosition(0, Console.WindowHeight - (1 + lowerLineResults));
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - (1 + lowerLineResults));
            Console.Write("┘");
            Console.SetCursorPosition(1, Console.WindowHeight - (1 + lowerLineResults));
            Console.Write(new string('─', Console.WindowWidth - 2));
            Console.SetCursorPosition(0, Console.WindowHeight - lowerLineResults);
            Console.Write("┌");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - lowerLineResults);
            Console.Write("┐");
            Console.SetCursorPosition(1, Console.WindowHeight - lowerLineResults);
            Console.Write(new string('─', Console.WindowWidth - 2));
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            Console.Write("┘");
            Console.SetCursorPosition(1, Console.WindowHeight - 1);
            Console.Write(new string('─', Console.WindowWidth - 2));
        }

        private static void HilightChar(string file, string search)
        {
            int subSearchLength;
            string restOfSearch = "";
            string subSearch = "";
            string restOfValid = "";
            bool presentOfChar = false;

            if (search.Length == 1)
            {
                subSearchLength = search.Length;
                subSearch = search.Substring(0, subSearchLength);
            }
            else
            {
                subSearchLength = search.Length - 1;
                subSearch = search.Substring(0, subSearchLength);
                restOfSearch = search.Substring(search.Length - 1);
            }

            int startingIndex = Path.GetFileName(file).IndexOf(subSearch, StringComparison.OrdinalIgnoreCase);
            restOfValid = Path.GetFileName(file).Substring(startingIndex + subSearch.Length);
            presentOfChar = ContainAllChar(restOfSearch, restOfValid);

            while (startingIndex < 0 || !presentOfChar)
            {
                restOfSearch = subSearch.Substring(subSearch.Length - 1) + restOfSearch;
                subSearch = subSearch.Substring(0, subSearch.Length - 1);
                startingIndex = Path.GetFileName(file).IndexOf(subSearch, StringComparison.OrdinalIgnoreCase);
                restOfValid = Path.GetFileName(file).Substring(startingIndex + subSearch.Length);
                presentOfChar = ContainAllChar(restOfSearch, restOfValid);
            }

            string firstSection = Path.GetFileName(file).Substring(0, startingIndex);
            string colored = Path.GetFileName(file).Substring(firstSection.Length, subSearch.Length);
            Console.Write(firstSection);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(colored);
            Console.ResetColor();

            if (search.Length == 1)
            {
                Console.Write(Path.GetFileName(file).Substring(startingIndex + subSearch.Length));
                return;
            }

            if (search.Length > 1)
            {
                file = Path.GetFileName(file).Substring(startingIndex + subSearch.Length);
                search = search.Substring(subSearch.Length);
            }

            HilightChar(file, search);
        }

        private static bool ContainAllChar(string search, string fileName)
        {
            int charIndex = 0;

            foreach (char c in fileName)
            {
                if (charIndex < search.Length && c == search[charIndex])
                {
                    charIndex++;
                }
            }

            return charIndex == search.Length;
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