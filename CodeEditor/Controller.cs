using System;
using System.Collections.Generic;

namespace CodeEditor
{
    public class Controller
    {
        public static void RunMenu()
        {
            int[] validOptions = new[] { 0, 1, 2 };
            Consola.Menu();
            int option = ReadOption(validOptions);
            bool exitApp = false;
            while (!exitApp)
            {
                switch (option)
                {
                    case 0:
                            exitApp = true;
                            break;
                    case 1:
                            Consola.ShowContentOfFile();
                            NavigateInConsole();
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

        public static void NavigateInConsole()
        {
            int upperPosition = 0;
            ConsoleKeyInfo arrowDirection = Console.ReadKey(true);
            while (arrowDirection.Key != ConsoleKey.Escape)
            {
                if (arrowDirection.Key == ConsoleKey.UpArrow && upperPosition > 0)
                {
                    upperPosition--;
                    Console.SetWindowPosition(0, upperPosition);
                }
                else if (arrowDirection.Key == ConsoleKey.DownArrow && upperPosition < Console.WindowHeight - 1)
                {
                    upperPosition++;
                    Console.SetWindowPosition(0, upperPosition);
                }

                arrowDirection = Console.ReadKey(true);
            }
        }
    }
}
