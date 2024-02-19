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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press \"escape\" when you finish navigating");
            Console.ResetColor();
            int originalVerticalPosition = Console.CursorTop;
            int originalHorizontalPosition = Console.CursorLeft;
            int verticalPosition = Console.CursorTop;
            int horizontalPosition = Console.CursorLeft;
            ConsoleKeyInfo arrowDirection = Console.ReadKey(true);
            while (arrowDirection.Key != ConsoleKey.Escape)
            {
                switch (arrowDirection.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (verticalPosition > 0)
                        {
                            verticalPosition--;
                        }

                        break;

                    case ConsoleKey.DownArrow:
                        verticalPosition++;
                        break;

                    case ConsoleKey.LeftArrow:
                        if (horizontalPosition > 0)
                        {
                            horizontalPosition--;
                        }

                        break;

                    case ConsoleKey.RightArrow:
                        if (horizontalPosition < 120)
                        {
                            horizontalPosition++;
                        }

                        break;
                }

                Console.SetCursorPosition(horizontalPosition, verticalPosition);
                arrowDirection = Console.ReadKey(true);
            }

            Console.SetCursorPosition(originalHorizontalPosition, originalVerticalPosition);
        }
    }
}
