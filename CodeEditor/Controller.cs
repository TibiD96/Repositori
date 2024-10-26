using System;

namespace CodeEditor
{
    public class Controller
    {
        public static void OpenFile()
        {
            string currentDirectory = Environment.CurrentDirectory;
            string[] filesFromDirectory = Directory.GetFiles(currentDirectory);
            bool fastTravelMode = Config.FastTravel;
            List<string> allFiles = new List<string>();

            GetAllFiles(ref allFiles, currentDirectory);
            string filePathToOpen = FuzzySearchLogic.FuzzySearch(filesFromDirectory, allFiles.ToArray());
            string[] fileContent = File.ReadAllLines(filePathToOpen);

            Consola.ClearConsole();
            Consola.ShowContentOfFile(fileContent, Console.WindowHeight - 4, fastTravelMode);
            InFileActions(fileContent, fastTravelMode, filePathToOpen);
        }

        public static int GetCursorCharIndex(int lineCounting, ref int horizontalPosition, int startingColumn, string[] fileContent)
        {
            bool fastTravelMode = Config.FastTravel;

            int currentStartColumn = Math.Max(0, Math.Min(startingColumn, fileContent[lineCounting].Length));
            int currentEndColumn = fileContent[lineCounting].Length - currentStartColumn < Console.WindowWidth ? fileContent[lineCounting].Length - currentStartColumn : Console.WindowWidth - 1;
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";

            if (horizontalPosition >= currentEndColumn + lineIndex.Length)
            {
                horizontalPosition = currentEndColumn + lineIndex.Length;
                return horizontalPosition + startingColumn - lineIndex.Length - 1;
            }

            return horizontalPosition + startingColumn - lineIndex.Length - 1;
        }

        public static ConsoleKeyInfo ReadKey(ref string numberOfMoves)
        {
            numberOfMoves = "";
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            while (char.IsDigit(keyInfo.KeyChar))
            {
                if (keyInfo.KeyChar == '0' && numberOfMoves.Length == 0)
                {
                    break;
                }

                numberOfMoves = numberOfMoves + keyInfo.KeyChar;
                keyInfo = Console.ReadKey(true);
            }

            if (numberOfMoves.Length == 0)
            {
                numberOfMoves = "1";
            }

            Dictionary<ConsoleKey, ConsoleKey> keyValue = new Dictionary<ConsoleKey, ConsoleKey>()
            {
                { ConsoleKey.K, ConsoleKey.UpArrow },
                { ConsoleKey.J, ConsoleKey.DownArrow },
                { ConsoleKey.H, ConsoleKey.LeftArrow },
                { ConsoleKey.L, ConsoleKey.RightArrow },
                { ConsoleKey.D0, ConsoleKey.Home }
            };

            foreach (var key in keyValue)
            {
                if (keyInfo.Key == key.Key)
                {
                    return new ConsoleKeyInfo((char)0, keyValue[keyInfo.Key], false, false, false);
                }
            }

            if (keyInfo.KeyChar == '$')
            {
                return new ConsoleKeyInfo((char)0, ConsoleKey.End, false, false, false);
            }

            return keyInfo;
        }

        private static void GetAllFiles(ref List<string> allFiles, string directory)
        {
            allFiles.AddRange(Directory.GetFiles(directory));

            foreach (string subDirectory in Directory.GetDirectories(directory))
            {
                GetAllFiles(ref allFiles, subDirectory);
            }
        }

