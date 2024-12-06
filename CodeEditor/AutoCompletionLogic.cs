using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CodeEditor
{
    public class AutoCompletionLogic
    {
        public static void AutoCompletion()
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

            while (!Directory.Exists(search) && !File.Exists(search))
            {
                key = Console.ReadKey();

                if (key.Key == ConsoleKey.Backspace && search.Length > 0)
                {
                    search = search.Substring(0, search.Length - 1);
                    Console.SetCursorPosition(1, Console.WindowHeight - 2);
                    Console.Write(new string(' ', Console.WindowWidth - 2));
                    Console.SetCursorPosition(1, Console.WindowHeight - 2);
                    Console.Write(search);
                }
                else
                {
                    search += key.KeyChar;
                }
            }

            search =  search + '\\';

            Console.SetCursorPosition(1, Console.WindowHeight - 2);

            Console.Write(search);

            Console.SetCursorPosition(search.Length + 1, Console.WindowHeight - 2);

            allFiles.AddRange(Directory.GetDirectories(search));
            allFiles.AddRange(Directory.GetFiles(search));

            Consola.ShowValidResults(allFiles, allFiles.Count, "", allFiles.ToArray());


            while (key.Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && search.Length > 0)
                {
                    search = search.Substring(0, search.Length - 1);
                    Console.SetCursorPosition(1, Console.WindowHeight - 2);
                    Console.Write(new string(' ', Console.WindowWidth - 2));
                    Console.SetCursorPosition(1, Console.WindowHeight - 2);
                    Console.Write(search);
                }
                else if (key.Key == ConsoleKey.Tab)
                {

                }

                Console.SetCursorPosition(search.Length + 1, Console.WindowHeight - 2);
                key = Console.ReadKey();
                search += key.KeyChar;
            }

        }

        private static void GetAllFiles(ref List<string> allFiles, string directory)
        {
            allFiles.AddRange(Directory.GetFiles(directory));

            foreach (string subDirectory in Directory.GetDirectories(directory))
            {
                GetAllFiles(ref allFiles, subDirectory);
            }
        }

    }
}
