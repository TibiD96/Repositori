using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace CodeEditor
{
    public class CommadnModeAutoCompletion
    {
        private static int highlightPoint = 0;
        private static int curentCompletion = 0;
        public static string AutoCompletion(string command, ConsoleKeyInfo key)
        {
            highlightPoint = 0;
            curentCompletion = 0;
            (int, int) cursoPos = Console.GetCursorPosition();
            List<string> validCommands = Config.Commands.Where(value => value.StartsWith(command)).ToList();
            Console.SetCursorPosition(cursoPos.Item1, cursoPos.Item2);

            AutocomplitinChooser(validCommands, key.Modifiers, ref command, Console.CursorLeft);

            Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight - 11);

            return command;

        }

        private static void AutocomplitinChooser(List<string> commands, ConsoleModifiers modifier, ref string command, int left)
        {
            int commandArea = 2;
            ConsoleKeyInfo key;
            for (int i = 0; i < commands.Count; i++)
            {
                if (i == highlightPoint)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                }

                Console.Write(commands[i]);
                Console.ResetColor();
                Console.SetCursorPosition(left, Console.CursorTop + 1);
            }

            Console.SetCursorPosition(Console.CursorLeft, 2);
            Console.Write(command);
            key = Console.ReadKey();

            while (key.Key == ConsoleKey.Tab)
            {
                Consola.ClearTabCompletion(commands.Count + 1);
                if (modifier != ConsoleModifiers.Shift)
                {
                    if (highlightPoint < commands.Count)
                    {
                        highlightPoint++;
                        curentCompletion++;
                    }

                    if (highlightPoint == 7 && commands.Count > highlightPoint)
                    {
                        highlightPoint--;
                    }
                }
                else
                {
                    if (highlightPoint > 0)
                    {
                        highlightPoint--;
                        curentCompletion--;
                    }

                    else
                    {
                        if (highlightPoint == 0 && curentCompletion > highlightPoint)
                        {
                            curentCompletion--;
                        }
                    }
                }

                if (curentCompletion == commands.Count)
                {
                    highlightPoint = 0;
                    curentCompletion = 0;
                }

                for (int i = 0; i < commands.Count; i++)
                {
                    if (i == highlightPoint)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                    }

                    Console.Write(commands[i]);
                    Console.ResetColor();
                    Console.SetCursorPosition(left, Console.CursorTop + 1);
                }

                Console.SetCursorPosition(Console.CursorLeft, 2);
                Console.Write(command);
                key = Console.ReadKey();
            }

            if (char.IsLetter((char)key.Key) || char.IsDigit((char)key.Key))
            {
                command += key.KeyChar;
            }
            
            if (command.Length > 0 && key.Key == ConsoleKey.Backspace)
            {
                command = command.Substring(0, command.Length - 1);
                if (command == "")
                {
                    Consola.ClearPartOfConsole(commandArea);
                    Console.SetCursorPosition(Console.CursorLeft, commandArea);
                }
            }

            if (key.Key == ConsoleKey.Enter)
            {
                command = commands[curentCompletion];
            }
        }
    }
}
