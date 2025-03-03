using System;
using System.Runtime.InteropServices;

namespace CodeEditor
{
    public class AutoCompletionLogic
    {
        private static string lastValidDirect = Environment.CurrentDirectory;
        private static string search = "";
        private static int highlightIndex = -1;
        private static int startingIndex = 0;
        private static int completion = 0;
        private static bool quite = false;
        private static int startingCompletionContour = 4;
        private static int complitionContourHight = 7;

        public static (string, bool) AutoCompletion(ConsoleKeyInfo action = default)
        {
            (int, int) cursoPos = Console.GetCursorPosition();
            Console.SetCursorPosition(cursoPos.Item1 + search.Length, cursoPos.Item2);
            ConsoleKeyInfo key;
            quite = false;
            lastValidDirect = Environment.CurrentDirectory;
            int left = Console.CursorLeft;
            int commandArea = 2;

            List<string> allFiles;

            if (search == "")
            {
                allFiles = FilesFromDirectory(lastValidDirect);
            }
            else
            {
                allFiles = FilesFromDirectory(search);
            }

            Console.SetCursorPosition(cursoPos.Item1 + search.Length, cursoPos.Item2);

            if (action == default)
            {
                key = Console.ReadKey();
            }
            else
            {
                key = action;
            }

            while (key.Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (search.Length > 0)
                    {
                        search = search.Substring(0, search.Length - 1);

                        CheckIfIsEnoughSpace(cursoPos.Item1);

                        left = Console.CursorLeft;

                        allFiles.Clear();
                        allFiles = FilesFromDirectory(search);

                        Consola.ClearPartOfConsole(startingCompletionContour + complitionContourHight + 1, startingCompletionContour, 20, 1);

                        if (allFiles.Count < complitionContourHight)
                        {
                            complitionContourHight = allFiles.Count;
                        }

                        Consola.CompletionContour(complitionContourHight);

                        Consola.ShowDirectoryContent(allFiles.ToArray(), startingCompletionContour + complitionContourHight + 1, startingIndex, highlightIndex);
                        Console.SetCursorPosition(left, cursoPos.Item2);

                        if (search.Length == 0)
                        {
                            complitionContourHight = 7;
                            lastValidDirect = Environment.CurrentDirectory;
                            allFiles.Clear();
                            allFiles = FilesFromDirectory(lastValidDirect);

                            Consola.ClearPartOfConsole(startingCompletionContour + complitionContourHight + 1, startingCompletionContour, 20, 1);

                            if (allFiles.Count < complitionContourHight)
                            {
                                complitionContourHight = allFiles.Count;
                            }

                            Consola.CompletionContour(complitionContourHight);

                            Consola.ShowDirectoryContent(allFiles.ToArray(), startingCompletionContour + complitionContourHight + 1, startingIndex, highlightIndex);
                            Console.SetCursorPosition(left, cursoPos.Item2);
                        }
                    }
                    else
                    {
                        Consola.ClearPartOfConsole(Console.WindowHeight - 12, commandArea + 2, 20,1);
                        search = "";
                        lastValidDirect = search;
                        Config.TabCompletion = false;
                        return (search, false);
                    }
                }

                if (key.Key == ConsoleKey.Tab)
                {
                    Config.TabCompletion = true;
                    AutocomplitinChooser(allFiles, key.Modifiers);
                    CheckIfIsEnoughSpace(cursoPos.Item1);

                    left = Console.CursorLeft;

                    Console.SetCursorPosition(left, commandArea);
                }

                if (key.Key == ConsoleKey.Escape)
                {
                    Console.Write("1");
                    search = "";
                    lastValidDirect = search;
                    quite = true;
                    return ("", quite);
                }

                if (key.Key != ConsoleKey.Tab && key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    complitionContourHight = 7;
                    CheckIfIsEnoughSpace(cursoPos.Item1);

                    allFiles.Clear();
                    allFiles = FilesFromDirectory(search);

                    Consola.ClearPartOfConsole(startingCompletionContour + complitionContourHight + 1, startingCompletionContour, 20, 1);

                    if (allFiles.Count < complitionContourHight)
                    {
                        complitionContourHight = allFiles.Count;
                    }

                    Consola.CompletionContour(complitionContourHight);

                    Consola.ShowDirectoryContent(allFiles.ToArray(), startingCompletionContour + complitionContourHight + 1, startingIndex, highlightIndex);

                    search += key.KeyChar;

                    CheckIfIsEnoughSpace(cursoPos.Item1);

                    left = Console.CursorLeft;

                    allFiles.Clear();
                    allFiles = FilesFromDirectory(search);

                    Consola.ClearPartOfConsole(startingCompletionContour + complitionContourHight + 1, startingCompletionContour, 20, 1);

                    if (allFiles.Count < complitionContourHight)
                    {
                        complitionContourHight = allFiles.Count;
                    }

                    Consola.CompletionContour(complitionContourHight);

                    Consola.ShowDirectoryContent(allFiles.ToArray(), startingCompletionContour + complitionContourHight + 1, startingIndex, highlightIndex);

                    Console.SetCursorPosition(left, cursoPos.Item2);
                }

                key = Console.ReadKey();

            }