        private static void InFileActions(string[] fileContent, bool fastTravelMode, string originalPath)
        {
            int startingLine = 0;
            int startingColumn = 0;
            string numberOfMoves = "";
            int lineCounting = Console.CursorTop;
            int verticalPosition = Console.CursorTop;
            int horizontalPosition = Console.CursorLeft;
            string[] originalFile = (string[])fileContent.Clone();
            bool quit = false;
            int charIndex;
            const bool editMode = false;

            Consola.Status(editMode, horizontalPosition, verticalPosition, lineCounting, startingColumn, fileContent, originalPath);
            ConsoleKeyInfo action = ReadKey(ref numberOfMoves);
            CursorMovement.FileParameter(fastTravelMode, fileContent);

            while (!quit)
            {
                switch (action.KeyChar)
                {
                    case 'A':
                        Variables.Undo.Push(new Stack<(int, string)>());
                        Variables.UndoDeleteLine.Push(new Stack<bool>());
                        Variables.UndoAddLine.Push(new Stack<bool>());
                        Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                        Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                        CursorMovement.EndButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);
                        EditMode(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, ref fileContent, originalPath);
                        break;

                    case 'I':
                        Variables.Undo.Push(new Stack<(int, string)>());
                        Variables.UndoDeleteLine.Push(new Stack<bool>());
                        Variables.UndoAddLine.Push(new Stack<bool>());
                        Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                        Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                        CursorMovement.MoveToFirstCharacter(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        EditMode(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, ref fileContent, originalPath);
                        break;

                    case 'i':
                        Variables.Undo.Push(new Stack<(int, string)>());
                        Variables.UndoDeleteLine.Push(new Stack<bool>());
                        Variables.UndoAddLine.Push(new Stack<bool>());
                        Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                        Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                        Variables.EditAfterCursor = true;
                        EditMode(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, ref fileContent, originalPath);
                        break;

                    case 'a':
                        Variables.Undo.Push(new Stack<(int, string)>());
                        Variables.UndoDeleteLine.Push(new Stack<bool>());
                        Variables.UndoAddLine.Push(new Stack<bool>());
                        Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                        Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                        Variables.EditAfterCursor = false;
                        EditMode(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, ref fileContent, originalPath);
                        break;

                    case 'o':
                        Variables.Undo.Push(new Stack<(int, string)>());
                        Variables.UndoDeleteLine.Push(new Stack<bool>());
                        Variables.UndoAddLine.Push(new Stack<bool>());
                        Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                        Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                        CursorMovement.EndButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);
                        charIndex = GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                        FileContentAlteration.AddLine(
                             ref lineCounting,
                             ref horizontalPosition,
                             ref verticalPosition,
                             ref startingLine,
                             ref startingColumn,
                             ref fileContent,
                             charIndex);
                        EditMode(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, ref fileContent, originalPath);
                        break;

                    case 'O':
                        Variables.Undo.Push(new Stack<(int, string)>());
                        Variables.UndoDeleteLine.Push(new Stack<bool>());
                        Variables.UndoAddLine.Push(new Stack<bool>());
                        Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                        Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                        CursorMovement.MoveToFirstCharacter(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        charIndex = GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                        FileContentAlteration.AddLine(
                             ref lineCounting,
                             ref horizontalPosition,
                             ref verticalPosition,
                             ref startingLine,
                             ref startingColumn,
                             ref fileContent,
                             charIndex);
                        CursorMovement.NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        EditMode(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, ref fileContent, originalPath);
                        break;

                    case ':':
                        CommandMode(ref quit, fileContent, originalFile, originalPath);
                        Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);

                        int currentStartColumn = Math.Max(0, Math.Min(startingColumn, fileContent[lineCounting].Length));
                        int currentEndColumn = fileContent[lineCounting].Length - currentStartColumn < Console.WindowWidth ?
                                               fileContent[lineCounting].Length - currentStartColumn : Console.WindowWidth - 1;
                        string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";
                        Console.SetCursorPosition(horizontalPosition > currentEndColumn + lineIndex.Length ? currentEndColumn + lineIndex.Length : horizontalPosition, verticalPosition);

                        originalFile = (string[])fileContent.Clone();
                        break;

                    case 'u':
                        Undo(ref fileContent, ref lineCounting, ref startingColumn, ref startingLine, ref horizontalPosition, ref verticalPosition);
                        Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                        Console.SetCursorPosition(horizontalPosition, verticalPosition);
                        CursorMovement.FileParameter(fastTravelMode, fileContent);
                        break;

                    case '\u0012':
                        Redo(ref fileContent, ref lineCounting, ref startingColumn, ref startingLine, ref horizontalPosition, ref verticalPosition);
                        Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                        Console.SetCursorPosition(horizontalPosition, verticalPosition);
                        CursorMovement.FileParameter(fastTravelMode, fileContent);
                        break;

                    case 'd':
                        Variables.Undo.Push(new Stack<(int, string)>());
                        Variables.UndoDeleteLine.Push(new Stack<bool>());
                        Variables.UndoAddLine.Push(new Stack<bool>());
                        Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                        Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                        FileContentAlteration.AutoDelete(
                              ref lineCounting,
                              ref horizontalPosition,
                              ref verticalPosition,
                              ref startingLine,
                              ref startingColumn,
                              ref fileContent);
                        break;

                    case 'D':
                        Variables.Undo.Push(new Stack<(int, string)>());
                        Variables.UndoDeleteLine.Push(new Stack<bool>());
                        Variables.UndoAddLine.Push(new Stack<bool>());
                        Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                        Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                        FileContentAlteration.DeleteTilTheEnd(
                              ref lineCounting,
                              ref horizontalPosition,
                              ref verticalPosition,
                              ref startingLine,
                              ref startingColumn,
                              ref fileContent);
                        break;
                }

                if (action.Key != ConsoleKey.I)
                {
                    Movements(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, numberOfMoves, action);
                }

                if (quit)
                {
                    break;
                }

                Consola.Status(editMode, horizontalPosition, verticalPosition, lineCounting, startingColumn, fileContent, originalPath);

                action = ReadKey(ref numberOfMoves);
            }

