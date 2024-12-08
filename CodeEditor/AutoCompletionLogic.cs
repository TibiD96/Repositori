using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CodeEditor
{
    public class AutoCompletionLogic
    {
        private static string lastValidDirect = "";

        public static string AutoCompletion()
        {
            Consola.ClearConsole();
            Consola.SearchContour();
            Console.SetCursorPosition(1, Console.WindowHeight - 2);
            Console.Write(new string(' ', Console.WindowWidth - 2));
            Console.SetCursorPosition(1, Console.WindowHeight - 2);

            ConsoleKeyInfo key;
            string search = "";
            key = Console.ReadKey();
            search += key.KeyChar;
            List<string> allFiles = new List<string>();

            allFiles = FilesFromDirectory(search, allFiles);

            Consola.ShowDirectoryContent(allFiles.ToArray());
            Console.SetCursorPosition(search.Length + 1, Console.WindowHeight - 2);

            while (key.Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && search.Length > 0)
                {
                    search = search.Substring(0, search.Length - 1);
                    Console.SetCursorPosition(1, Console.WindowHeight - 2);
                    Console.Write(new string(' ', Console.WindowWidth - 2));
                    Console.SetCursorPosition(1, Console.WindowHeight - 2);
                    Console.Write(search);

                    allFiles.Clear();
                    allFiles = FilesFromDirectory(search, allFiles);
                    Consola.ShowDirectoryContent(allFiles.ToArray());
                    Console.SetCursorPosition(search.Length + 1, Console.WindowHeight - 2);
                }

                if (key.Key == ConsoleKey.Tab)
                {
                    search = Completion(allFiles, search);

                    Console.SetCursorPosition(1, Console.WindowHeight - 2);
                    Console.Write(new string(' ', Console.WindowWidth - 2));
                    Console.SetCursorPosition(1, Console.WindowHeight - 2);
                    Console.Write(search);

                    allFiles.Clear();
                    allFiles = FilesFromDirectory(search, allFiles);

                    Consola.ShowDirectoryContent(allFiles.ToArray());
                    Console.SetCursorPosition(search.Length + 1, Console.WindowHeight - 2);
                }

                key = Console.ReadKey();

                if (key.Key != ConsoleKey.Tab && key.Key != ConsoleKey.Backspace)
                {
                    search += key.KeyChar;

                    allFiles.Clear();
                    allFiles = FilesFromDirectory(search, allFiles);

                    Consola.ShowDirectoryContent(allFiles.ToArray());
                    Console.SetCursorPosition(search.Length + 1, Console.WindowHeight - 2);
                }
            }

            return '"' + search + '"';

        }

        private static string Completion(List<string> allFiles, string search)
        {
            int posibilities = 0;
            int indexOfCompletion = 0;

            for (int i = 0; i < allFiles.Count; i++)
            {
                if (allFiles[i].StartsWith(search))
                {
                    posibilities++;
                    indexOfCompletion = i;
                }
            }

            if (posibilities == 1)
            {
                Consola.ShowDirectoryContent(FilesFromDirectory(allFiles[indexOfCompletion], allFiles).ToArray());
                return allFiles[indexOfCompletion];
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
                allFiles.AddRange(Directory.GetDirectories(lastValidDirect));
                allFiles.AddRange(Directory.GetFiles(lastValidDirect));
            }

            return allFiles;
        }

    }
}
