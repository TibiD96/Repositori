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
            bool exitApp = false;
            while (!exitApp)
            {
                switch (ReadOption(validOptions))
                {
                    case 0:
                            exitApp = true;
                            break;
                    case 1:
                            Consola.ShowContentOfFile();
                            NavigateInConsole();
                            Consola.Menu();
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
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press \"escape\" when you finish navigating");
            Console.ResetColor();
            int originalPosition = Console.CursorTop;
            int position = Console.CursorTop;
            ConsoleKeyInfo arrowDirection = Console.ReadKey(true);
            while (arrowDirection.Key != ConsoleKey.Escape)
            {
                if (arrowDirection.Key == ConsoleKey.UpArrow && position > 0)
                {
                    position = Console.WindowTop;

                    if (position >= 1)
                    {
                        position--;
                    }
                }

                if (arrowDirection.Key == ConsoleKey.DownArrow)
                {
                    if (position == Console.WindowTop)
                    {
                        position = Console.CursorTop + Console.WindowHeight - 1;
                    }

                    if (position < originalPosition)
                    {
                        position++;
                    }
                }

                Console.SetCursorPosition(0, position);
                arrowDirection = Console.ReadKey(true);
            }

            Console.CursorVisible = true;
            Console.SetCursorPosition(0, originalPosition);
        }
    }
}