            Console.SetCursorPosition(0, Console.WindowTop);
        }

        private static void Movements(
                                      ref int lineCounting,
                                      ref int horizontalPosition,
                                      ref int verticalPosition,
                                      ref int startingLine,
                                      ref int startingColumn,
                                      string numberOfMoves,
                                      ConsoleKeyInfo action)
        {
            char? character = ' ';
            int oldVerticalPosition = verticalPosition;

            if (verticalPosition > Console.WindowHeight - 3)
            {
                verticalPosition = Console.WindowHeight - 3;
                lineCounting = lineCounting - (oldVerticalPosition - verticalPosition);
            }

            for (int i = 1; i <= Convert.ToInt32(numberOfMoves); i++)
            {
                switch (action.Key)
                {
                    case ConsoleKey.UpArrow:

                        CursorMovement.NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                        break;

                    case ConsoleKey.DownArrow:

                        CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                        break;

                    case ConsoleKey.LeftArrow:

                        CursorMovement.NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                        break;

                    case ConsoleKey.RightArrow:

                        CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                        break;

                    case ConsoleKey.End:

                        CursorMovement.EndButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);

                        break;

                    case ConsoleKey.Home:

                        CursorMovement.HomeButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);

                        break;

                    case ConsoleKey.PageDown:

                        CursorMovement.PageDownBehaviour(ref lineCounting, horizontalPosition, verticalPosition, ref startingLine, ref startingColumn);

                        break;

                    case ConsoleKey.PageUp:

                        CursorMovement.PageUpBehaviour(ref lineCounting, horizontalPosition, verticalPosition, ref startingLine, ref startingColumn);

                        break;

                    case ConsoleKey.W:

                        CursorMovement.MoveWordRight(action.KeyChar, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                        break;

                    case ConsoleKey.B:

                        CursorMovement.MoveWordLeft(action.KeyChar, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                        break;

                    case ConsoleKey.F:

                        character = ReadChar(ref character);

                        CursorMovement.GoOnDesiredCharacter(action.KeyChar, lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, character);

                        break;

                    case ConsoleKey.T:

                        character = ReadChar(ref character);

                        CursorMovement.GoTillDesiredCharacter(action.KeyChar, lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, character);

                        break;

                    case ConsoleKey.M:

                        character = ReadChar(ref character);

                        char key = Convert.ToChar(character);

                        CursorMovement.MarkLine(lineCounting, key);

                        break;
                }

                if (action.Modifiers == ConsoleModifiers.Control)
                {
                    if (action.Key == ConsoleKey.U)
                    {
                        CursorMovement.PageUpBehaviour(ref lineCounting, horizontalPosition, verticalPosition, ref startingLine, ref startingColumn);
                    }

                    if (action.Key == ConsoleKey.D)
                    {
                        CursorMovement.PageDownBehaviour(ref lineCounting, horizontalPosition, verticalPosition, ref startingLine, ref startingColumn);
                    }
                }
            }

            if (action.KeyChar == '^')
            {
                CursorMovement.MoveToFirstCharacter(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            }

            if (action.KeyChar == '\'')
            {
                character = ReadChar(ref character);

                char key = Convert.ToChar(character);

                CursorMovement.GoToMarkedLine(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, key);
            }

            if (action.KeyChar == 'g')
            {
                action = Console.ReadKey(true);
            }

            CursorMovement.GoToEndOrBeginingOfFile(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, action);
        }

        private static void EditMode(
                                     ref int lineCounting,
                                     ref int horizontalPosition,
                                     ref int verticalPosition,
                                     ref int startingLine,
                                     ref int startingColumn,
                                     ref string[] fileContent,
                                     string originalPath)
        {
            bool arrowButton = false;
            const bool editMode = true;
            bool fastTravelMode = Config.FastTravel;

            Consola.Status(editMode, horizontalPosition, verticalPosition, lineCounting, startingColumn, fileContent, originalPath);

            ConsoleKeyInfo action = Console.ReadKey(true);
            while (action.Key != ConsoleKey.Escape)
            {
                int oldVerticalPosition = verticalPosition;

                if (verticalPosition > Console.WindowHeight - 3)
                {
                    verticalPosition = Console.WindowHeight - 3;
                    lineCounting = lineCounting - (oldVerticalPosition - verticalPosition);
                }

                switch (action.Key)
                {
                    case ConsoleKey.UpArrow:

                        CursorMovement.NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        arrowButton = true;
                        break;

                    case ConsoleKey.DownArrow:

                        CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        arrowButton = true;
                        break;

                    case ConsoleKey.LeftArrow:

                        CursorMovement.NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        arrowButton = true;
                        break;

                    case ConsoleKey.RightArrow:

                        CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        arrowButton = true;
                        break;
                }

                if (!arrowButton)
                {
                    FileContentAlteration.EditText(
                              ref lineCounting,
                              ref horizontalPosition,
                              ref verticalPosition,
                              ref startingLine,
                              ref startingColumn,
                              ref fileContent,
                              action);
                    CursorMovement.FileParameter(fastTravelMode, fileContent);
                }

                Consola.Status(editMode, horizontalPosition, verticalPosition, lineCounting, startingColumn, fileContent, originalPath);

                action = Console.ReadKey(true);
                arrowButton = false;
            }
        }

        private static void CommandMode(ref bool quit, string[] fileLastVersion, string[] fileOriginalVersion, string originalPath)
        {
            Consola.CommandModeContour();
            ConsoleKeyInfo action = Console.ReadKey(true);
            string command = "";
            int bottomLane = Console.WindowHeight - 10;
            const int leftLane = 20;
            int rightLane = Console.WindowWidth - 20;
            while (!quit)
            {
                Console.SetCursorPosition(leftLane + 1, bottomLane - 1);
                Console.Write(new string(' ', rightLane - leftLane - 1));

                if (action.Key == ConsoleKey.Backspace)
                {
                    if (command.Length > 0)
                    {
                        command = command.Substring(0, command.Length - 1);
                    }
                }
                else if (action.Key != ConsoleKey.Enter)
                {
                    command = command + action.KeyChar;
                }

                string commandToShow = command;
                if (command.Length > rightLane - leftLane - 1)
                {
                    commandToShow = command.Substring(command.Length - (rightLane - leftLane - 1));
                }

                Console.SetCursorPosition(leftLane + 1, bottomLane - 1);
                Console.Write(commandToShow);
                Console.SetCursorPosition(commandToShow.Length + leftLane + 1, Console.CursorTop);
                action = Console.ReadKey(true);

                if (action.Key == ConsoleKey.Enter)
                {
                    Commands(ref command, ref quit, fileLastVersion, fileOriginalVersion, originalPath);
                    if (command.Contains('w'))
                    {
                        return;
                    }
                }
            }
        }

        private static void Commands(ref string command, ref bool quit, string[] fileLastVersion, string[] fileOriginalVersion, string originalPath)
        {
            const int topLane = 10;
            const int leftLane = 20;
            string path = originalPath;

            if (command.Contains("write") && command.Length > 5)
            {
                path = command.Replace("write", "").Trim() + '\\' + Path.GetFileName(originalPath);
                command = command.Substring(0, 5);
            }
            else
            {
                if (command.Contains("w") && command.Length > 5)
                {
                    path = command.Replace("w", "").Trim() + '\\' + Path.GetFileName(originalPath);
                    command = command.Substring(0, 1);
                }
            }

            switch (command)
            {
                case "quit!":

                case "q!":

                    quit = true;
                    break;

                case "quit":

                case "q":

                    if (fileLastVersion.SequenceEqual(fileOriginalVersion))
                    {
                        quit = true;
                        break;
                    }

                    Console.SetCursorPosition(leftLane + 1, topLane + 1);
                    Console.Write("File has modifications please save first ore use quit!/q!");
                    break;

                case "write":

                case "w":

                    File.WriteAllLines(path, fileLastVersion);
                    break;
            }
        }

        private static int ReadOption(int[] validOption)
        {
            try
            {
                int answer = Convert.ToInt32(Console.ReadLine());
                if (validOption.Contains(answer))
                {
                    return answer;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Choose a valid option");
                Console.ResetColor();

                return ReadOption(validOption);
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Choose a valid option");
                Console.ResetColor();
                return ReadOption(validOption);
            }
        }

        private static char? ReadChar(ref char? character)
        {
            if (character == ' ')
            {
                ConsoleKeyInfo baseInput = Console.ReadKey(true);

                character = baseInput.KeyChar;

                return character;
            }

            return character;
        }

        private static void Undo(ref string[] fileContent, ref int lineCounting, ref int startingColumn, ref int startingLine, ref int horizontalPosition, ref int verticalPosition)
        {
            if (Variables.Undo.Count == 0)
            {
                return;
            }

            Variables.Redo.Push(new Stack<(int, string)>());
            Variables.InfoToShowRedo.Push((lineCounting, startingLine, startingColumn));
            Variables.CursorPositionRedo.Push((horizontalPosition, verticalPosition));

            var deletedLines = Variables.UndoDeleteLine.Pop();
            Variables.RedoDeleteLine.Push(deletedLines);

            var addLines = Variables.UndoAddLine.Pop();
            Variables.RedoAddLine.Push(addLines);

            if (deletedLines.Contains(true) || addLines.Contains(true))
            {
                AddOrDeletedUndo(deletedLines, addLines, ref fileContent);
            }
            else
            {
                foreach (var (lineNumber, oldContent) in Variables.Undo.Pop())
                {
                    Variables.Redo.Peek().Push((lineNumber, fileContent[lineNumber]));
                    fileContent[lineNumber] = oldContent;
                }
            }

            lineCounting = Variables.InfoToShowUndo.Peek().Item1;
            startingLine = Variables.InfoToShowUndo.Peek().Item2;
            startingColumn = Variables.InfoToShowUndo.Peek().Item3;
            Variables.InfoToShowUndo.Pop();
            horizontalPosition = Variables.CursorPositionUndo.Peek().Item1;
            verticalPosition = Variables.CursorPositionUndo.Peek().Item2;
            Variables.CursorPositionUndo.Pop();
        }

        private static void Redo(ref string[] fileContent, ref int lineCounting, ref int startingColumn, ref int startingLine, ref int horizontalPosition, ref int verticalPosition)
        {
            if (Variables.Redo.Count == 0)
            {
                return;
            }

            Variables.Undo.Push(new Stack<(int, string)>());
            Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
            Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));

            var deletedLines = Variables.RedoDeleteLine.Pop();
            Variables.UndoDeleteLine.Push(deletedLines);

            var addLines = Variables.RedoAddLine.Pop();
            Variables.UndoAddLine.Push(addLines);

            if (deletedLines.Contains(true) || addLines.Contains(true))
            {
                AddOrDeletedRedo(deletedLines, addLines, ref fileContent);
            }
            else
            {
                foreach (var (lineNumber, newContent) in Variables.Redo.Pop())
                {
                    Variables.Undo.Peek().Push((lineNumber, fileContent[lineNumber]));
                    fileContent[lineNumber] = newContent;
                }
            }

            lineCounting = Variables.InfoToShowRedo.Peek().Item1;
            startingLine = Variables.InfoToShowRedo.Peek().Item2;
            startingColumn = Variables.InfoToShowRedo.Peek().Item3;
            Variables.InfoToShowRedo.Pop();
            horizontalPosition = Variables.CursorPositionRedo.Peek().Item1;
            verticalPosition = Variables.CursorPositionRedo.Peek().Item2;
            Variables.CursorPositionRedo.Pop();
        }

