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

        public static (string, bool) AutoCompletion()
        {
            (int, int) cursoPos = Console.GetCursorPosition();
            Console.SetCursorPosition(cursoPos.Item1 + search.Length, cursoPos.Item2);
            int left = 0;

            ConsoleKeyInfo key;
            key = Console.ReadKey();

            if (key.Key != ConsoleKey.Enter)
            {
                search += key.KeyChar;
            }

            List<string> allFiles = new List<string>();

            allFiles = FilesFromDirectory(search, allFiles);

            Consola.ClearPartOfConsole(Console.WindowHeight - 12);
            Consola.ShowDirectoryContent(allFiles.ToArray());
            Console.SetCursorPosition(cursoPos.Item1 + search.Length, cursoPos.Item2);

            while (key.Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && search.Length > 0)
                {
                    search = search.Substring(0, search.Length - 1);

                    CheckIfIsEnoughSpace(cursoPos.Item1, search);

                    left = Console.CursorLeft;

                    allFiles.Clear();
                    allFiles = FilesFromDirectory(search, allFiles);
                    Consola.ShowDirectoryContent(allFiles.ToArray());
                    Console.SetCursorPosition(left, Console.WindowHeight - 11);

                    if (search.Length == 0)
                    {
                        lastValidDirect = Environment.CurrentDirectory;
                    }
                }

                if (key.Key == ConsoleKey.Tab)
                {
                    search = Completion(allFiles, search);

                    CheckIfIsEnoughSpace(cursoPos.Item1,search);

                    left = Console.CursorLeft;

                    allFiles.Clear();
                    allFiles = FilesFromDirectory(search, allFiles);

                    Consola.ShowDirectoryContent(allFiles.ToArray());
                    Console.SetCursorPosition(left, Console.WindowHeight - 11);
                }

                key = Console.ReadKey();

                if (key.Key != ConsoleKey.Tab && key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    search += key.KeyChar;

                    CheckIfIsEnoughSpace(cursoPos.Item1, search);

                    left = Console.CursorLeft;
                    
                    allFiles.Clear();
                    allFiles = FilesFromDirectory(search, allFiles);

                    Consola.ShowDirectoryContent(allFiles.ToArray());
                    Console.SetCursorPosition(left, cursoPos.Item2);
                }

                if (key.Key == ConsoleKey.Backspace && search.Length == 0)
                {
                    Consola.ClearPartOfConsole(Console.WindowHeight - 12);
                    return (search, false);
                }

                if (key.Key == ConsoleKey.Escape)
                {
                    return ("", true);
                }
            }

            if (!File.Exists(search))
            {
                Console.SetCursorPosition(cursoPos.Item1, cursoPos.Item2);
                AutoCompletion();
            }

            return (search, false);

        }

        private static string Completion(List<string> allFiles, string search)
        {
            int posibilities = 0;
            int indexOfCompletion = 0;

            for (int i = 0; i < allFiles.Count; i++)
            {
                if (Path.GetFileName(allFiles[i]).StartsWith(search) || allFiles[i].StartsWith(search))
                {
                    posibilities++;
                    indexOfCompletion = i;
                }
            }

            if (posibilities == 1)
            {

                if (lastValidDirect == Environment.CurrentDirectory)
                {
                    search = Path.GetFileName(allFiles[indexOfCompletion]);
                }
                else
                {
                    search = allFiles[indexOfCompletion];

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        search += '\\';
                    }
                    else
                    {
                        search += '/';
                    }
                }

                Consola.ShowDirectoryContent(FilesFromDirectory(allFiles[indexOfCompletion], allFiles).ToArray());
                return search;
            }

            return search;
        }

        private static List<string> FilesFromDirectory(string search, List<string> allFiles)
        {
            if (Directory.Exists(search))
            {
                lastValidDirect = search;
                allFiles.AddRange(Directory.GetDirectories(search));
                allFiles.AddRange(Directory.GetFiles(search));
            }
            else if (lastValidDirect != "")
            {
                allFiles.AddRange(Directory.GetDirectories(lastValidDirect).Where(dir => dir.StartsWith(search)));
                allFiles.AddRange(Directory.GetFiles(lastValidDirect).Where(file => file.StartsWith(search)));
            }

            return allFiles;
        }

        private static void CheckIfIsEnoughSpace(int startingPosition, string search)
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

    }
}
