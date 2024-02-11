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
                switch (ReadOption(ValidOptions))
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
        }

        public static int ReadOption(int[] validOption)
        {
            try
            {
                int answer = Convert.ToInt32(Console.ReadLine());
                if (validOption.Contains(answer))
                {
                    return answer;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Choose a valid option");
                Console.ResetColor();

                return ReadOption(validOption);
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Choose a valid option");
                Console.ResetColor();
                return ReadOption(validOption);
            }
        }
    }
}
