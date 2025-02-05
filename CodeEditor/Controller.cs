using System;
using System.Data;
using System.Numerics;
using static System.Collections.Specialized.BitVector32;

namespace CodeEditor
{
    public class Controller
    {
        private static string[] fileContent = [""];
        private static string originalPath = "";

        public static void Open()
        {
            bool fastTravelMode = Config.FastTravel;
            Consola.ClearEntireConsole();
            Consola.ShowContentOfFile(fileContent, fileContent.Length - 1, fastTravelMode);
            InFileActions(fastTravelMode);
        }

        public static void OpenFile()
        {
            string currentDirectory = Environment.CurrentDirectory;
            string[] filesFromDirectory = Directory.GetFiles(currentDirectory);
            bool fastTravelMode = Config.FastTravel;
            List<string> allFiles = new List<string>();
            int lineToShow;

            GetAllFiles(ref allFiles, currentDirectory);
            originalPath = FuzzySearchLogic.FuzzySearch(filesFromDirectory, allFiles.ToArray());
            fileContent = File.ReadAllLines(originalPath);

            if (fileContent.Length == 0)
            {
                fileContent = [""];

            }

            if (fileContent.Length > Console.WindowHeight - 4)
            {
                lineToShow = Console.WindowHeight - 4;
            }
            else
            {
                lineToShow = fileContent.Length - 1;
            }

            Consola.ClearEntireConsole();
            Consola.ShowContentOfFile(fileContent, lineToShow, fastTravelMode);
        }

        public static int GetCursorCharIndex(int lineCounting, ref int horizontalPosition, int startingColumn, string[] fileContent)
        {
            bool fastTravelMode = Config.FastTravel;

            (int, int) currentStartEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent);
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";

            if (horizontalPosition >= currentStartEndColumn.Item2 + lineIndex.Length)
            {
                horizontalPosition = currentStartEndColumn.Item2 + lineIndex.Length;
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

            if (keyInfo.Modifiers == ConsoleModifiers.Control)
            {
                if (keyInfo.Key == ConsoleKey.U)
                {
                    return new ConsoleKeyInfo((char)0, ConsoleKey.PageUp, false, false, true);
                }

                if (keyInfo.Key == ConsoleKey.D)
                {
                    return new ConsoleKeyInfo((char)0, ConsoleKey.PageDown, false, false, true);
                }
            }

            return keyInfo;
        }

        public static char? ReadChar(ref char? character)
        {
            if (character == ' ')
            {
                ConsoleKeyInfo baseInput = Console.ReadKey(true);

                character = baseInput.KeyChar;

                return character;
            }

            return character;
        }

        private static void GetAllFiles(ref List<string> allFiles, string directory)
        {
            allFiles.AddRange(Directory.GetFiles(directory));

            foreach (string subDirectory in Directory.GetDirectories(directory))
            {
                GetAllFiles(ref allFiles, subDirectory);
            }
        }

        private static void InFileActions(bool fastTravelMode)
        {
            int startingLine = 0;
            int startingColumn = 0;
            string numberOfMoves = "";
            int baseStartingLine = 0;
            int baseStrartingColumn = 0;
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

                if (action.Key == ConsoleKey.Spacebar)
                {
                    if ((ReadKey(ref numberOfMoves)).Key == ConsoleKey.F)
                    {
                        OpenFile();
                        lineCounting = Console.CursorTop;
                        verticalPosition = Console.CursorTop;
                        horizontalPosition = Console.CursorLeft;
                        originalFile = (string[])fileContent.Clone();
                        CursorMovement.FileParameter(fastTravelMode, fileContent);
                    }
                }

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
                        string lastPath = originalPath;
                        CommandMode(ref quit, fileContent, originalFile, originalPath);

                        if (lastPath != originalPath)
                        {
                            if (fileContent.Length > Console.WindowHeight - 4)
                            {
                                lineCounting = Console.WindowHeight - 4;
                            }
                            else
                            {
                                lineCounting = fileContent.Length - 1;
                            }

                            Consola.ClearEntireConsole();
                            Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode);

                            lineCounting = Console.CursorTop;
                            verticalPosition = Console.CursorTop;
                            horizontalPosition = Console.CursorLeft;
                            CursorMovement.FileParameter(fastTravelMode, fileContent);

                        }
                        else
                        {
                            Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                        }

