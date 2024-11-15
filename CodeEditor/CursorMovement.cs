namespace CodeEditor
{
    public class CursorMovement
    {
        private static bool fastTravelMode;
        private static string[] fileContent = new string[0];
        private static Dictionary<char, int> markedLines = new Dictionary<char, int>();
        private static int basestartingLine = 0;
        private static int basestartingColumn = 0;

        public static void FileParameter(bool fastTravel, string[] fileLines)
        {
            fastTravelMode = fastTravel;
            fileContent = fileLines;
        }

        public static void NavigateUp(ref int lineCounting, int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            NullOrEmptyCases.ArgumentNullException(fileContent);

            if (lineCounting == 0)
            {
                return;
            }

            lineCounting--;
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";
            (int, int) currentStartEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent);

            if (currentStartEndColumn.Item1 < startingColumn)
            {
                startingColumn = currentStartEndColumn.Item1;
            }

            if (verticalPosition == 0 && startingLine != 0)
            {
                startingLine--;
            }
            else if (verticalPosition > 0)
            {
                verticalPosition--;
            }

            Console.SetCursorPosition(horizontalPosition > currentStartEndColumn.Item2 + lineIndex.Length ? currentStartEndColumn.Item2 + lineIndex.Length : horizontalPosition, verticalPosition);
        }

        public static void NavigateDown(ref int lineCounting, int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            NullOrEmptyCases.ArgumentNullException(fileContent);

            if (lineCounting >= fileContent.Length - 1)
            {
                return;
            }

            lineCounting++;
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";
            (int, int) currentStartEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent);

            if (currentStartEndColumn.Item1 < startingColumn)
            {
                startingColumn = currentStartEndColumn.Item1;
            }

            if (verticalPosition + 1 == Console.WindowHeight - 3)
            {
                if (startingLine + 1 <= fileContent.Length - (Console.WindowHeight - 3))
                {
                    startingLine++;
                }
            }
            else
            {
                verticalPosition++;
            }

            Console.SetCursorPosition(horizontalPosition > currentStartEndColumn.Item2 + lineIndex.Length ? currentStartEndColumn.Item2 + lineIndex.Length : horizontalPosition, verticalPosition);
        }

        public static void NavigateLeft(ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            NullOrEmptyCases.ArgumentNullException(fileContent);

            (int, int) currentStartEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent);

            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";

            if (Console.CursorLeft == lineIndex.Length && lineCounting >= 0)
            {
                horizontalPosition = lineIndex.Length;
                if (startingColumn == 0 && lineCounting != 0)
                {
                    currentStartEndColumn.Item2 = fileContent[lineCounting - 1].Length - currentStartEndColumn.Item1;
                    while (currentStartEndColumn.Item2 > Console.WindowWidth - lineIndex.Length - 1)
                    {
                        startingColumn++;
                        currentStartEndColumn.Item2--;
                    }

                    horizontalPosition = currentStartEndColumn.Item2 + lineIndex.Length;
                    NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                    return;
                }

                if (startingColumn == 0 && lineCounting == 0)
                {
                    return;
                }

                startingColumn--;
            }
            else
            {
                if (horizontalPosition > currentStartEndColumn.Item2 + lineIndex.Length)
                {
                    horizontalPosition = currentStartEndColumn.Item2 + lineIndex.Length;
                    horizontalPosition--;
                }
                else if (horizontalPosition > lineIndex.Length)
                {
                    horizontalPosition--;
                }
            }

            Console.SetCursorPosition(horizontalPosition > currentStartEndColumn.Item2 + lineIndex.Length ? currentStartEndColumn.Item2 + lineIndex.Length : horizontalPosition, verticalPosition);
        }

        public static void NavigateRight(ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            NullOrEmptyCases.ArgumentNullException(fileContent);

            (int, int) currentStartEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent);
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";

            if (horizontalPosition + 1 == Console.WindowWidth && fileContent[lineCounting].Length - currentStartEndColumn.Item1 + lineIndex.Length > Console.WindowWidth - 1)
            {
                startingColumn++;
                Console.SetCursorPosition(horizontalPosition, verticalPosition);
            }
            else
            {
                if (horizontalPosition >= currentStartEndColumn.Item2 + lineIndex.Length || currentStartEndColumn.Item2 == 0)
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
            NullOrEmptyCases.ArgumentNullException(fileContent);

            (int, int) currentStartEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent);
            int currentEndColumn = 0;
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";

            if (fileContent.Length != 0)
            {
                currentEndColumn = fileContent[lineCounting].Length - currentStartEndColumn.Item1;
            }

            while (currentEndColumn > Console.WindowWidth - lineIndex.Length - 1)
            {
                startingColumn++;
                currentEndColumn--;
            }

            horizontalPosition = currentEndColumn + lineIndex.Length;
            Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition > currentEndColumn + lineIndex.Length ? currentEndColumn + lineIndex.Length : horizontalPosition, verticalPosition);
        }

        public static void HomeButtonBehaviour(int lineCounting, ref int horizontalPosition, int verticalPosition, int startingLine, ref int startingColumn)
        {
            NullOrEmptyCases.ArgumentNullException(fileContent);

            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";
            horizontalPosition = lineIndex.Length;
            startingColumn = 0;
            Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition, verticalPosition);
        }

        public static void PageDownBehaviour(ref int lineCounting, int horizontalPosition, int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            NullOrEmptyCases.ArgumentNullException(fileContent);

            int newStartingLine = startingLine + Console.WindowHeight - 3;
            int originalVerticalPosition = verticalPosition;
            int downSteps = fileContent.Length - 1 - newStartingLine;
            if (newStartingLine + Console.WindowHeight - 3 <= fileContent.Length - 1)
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
            NullOrEmptyCases.ArgumentNullException(fileContent);

            int newStartingLine = startingLine - (Console.WindowHeight - 2) + 1;
            int originalVerticalPosition = verticalPosition;
            if (newStartingLine - Console.WindowHeight - 4 >= 0)
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

        public static void MoveToFirstCharacter(ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            NullOrEmptyCases.ArgumentNullException(fileContent);

            string currentLine = fileContent[lineCounting];

            if (currentLine == "")
            {
                return;
            }

            HomeButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);
            for (int i = 0; i < currentLine.Length && currentLine[i] == ' '; i++)
            {
                NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            }
        }

        public static void MoveWordRight(char charType, ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            NullOrEmptyCases.ArgumentNullException(fileContent);
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
            char baseCharacter = GetChar(lineNumber, startingColumn, lineCounting);
            char currentCharacter;
            char[] wordDelimitation = charType == 'w' ? new[] { ' ', '.', '?', '!', ',', ';', ':', '"', '\'', '-', '/', '\\', '(', ')', '[', ']', '{', '}', '=', '+', '-', '<', '>' } : new[] { ' ' };
            char[] open = new[] { '(', '[', '{' };
            char[] close = new[] { ')', ']', '}' };
            if (lineCounting >= fileContent.Length - 1)
            {
                return;
            }

            if (charType == 'W')
            {
                MoveWRight(wordDelimitation, charType, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                return;
            }

            NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
            currentCharacter = GetChar(lineNumber, startingColumn, lineCounting);

            if (!wordDelimitation.Contains(baseCharacter) && !wordDelimitation.Contains(currentCharacter))
            {
                MoveWRight(wordDelimitation, charType, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                return;
            }

            if (open.Contains(baseCharacter) && close.Contains(currentCharacter))
            {
                NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
                currentCharacter = GetChar(lineNumber, startingColumn, lineCounting);
            }

            while (currentCharacter == ' ')
            {
                NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
                currentCharacter = GetChar(lineNumber, startingColumn, lineCounting);
            }
        }

        public static void MoveWordLeft(char charType, ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            NullOrEmptyCases.ArgumentNullException(fileContent);
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
            char baseCharacter = GetChar(lineNumber, startingColumn, lineCounting);
            char currentCharacter;
            int beginningOfLine = lineNumber.Length + 1;
            char[] wordDelimitation = charType == 'b' ? new[] { ' ', '.', '?', '!', ',', ';', ':', '"', '\'', '-', '/', '\\', '(', ')', '[', ']', '{', '}', '=', '+', '-', '<', '>' } : new[] { ' ' };
            char[] open = new[] { '(', '[', '{' };
            char[] close = new[] { ')', ']', '}' };
            char[] punctuation = new[] { '.', '?', '!', ',', ';', ':', '"', '\'', '-', '/', '\\', '(', ')', '[', ']', '{', '}', '=', '+', '-', '<', '>' };

            if (lineCounting == 0 && Console.CursorLeft == beginningOfLine)
            {
                return;
            }

            if (charType == 'B')
            {
                MoveWLeft(wordDelimitation, charType, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                return;
            }

            if (!punctuation.Contains(baseCharacter))
            {
                MoveWLeft(wordDelimitation, charType, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                return;
            }

            NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
            currentCharacter = GetChar(lineNumber, startingColumn, lineCounting);

            if (open.Contains(currentCharacter) && close.Contains(baseCharacter))
            {
                NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
                currentCharacter = GetChar(lineNumber, startingColumn, lineCounting);
            }

            if (punctuation.Contains(currentCharacter))
            {
                return;
            }

            if (currentCharacter == ' ')
            {
                MoveWLeft(wordDelimitation, charType, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            }
            else
            {
                CursorOnLetter(wordDelimitation, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            }
        }

        public static void GoOnDesiredCharacter(char charType, int lineCounting, ref int horizontalPosition, int verticalPosition, int startingLine, ref int startingColumn, char? character)
        {
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
            (int, int) currentStartEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent);

            Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition > currentStartEndColumn.Item2 + lineNumber.Length ? currentStartEndColumn.Item2 + lineNumber.Length : horizontalPosition, verticalPosition);

            if (charType == 'f')
            {
                GoOnCharacterLow(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, character);
            }
            else
            {
                GoOnCharacterUpper(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, character);
            }
        }

        public static void GoTillDesiredCharacter(char charType, int lineCounting, ref int horizontalPosition, int verticalPosition, int startingLine, ref int startingColumn, char? character)
        {
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
            (int, int) currentStartEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent);

            Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition > currentStartEndColumn.Item2 + lineNumber.Length ? currentStartEndColumn.Item2 + lineNumber.Length : horizontalPosition, verticalPosition);

            if (charType == 't')
            {
                GoTillCharacterLow(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, character);
            }
            else
            {
                GoTillCharacterUpper(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, character);
            }
        }

        public static void MarkLine(int lineCounting, char key)
        {
            if (markedLines.Keys.Contains(key))
            {
                markedLines.Remove(key);
            }

            markedLines.Add(key, lineCounting);
        }

        public static void GoToMarkedLine(ref int lineCounting, int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn, char key)
        {
            int moves;
            if (markedLines.TryGetValue(key, out int markedLineNumber))
            {
                moves = markedLineNumber - lineCounting;
            }
            else
            {
                return;
            }

            if (moves < 0)
            {
                for (int i = 0; i > moves; i--)
                {
                    NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }
            }
            else
            {
                for (int i = 0; i < moves; i++)
                {
                    NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }
            }
        }

        public static void GoToEndOrBeginingOfFile(ref int lineCounting, int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn, ConsoleKeyInfo action)
        {
            if (action.KeyChar == 'G' && fileContent.Length > 0)
            {
                lineCounting = fileContent.Length - 1;
                verticalPosition = Console.WindowHeight - 4;

                if (fileContent.Length - (Console.WindowHeight - 3) > (Console.WindowHeight - 3))
                {
                    startingLine = fileContent.Length - (Console.WindowHeight - 3);
                }

                string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";
                (int, int) currentStartEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent);

                Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                Console.SetCursorPosition(horizontalPosition > currentStartEndColumn.Item2 + lineIndex.Length ? currentStartEndColumn.Item2 + lineIndex.Length : horizontalPosition, verticalPosition);
            }

            if (action.KeyChar == 'g')
            {
                lineCounting = 0;
                startingLine = 0;
                verticalPosition = 0;

                string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";
                (int, int) currentStartEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent);

                Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                Console.SetCursorPosition(horizontalPosition > currentStartEndColumn.Item2 + lineIndex.Length ? currentStartEndColumn.Item2 + lineIndex.Length : horizontalPosition, verticalPosition);
            }
        }

        private static void GoOnCharacterLow(int lineCounting, ref int horizontalPosition, int verticalPosition, int startingLine, ref int startingColumn, char? character)
        {
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
            (int, int) currentStartEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent);
            int currentEndColumn = currentStartEndColumn.Item2;
            char currentCharacter = GetChar(lineNumber, startingColumn, lineCounting);
            int steptUntilLastCorrectPosition = 0;

            if (character == currentCharacter && !(horizontalPosition >= currentEndColumn + lineNumber.Length - 1 || currentEndColumn == 0))
            {
                NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                currentCharacter = GetChar(lineNumber, startingColumn, lineCounting);
                steptUntilLastCorrectPosition++;
            }

            while (!(horizontalPosition >= currentEndColumn + lineNumber.Length || currentEndColumn == 0) && character != currentCharacter)
            {
                NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                currentStartEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent);
                currentEndColumn = fileContent[lineCounting].Length - currentStartEndColumn.Item1;
                currentCharacter = GetChar(lineNumber, startingColumn, lineCounting);
                steptUntilLastCorrectPosition++;
            }

            for (int i = 0; i < steptUntilLastCorrectPosition && currentCharacter != character; i++)
            {
                NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            }
        }

        private static void GoOnCharacterUpper(int lineCounting, ref int horizontalPosition, int verticalPosition, int startingLine, ref int startingColumn, char? character)
        {
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
            char currentCharacter = GetChar(lineNumber, startingColumn, lineCounting);
            int steptUntilLastCorrectPosition = 0;

            if (character == currentCharacter && horizontalPosition >= lineNumber.Length + 1)
            {
                if (horizontalPosition == lineNumber.Length + 1 && startingColumn == 0)
                {
                    return;
                }

                NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                currentCharacter = GetChar(lineNumber, startingColumn, lineCounting);
                steptUntilLastCorrectPosition++;
            }

            while (horizontalPosition >= lineNumber.Length + 1 && character != currentCharacter)
            {
                if (horizontalPosition == lineNumber.Length + 1 && startingColumn == 0)
                {
                    break;
                }

                NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                currentCharacter = GetChar(lineNumber, startingColumn, lineCounting);
                steptUntilLastCorrectPosition++;
            }

            for (int i = 0; i < steptUntilLastCorrectPosition && currentCharacter != character; i++)
            {
                NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            }
        }

        private static void GoTillCharacterLow(int lineCounting, ref int horizontalPosition, int verticalPosition, int startingLine, ref int startingColumn, char? character)
        {
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
            (int, int) currentStartEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent);
            int currentEndColumn = fileContent[lineCounting].Length - currentStartEndColumn.Item1 < Console.WindowWidth ? fileContent[lineCounting].Length - currentStartEndColumn.Item1 : Console.WindowWidth - 1;
            int steptUntilLastCorrectPosition = 0;

            if (character == GetChar(lineNumber, startingColumn + 1, lineCounting) && !(horizontalPosition >= currentEndColumn + lineNumber.Length - 1 || currentEndColumn == 0))
            {
                NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                steptUntilLastCorrectPosition++;
            }

            while (!(horizontalPosition >= currentEndColumn + lineNumber.Length || currentEndColumn == 0) && character != GetChar(lineNumber, startingColumn + 1, lineCounting))
            {
                NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                currentStartEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent);
                currentEndColumn = fileContent[lineCounting].Length - currentStartEndColumn.Item1;
                steptUntilLastCorrectPosition++;
            }

            for (int i = 0; i < steptUntilLastCorrectPosition && GetChar(lineNumber, startingColumn + 1, lineCounting) != character; i++)
            {
                NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            }
        }

        private static void GoTillCharacterUpper(int lineCounting, ref int horizontalPosition, int verticalPosition, int startingLine, ref int startingColumn, char? character)
        {
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
            int steptUntilLastCorrectPosition = 0;

            if (character == GetChar(lineNumber, startingColumn - 1, lineCounting) && horizontalPosition >= lineNumber.Length + 1)
            {
                if (horizontalPosition == lineNumber.Length + 1 && startingColumn == 0)
                {
                    return;
                }

                NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                steptUntilLastCorrectPosition++;
            }

            while (horizontalPosition > lineNumber.Length + 1 && character != GetChar(lineNumber, startingColumn - 1, lineCounting))
            {
                if (horizontalPosition == lineNumber.Length + 1 && startingColumn == 0)
                {
                    break;
                }

                NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                steptUntilLastCorrectPosition++;
            }

            if (horizontalPosition > lineNumber.Length + 1)
            {
                for (int i = 0; i < steptUntilLastCorrectPosition && GetChar(lineNumber, startingColumn - 1, lineCounting) != character; i++)
                {
                    NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }
            }
            else
            {
                for (int i = 0; i < steptUntilLastCorrectPosition && GetChar(lineNumber, startingColumn + 1, lineCounting) != character; i++)
                {
                    NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }
            }
        }

        private static void CursorOnDelimitation(char charType, char[] wordSplit, ref int lineCount, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCount, lineCount, Convert.ToString(fileContent.Length));
            char character = GetChar(lineNumber, startingColumn, lineCount);
            char[] punctuation = new[] { '.', '?', '!', ',', ';', ':', '"', '\'', '-', '/', '\\', '(', ')', '[', ']', '{', '}', '=', '+', '-', '<', '>' };

            while (wordSplit.Contains(character))
            {
                if (charType == 'b' && punctuation.Contains(character))
                {
                    break;
                }

                NavigateLeft(ref lineCount, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                if (fileContent[lineCount] == "")
                {
                    NavigateLeft(ref lineCount, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }

                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCount, lineCount, Convert.ToString(fileContent.Length));
                character = GetChar(lineNumber, startingColumn, lineCount);
            }
        }

        private static void CursorOnLetter(char[] wordSplit, ref int lineCount, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCount, lineCount, Convert.ToString(fileContent.Length));
            char character = GetChar(lineNumber, startingColumn, lineCount);

            while (!wordSplit.Contains(character))
            {
                int beginningOfLine = lineNumber.Length + 1;
                if (Console.CursorLeft == beginningOfLine && startingColumn == 0)
                {
                    return;
                }

                NavigateLeft(ref lineCount, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                if (fileContent[lineCount] == "")
                {
                    NavigateLeft(ref lineCount, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }

                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCount, lineCount, Convert.ToString(fileContent.Length));
                character = GetChar(lineNumber, startingColumn, lineCount);
            }

            NavigateRight(ref lineCount, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
        }

        private static void MoveWRight(char[] wordDelimitation, char charType, ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
            char character = GetChar(lineNumber, startingColumn, lineCounting);
            int baseLineCounting = lineCounting;
            char[] punctuation = new[] { '.', '?', '!', ',', ';', ':', '"', '\'', '-', '/', '\\', '(', ')', '[', ']', '{', '}', '=', '+', '-', '<', '>' };
            while (!wordDelimitation.Contains(character))
            {
                NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                if (fileContent[lineCounting] == "")
                {
                    NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }

                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
                character = GetChar(lineNumber, startingColumn, lineCounting);

                if (baseLineCounting != lineCounting)
                {
                    if (wordDelimitation.Contains(character))
                    {
                        MoveWordRight(charType, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        return;
                    }

                    return;
                }
            }

            if (punctuation.Contains(character))
            {
                return;
            }

            NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            character = GetChar(lineNumber, startingColumn, lineCounting);

            if (punctuation.Contains(character))
            {
                return;
            }

            while (wordDelimitation.Contains(character))
            {
                NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                if (fileContent[lineCounting] == "")
                {
                    NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }

                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
                character = GetChar(lineNumber, startingColumn, lineCounting);
            }
        }

        private static void MoveWLeft(char[] wordDelimitation, char charType, ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn)
        {
            NullOrEmptyCases.ArgumentNullException(fileContent);
            string lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
            char character = GetChar(lineNumber, startingColumn, lineCounting);
            char[] punctuation = new[] { '.', '?', '!', ',', ';', ':', '"', '\'', '-', '/', '\\', '(', ')', '[', ']', '{', '}', '=', '+', '-', '<', '>' };

            while (!wordDelimitation.Contains(character))
            {
                NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                if (fileContent[lineCounting] == "")
                {
                    NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }

                lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
                character = GetChar(lineNumber, startingColumn, lineCounting);
            }

            CursorOnDelimitation(charType, wordDelimitation, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            lineNumber = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length));
            character = GetChar(lineNumber, startingColumn, lineCounting);

            if (charType == 'b' && punctuation.Contains(character))
            {
                return;
            }

            CursorOnLetter(wordDelimitation, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
        }

        private static char GetChar(string lineNumber, int startingColumn, int lineCounting)
        {
            int currentEndColumn;
            int horizontalPosition = (Console.CursorLeft + startingColumn - 1) - lineNumber.Length;

            if (fileContent.Length != 0)
            {
                currentEndColumn = fileContent[lineCounting].Length - 1;

                if (fastTravelMode)
                {
                    if (fileContent[Convert.ToInt32(lineNumber)] == "" || horizontalPosition > currentEndColumn)
                    {
                        return ' ';
                    }

                    return fileContent[Convert.ToInt32(lineNumber)][(Console.CursorLeft + startingColumn - 1) - lineNumber.Length];
                }

                if (fileContent[Convert.ToInt32(lineNumber) - 1] == "" || horizontalPosition > currentEndColumn)
                {
                    return ' ';
                }

                return fileContent[Convert.ToInt32(lineNumber) - 1][(Console.CursorLeft + startingColumn - 1) - lineNumber.Length];
            }

            return ' ';
        }
    }
}
