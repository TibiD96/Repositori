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

                        NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);

                        break;

                    case ConsoleKey.DownArrow:

                        NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);

                        break;

                    case ConsoleKey.LeftArrow:

                        NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);

                        break;

                    case ConsoleKey.RightArrow:

                        NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);

                        break;

                    case ConsoleKey.End:

                        EndButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, lines);

                        break;

                    case ConsoleKey.Home:

                        HomeButtonBehaviour(ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, lines);

                        break;

                    case ConsoleKey.PageDown:

                        PageDownBehaviour(ref lineCounting, horizontalPosition, verticalPosition, ref startingLine, ref startingColumn, lines);

                        break;

                    case ConsoleKey.PageUp:

                        PageUpBehaviour(ref lineCounting, horizontalPosition, verticalPosition, ref startingLine, ref startingColumn, lines);

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

        private static void NavigateUp(ref int lineCounting, int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
        {
            if (lineCounting == 0)
            {
                return;
            }

            lineCounting--;
            string lineIndex = Convert.ToString(lines.Length - 1) + " ";
            int currentStartColumn = Math.Max(0, Math.Min(startingColumn, lines[lineCounting].Length));
            int currentEndColumn = lines[lineCounting].Length - currentStartColumn < Console.WindowWidth ? lines[lineCounting].Length - currentStartColumn : Console.WindowWidth - 1;

            if (currentStartColumn < startingColumn)
            {
                startingColumn = currentStartColumn;
            }

            if (verticalPosition == 0 && startingLine != 0)
            {
                startingLine--;
            }
            else if (verticalPosition > 0)
            {
                verticalPosition--;
            }

            Consola.ShowContentOfFile(lines, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition > currentEndColumn + lineIndex.Length ? currentEndColumn + lineIndex.Length : horizontalPosition, verticalPosition);
        }

        private static void NavigateDown(ref int lineCounting, int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
        {
            if (lineCounting >= lines.Length - 1)
            {
                return;
            }

            lineCounting++;
            string lineIndex = Convert.ToString(lines.Length - 1) + " ";
            int currentStartColumn = Math.Max(0, Math.Min(startingColumn, lines[lineCounting].Length));
            int currentEndColumn = lines[lineCounting].Length - currentStartColumn < Console.WindowWidth ? lines[lineCounting].Length - currentStartColumn : Console.WindowWidth - 1;

            if (currentStartColumn < startingColumn)
            {
                startingColumn = currentStartColumn;
            }

            if (verticalPosition + 1 == Console.WindowHeight)
            {
                if (startingLine + 1 <= lines.Length - Console.WindowHeight)
                {
                    startingLine++;
                }
            }
            else
            {
                verticalPosition++;
            }

            Consola.ShowContentOfFile(lines, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition > currentEndColumn + lineIndex.Length ? currentEndColumn + lineIndex.Length : horizontalPosition, verticalPosition);
        }

        private static void NavigateLeft(ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
        {
            int currentStartColumn = Math.Max(0, Math.Min(startingColumn, lines[lineCounting].Length));
            int currentEndColumn = lines[lineCounting].Length - currentStartColumn < Console.WindowWidth ? lines[lineCounting].Length - currentStartColumn : Console.WindowWidth - 1;
            string lineIndex = Convert.ToString(lines.Length - 1) + " ";

            if (Console.CursorLeft == lineIndex.Length && lineCounting != 0)
            {
                horizontalPosition = lineIndex.Length;
                if (startingColumn == 0)
                {
                    currentEndColumn = lines[lineCounting - 1].Length - currentStartColumn;
                    while (currentEndColumn > Console.WindowWidth - lineIndex.Length - 1)
                    {
                        startingColumn++;
                        currentEndColumn--;
                    }

                    horizontalPosition = currentEndColumn + lineIndex.Length;
                    NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                    return;
                }

                startingColumn--;
                Consola.ShowContentOfFile(lines, startingLine, startingColumn);
            }
            else
            {
                if (horizontalPosition > currentEndColumn + lineIndex.Length)
                {
                    horizontalPosition = currentEndColumn - 1 + lineIndex.Length;
                }
                else if (horizontalPosition > lineIndex.Length)
                {
                    horizontalPosition--;
                }
            }

            Console.SetCursorPosition(horizontalPosition > currentEndColumn + lineIndex.Length ? currentEndColumn + lineIndex.Length : horizontalPosition, verticalPosition);
        }

        private static void NavigateRight(ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
        {
            int currentStartColumn = Math.Max(0, Math.Min(startingColumn, lines[lineCounting].Length));
            int currentEndColumn = lines[lineCounting].Length - currentStartColumn < Console.WindowWidth ? lines[lineCounting].Length - currentStartColumn : Console.WindowWidth - 1;
            string lineIndex = Convert.ToString(lines.Length - 1) + " ";

            if (horizontalPosition + 1 == Console.WindowWidth && lines[lineCounting].Length - currentStartColumn + lineIndex.Length - 1 > Console.WindowWidth - 1)
            {
                startingColumn++;
                Consola.ShowContentOfFile(lines, startingLine, startingColumn);
                Console.SetCursorPosition(horizontalPosition, verticalPosition);
            }
            else
            {
                if (horizontalPosition >= currentEndColumn + lineIndex.Length - 1 || currentEndColumn == 0)
                {
                    horizontalPosition = lineIndex.Length;
                    startingColumn = 0;
                    NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                }
                else
                {
                    horizontalPosition++;
                    Console.SetCursorPosition(horizontalPosition, verticalPosition);
                }
            }
        }

        private static void EndButtonBehaviour(int lineCounting, ref int horizontalPosition, int verticalPosition, int startingLine, ref int startingColumn, string[] lines)
        {
            int currentStartColumn = Math.Max(0, Math.Min(startingColumn, lines[lineCounting].Length));
            int currentEndColumn = lines[lineCounting].Length - currentStartColumn;
            string lineIndex = Convert.ToString(lines.Length - 1) + " ";
            while (currentEndColumn > Console.WindowWidth - lineIndex.Length - 1)
            {
                startingColumn++;
                currentEndColumn--;
            }

            horizontalPosition = currentEndColumn + lineIndex.Length;
            Consola.ShowContentOfFile(lines, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition > currentEndColumn + lineIndex.Length ? currentEndColumn + lineIndex.Length : horizontalPosition, verticalPosition);
        }

        private static void HomeButtonBehaviour(ref int horizontalPosition, int verticalPosition, int startingLine, ref int startingColumn, string[] lines)
        {
            string lineIndex = Convert.ToString(lines.Length - 1) + " ";
            horizontalPosition = lineIndex.Length;
            startingColumn = 0;
            Consola.ShowContentOfFile(lines, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition, verticalPosition);
        }

        private static void PageDownBehaviour(ref int lineCounting, int horizontalPosition, int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
        {
            int newStartingLine = startingLine + Console.WindowHeight - 1;
            int originalVerticalPosition = verticalPosition;
            int downSteps = lines.Length - 1 - newStartingLine;
            if (newStartingLine + Console.WindowHeight - 1 <= lines.Length - 1)
            {
                while (startingLine < newStartingLine)
                {
                    NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                }
            }
            else
            {
                newStartingLine = startingLine + downSteps;
                while (startingLine < newStartingLine)
                {
                    NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                }
            }

            while (verticalPosition != originalVerticalPosition)
            {
                NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
            }
        }

        private static void PageUpBehaviour(ref int lineCounting, int horizontalPosition, int verticalPosition, ref int startingLine, ref int startingColumn, string[] lines)
        {
            int newStartingLine = startingLine - Console.WindowHeight + 1;
            int originalVerticalPosition = verticalPosition;
            if (newStartingLine - Console.WindowHeight - 1 >= 0)
            {
                while (startingLine > newStartingLine)
                {
                    NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                }
            }
            else
            {
                while (startingLine > 0)
                {
                    NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
                }
            }

            while (verticalPosition != originalVerticalPosition)
            {
                NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);
            }
        }
    }
}