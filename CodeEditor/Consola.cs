using System;
using System.Collections.Generic;
using System.IO;

namespace CodeEditor
{
    public class Consola
    {
        public static void Menu()
        {
            Console.WriteLine("0. Exit application");
            Console.WriteLine("1. Show from file");
            Console.WriteLine("2. Show Menu");
        }

        public static void ShowContentOfFile()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Give the path to the file like in the next example!\nExample:\nC:\\Users\\danit\\OneDrive\\Desktop\\Fisier.TXT");
            Console.ResetColor();
            string fullPath = Console.ReadLine();
            if (File.Exists(fullPath))
            {
                Console.WriteLine();
                foreach (string line in File.ReadAllLines(fullPath))
                {
                    Console.WriteLine(line);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File don't exist");
                Console.ResetColor();
                WrongPath();
            }
        }

        private static void WrongPath()
        {
            Console.WriteLine("Have you added a wrong path?\nPress 1 for \"yes\" or 2 for \"no\"");
            int[] validOptions = new[] { 1, 2 };
            int answer = Controller.ReadOption(validOptions);

            if (answer != 1)
            {
                return;
            }

            ShowContentOfFile();
        }
    }
}
