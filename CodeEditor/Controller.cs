namespace CodeEditor
{
    public class Controller
    {
        public static void Finder()
        {
            string currentDirectory = Environment.CurrentDirectory;
            string[] filesFromDirectory = Directory.GetFiles(currentDirectory);
            List<string> filesFromDir = new List<string>();
            List<string> allFiles = new List<string>();

            GetAllFiles(ref allFiles, currentDirectory);
            filesFromDir.AddRange(filesFromDirectory);

            ShowContent(FuzzySearch(filesFromDirectory, allFiles.ToArray()));
        }

        public static void ShowContent(string pathToFile)
        {
            int currentLine = Console.WindowHeight - 1;
            string fullPath = pathToFile;

            bool fastTravelMode = Config.FastTravel;
            string[] lines = File.ReadAllLines(fullPath);
            Consola.ShowContentOfFile(lines, currentLine, fastTravelMode);
            NavigateInFile(lines, fastTravelMode);
        }

        private static void GetAllFiles(ref List<string> allFiles, string directory)
        {
            allFiles.AddRange(Directory.GetFiles(directory));

            foreach (string subDirectory in Directory.GetDirectories(directory))
            {
                GetAllFiles(ref allFiles, subDirectory);
            }
        }

        private static string FuzzySearch(string[] filesFromDirectory, string[] allFiles)
        {
            ConsoleKeyInfo key;
            List<string> listOfValidFiles = new List<string>(filesFromDirectory);
            string search = "";
            (string, ConsoleKeyInfo) navigationResult;

            Consola.ClearConsole();
            Consola.DrawContour();
            Consola.ShowValidResults(listOfValidFiles, listOfValidFiles.Count, "", allFiles.ToArray());

            string file = ValidFileByDefault(listOfValidFiles);

            Console.CursorVisible = true;
            Console.SetCursorPosition(1, Console.WindowHeight - 2);

            key = Console.ReadKey();

            while (key.Key != ConsoleKey.Enter)
            {
                if (key.Key != ConsoleKey.UpArrow && key.Key != ConsoleKey.DownArrow)
                {
                    UpdateSearch(ref search, key, ref listOfValidFiles, ref file, allFiles);
                }

                if (listOfValidFiles.Count > 0 && (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.DownArrow))
                {
                    navigationResult = NavigateThroughValid(listOfValidFiles, search, allFiles);
                    Console.CursorVisible = true;
                    file = navigationResult.Item1;
                    Console.SetCursorPosition(search.Length + 1, Console.WindowHeight - 2);
                    key = navigationResult.Item2;
                    Console.Write(key.KeyChar);
                }
                else
                {
                    Console.SetCursorPosition(search.Length + 1, Console.WindowHeight - 2);
                    key = Console.ReadKey();
                    while (key.Key == ConsoleKey.Enter && listOfValidFiles.Count == 0)
                    {
                        Console.SetCursorPosition(search.Length + 1, Console.WindowHeight - 2);
                        key = Console.ReadKey();
                    }
                }
            }

            return file;
        }

        private static void UpdateSearch(ref string search, ConsoleKeyInfo key, ref List<string> listOfValidFiles, ref string file, string[] allFiles)
        {
            if (key.Key == ConsoleKey.Backspace && search.Length > 0)
            {
                search = search.Substring(0, search.Length - 1);
                Console.SetCursorPosition(1, Console.WindowHeight - 2);
                Console.Write(new string(' ', Console.WindowWidth - 2));
                Console.SetCursorPosition(1, Console.WindowHeight - 2);
                Console.Write(search);
            }
            else if (key.Key != ConsoleKey.Backspace)
            {
                search = search + key.KeyChar;
            }

            listOfValidFiles.Clear();
            listOfValidFiles = GetValidFiles(search, allFiles);

            if (search.Length == 0)
            {
                Consola.ClearResultsWindow();
                Console.SetCursorPosition(1, Console.WindowHeight - 2);
                Console.Write(new string(' ', Console.WindowWidth - 2));
                listOfValidFiles.Clear();
                listOfValidFiles.AddRange(allFiles);
                Consola.ShowValidResults(listOfValidFiles, listOfValidFiles.Count, search, allFiles);
                file = ValidFileByDefault(listOfValidFiles);
                Console.CursorVisible = true;
                Console.SetCursorPosition(search.Length + 1, Console.WindowHeight - 2);
            }
            else
            {
                Consola.ShowValidResults(listOfValidFiles, listOfValidFiles.Count, search, allFiles);
                file = ValidFileByDefault(listOfValidFiles);
                Console.CursorVisible = true;
                Console.SetCursorPosition(search.Length + 1, Console.WindowHeight - 2);
            }
        }

        private static List<string> GetValidFiles(string search, string[] allFiles)
        {
            List<string> preliminaryList = FilesWhichContainAllChar(search, allFiles);
            SortedList<int, List<string>> validOrderedFiles = new SortedList<int, List<string>>();
            List<string> finalList = new List<string>();
            int distance;

            foreach (string file in preliminaryList)
            {
                distance = Path.GetFileName(file).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ? 1 : LevenshteinDistance(search, Path.GetFileName(file));

                if (distance < Path.GetFileName(file).Length)
                {
                    List<string> toAdd = new List<string> { file };
                    if (validOrderedFiles.ContainsKey(distance))
                    {
                        validOrderedFiles[distance].Add(file);
                    }
                    else
                    {
                        validOrderedFiles.Add(distance, toAdd);
                    }
                }
            }

            foreach (List<string> list in validOrderedFiles.Values)
            {
                finalList.AddRange(list);
            }

            return finalList;
        }

        private static List<string> FilesWhichContainAllChar(string search, string[] allFiles)
        {
            int charIndex = 0;
            List<string> preliminaryList = new List<string>();

            foreach (string file in allFiles)
            {
                foreach (char c in Path.GetFileName(file))
                {
                    if (charIndex < search.Length && char.ToLower(c) == char.ToLower(search[charIndex]))
                    {
                        charIndex++;
                    }
                }

                if (charIndex == search.Length)
                {
                    preliminaryList.Add(file);
                }

                charIndex = 0;
            }

            return preliminaryList.OrderBy(file => Path.GetFileName(file).Length).ToList();
        }

        private static int LevenshteinDistance(string search, string file)
        {
            int line = search.Length;
            int column = file.Length;
            int[,] table = new int[line + 1, column + 1];

            for (int i = 0; i <= line; i++)
            {
                table[i, 0] = i;
            }

            for (int j = 0; j <= column; j++)
            {
                table[0, j] = j;
            }

            for (int i = 1; i <= line; i++)
            {
                for (int j = 1; j <= column; j++)
                {
                    int cost = file[j - 1] == search[i - 1] ? 0 : 1;
                    int deletion = table[i - 1, j] + 1;
                    int insertion = table[i, j - 1] + 1;
                    int substitution = table[i - 1, j - 1] + cost;
                    table[i, j] = Math.Min(Math.Min(deletion, insertion), substitution);
                }
            }

            return table[line, column];
        }

        private static string ValidFileByDefault(List<string> validFiles)
        {
            int verticalPosition = Console.WindowHeight - 5;
            string file = "";

            if (validFiles.Count > 0)
            {
                file = validFiles[0];
                Console.CursorVisible = false;
                Console.SetCursorPosition(1, verticalPosition);
                Console.Write(new string(' ', Console.WindowWidth - 2));
                Console.SetCursorPosition(1, verticalPosition);
                Console.Write("->" + Path.GetFileName(file));
                return file;
            }

            return file;
        }

        private static (string, ConsoleKeyInfo) NavigateThroughValid(List<string> validFiles, string search, string[] totalNumberOFiles)
        {
            int verticalPosition = Console.WindowHeight - 5;
            int fileCurrentLine = 0;
            string current = validFiles[fileCurrentLine];
            int startingLineIndex = 0;

            if (validFiles.Count > 1)
            {
                verticalPosition--;
                fileCurrentLine++;
                current = validFiles[fileCurrentLine];
                Consola.ClearResultsWindow();
                Consola.ShowValidResults(validFiles, validFiles.Count, search, totalNumberOFiles);
                Console.CursorVisible = false;
                Console.SetCursorPosition(1, verticalPosition);
                Console.Write(new string(' ', Console.WindowWidth - 2));
                Console.SetCursorPosition(1, verticalPosition);
                Console.Write("->" + Path.GetFileName(current));
            }

            ConsoleKeyInfo navigationDirection = Console.ReadKey(true);
            while (navigationDirection.Key == ConsoleKey.UpArrow || navigationDirection.Key == ConsoleKey.DownArrow)
            {
                List<string> visibleValidFiles = ValidFilesToShow(validFiles, navigationDirection, ref startingLineIndex);

                if (navigationDirection.Key == ConsoleKey.UpArrow)
                {
                    if (fileCurrentLine < visibleValidFiles.Count - 1)
                    {
                        fileCurrentLine++;
                        verticalPosition--;
                    }

                    Consola.ShowValidResults(visibleValidFiles, validFiles.Count, search, totalNumberOFiles);
                    current = visibleValidFiles[fileCurrentLine];
                    Console.SetCursorPosition(1, verticalPosition);
                    Console.Write(new string(' ', Console.WindowWidth - 2));
                    Console.SetCursorPosition(1, verticalPosition);
                    Console.Write("->" + Path.GetFileName(current));
                }

                if (navigationDirection.Key == ConsoleKey.DownArrow)
                {
                    if (fileCurrentLine != 0)
                    {
                        verticalPosition++;
                        fileCurrentLine--;
                    }

                    Consola.ShowValidResults(visibleValidFiles, validFiles.Count, search, totalNumberOFiles);
                    current = visibleValidFiles[fileCurrentLine];
                    Console.SetCursorPosition(1, verticalPosition);
                    Console.Write(new string(' ', Console.WindowWidth - 2));
                    Console.SetCursorPosition(1, verticalPosition);
                    Console.Write("->" + Path.GetFileName(current));
                }

                navigationDirection = Console.ReadKey(true);
            }

            return (current, navigationDirection);
        }

        private static List<string> ValidFilesToShow(List<string> validFiles, ConsoleKeyInfo key, ref int startingLineIndex)
        {
            const int searchBarDim = 4;
            int lastPositionInConsole = Console.WindowHeight - (searchBarDim + 1);
            const int firstPositionInConsole = 1;
            int maxNumberOfLines = lastPositionInConsole;
            int count;
            int lastValidIndex = validFiles.Count - maxNumberOfLines;
            if (Console.CursorTop != firstPositionInConsole && Console.CursorTop != lastPositionInConsole)
            {
                count = Math.Min(maxNumberOfLines, validFiles.Count - startingLineIndex);
                return validFiles.GetRange(startingLineIndex, count);
            }

            if (key.Key == ConsoleKey.DownArrow)
            {
                if (startingLineIndex == 0 || Console.CursorTop == firstPositionInConsole)
                {
                    count = Math.Min(maxNumberOfLines, validFiles.Count - startingLineIndex);
                    return validFiles.GetRange(startingLineIndex, count);
                }

                startingLineIndex--;
                count = Math.Min(maxNumberOfLines, validFiles.Count - startingLineIndex);

                return validFiles.GetRange(startingLineIndex, count);
            }

            if (startingLineIndex + maxNumberOfLines == validFiles.Count)
            {
                count = Math.Min(maxNumberOfLines, validFiles.Count - startingLineIndex);
                return validFiles.GetRange(startingLineIndex, count);
            }

            if (startingLineIndex == lastValidIndex || Console.CursorTop == lastPositionInConsole)
            {
                count = Math.Min(maxNumberOfLines, validFiles.Count - startingLineIndex);
                return validFiles.GetRange(startingLineIndex, count);
            }

            startingLineIndex++;
            count = Math.Min(maxNumberOfLines, validFiles.Count - startingLineIndex);

            return validFiles.GetRange(startingLineIndex, count);
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