namespace CodeEditor
{
    public class Controller
    {
        public static void ShowContent(string pathToFile)
        {
            int currentLine = Console.WindowHeight - 1;
            string fullPath = pathToFile;

            bool fastTravelMode = Config.FastTravel;
            string[] lines = File.ReadAllLines(fullPath);
            Consola.ShowContentOfFile(lines, currentLine, fastTravelMode);
            NavigateInFile(lines, fastTravelMode);
        }

        public static void Finder()
        {
            string currentDirectory = Environment.CurrentDirectory;
            string[] filesFromDirectory = Directory.GetFiles(currentDirectory);
            Consola.ClearConsole();
            Consola.ShowDirectoryContent(filesFromDirectory);
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            ShowContent(FuzzySearch(filesFromDirectory));
        }

        private static string FuzzySearch(string[] filesFromDirectory)
        {
            ConsoleKeyInfo key;
            List<string> listOfValidFiles = new List<string>();
            string search = "";

            key = Console.ReadKey();

            while (key.Key != ConsoleKey.UpArrow && key.Key != ConsoleKey.Escape)
            {
                UpdateSearch(ref search, key);
                listOfValidFiles.Clear();
                listOfValidFiles = SearchingLogic(filesFromDirectory, search);

                if (listOfValidFiles.Count == 0 && search.Length != 0)
                {
                    Consola.ShowValidResults(listOfValidFiles, search.Length, filesFromDirectory);
                }

                if (search.Length == 0)
                {
                    Consola.ClearResultsWindow();
                    Consola.ShowDirectoryContent(filesFromDirectory);
                    Console.SetCursorPosition(0, Console.WindowHeight - 1);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, Console.WindowHeight - 1);
                }

                if (listOfValidFiles.Count != 0)
                {
                    Consola.ShowValidResults(listOfValidFiles, search.Length, filesFromDirectory);
                }

                Console.SetCursorPosition(search.Length, Console.WindowHeight - 1);

                key = Console.ReadKey();
            }

            return NavigateThroughValid(listOfValidFiles, search.Length, filesFromDirectory);
        }

        private static string NavigateThroughValid(List<string> validFiles, int identicalCharacter, string[] totalNumberOFiles)
        {
            int verticalPosition = Console.WindowHeight - 3;
            int fileCurrentLine = 0;
            string current = validFiles[fileCurrentLine];

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, verticalPosition);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, verticalPosition);
            Console.Write("->" + Path.GetFileName(current));

            ConsoleKeyInfo navigationDirection = Console.ReadKey(true);
            while (navigationDirection.Key != ConsoleKey.Enter)
            {
                if (navigationDirection.Key == ConsoleKey.UpArrow && fileCurrentLine < validFiles.Count - 1)
                {
                    Consola.ShowValidResults(validFiles, identicalCharacter, totalNumberOFiles);
                    verticalPosition--;
                    fileCurrentLine++;
                    current = validFiles[fileCurrentLine];
                    Console.SetCursorPosition(0, verticalPosition);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, verticalPosition);
                    Console.Write("->" + Path.GetFileName(current));
                }

                if (navigationDirection.Key == ConsoleKey.DownArrow && fileCurrentLine > 0)
                {
                    if (fileCurrentLine == 0)
                    {
                        break;
                    }

                    Consola.ShowValidResults(validFiles, identicalCharacter, totalNumberOFiles);
                    verticalPosition++;
                    fileCurrentLine--;
                    current = validFiles[fileCurrentLine];
                    Console.SetCursorPosition(0, verticalPosition);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, verticalPosition);
                    Console.Write("->" + Path.GetFileName(current));
                }

                navigationDirection = Console.ReadKey(true);
            }

            return current;
        }

        private static List<string> SearchingLogic(string[] filesFromDirectory, string search)
        {
            List<string> listOfValidFiles = new List<string>();
            string file = "";

            if (search.Length > 0)
            {
                for (int i = 0; i < filesFromDirectory.Length; i++)
                {
                    if (Path.GetFileName(filesFromDirectory[i]).Length >= search.Length)
                    {
                        file = Path.GetFileName(filesFromDirectory[i]).Substring(0, search.Length);
                    }

                    if (file == search && !listOfValidFiles.Contains(Path.GetFileName(filesFromDirectory[i])))
                    {
                        listOfValidFiles.Add(filesFromDirectory[i]);
                    }
                }
            }

            return listOfValidFiles;
        }

        private static void UpdateSearch(ref string search, ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Backspace && search.Length > 0)
            {
                search = search.Substring(0, search.Length - 1);
                Console.SetCursorPosition(0, Console.WindowHeight - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.WindowHeight - 1);
                Console.Write(search);
            }
            else if (key.Key != ConsoleKey.Backspace)
            {
                search = search + key.KeyChar;
            }
        }

        private static void NavigateInFile(string[] lines, bool fastTravelMode)
        {
            int startingLine = 0;
            int startingColumn = 0;
            string numberOfMoves = "";
            char? character = ' ';
            int lineCounting = Console.CursorTop;
            int verticalPosition = Console.CursorTop;
            int horizontalPosition = Console.CursorLeft;
            ConsoleKeyInfo navigationDirection = ReadKey(ref numberOfMoves);
            CursorMovement.FileParameter(fastTravelMode, lines);
            while (navigationDirection.Key != ConsoleKey.Escape)
            {
                for (int i = 1; i <= Convert.ToInt32(numberOfMoves); i++)
                {
                    switch (navigationDirection.Key)
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

                            CursorMovement.MoveWordRight(navigationDirection.KeyChar, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                            break;

                        case ConsoleKey.B:

                            CursorMovement.MoveWordLeft(navigationDirection.KeyChar, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                            break;

                        case ConsoleKey.F:

                            character = ReadChar(ref character);

                            CursorMovement.FindCharacter(navigationDirection.KeyChar, lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, character);

                            break;

                        case ConsoleKey.M:

                            character = ReadChar(ref character);

                            char key = Convert.ToChar(character);

                            CursorMovement.MarkLine(lineCounting, key);

                            break;
                    }
                }

                if (navigationDirection.KeyChar == '^')
                {
                    CursorMovement.CaretBehaviour(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }

                if (navigationDirection.KeyChar == '\'')
                {
                    character = ReadChar(ref character);

                    char key = Convert.ToChar(character);

                    CursorMovement.GoToMarkedLine(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, key);
                }

                character = ' ';
                navigationDirection = ReadKey(ref numberOfMoves);
            }

            Console.SetCursorPosition(0, Console.WindowWidth);
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