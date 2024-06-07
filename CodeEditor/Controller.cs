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

            Consola.ShowContentOfFile(fileContent, Console.WindowHeight - 1, fastTravelMode);
            NavigateInFile(fileContent, fastTravelMode);
        }

        private static void GetAllFiles(ref List<string> allFiles, string directory)
        {
            allFiles.AddRange(Directory.GetFiles(directory));

            foreach (string subDirectory in Directory.GetDirectories(directory))
            {
                GetAllFiles(ref allFiles, subDirectory);
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