        private static void AddOrDeletedUndo(Stack<bool> deletedLines, Stack<bool> addLines, ref string[] fileContent)
        {
            if (deletedLines.Contains(true))
            {
                int deletedLinesNumber = deletedLines.Count(value => value);
                string[] undoContent = new string[fileContent.Length + deletedLinesNumber];

                for (int i = 0; i < fileContent.Length; i++)
                {
                    Variables.Redo.Peek().Push((i, fileContent[i]));
                    undoContent[i] = fileContent[i];
                }

                fileContent = (string[])undoContent.Clone();

                foreach (var (lineNumber, oldContent) in Variables.Undo.Pop())
                {
                    fileContent[lineNumber] = oldContent;
                }
            }

            if (addLines.Contains(true))
            {
                int addedLinesNumber = addLines.Count(value => value);
                string[] undoContent = new string[fileContent.Length - addedLinesNumber];

                for (int i = undoContent.Length; i < fileContent.Length; i++)
                {
                    Variables.Redo.Peek().Push((i, fileContent[i]));
                }

                foreach (var (lineNumber, oldContent) in Variables.Undo.Pop())
                {
                    Variables.Redo.Peek().Push((lineNumber, fileContent[lineNumber]));
                    fileContent[lineNumber] = oldContent;
                }

                Array.Copy(fileContent, 0, undoContent, 0, fileContent.Length - addedLinesNumber);

                fileContent = (string[])undoContent.Clone();
            }
        }

