namespace CodeEditor
{
    public class FuzzySearchLogic
    {
        public static string FuzzySearch(string[] filesFromDirectory, string[] allFiles)
        {
            ConsoleKeyInfo key;
            List<string> listOfValidFiles = new List<string>(filesFromDirectory);
            string search = "";
            (string, ConsoleKeyInfo) navigationResult;

            Consola.ClearConsole();
            Consola.SearchContour();
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
    }
}
