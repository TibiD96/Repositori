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
            const bool editMode = false;

            Consola.Status(editMode, horizontalPosition, verticalPosition, lineCounting, startingColumn, fileContent, originalPath);
            ConsoleKeyInfo action = ReadKey(ref numberOfMoves);
            CursorMovement.FileParameter(fastTravelMode, fileContent);

            while (!quit)
            {
                if (action.Key != ConsoleKey.I)
                {
                    Movements(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, numberOfMoves, action);
                }
                else
                {
                    EditMode(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, ref fileContent, originalPath);
                }

                if (action.KeyChar == ':')
                {
                    CommandMode(ref quit, fileContent, originalFile, originalPath);
                    Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);

                    int currentStartColumn = Math.Max(0, Math.Min(startingColumn, fileContent[lineCounting].Length));
                    int currentEndColumn = fileContent[lineCounting].Length - currentStartColumn < Console.WindowWidth ?
                                           fileContent[lineCounting].Length - currentStartColumn : Console.WindowWidth - 1;
                    string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";
                    Console.SetCursorPosition(horizontalPosition > currentEndColumn + lineIndex.Length ? currentEndColumn + lineIndex.Length : horizontalPosition, verticalPosition);

                    originalFile = (string[])fileContent.Clone();
                }

                if (quit)
                {
                    break;
                }

                Consola.Status(editMode, horizontalPosition, verticalPosition, lineCounting, startingColumn, fileContent, originalPath);

                action = ReadKey(ref numberOfMoves);
            }

            Console.SetCursorPosition(0, Console.WindowWidth);
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

                        CursorMovement.FindCharacter(action.KeyChar, lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, character);

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
                CursorMovement.CaretBehaviour(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            }

            if (action.KeyChar == '\'')
            {
                character = ReadChar(ref character);

                char key = Convert.ToChar(character);

                CursorMovement.GoToMarkedLine(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, key);
            }
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
                    EditText(
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

        private static void EditText(
                                     ref int lineCounting,
                                     ref int horizontalPosition,
                                     ref int verticalPosition,
                                     ref int startingLine,
                                     ref int startingColumn,
                                     ref string[] fileContent,
                                     ConsoleKeyInfo action)
        {
            bool fastTravelMode = Config.FastTravel;
            int charIndex;

            int currentStartColumn = Math.Max(0, Math.Min(startingColumn, fileContent[lineCounting].Length));
            int currentEndColumn = fileContent[lineCounting].Length - currentStartColumn < Console.WindowWidth ? fileContent[lineCounting].Length - currentStartColumn : Console.WindowWidth - 1;
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";

            if (horizontalPosition >= currentEndColumn + lineIndex.Length)
            {
                horizontalPosition = currentEndColumn + lineIndex.Length;
                charIndex = horizontalPosition + startingColumn - lineIndex.Length - 1;
            }
            else
            {
                charIndex = horizontalPosition + startingColumn - lineIndex.Length - 1;
            }

            switch (action.Key)
            {
                case ConsoleKey.Backspace:
                    DeleteLine(
                                ref lineCounting,
                                ref horizontalPosition,
                                ref verticalPosition,
                                ref startingLine,
                                ref startingColumn,
                                ref fileContent,
                                charIndex);
                    break;

                case ConsoleKey.Enter:
                    AddLine(
                             ref lineCounting,
                             ref horizontalPosition,
                             ref verticalPosition,
                             ref startingLine,
                             ref startingColumn,
                             ref fileContent,
                             charIndex);
                    break;

                default:
                    fileContent[lineCounting] = charIndex == fileContent[lineCounting].Length
                    ? fileContent[lineCounting].Substring(charIndex) + action.KeyChar
                    : fileContent[lineCounting].Substring(0, charIndex + 1) + action.KeyChar + fileContent[lineCounting].Substring(charIndex + 1);

                    CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                    Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                    Console.SetCursorPosition(horizontalPosition, verticalPosition);
                    break;
            }
        }

        private static void DeleteLine(
                                       ref int lineCounting,
                                       ref int horizontalPosition,
                                       ref int verticalPosition,
                                       ref int startingLine,
                                       ref int startingColumn,
                                       ref string[] fileContent,
                                       int charIndex)
        {
            bool fastTravelMode = Config.FastTravel;

            if (charIndex >= 0)
            {
                fileContent[lineCounting] = fileContent[lineCounting].Remove(charIndex, 1);
                CursorMovement.NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                Console.SetCursorPosition(horizontalPosition, verticalPosition);
            }
            else
            {
                if (lineCounting > 0)
                {
                    string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting - 1, lineCounting - 1, Convert.ToString(fileContent.Length)) + " ";
                    int indexOfLastChar = fileContent[lineCounting - 1].Length - 1 + lineIndex.Length;
                    if (fileContent[lineCounting].Length > 0)
                    {
                        fileContent[lineCounting - 1] = fileContent[lineCounting - 1] + fileContent[lineCounting];
                    }

                    for (int i = lineCounting; i < fileContent.Length - 1; i++)
                    {
                        fileContent[i] = fileContent[i + 1];
                    }

                    string[] newfileContent = fileContent.Take(fileContent.Length - 1).ToArray();

                    fileContent = (string[])newfileContent.Clone();

                    CursorMovement.FileParameter(fastTravelMode, fileContent);
                    CursorMovement.NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                    lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";

                    startingColumn = 0;
                    horizontalPosition = lineIndex.Length;

                    while (horizontalPosition + startingColumn < indexOfLastChar)
                    {
                        CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                    }

                    Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                    Console.SetCursorPosition(horizontalPosition, verticalPosition);
                }
            }
        }

        private static void AddLine(
                                      ref int lineCounting,
                                      ref int horizontalPosition,
                                      ref int verticalPosition,
                                      ref int startingLine,
                                      ref int startingColumn,
                                      ref string[] fileContent,
                                      int charIndex)
        {
            bool fastTravelMode = Config.FastTravel;
            string[] newFileContent = new string[fileContent.Length + 1];
            string newLine;
            if (charIndex > 0)
            {
                newLine = fileContent[lineCounting].Substring(charIndex + 1);
                fileContent[lineCounting] = fileContent[lineCounting].Substring(0, charIndex + 1);
            }
            else
            {
                newLine = fileContent[lineCounting];
                fileContent[lineCounting] = fileContent[lineCounting].Remove(0);
            }

            if (lineCounting < fileContent.Length - 1)
            {
                Array.Copy(fileContent, 0, newFileContent, 0, lineCounting + 1);
                newFileContent[lineCounting + 1] = newLine;
                Array.Copy(fileContent, lineCounting + 1, newFileContent, lineCounting + 2, fileContent.Length - (lineCounting + 1));
            }
            else
            {
                Array.Copy(fileContent, 0, newFileContent, 0, lineCounting + 1);
                newFileContent[lineCounting + 1] = newLine;
            }

            fileContent = (string[])newFileContent.Clone();

            CursorMovement.FileParameter(fastTravelMode, fileContent);
            CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";
            startingColumn = 0;
            horizontalPosition = lineIndex.Length;
            Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition, verticalPosition);
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

        private static ConsoleKeyInfo ReadKey(ref string numberOfMoves)
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
    }
}