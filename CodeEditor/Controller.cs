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
                        string fullPath = PathToFile();
                        if (fullPath != "")
                        {
                            string[] lines = File.ReadAllLines(fullPath);
                            Consola.ShowContentOfFile(lines);
                            NavigateInConsole(lines);
                        }

                        exitApp = true;
                        break;
                    case 2:
                        Consola.Menu();
                        break;
                }
            }
        }

        private static void NavigateInConsole(string[] lines)
        {
            int startingLine = 0;
            int startingColumn = 0;
            int lineCounting = Console.CursorTop;
            int verticalPosition = Console.CursorTop;
            int horizontalPosition = Console.CursorLeft;
            ConsoleKeyInfo arrowDirection = Console.ReadKey(true);
            while (arrowDirection.Key != ConsoleKey.Escape)
            {
                switch (arrowDirection.Key)
                {
                    case ConsoleKey.UpArrow:

                        NavigateUp(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, startingColumn, lines);

                        break;

                    case ConsoleKey.DownArrow:

                        NavigateDown(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, startingColumn, lines);

                        break;

                    case ConsoleKey.LeftArrow:

                        NavigateLeft(ref lineCounting, ref horizontalPosition, startingLine, ref startingColumn, lines);

                        break;

                    case ConsoleKey.RightArrow:

                        NavigateRight(ref lineCounting, verticalPosition, ref horizontalPosition, startingLine, ref startingColumn, lines);

                        break;
                }

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

        private static string PathToFile()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Give the path to the file like in the next example!\nExample:\nC:\\Users\\danit\\OneDrive\\Desktop\\Fisier.TXT");
            Console.ResetColor();
            int[] validOptions = new[] { 1, 2 };
            string? pathOfFile = Console.ReadLine();
            int answer;

            if (File.Exists(pathOfFile))
            {
                return pathOfFile;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("File don't exist");
            Console.ResetColor();
            Console.WriteLine("Have you added a wrong path?\nPress 1 for \"yes\" or 2 for \"no\"");
            answer = ReadOption(validOptions);
            return answer == 1 ? PathToFile() : "";
        }

        private static void NavigateUp(ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, int startingColumn, string[] lines)
        {
            int currentStartColumn;
            int currentEndColumn;

            if (lineCounting != 0)
            {
                lineCounting--;
            }

            currentStartColumn = Math.Max(0, Math.Min(startingColumn, lines[lineCounting].Length));
            currentEndColumn = lines[lineCounting].Length - currentStartColumn <= Console.WindowWidth ? lines[lineCounting].Length - currentStartColumn : Console.WindowWidth;

            if (verticalPosition == 0 && startingLine != 0)
            {
                startingLine--;
                Consola.ShowContentOfFile(lines, startingLine, startingColumn);
            }
            else if (verticalPosition > 0)
            {
                verticalPosition--;
            }

            Console.SetCursorPosition(horizontalPosition > currentEndColumn ? currentEndColumn : horizontalPosition, verticalPosition);
        }

        private static void NavigateDown(ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, int startingColumn, string[] lines)
        {
            int currentStartColumn;
            int currentEndColumn;

            if (lineCounting < lines.Length - 1)
            {
                lineCounting++;
            }

            currentStartColumn = Math.Max(0, Math.Min(startingColumn, lines[lineCounting].Length));
            currentEndColumn = lines[lineCounting].Length - currentStartColumn <= Console.WindowWidth ? lines[lineCounting].Length - currentStartColumn : Console.WindowWidth;

            if (verticalPosition + 1 == Console.WindowHeight)
            {
                startingLine++;
                if (startingLine > lines.Length - Console.WindowHeight)
                {
                    startingLine--;
                }
                else
                {
                    Consola.ShowContentOfFile(lines, startingLine, startingColumn);
                }
            }
            else
            {
                verticalPosition++;
            }

            Console.SetCursorPosition(horizontalPosition > currentEndColumn ? currentEndColumn : horizontalPosition, verticalPosition);
        }

        private static void NavigateLeft(ref int lineCounting, ref int horizontalPosition, int startingLine, ref int startingColumn, string[] lines)
        {
            if (horizontalPosition == 0 && startingColumn != 0)
            {
                startingColumn--;
                Consola.ShowContentOfFile(lines, startingLine, startingColumn);
            }
            else if (horizontalPosition > 0)
            {
                horizontalPosition--;
            }
        }

        private static void NavigateRight(ref int lineCounting, int verticalPosition, ref int horizontalPosition, int startingLine, ref int startingColumn, string[] lines)
        {
            if (horizontalPosition + 1 == Console.WindowWidth)
            {
                startingColumn++;
                if (startingColumn > lines[verticalPosition].Length - Console.WindowWidth)
                {
                    startingColumn--;
                }
                else
                {
                    Consola.ShowContentOfFile(lines, startingLine, startingColumn);
                }
            }
            else
            {
                horizontalPosition++;
            }
        }
    }
}