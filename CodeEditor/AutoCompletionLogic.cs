using System;
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

            while (!Directory.Exists(search))
            {
                key = Console.ReadKey();
                search += key.KeyChar;
            }

            Consola.ClearConsole();
            Consola.SearchContour();
            Console.SetCursorPosition(1, Console.WindowHeight - 2);
            Console.Write(new string(' ', Console.WindowWidth - 2));
            Console.SetCursorPosition(1, 1);

            string[] filesFromDirectory = Directory.GetFiles(search);

        }

    }
}