        private static void AddOrDeletedRedo(Stack<bool> deletedLines, Stack<bool> addLines, ref string[] fileContent)
        {
            if (deletedLines.Contains(true))
            {
                int deletedLinesNumber = deletedLines.Count(value => value);
                string[] redoContent = new string[fileContent.Length - deletedLinesNumber];

                for (int i = 0; i < fileContent.Length - deletedLinesNumber; i++)
                {
                    Variables.Undo.Peek().Push((i, fileContent[i]));
                    redoContent[i] = fileContent[i];
                }

                Variables.Undo.Peek().Push((fileContent.Length - 1, fileContent[fileContent.Length - 1]));

                fileContent = (string[])redoContent.Clone();

                foreach (var (lineNumber, newContent) in Variables.Redo.Pop())
                {
                    fileContent[lineNumber] = newContent;
                }
            }

            if (addLines.Contains(true))
            {
                int addedLinesNumber = addLines.Count(value => value);
                string[] redoContent = new string[fileContent.Length + addedLinesNumber];

                for (int i = 0; i < fileContent.Length; i++)
                {
                    Variables.Undo.Peek().Push((i, fileContent[i]));
                    redoContent[i] = fileContent[i];
                }

                fileContent = (string[])redoContent.Clone();

                foreach (var (lineNumber, newContent) in Variables.Redo.Pop())
                {
                    Variables.Undo.Peek().Push((lineNumber, fileContent[lineNumber]));
                    fileContent[lineNumber] = newContent;
                }
            }
        }
    }
}