using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeEditor
{
    public class CommadnModeAutoCompletion
    {
        private static int highlightPoint = 0;
        private static int startingPoint = 0;
        private static int curentCompletion = 0;
        public static string AutoCompletion(string command, ConsoleKeyInfo key)
        {
            (int, int) cursoPos = Console.GetCursorPosition();
            Console.SetCursorPosition(cursoPos.Item1 + command.Length, cursoPos.Item2);

            int left;

            Consola.ClearPartOfConsole(Console.WindowHeight - 12);
            Consola.ShowDirectoryContent([.. Config.Commands], startingPoint, highlightPoint);
            Console.SetCursorPosition(cursoPos.Item1 + command.Length, cursoPos.Item2);

            AutocomplitinChooser(Config.Commands, key.Modifiers, ref command);

            left = Console.CursorLeft;

            Console.SetCursorPosition(left, Console.WindowHeight - 11);

            return command;

        }

        private static void AutocomplitinChooser(List<string> commands, ConsoleModifiers modifier, ref string command)
        {
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
                    startingPoint++;
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
                        startingPoint--;
                        curentCompletion--;
                    }
                }
            }

            if (curentCompletion == commands.Count)
            {
                highlightPoint = 0;
                startingPoint = 0;
                curentCompletion = 0;
            }

            command = commands[curentCompletion];

            Consola.ShowDirectoryContent([.. commands], startingPoint, highlightPoint);
        }
    }
}
