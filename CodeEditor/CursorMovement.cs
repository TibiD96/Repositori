namespace CodeEditor
{
    public class CursorMovement
    {
        private static bool fastTravelMode;
        private static string[] lines = new string[0];

        public static void FileParameter(bool fastTravel, string[] fileLines)
        {
            fastTravelMode = fastTravel;
            lines = fileLines;
        }

        public static void NavigateUp(ref int lineCounting, int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            CheckForNull(lines);

            if (lineCounting == 0)
            {
                return;
            }

            lineCounting--;
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length)) + " ";
            int currentStartColumn = Math.Max(0, Math.Min(startingColumn, lines[lineCounting].Length));
            int currentEndColumn = lines[lineCounting].Length - currentStartColumn < Console.WindowWidth ? lines[lineCounting].Length - currentStartColumn : Console.WindowWidth - 1;

            if (currentStartColumn < startingColumn)
            {
                startingColumn = currentStartColumn;
            }

            if (verticalPosition == 0 && startingLine != 0)
            {
                startingLine--;
            }
            else if (verticalPosition > 0)
            {
                verticalPosition--;
            }

            Consola.ShowContentOfFile(lines, lineCounting, fastTravelMode, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition > currentEndColumn + lineIndex.Length ? currentEndColumn + lineIndex.Length : horizontalPosition, verticalPosition);
        }

        public static void NavigateDown(ref int lineCounting, int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            CheckForNull(lines);

            if (lineCounting >= lines.Length - 1)
            {
                return;
            }

            lineCounting++;
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length)) + " ";
            int currentStartColumn = Math.Max(0, Math.Min(startingColumn, lines[lineCounting].Length));
            int currentEndColumn = lines[lineCounting].Length - currentStartColumn < Console.WindowWidth ? lines[lineCounting].Length - currentStartColumn : Console.WindowWidth - 1;

            if (currentStartColumn < startingColumn)
            {
                startingColumn = currentStartColumn;
            }

            if (verticalPosition + 1 == Console.WindowHeight)
            {
                if (startingLine + 1 <= lines.Length - Console.WindowHeight)
                {
                    startingLine++;
                }
            }
            else
            {
                verticalPosition++;
            }

            Consola.ShowContentOfFile(lines, lineCounting, fastTravelMode, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition > currentEndColumn + lineIndex.Length ? currentEndColumn + lineIndex.Length : horizontalPosition, verticalPosition);
        }

        public static void NavigateLeft(ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            CheckForNull(lines);

            int currentStartColumn = Math.Max(0, Math.Min(startingColumn, lines[lineCounting].Length));
            int currentEndColumn = lines[lineCounting].Length - currentStartColumn < Console.WindowWidth ? lines[lineCounting].Length - currentStartColumn : Console.WindowWidth - 1;
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length)) + " ";

            if (Console.CursorLeft == lineIndex.Length && lineCounting != 0)
            {
                horizontalPosition = lineIndex.Length;
                if (startingColumn == 0)
                {
                    currentEndColumn = lines[lineCounting - 1].Length - currentStartColumn;
                    while (currentEndColumn > Console.WindowWidth - lineIndex.Length - 1)
                    {
                        startingColumn++;
                        currentEndColumn--;
                    }

                    horizontalPosition = currentEndColumn + lineIndex.Length;
                    NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                    return;
                }

                startingColumn--;
                Consola.ShowContentOfFile(lines, lineCounting, fastTravelMode, startingLine, startingColumn);
            }
            else
            {
                if (horizontalPosition > currentEndColumn + lineIndex.Length)
                {
                    horizontalPosition = currentEndColumn - 1 + lineIndex.Length;
                }
                else if (horizontalPosition > lineIndex.Length)
                {
                    horizontalPosition--;
                }
            }

            Console.SetCursorPosition(horizontalPosition > currentEndColumn + lineIndex.Length ? currentEndColumn + lineIndex.Length : horizontalPosition, verticalPosition);
        }

        public static void NavigateRight(ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            CheckForNull(lines);

            int currentStartColumn = Math.Max(0, Math.Min(startingColumn, lines[lineCounting].Length));
            int currentEndColumn = lines[lineCounting].Length - currentStartColumn < Console.WindowWidth ? lines[lineCounting].Length - currentStartColumn : Console.WindowWidth - 1;
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length)) + " ";

            if (horizontalPosition + 1 == Console.WindowWidth && lines[lineCounting].Length - currentStartColumn + lineIndex.Length - 1 > Console.WindowWidth - 1)
            {
                startingColumn++;
                Consola.ShowContentOfFile(lines, lineCounting, fastTravelMode, startingLine, startingColumn);
                Console.SetCursorPosition(horizontalPosition, verticalPosition);
            }
            else
            {
                if (horizontalPosition >= currentEndColumn + lineIndex.Length - 1 || currentEndColumn == 0)
                {
                    horizontalPosition = lineIndex.Length;
                    startingColumn = 0;
                    NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }
                else
                {
                    horizontalPosition++;
                    Console.SetCursorPosition(horizontalPosition, verticalPosition);
                }
            }
        }

        public static void EndButtonBehaviour(int lineCounting, ref int horizontalPosition, int verticalPosition, int startingLine, ref int startingColumn)
        {
            CheckForNull(lines);

            int currentStartColumn = Math.Max(0, Math.Min(startingColumn, lines[lineCounting].Length));
            int currentEndColumn = lines[lineCounting].Length - currentStartColumn;
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length)) + " ";
            while (currentEndColumn > Console.WindowWidth - lineIndex.Length - 1)
            {
                startingColumn++;
                currentEndColumn--;
            }

            horizontalPosition = currentEndColumn + lineIndex.Length;
            Consola.ShowContentOfFile(lines, lineCounting, fastTravelMode, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition > currentEndColumn + lineIndex.Length ? currentEndColumn + lineIndex.Length : horizontalPosition, verticalPosition);
        }

        public static void HomeButtonBehaviour(int lineCounting, ref int horizontalPosition, int verticalPosition, int startingLine, ref int startingColumn)
        {
            CheckForNull(lines);

            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length)) + " ";
            horizontalPosition = lineIndex.Length;
            startingColumn = 0;
            Consola.ShowContentOfFile(lines, lineCounting, fastTravelMode, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition, verticalPosition);
        }

        public static void PageDownBehaviour(ref int lineCounting, int horizontalPosition, int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            CheckForNull(lines);

            int newStartingLine = startingLine + Console.WindowHeight - 1;
            int originalVerticalPosition = verticalPosition;
            int downSteps = lines.Length - 1 - newStartingLine;
            if (newStartingLine + Console.WindowHeight - 1 <= lines.Length - 1)
            {
                while (startingLine < newStartingLine)
                {
                    NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }
            }
            else
            {
                newStartingLine = startingLine + downSteps;
                while (startingLine < newStartingLine)
                {
                    NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }
            }

            while (verticalPosition != originalVerticalPosition)
            {
                NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            }
        }

        public static void PageUpBehaviour(ref int lineCounting, int horizontalPosition, int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            CheckForNull(lines);

            int newStartingLine = startingLine - Console.WindowHeight + 1;
            int originalVerticalPosition = verticalPosition;
            if (newStartingLine - Console.WindowHeight - 1 >= 0)
            {
                while (startingLine > newStartingLine)
                {
                    NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }
            }
            else
            {
                while (startingLine > 0)
                {
                    NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }
            }

            while (verticalPosition != originalVerticalPosition)
            {
                NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            }
        }

        public static void CaretBehaviour(ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            CheckForNull(lines);

            string currentLine = lines[lineCounting];

            if (currentLine == "")
            {
                return;
            }

            HomeButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);
            for (int i = 0; currentLine[i] == ' ' && i < currentLine.Length; i++)
            {
                NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            }
        }

        public static void MoveWordRight(char charType, ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            CheckForNull(lines);
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length));
            char character = GetChar(lineNumber, startingColumn, lineCounting);
            int baseLineCounting = lineCounting;
            char[] punctuation = charType == 'W' ? new[] { ' ', '.', '?', '!', ',', ';', ':', '"', '\'', '-', '/' } : new[] { ' ' };
            if (lineCounting >= lines.Length - 1)
            {
                return;
            }

            while (!punctuation.Contains(character))
            {
                NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                if (lines[lineCounting] == "")
                {
                    NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }

                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length));
                character = GetChar(lineNumber, startingColumn, lineCounting);

                if (baseLineCounting != lineCounting)
                {
                    if (punctuation.Contains(character))
                    {
                        MoveWordRight(charType, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        return;
                    }

                    return;
                }
            }

            NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            character = GetChar(lineNumber, startingColumn, lineCounting);

            while (punctuation.Contains(character))
            {
                NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                if (lines[lineCounting] == "")
                {
                    NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }

                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length));
                character = GetChar(lineNumber, startingColumn, lineCounting);
            }
        }

        public static void MoveWordLeft(char charType, ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            CheckForNull(lines);
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length));
            char character = GetChar(lineNumber, startingColumn, lineCounting);
            int beginningOfLine = lineNumber.Length + 1;
            char[] punctuation = charType == 'B' ? new[] { ' ', '.', '?', '!', ',', ';', ':', '"', '\'', '-', '/' } : new[] { ' ' };

            if (lineCounting == 0 && Console.CursorLeft == beginningOfLine)
            {
                return;
            }

            while (!punctuation.Contains(character))
            {
                NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                if (lines[lineCounting] == "")
                {
                    NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }

                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length));
                character = GetChar(lineNumber, startingColumn, lineCounting);
            }

            CursorOnPnct(charType, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length));
            character = GetChar(lineNumber, startingColumn, lineCounting);

            while (!punctuation.Contains(character))
            {
                beginningOfLine = lineNumber.Length + 1;
                if (Console.CursorLeft == beginningOfLine && startingColumn == 0)
                {
                    return;
                }

                NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                if (lines[lineCounting] == "")
                {
                    NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }

                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length));
                character = GetChar(lineNumber, startingColumn, lineCounting);
            }

            NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
        }

        public static void SeeKeys(ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            CheckForNull(lines);
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length)) + " ";
            int currentStartColumn = Math.Max(0, Math.Min(startingColumn, lines[lineCounting].Length));
            int currentEndColumn = lines[lineCounting].Length - currentStartColumn < Console.WindowWidth ? lines[lineCounting].Length - currentStartColumn : Console.WindowWidth - 1;
            Consola.KeysForMovement();
            Consola.ShowContentOfFile(lines, lineCounting, fastTravelMode, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition > currentEndColumn + lineIndex.Length ? currentEndColumn + lineIndex.Length : horizontalPosition, verticalPosition);
        }

        private static void CursorOnPnct(char charType, ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length));
            char character = GetChar(lineNumber, startingColumn, lineCounting);
            char[] punctuation = charType == 'B' ? new[] { ' ', '.', '?', '!', ',', ';', ':', '"', '\'', '-', '/' } : new[] { ' ' };

            while (punctuation.Contains(character))
            {
                NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                if (lines[lineCounting] == "")
                {
                    NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }

                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length));
                character = GetChar(lineNumber, startingColumn, lineCounting);
            }
        }

        private static char GetChar(string lineNumber, int startingColumn, int lineCounting)
        {
            int currentEndColumn = lines[lineCounting].Length - 1;
            int horizontalPosition = (Console.CursorLeft + startingColumn - 1) - lineNumber.Length;
            if (fastTravelMode)
            {
                if (lines[Convert.ToInt32(lineNumber)] == "" || horizontalPosition > currentEndColumn)
                {
                    return ' ';
                }

                return lines[Convert.ToInt32(lineNumber)][(Console.CursorLeft + startingColumn - 1) - lineNumber.Length];
            }

            if (lines[Convert.ToInt32(lineNumber) - 1] == "" || horizontalPosition > currentEndColumn)
            {
                return ' ';
            }

            return lines[Convert.ToInt32(lineNumber) - 1][(Console.CursorLeft + startingColumn - 1) - lineNumber.Length];
        }

        private static void CheckForNull(string[] lines)
        {
            if (lines != null)
            {
                return;
            }

            throw new ArgumentNullException(nameof(lines));
        }
    }
}