                        (int, int) currentStartEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent);
                        string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";
                        Console.SetCursorPosition(horizontalPosition > currentStartEndColumn.Item2 + lineIndex.Length ? currentStartEndColumn.Item2 + lineIndex.Length : horizontalPosition, verticalPosition);

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
                    Consola.ClearEntireConsole();
                    break;
                }

                if (baseStartingLine != startingLine || baseStrartingColumn != startingColumn)
                {
                    baseStartingLine = startingLine;
                    baseStrartingColumn = startingColumn;
                    Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                }
                else
                {
                    Console.CursorVisible = false;
                    Console.SetCursorPosition(0, 0);
                    Consola.WriteIndexWithoutLine(fileContent, lineCounting, fastTravelMode, startingLine);
                    Console.CursorVisible = true;
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
            }

            if (action.KeyChar == '^')
            {
                CursorMovement.MoveToFirstCharacter(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            }

            if (action.KeyChar == '\\')
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
            int baseStartingLine = startingLine;
            int baseStrartingColumn = startingColumn;

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

                if (baseStartingLine != startingLine || baseStrartingColumn != startingColumn)
                {
                    baseStartingLine = startingLine;
                    baseStrartingColumn = startingColumn;
                    Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                }
                else
                {
                    Console.CursorVisible = false;
                    Console.SetCursorPosition(0, 0);
                    Consola.WriteIndexWithoutLine(fileContent, lineCounting, fastTravelMode, startingLine);
                    Console.CursorVisible = true;
                }

                Consola.Status(editMode, horizontalPosition, verticalPosition, lineCounting, startingColumn, fileContent, originalPath);

                action = Console.ReadKey(true);
                arrowButton = false;
            }
        }

        private static void CommandMode(ref bool quit, string[] fileLastVersion, string[] fileOriginalVersion, string lastPath)
        {
            Consola.CommandModeContour();
            string command = "";
            int commandArea = 2;
            const int startingCompletionContour = 4;
            const int leftLane = 20;
            int rightLane = Console.WindowWidth - 20;
            string commandToShow;
            List<string> validCommands = [];

            Console.SetCursorPosition(leftLane + 1, commandArea);
            ConsoleKeyInfo action = Console.ReadKey(true);
            while (!quit)
            {
                Consola.ClearPartOfConsole(startingCompletionContour + validCommands.Count + 1, startingCompletionContour, leftLane, 1);
                validCommands = Config.Commands.Where(value => value.StartsWith(command)).ToList();
                Console.SetCursorPosition(leftLane + 1, commandArea);
                Console.Write(new string(' ', rightLane - leftLane - 1));

                if (action.Key == ConsoleKey.Tab)
                {
                    Config.TabCompletion = true;
                }

                if (action.Key == ConsoleKey.Escape)
                {
                    Config.TabCompletion = false;
                    Console.Write('a');
                    return;
                }

                if (action.Key == ConsoleKey.Backspace)
                {
                    if (command.Length > 0)
                    {
                        command = command.Substring(0, command.Length - 1);
                    }
                }
                else if (char.IsLetter((char)action.Key) || char.IsDigit((char)action.Key))
                {
                    command += action.KeyChar;
                }

                command = TabCompletion(command, action, validCommands);
                validCommands = Config.Commands.Where(value => value.StartsWith(command)).ToList();

                commandToShow = command;

                if (command.Length > rightLane - leftLane - 1)
                {
                    commandToShow = command.Substring(command.Length - (rightLane - leftLane - 1));
                }

                Consola.ClearPartOfConsole(commandArea, commandArea);
                Console.SetCursorPosition(leftLane + 1, commandArea);
                Console.Write(commandToShow);
                Console.SetCursorPosition(commandToShow.Length + leftLane + 1, Console.CursorTop);

                action = Console.ReadKey(false);

                if (action.Key == ConsoleKey.Enter)
                {
                    Config.TabCompletion = false;

                    if (command == "e" || command == "edit")
                    {
                        Consola.ShowDirectoryContent([.. (AutoCompletionLogic.FilesFromDirectory(Environment.CurrentDirectory))]);
                        Console.SetCursorPosition(leftLane + 1 + command.Length, commandArea - 1);
                        Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                        (string, bool) autoCompResult = AutoCompletionLogic.AutoCompletion(action);
                        originalPath = autoCompResult.Item1;
                        quit = autoCompResult.Item2;

                        if (originalPath != "")
                        {
                            fileContent = File.ReadAllLines(originalPath);
                            Consola.ClearEntireConsole();

                            return;
                        }

                        if (quit)
                        {
                            quit = false;
                            return;
                        }

                        Consola.ClearPartOfConsole(Console.WindowHeight - 12);
                        command = command.Substring(0, command.Length - 1);
                        Console.SetCursorPosition(leftLane + 1, commandArea - 1);
                        Console.Write(commandToShow);
                    }
                    else
                    {
                        Commands(ref command, ref quit, fileLastVersion, fileOriginalVersion, lastPath);
                        if (command.Contains('w'))
                        {
                            return;
                        }
                    }
                }
            }
        }

        private static void Commands(ref string command, ref bool quit, string[] fileLastVersion, string[] fileOriginalVersion, string lastPath)
        {
            const int topLane = 10;
            const int leftLane = 20;
            string path = lastPath;

            if (command.Contains("write") && command.Length > 5)
            {
                path = command.Replace("write", "").Trim() + '\\' + Path.GetFileName(lastPath);
                command = command.Substring(0, 5);
            }
            else
            {
                if (command.Contains("w") && command.Length > 5)
                {
                    path = command.Replace("w", "").Trim() + '\\' + Path.GetFileName(lastPath);
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

        private static string TabCompletion(string command, ConsoleKeyInfo action, List<string> validCommands)
        {
            const int startingCompletionContour = 4;
            const int left = 21;

            if (Config.TabCompletion)
            {
                validCommands = Config.Commands.Where(value => value.StartsWith(command)).ToList();
                Consola.CompletionContour(validCommands.Count);

                Console.SetCursorPosition(left, startingCompletionContour + 1);

                command = CommadnModeAutoCompletion.AutoCompletion(command, action);

                Consola.ClearPartOfConsole(startingCompletionContour + validCommands.Count + 1, startingCompletionContour, left - 1, 1);
                validCommands = Config.Commands.Where(value => value.StartsWith(command)).ToList();

                if (validCommands.Count > 0)
                {
                    Consola.CompletionContour(validCommands.Count);

                    Console.BackgroundColor = ConsoleColor.Blue;
                    for (int i = 0; i < validCommands.Count; i++)
                    {
                        Console.Write(validCommands[i]);
                        Console.ResetColor();
                        Console.SetCursorPosition(left, Console.CursorTop + 1);
                    }
                }

                    return command;
            }

            return command;
        }

    }
}