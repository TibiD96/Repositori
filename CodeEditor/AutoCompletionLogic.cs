using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CodeEditor
{
    public class AutoCompletionLogic
    {
        private static string lastValidDirect = Environment.CurrentDirectory;
        private static string search = "";
        private static int highlightIndex = 0;
        private static int startingIndex = 0;
        private static int completion = 0;
        private static bool quite = false;

        public static (string, bool) AutoCompletion(ConsoleKeyInfo action = default)
        {
            (int, int) cursoPos = Console.GetCursorPosition();
            Console.SetCursorPosition(cursoPos.Item1 + search.Length, cursoPos.Item2);
            ConsoleKeyInfo key;
            quite = false;
            lastValidDirect = Environment.CurrentDirectory;
            int left = Console.CursorLeft;

            List<string> allFiles;

            if (search == "")
            {
                allFiles = FilesFromDirectory(lastValidDirect);
            }
            else
            {
                allFiles = FilesFromDirectory(search);
            }

            Consola.ClearPartOfConsole(Console.WindowHeight - 12);
            Consola.ShowDirectoryContent(allFiles.ToArray(), startingIndex, highlightIndex);
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
                        Consola.ShowDirectoryContent(allFiles.ToArray(), startingIndex, highlightIndex);
                        Console.SetCursorPosition(left, Console.WindowHeight - 11);

                        if (search.Length == 0)
                        {
                            lastValidDirect = Environment.CurrentDirectory;
                            allFiles.Clear();
                            allFiles = FilesFromDirectory(lastValidDirect);
                            Consola.ShowDirectoryContent(allFiles.ToArray(), startingIndex, highlightIndex);
                            Console.SetCursorPosition(left, Console.WindowHeight - 11);
                        }
                    }
                    else
                    {
                        Consola.ClearPartOfConsole(Console.WindowHeight - 12);
                        search = "";
                        lastValidDirect = search;
                        return (search, false);
                    }
                }

                if (key.Key == ConsoleKey.Tab)
                {
                    AutocomplitinChooser(allFiles, key.Modifiers);
                    CheckIfIsEnoughSpace(cursoPos.Item1);

                    left = Console.CursorLeft;

                    Console.SetCursorPosition(left, Console.WindowHeight - 11);
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
                    CheckIfIsEnoughSpace(cursoPos.Item1);

                    allFiles.Clear();
                    allFiles = FilesFromDirectory(search);

                    Consola.ShowDirectoryContent(allFiles.ToArray(), startingIndex, highlightIndex);

                    search += key.KeyChar;

                    CheckIfIsEnoughSpace(cursoPos.Item1);

                    left = Console.CursorLeft;

                    allFiles.Clear();
                    allFiles = FilesFromDirectory(search);

                    Consola.ShowDirectoryContent(allFiles.ToArray(), startingIndex, highlightIndex);

                    Console.SetCursorPosition(left, cursoPos.Item2);
                }

                key = Console.ReadKey();

            }

            if (!File.Exists(search))
            {
                allFiles.Clear();
                allFiles = FilesFromDirectory(search);

                Consola.ShowDirectoryContent(allFiles.ToArray(), startingIndex, highlightIndex);
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
                highlightIndex = 0;
                startingIndex = 0;
                completion = 0;
            }
            else if (lastValidDirect != "")
            {
                files.AddRange(Directory.GetDirectories(lastValidDirect).Where(dir => dir.StartsWith(search)));
                files.AddRange(Directory.GetFiles(lastValidDirect).Where(file => file.StartsWith(search)));
                files.AddRange(Directory.GetFiles(lastValidDirect).Where(file => Path.GetFileName(file).StartsWith(search)));
                highlightIndex = 0;
                startingIndex = 0;
                completion = 0;
            }

            return files;
        }

        private static void Completion(List<string> allFiles)
            {
            if (lastValidDirect == Environment.CurrentDirectory)
            {
                search = Path.GetFileName(allFiles[completion]);
            }
            else
            {
                search = allFiles[completion];

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Directory.Exists(search))
                {
                    search += '\\';
                }
                else if (Directory.Exists(search))
                {
                    search += '/';
                }
            }

            Consola.ShowDirectoryContent(allFiles.ToArray());
        }

        private static void CheckIfIsEnoughSpace(int startingPosition)
        {
            int emptySpace = (Console.WindowWidth - 21) - (startingPosition + search.Length);

            Console.SetCursorPosition(startingPosition, Console.WindowHeight - 11);
            Console.Write(new string(' ', ((Console.WindowWidth - 20) - startingPosition)));
            Console.SetCursorPosition(startingPosition, Console.WindowHeight - 11);

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
            if (modifier != ConsoleModifiers.Shift)
            {
                if (highlightIndex < allFiles.Count)
                {
                    highlightIndex++;
                    completion++;
                }

                if (highlightIndex == 7 && allFiles.Count > highlightIndex)
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
                    if (highlightIndex == 0 && completion > highlightIndex)
                    {
                        startingIndex--;
                        completion--;
                    }
                }
            }

            if (completion == allFiles.Count)
            {
                highlightIndex = 0;
                startingIndex = 0;
                completion = 0;
            }

            Completion(allFiles);

            Consola.ShowDirectoryContent(allFiles.ToArray(), startingIndex, highlightIndex);
        }

    }
}