            if (!File.Exists(search))
            {
                allFiles.Clear();
                allFiles = FilesFromDirectory(search);

                if (Config.TabCompletion)
                {
                    Consola.ClearPartOfConsole(startingCompletionContour + complitionContourHight + 1, startingCompletionContour, 20, 1);

                    if (allFiles.Count < complitionContourHight)
                    {
                        complitionContourHight = allFiles.Count;
                    }

                    Consola.CompletionContour(complitionContourHight);
                }

                Consola.ShowDirectoryContent(allFiles.ToArray(), startingCompletionContour + complitionContourHight + 1, startingIndex, highlightIndex);
                Console.SetCursorPosition(left, cursoPos.Item2);

                Console.SetCursorPosition(cursoPos.Item1, cursoPos.Item2);
                AutoCompletion();
            }
            else
            {
                lastValidDirect = search;
                search = "";
            }
            return (lastValidDirect, quite);

        }

        public static List<string> FilesFromDirectory(string search)
        {
            List<string> files = new List<string>();
            if (Directory.Exists(search))
            {
                lastValidDirect = search;
                files.AddRange(Directory.GetDirectories(search));
                files.AddRange(Directory.GetFiles(search));
                highlightIndex = -1;
                startingIndex = 0;
                completion = 0;
            }
            else if (lastValidDirect != "")
            {
                files.AddRange(Directory.GetDirectories(lastValidDirect).Where(dir => dir.StartsWith(search)));
                files.AddRange(Directory.GetFiles(lastValidDirect).Where(file => file.StartsWith(search)));
                files.AddRange(Directory.GetFiles(lastValidDirect).Where(file => Path.GetFileName(file).StartsWith(search)));
                highlightIndex = -1;
                startingIndex = 0;
                completion = 0;
            }

            return files;
        }

        private static void Completion(List<string> allFiles)
            {
            if (lastValidDirect == Environment.CurrentDirectory)
            {
                if (completion > 0)
                {
                    search = Path.GetFileName(allFiles[completion - 1]);
                }
                else
                {
                    search = Path.GetFileName(allFiles[completion]);
                }
            }
            else
            {
                if (completion > 0)
                {
                    search = allFiles[completion - 1];
                }
                else
                {
                    search = allFiles[completion];
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Directory.Exists(search))
                {
                    search += '\\';
                }
                else if (Directory.Exists(search))
                {
                    search += '/';
                }
            }
        }

        private static void CheckIfIsEnoughSpace(int startingPosition)
        {
            int emptySpace = (Console.WindowWidth - 21) - (startingPosition + search.Length);
            const int commandArea = 2;

            Console.SetCursorPosition(startingPosition, commandArea);
            Console.Write(new string(' ', ((Console.WindowWidth - 20) - startingPosition)));
            Console.SetCursorPosition(startingPosition, commandArea);

            if (emptySpace >= 0)
            {
                Console.Write(search);
            }
            else
            {
                Console.Write(search[(emptySpace * -1)..]);
            }
        }

        private static void AutocomplitinChooser(List<string> allFiles, ConsoleModifiers modifier)
        {
            const int left = 21;
            const int commandArea = 2;

            if (allFiles.Count < complitionContourHight)
            {
                complitionContourHight = allFiles.Count;
            }

            Consola.ClearPartOfConsole(startingCompletionContour + complitionContourHight + 1, startingCompletionContour, left - 1, 1);
            Console.SetCursorPosition(left, commandArea);

            Consola.CompletionContour(complitionContourHight);
            Console.SetCursorPosition(left, startingCompletionContour + 1);

            if (modifier != ConsoleModifiers.Shift)
            {
                if (highlightIndex < allFiles.Count)
                {
                    highlightIndex++;
                    completion++;
                }

                if (highlightIndex == complitionContourHight && allFiles.Count > highlightIndex)
                {
                    highlightIndex--;
                    startingIndex++;
                }
            }
            else
            {
                if (highlightIndex > 0)
                {
                    highlightIndex--;
                    completion--;
                }
                else 
                {
                    if (highlightIndex == 0 && completion > highlightIndex && completion > 1)
                    {
                        completion--;
                        startingIndex--;
                    }
                }
            }

            if (completion > allFiles.Count)
            {
                highlightIndex = 0;
                startingIndex = 0;
                completion = 0;

                Completion(allFiles);

                Consola.ShowDirectoryContent(allFiles.ToArray(), startingCompletionContour + complitionContourHight + 1, startingIndex, highlightIndex);

                completion = 1;
            }
            else
            {
                Completion(allFiles);

                Consola.ShowDirectoryContent(allFiles.ToArray(), startingCompletionContour + complitionContourHight + 1, startingIndex, highlightIndex);
            }

            complitionContourHight = 7;
        }

    }
}
