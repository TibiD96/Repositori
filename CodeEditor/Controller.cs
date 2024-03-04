using System;
using System.Collections.Generic;

namespace CodeEditor
{
    public class Controller
    {
        private static string? pathOfFile;

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
                        PathToFile();
                        if (pathOfFile != null)
                        {
                            Consola.ShowContentOfFile(pathOfFile);
                            NavigateInConsole(pathOfFile);
                        }

                        exitApp = true;
                        break;
                    case 2:
                        Consola.Menu();
                        break;
                }
            }
        }

        private static void NavigateInConsole(string pathOfFile)
        {
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
                            break;
                        }

                        Consola.MoveWindow(horizontalPosition, verticalPosition, arrowDirection, pathOfFile);

                        break;

                    case ConsoleKey.DownArrow:
                        if (verticalPosition + 1 == Console.WindowHeight)
                        {
                            Consola.MoveWindow(horizontalPosition, verticalPosition, arrowDirection, pathOfFile);
                            break;
                        }

                        verticalPosition++;

                        break;

                    case ConsoleKey.LeftArrow:
                        if (horizontalPosition > 0)
                        {
                            horizontalPosition--;
                            break;
                        }

                        Consola.MoveWindow(horizontalPosition, verticalPosition, arrowDirection, pathOfFile);

                        break;

                    case ConsoleKey.RightArrow:
                        if (horizontalPosition + 1 == Console.WindowWidth)
                        {
                            Consola.MoveWindow(horizontalPosition, verticalPosition, arrowDirection, pathOfFile);
                            break;
                        }

                        horizontalPosition++;

                        break;
                }

                Console.SetCursorPosition(Math.Min(horizontalPosition, Console.WindowWidth), Math.Min(verticalPosition, Console.WindowHeight));

                arrowDirection = Console.ReadKey(true);
            }

            Console.SetCursorPosition(0, Console.WindowWidth);
        }

        private static int ReadOption(int[] validOption)
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

        private static void PathToFile()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Give the path to the file like in the next example!\nExample:\nC:\\Users\\danit\\OneDrive\\Desktop\\Fisier.TXT");
            Console.ResetColor();
            pathOfFile = Console.ReadLine();

            if (File.Exists(pathOfFile))
            {
                return;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("File don't exist");
            Console.ResetColor();
            WrongPath();
        }

        private static void WrongPath()
        {
            Console.WriteLine("Have you added a wrong path?\nPress 1 for \"yes\" or 2 for \"no\"");
            int[] validOptions = new[] { 1, 2 };
            int answer = ReadOption(validOptions);

            if (answer != 1)
            {
                return;
            }

            PathToFile();
        }
    }
}
