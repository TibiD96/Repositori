using System;
using System.Collections.Generic;

namespace CodeEditor
{
    public class Controller
    {
        private static readonly int[] ValidOptions = new[] { 0, 1, 2 };

        public static void RunMenu()
        {
            Consola.Menu();
            bool exitApp = false;
            while (!exitApp)
            {
                int option = ReadOption();

                if (ValidOptions.Contains(option))
                {
                    switch (option)
                    {
                        case 0:
                            exitApp = true;
                            break;
                        case 1:
                            Consola.ShowContentOfFile();
                            exitApp = true;
                            break;
                        case 2:
                            Consola.Menu();
                            break;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Choose a valid option");
                    Console.ResetColor();
                }
            }
        }

        private static int ReadOption()
        {
            try
            {
                return Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please insert an integer");
                Console.ResetColor();
                return ReadOption();
            }
        }
    }
}
