using System;
using System.Collections.Generic;

namespace CodeEditor
{
    public class Controller
    {
        public static void RunMenu()
        {
            bool exitApp = false;
            while (!exitApp)
            {
                Consola.Menu();

                switch (ReadOption())
                {
                    case 0:
                        exitApp = true;
                        break;
                    case 1:
                        Consola.ShowContentOfFile();
                        exitApp = true;
                        break;
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
