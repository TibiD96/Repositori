namespace CodeEditor
{
    public class CursorMovement
    {
        public static void NavigateUp(bool fastTravelMode, ref int lineCounting, int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
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

        public static void NavigateDown(bool fastTravelMode, ref int lineCounting, int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
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

        public static void NavigateLeft(bool fastTravelMode, ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
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

                    horizontalPosition = GetHorizontalPositionForLeftAndEndMoves(currentEndColumn, lineIndex.Length);
                    NavigateUp(fastTravelMode, ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
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

        public static void NavigateRight(bool fastTravelMode, ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
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
                    NavigateDown(fastTravelMode, ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                }
                else
                {
                    horizontalPosition++;
                    Console.SetCursorPosition(horizontalPosition, verticalPosition);
                }
            }
        }

        public static void EndButtonBehaviour(bool fastTravelMode, int lineCounting, ref int horizontalPosition, int verticalPosition, int startingLine, ref int startingColumn, string[] lines)
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

            horizontalPosition = GetHorizontalPositionForLeftAndEndMoves(currentEndColumn, lineIndex.Length);
            Consola.ShowContentOfFile(lines, lineCounting, fastTravelMode, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition > currentEndColumn + lineIndex.Length ? currentEndColumn + lineIndex.Length : horizontalPosition, verticalPosition);
        }

        public static void HomeButtonBehaviour(bool fastTravelMode, int lineCounting, ref int horizontalPosition, int verticalPosition, int startingLine, ref int startingColumn, string[] lines)
        {
            CheckForNull(lines);

            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length)) + " ";
            horizontalPosition = lineIndex.Length;
            startingColumn = 0;
            Consola.ShowContentOfFile(lines, lineCounting, fastTravelMode, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition, verticalPosition);
        }

        public static void PageDownBehaviour(bool fastTravelMode, ref int lineCounting, int horizontalPosition, int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
        {
            CheckForNull(lines);

            int newStartingLine = startingLine + Console.WindowHeight - 1;
            int originalVerticalPosition = verticalPosition;
            int downSteps = lines.Length - 1 - newStartingLine;
            if (newStartingLine + Console.WindowHeight - 1 <= lines.Length - 1)
            {
                while (startingLine < newStartingLine)
                {
                    NavigateDown(fastTravelMode, ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                }
            }
            else
            {
                newStartingLine = startingLine + downSteps;
                while (startingLine < newStartingLine)
                {
                    NavigateDown(fastTravelMode, ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                }
            }

            while (verticalPosition != originalVerticalPosition)
            {
                NavigateUp(fastTravelMode, ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
            }
        }

        public static void PageUpBehaviour(bool fastTravelMode, ref int lineCounting, int horizontalPosition, int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
        {
            CheckForNull(lines);

            int newStartingLine = startingLine - Console.WindowHeight + 1;
            int originalVerticalPosition = verticalPosition;
            if (newStartingLine - Console.WindowHeight - 1 >= 0)
            {
                while (startingLine > newStartingLine)
                {
                    NavigateUp(fastTravelMode, ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                }
            }
            else
            {
                while (startingLine > 0)
                {
                    NavigateUp(fastTravelMode, ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                }
            }

            while (verticalPosition != originalVerticalPosition)
            {
                NavigateDown(fastTravelMode, ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
            }
        }

        public static void CaretBehaviour(bool fastTravelMode, ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
        {
            CheckForNull(lines);

            string currentLine = lines[lineCounting];

            if (currentLine == "")
            {
                return;
            }

            HomeButtonBehaviour(fastTravelMode, lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, lines);
            for (int i = 0; currentLine[i] == ' ' && i < currentLine.Length; i++)
            {
                NavigateRight(fastTravelMode, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
            }
        }

        public static void MoveWordRight(bool fastTravelMode, ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
        {
            CheckForNull(lines);

            if (fastTravelMode)
            {
                WordRightFST(fastTravelMode, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
            }
            else
            {
                WordRightDFLT(fastTravelMode, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
            }
        }

        private static void WordRightFST(bool fastTravelMode, ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
        {
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length));
            char character = GetChar(fastTravelMode, lines, lineNumber, startingColumn);
            int baseLineCounting = lineCounting;
            while (character != ' ')
            {
                NavigateRight(fastTravelMode, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                if (lines[lineCounting] == "")
                {
                    NavigateRight(fastTravelMode, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                }

                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length));
                character = GetChar(fastTravelMode, lines, lineNumber, startingColumn);

                if (baseLineCounting != lineCounting)
                {
                    if (character == ' ')
                    {
                        WordRightFST(fastTravelMode, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                        return;
                    }

                    return;
                }

                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length));
                character = GetChar(fastTravelMode, lines, lineNumber, startingColumn);
            }

            NavigateRight(fastTravelMode, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
            character = GetChar(fastTravelMode, lines, lineNumber, startingColumn);

            while (character == ' ')
            {
                NavigateRight(fastTravelMode, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                if (lines[lineCounting] == "")
                {
                    NavigateRight(fastTravelMode, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                }

                character = GetChar(fastTravelMode, lines, lineNumber, startingColumn);
            }
        }

        private static void WordRightDFLT(bool fastTravelMode, ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
        {
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length));
            char character = GetChar(fastTravelMode, lines, lineNumber, startingColumn);
            int baseLineCounting = lineCounting;
            while (character != ' ')
            {
                NavigateRight(fastTravelMode, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                if (lines[lineCounting] == "")
                {
                    NavigateRight(fastTravelMode, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                }

                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length));
                character = GetChar(fastTravelMode, lines, lineNumber, startingColumn);

                if (baseLineCounting != lineCounting)
                {
                    if (character == ' ')
                    {
                        WordRightDFLT(fastTravelMode, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                        return;
                    }

                    return;
                }

                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(lines.Length));
                character = GetChar(fastTravelMode, lines, lineNumber, startingColumn);
            }

            NavigateRight(fastTravelMode, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
            character = GetChar(fastTravelMode, lines, lineNumber, startingColumn);

            while (character == ' ')
            {
                NavigateRight(fastTravelMode, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                if (lines[lineCounting] == "")
                {
                    NavigateRight(fastTravelMode, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                }

                character = GetChar(fastTravelMode, lines, lineNumber, startingColumn);
            }
        }

        private static int GetHorizontalPositionForLeftAndEndMoves(int currentEndColumn, int lineIndexLength)
        {
            return currentEndColumn == 0 ? lineIndexLength : currentEndColumn + lineIndexLength - 1;
        }

        private static char GetChar(bool fastTravelMode, string[] lines, string lineNumber, int startingColumn)
        {
            if (fastTravelMode)
            {
                if (lines[Convert.ToInt32(lineNumber)] == "")
                {
                    return 'a';
                }

                return lines[Convert.ToInt32(lineNumber)][(Console.CursorLeft + startingColumn - 1) - lineNumber.Length];
            }

            if (lines[Convert.ToInt32(lineNumber) - 1] == "")
            {
                return 'a';
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
